#include "spreadsheetSession.h"
#include "../Parser/xmlParser.h"
#include "../Parser/commandParser.h"
#include <fstream>
#include <pthread.h>
#include <iostream>
#include <stdio.h>
#include <string.h>
#include <ctime>
#include <math.h>
#include <algorithm>


using namespace std;

//Circular exception for circular dependencies of cells
class circularException: public exception
{
  virtual const char* what() const throw()
  {
    return "Circular Exception Occured";
  }
}circularException;


//Structures needed for the new thread
typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  stack<cell::cell*> *sessionStack;
  vector<int> *socketFDs;
  DependencyGraph::DependencyGraph *dependencyGraph;
  map<string, string> *cellContentsMap;
  string spreadsheetName;
} queueArgs;

//Strucutres needed for the listensocket thread
typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  vector<int> *socketFDs;
  int socketFD;
} listenArgs;


//Used to add a new user to the session.
void * listenSocket(void * args)
{
  listenArgs *lArgs = (listenArgs *) args;
  char buffer[256];
  int readCount;
  string command;
  while(1)
    {
      bzero(buffer, 256);
      readCount = read(lArgs->socketFD, buffer, 255);
      if (readCount == 0)
	{
	  cout << lArgs->socketFDs->size() << endl;
	cout << "read error in listen" << endl;
	vector<int>::iterator it;
	it = find(lArgs->socketFDs->begin(),lArgs->socketFDs->end(),lArgs->socketFD);
	close(lArgs->socketFD);
	lArgs->socketFDs->erase(it);
	cout << lArgs->socketFDs->size() << endl;
	return (void *) args;
	}
      else 
	{
	  command = commandParser::parseCommand(buffer);
	  workItem::workItem* item = new workItem::workItem(lArgs->socketFD, buffer);
	  lArgs->sessionQueue->push(item);
	}
      sleep(1);
    }
}

void * doWork(void * args)
{
  queueArgs *qArgs = (queueArgs *) args;
  char buffer[256];
  int writeCount = 0;

  //Variables for autosaving after a certain interval has passed
  time_t startTime = clock();
  time_t testTime;
  int timePassed;
  const int SAVE_TIMER_SEC = 10;

  while(true)
    {
      //Get the time that has passed
      testTime = time(NULL);
      timePassed = static_cast<int>(testTime - startTime);
      //Auto save on the modulous in seconds
      if(timePassed % SAVE_TIMER_SEC == 0)
       {
	 string name = qArgs->spreadsheetName;
	 name += ".xml";
	 ofstream output(name.c_str());
	 map<string,string> *mPointer = qArgs->cellContentsMap;
	 writeFile(*mPointer,output);
        }

      /*
       *Queue work session
       */
      if(qArgs->sessionQueue->size() > 0)
	{
	   workItem::workItem* wrkItem;
	   wrkItem = qArgs->sessionQueue->front();
	   qArgs->sessionQueue->pop();
	   string msg = wrkItem->getCommand();
	   string command = commandParser::parseCommand(msg);

	   //Add Request
	   if (command.compare("add") == 0)
	     {
	       pthread_t thread;
	       listenArgs* listen = new listenArgs();
	       //Create a new thread for the user to listen on
	       listen->sessionQueue = qArgs->sessionQueue;
	       listen->socketFD = wrkItem->getSocket();
	       listen->socketFDs = qArgs->socketFDs;
	       pthread_create(&thread, 0, listenSocket, (void *) listen);
	       pthread_detach(thread);
	       //Keep the socket in a vector of active users
	       qArgs->socketFDs->push_back(wrkItem->getSocket());
	       //Send connection conmand to the client
	       bzero(buffer,256); 
	       sprintf(buffer, "connected %d\n", qArgs->cellContentsMap->size());
	       writeCount = write(wrkItem->getSocket(),buffer,strlen(buffer));
	       //Iterate over the map of cells and send it to the new user 
	       for (map<string, string>::iterator it = qArgs->cellContentsMap->begin(); it != qArgs->cellContentsMap->end(); it++)
		 {
		   bzero(buffer,256);
		   string first = it->first;
		   string second = it->second;
		   sprintf(buffer, "cell %s %s\n", first.c_str(), second.c_str());
		   writeCount = write(wrkItem->getSocket(),buffer,strlen(buffer));
		 } 
	     }
	   //New cell change
	   if (command.compare("cell") == 0)
	     {	  
	       string cellName = commandParser::parseCellName(wrkItem->getCommand());
	       string cellContents = commandParser::parseCellContents(wrkItem->getCommand());
	       list<string> listOfCells;
	       list<string> currDependents;
	       list<string> incomingCellDependents;
	       bool cirDep = false;
	       //List of dependents of the cell being changed
	       incomingCellDependents = qArgs->dependencyGraph->GetDependents(cellName);
	       //List of cells involved in the cell change request
	       listOfCells = commandParser::parseCells(cellContents);
	       try
		 {
		   //Get the dependents of every cell involved in the change request
		   for(list<string>::iterator it = listOfCells.begin(); it != listOfCells.end(); it++)
		     {
		       currDependents = qArgs->dependencyGraph->GetDependents(*it);
		       for(list<string>::iterator itr = currDependents.begin(); itr != currDependents.end(); itr++)
			 {
			   //If the cellName matches a current dependent it is a circular dependency
			   if ((*itr).compare(cellName) == 0)
			     {
			       throw circularException;
			     }
			 }
		     }
		 }
	       //Circular exception happened
	       catch (exception& e)
		 {
		   cirDep = true;
		   bzero(buffer,256);
		   sprintf(buffer, "error 1 circular dependency\n");
		   //Get socket of the user that sent the bad change request
		   int senderSocketFD = wrkItem->getSocketFD();
		   writeCount = write(senderSocketFD,buffer,strlen(buffer));
		 }
	       //Only want to actually update structures if a circular dependency didn't happen
	       if (!cirDep)
		 {
		   for (list<string>::iterator it = incomingCellDependents.begin(); it != incomingCellDependents.end(); it++)
		     {
		       qArgs->dependencyGraph->RemoveDependency(cellName, (*it));
		     }
		  
		   for(list<string>::iterator itr = listOfCells.begin(); itr != listOfCells.end(); itr++)
		     {
		       cout << (*itr) << "\t";
		       qArgs->dependencyGraph->AddDependency(cellName, (*itr));
		     }
	       	   //Update the cell map
		   if (qArgs->cellContentsMap->count(cellName)>0)
		     {
		       cell::cell* lastCell = new cell::cell(cellName, qArgs->cellContentsMap->operator[](cellName));
		       qArgs->sessionStack->push(lastCell);
		       cout << "cell Name: " << cellName << endl;
		       cout << "old cell Contents: " << qArgs->cellContentsMap->operator[](cellName) << endl;
		     }
		   else{
		   qArgs->cellContentsMap->insert(pair<string, string>(cellName, cellContents));
		   cell::cell* lastCell = new cell::cell(cellName,"");
		   qArgs->sessionStack->push(lastCell);
		   }
		   //Send the cell change to all the users
		   bzero(buffer,256);
		   sprintf(buffer, "cell %s %s\n", cellName.c_str(), cellContents.c_str());
		   for (vector<int>::iterator it = qArgs->socketFDs->begin(); it != qArgs->socketFDs->end(); it++)
		     {
		       writeCount = write((*it),buffer,strlen(buffer));
		     }
		 }
	     }
	   if(command.compare("undo") == 0)
	     {
	       string cellContents = "";
	       string cellName = "";
	       cout << qArgs->sessionStack->size() << " stack size" << endl;
	       if (qArgs->sessionStack->size() > 0)
		 {
		 cell:cell* lastCell; 
		 lastCell = qArgs->sessionStack->top();
		 cellName = lastCell->getName();
		 cellContents = lastCell->getContents();
		 locale loc;
		 string result = "";
		 //Put new cell in cell contents map
		 qArgs->cellContentsMap->insert(pair<string,string>(cellName,cellContents));
		 qArgs->cellContentsMap->operator[](cellName);
		 qArgs->sessionStack->pop();
		 }
	      
	      //Send the cell change to all the users
	      bzero(buffer,256);
	      sprintf(buffer, "cell %s %s\n", cellName.c_str(), cellContents.c_str());
	      for (vector<int>::iterator it = qArgs->socketFDs->begin(); it != qArgs->socketFDs->end(); it++)
	       {
		 writeCount = write((*it),buffer,strlen(buffer));
	       }
	     }
	}
     sleep(1);
    }
}
//Constructor for a spreadsheetSession. This constructor creates a spreadsheetSession on a new thread
//and handles enqueuing and dequeueing incoming spreadsheet commands.
spreadsheetSession::spreadsheetSession(std::string name)
{
  spreadsheetName = name;
  string xmlName = name + ".xml";
  this->active = true;
  ifstream f(xmlName.c_str());
  //Check for an existing spreadsheet file. If it's not found
  //write a new xml file for the spreadsheet
  if(!f.good())
    {
      this->cellContentsMap = map<string, string>();
      ofstream output(xmlName.c_str());
      writeFile(cellContentsMap,output);
    }
  //If an existing spreadsheet file is found, read in the xml
  //and populate the map of cell contents
  else
    {
      this->cellContentsMap = getCells(xmlName);
    }
  this->sessionQueue = queue<workItem::workItem*>();
  this->sessionStack = stack<cell::cell*>();
  this->socketFDs = vector<int>();
  this->dependencyGraph = DependencyGraph::DependencyGraph();
  
  //Set up the structures needed for the new thread
  queueArgs* qArgs = new queueArgs();
  qArgs->sessionQueue = &(this->sessionQueue);
  qArgs->sessionStack = &(this->sessionStack);
  qArgs->socketFDs = &(this->socketFDs);
  qArgs->cellContentsMap = &(this->cellContentsMap);
  qArgs->dependencyGraph = &(this->dependencyGraph);
  qArgs->spreadsheetName = this->spreadsheetName;

  pthread_t thread;
  pthread_create(&thread, 0, doWork,  (void *) qArgs);
  pthread_detach(thread);
  //pthread_join(thread, NULL);
}

spreadsheetSession::spreadsheetSession(const spreadsheetSession &other)
{

}
spreadsheetSession::~spreadsheetSession()
{
  // Do this?
  this->active = false;
}

std::string spreadsheetSession::getspreadsheetName()
{
  return spreadsheetName;
}

void spreadsheetSession::enqueue(workItem::workItem* item)
{
  sessionQueue.push(item);
}
