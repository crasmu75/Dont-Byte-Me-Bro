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


using namespace std;

class circularException: public exception
{
  virtual const char* what() const throw()
  {
    return "Circular Exception Occured";
  }
}circularException;



typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  stack<cell::cell*> *sessionStack;
  vector<int> *socketFDs;
  DependencyGraph::DependencyGraph *dependencyGraph;
  map<string, string> *cellContentsMap;
  string spreadsheetName;
  
} queueArgs;

typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  int socketFD;
} listenArgs;

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
	cout << "read error in listen" << endl;
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

  time_t startTime = clock();
  cout << startTime << endl;
  time_t testTime;
  int timePassed;

 while(true)
    {

      testTime = time(NULL);
      timePassed = static_cast<int>(testTime - startTime);
      cout << timePassed << endl;
	   
      if(timePassed % 10 == 0)
       {
	 string name = qArgs->spreadsheetName;
	 name += ".xml";
	 ofstream output(name.c_str());
	 map<string,string> *mPointer = qArgs->cellContentsMap;
	 cout << "saving file" << output << endl;
	 writeFile(*mPointer,output);
        }

      if(qArgs->sessionQueue->size() > 0)
	{
	  
	   workItem::workItem* wrkItem;
	   
	   wrkItem = qArgs->sessionQueue->front();
	   qArgs->sessionQueue->pop();
	   string msg = wrkItem->getCommand();
	   string command = commandParser::parseCommand(msg);

	   
	   	     
	   if (command.compare("add") == 0)
	     {
	       pthread_t thread;
	       listenArgs* listen = new listenArgs();

	       listen->sessionQueue = qArgs->sessionQueue;
	       listen->socketFD = wrkItem->getSocket();
	       pthread_create(&thread, 0, listenSocket, (void *) listen);
	       pthread_detach(thread);

	       qArgs->socketFDs->push_back(wrkItem->getSocket());
	       
	       bzero(buffer,256);
	       
	       sprintf(buffer, "connected %d\n", qArgs->cellContentsMap->size());
	       writeCount = write(wrkItem->getSocket(),buffer,strlen(buffer));
	        
	       for (map<string, string>::iterator it = qArgs->cellContentsMap->begin(); it != qArgs->cellContentsMap->end(); it++)
		 {
		   bzero(buffer,256);
		   string first = it->first;
		   string second = it->second;
		   sprintf(buffer, "cell %s %s\n", first.c_str(), second.c_str());
		   writeCount = write(wrkItem->getSocket(),buffer,strlen(buffer));
		 } 
	     }
	   
	   if (command.compare("cell") == 0)
	     {	  
	      
	       string cellName = commandParser::parseCellName(wrkItem->getCommand());
	       string cellContents = commandParser::parseCellContents(wrkItem->getCommand());
	       cout << cellContents << endl;
	       list<string> listOfCells;
	       list<string> currDependents;
	       list<string> incomingCellDependents;
	       bool cirDep = false;

	       incomingCellDependents = qArgs->dependencyGraph->GetDependents(cellName);
	       listOfCells = commandParser::parseCells(cellContents);

	       try
		 {
		   for(list<string>::iterator it = listOfCells.begin(); it != listOfCells.end(); it++)
		     {
		       currDependents = qArgs->dependencyGraph->GetDependents(*it);
		       for(list<string>::iterator itr = currDependents.begin(); itr != currDependents.end(); itr++)
			 {
			   cout << *itr << endl;
			   if ((*itr).compare(cellName) == 0)
			     {
			       throw circularException;
			     }
			 }
		     }
		 }
	       
	       catch (exception& e)
		 {
		   cirDep = true;
		   bzero(buffer,256);
		   sprintf(buffer, "error 1 circular dependency\n");
		   int senderSocketFD = wrkItem->getSocketFD();
		   writeCount = write(senderSocketFD,buffer,strlen(buffer));
		 }
 
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
	       	       
		   if (qArgs->cellContentsMap->count(cellName)>0)
		     {
		       cell::cell* lastCell = new cell::cell(cellName, qArgs->cellContentsMap->operator[](cellName));
		       qArgs->sessionStack->push(lastCell);
		     }

		   qArgs->cellContentsMap->insert(pair<string, string>(cellName, cellContents));

	        
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
	       
	     }
	}
     sleep(1);
    }
 
  
}
spreadsheetSession::spreadsheetSession(std::string name)
{
  spreadsheetName = name;
  string xmlName = name + ".xml";
  
 
  this->active = true;
  ifstream f(xmlName.c_str());
  
  if(!f.good())
    {
      cout << "in making new xml" << endl;
      this->cellContentsMap = map<string, string>();
      ofstream output(xmlName.c_str());
      writeFile(cellContentsMap,output);
    }
  else
    {
      this->cellContentsMap = getCells(xmlName);
    }
  //Populate dependency graph
  this->sessionQueue = queue<workItem::workItem*>();
  this->sessionStack = stack<cell::cell*>();
  this->socketFDs = vector<int>();
  
  if(!f.good())
    {
      cout << "in making new xml" << endl;
      this->cellContentsMap = map<string, string>();
      ofstream output(xmlName.c_str());
      writeFile(cellContentsMap,output);
    }
  else
    {
      this->cellContentsMap = getCells(xmlName);
    }
  //Populate dependency graph
  this->sessionQueue = queue<workItem::workItem*>();
  this->sessionStack = stack<cell::cell*>();
  this->socketFDs = vector<int>();
  this->dependencyGraph = DependencyGraph::DependencyGraph();
  
  
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
