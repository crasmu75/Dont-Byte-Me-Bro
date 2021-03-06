#include "spreadsheetSession.h"
#include "../Parser/xmlParser.h"
#include "../Parser/commandParser.h"
#include <fstream>
#include<vector>
#include <pthread.h>
#include <iostream>
#include <stdio.h>
#include <string.h>
#include <ctime>
#include <math.h>
#include <algorithm>
#include <mutex>
#include <exception>

using namespace std;

//This class creates a spreadsheet session. The session creates
//all threads needed to ensure asynchronous communication is 
//continuous between the users of the spreadsheet and the server

//Circular exception for circular dependencies of cells
class circularException: public exception
{
  virtual const char* what() const throw()
  {
    return "Circular Exception Occured";
  }
}circularException;


//Structures needed for the doWork thread. This thread
//will dequeue items from the queue and try to process the
//commands
typedef struct
{
  vector<string>* users;
  queue<workItem::workItem*> *sessionQueue;
  stack<cell::cell*> *sessionStack;
  vector<int> *socketFDs;
  DependencyGraph::DependencyGraph *dependencyGraph;
  map<string, string> *cellContentsMap;
  string spreadsheetName;
  mutex *usersLock;
  mutex *queueLock;
  bool *closed;
} queueArgs;


//Strucutres needed for the listensocket thread
//This thread will continue listening on the connected users
//socket and enqueue commands 
typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  vector<int> *socketFDs;
  int socketFD;
  vector<string> *users;
  mutex *socketsLock;
  mutex *usersLock;
  mutex *queueLock;
} listenArgs;

//Used to add a new user to the session.
//And listen for incoming commands
void * listenSocket(void * args)
{
  listenArgs *lArgs = (listenArgs *) args;
  char buffer[256];
  int readCount;
  string command;
  string incMsg = "";
  while(1)
    {
      bzero(buffer, 256);
      size_t posEndLine = string::npos;
      //Make sure all of the message is recieved up until the
      //\n return.
      while(posEndLine == string::npos)
      {
      readCount = read(lArgs->socketFD, buffer, 255);
      incMsg += buffer;
      posEndLine = incMsg.find('\n');
      //If the readcount is 0, the user's socket
      //has been closed or diconected
      if (readCount == 0)
	{
	cout << "closing socket" << endl;
	vector<int>::iterator it;
	lArgs->socketsLock->lock();
	//Find the socket that diconected in the socketFDs list and remove it
	it = find(lArgs->socketFDs->begin(),lArgs->socketFDs->end(),lArgs->socketFD);
	close(lArgs->socketFD);
	lArgs->socketFDs->erase(it);
	lArgs->socketsLock->unlock();
	return (void *) args;
	}
	}
      //Make sure to receive all bytes of the incoming
      //message unti the \n
      while(incMsg.find('\n') != string::npos)
	{
	  posEndLine = incMsg.find('\n');
	  string msg = incMsg.substr(0,posEndLine);
	  incMsg = incMsg.substr(posEndLine + 1);
	command = commandParser::parseCommand(msg);
	//Register the user
	if (command.compare("register") == 0)
	 {
	   cout << command << endl;
	   string username = commandParser::parseUsername(msg);
	   lArgs->usersLock->lock();
	   //add the user to the list of users
	   lArgs->users->push_back(username);
	   //write the new user to the file of users
	   writeUsers(lArgs->users);
	   lArgs->usersLock->unlock();
	 }
	else{
	//Create a new workItem to enqueue the command
	  workItem::workItem* item = new workItem::workItem(lArgs->socketFD, msg);
	  lArgs->queueLock->lock();
	  lArgs->sessionQueue->push(item);
	  lArgs->queueLock->unlock();
	}
	}
      usleep(100000);
    }
}

//This function is called on a new thread in order to process the 
//spreadsheet session queue. The queue contains workItems that hold
//a command and the socket the command came in on. This allows the 
//session to keep track of what sockets sent what commands.
void * doWork(void * args)
{
  queueArgs *qArgs = (queueArgs *) args;
  char buffer[256];
  int writeCount = 0;
  //Variables for autosaving after a certain interval has passed
  time_t startTime = clock();
  time_t testTime;
  int timePassed;
  const int SAVE_TIMER_SEC = 2;
  mutex *socketsLock = new mutex();
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
      qArgs->queueLock->lock();
      if(qArgs->sessionQueue->size() > 0)
	{
	  //Get the workItem off the queue
	   workItem::workItem* wrkItem;
	   wrkItem = qArgs->sessionQueue->front();
	   qArgs->sessionQueue->pop();
	   string msg = wrkItem->getCommand();
	   //Parse the command
	   string command = commandParser::parseCommand(msg);

	   //Add Request
	   if (command.compare("add") == 0)
	     {
	       pthread_t thread;
	       listenArgs* listen = new listenArgs();
	       //Create a new thread for the user to listen on
	       listen->users = qArgs->users;
	       listen->sessionQueue = qArgs->sessionQueue;
	       listen->socketFD = wrkItem->getSocket();
	       listen->socketFDs = qArgs->socketFDs;
	       listen->usersLock = qArgs->usersLock;
	       listen->queueLock = qArgs->queueLock;
	       listen->socketsLock = socketsLock;
	       pthread_create(&thread, 0, listenSocket, (void *) listen);
	       pthread_detach(thread);
	       //Keep the socket in a vector of active users
	       socketsLock->lock();
	       qArgs->socketFDs->push_back(wrkItem->getSocket());
	       socketsLock->unlock();
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
	   else if (command.compare("cell") == 0)
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
		   //Get dependents of every cell involved in the change request and remove the old dependencies
		   for (list<string>::iterator it = incomingCellDependents.begin(); it != incomingCellDependents.end(); it++)
		     {
		       qArgs->dependencyGraph->RemoveDependency(cellName, (*it));
		     }
		   //Update the new dependencies for the incoming cell
		   for(list<string>::iterator itr = listOfCells.begin(); itr != listOfCells.end(); itr++)
		     {
		       qArgs->dependencyGraph->AddDependency(cellName, (*itr));
		     }
	       	   //Update the cell map
		   if (qArgs->cellContentsMap->count(cellName)>0)
		     {
		       cell::cell* lastCell = new cell::cell(cellName, qArgs->cellContentsMap->operator[](cellName));
		       qArgs->sessionStack->push(lastCell);
		       //If the cellContents is blank we want to erase it from the map so we don't have a hgue map of blank
		       //cells
		       if(cellContents.compare("") == 0)
			 {
			   qArgs->cellContentsMap->erase(cellName);
			 }
		       else
			 {
			   qArgs->cellContentsMap->operator[](cellName) = cellContents;
			 }  
		     }
		   else
		   {
		     //add the new cell to the map
		     if(cellContents.compare("") != 0)
		       {
			 qArgs->cellContentsMap->insert(pair<string, string>(cellName, cellContents));
			 cell::cell* lastCell = new cell::cell(cellName,"");
			 qArgs->sessionStack->push(lastCell);
		       }
		   }
		   //Send the cell change to all the users
		   bzero(buffer,256);
		   sprintf(buffer, "cell %s %s\n", cellName.c_str(), cellContents.c_str());
		   socketsLock->lock();
		   for (vector<int>::iterator it = qArgs->socketFDs->begin(); it != qArgs->socketFDs->end(); it++)
		     {
		       writeCount = write((*it),buffer,strlen(buffer));
		     }
		   socketsLock->unlock();
		 }
	     }
	   else if(command.compare("undo") == 0)
	     {
	       string cellContents = "";
	       string cellName = "";
	       //If the stack is < 0 there's nothing to undo
	       if (qArgs->sessionStack->size() > 0)
		 {
		 cell:cell* lastCell; 
		 lastCell = qArgs->sessionStack->top();
		 cellName = lastCell->getName();
		 cellContents = lastCell->getContents();
		 //Put new cell in cell contents map
		 if(cellContents.compare("") == 0)
		   {
		     qArgs->cellContentsMap->erase(cellName);
		   }
		 else
		   {
		     qArgs->cellContentsMap->operator[](cellName) = cellContents;
		   }
		 qArgs->sessionStack->pop();
	      //Send the cell change to all the users
	      bzero(buffer,256);
	      sprintf(buffer, "cell %s %s\n", cellName.c_str(), cellContents.c_str());
	      socketsLock->lock();
	      for (vector<int>::iterator it = qArgs->socketFDs->begin(); it != qArgs->socketFDs->end(); it++)
	       {		 
		 writeCount = write((*it),buffer,strlen(buffer));
	       }
	      socketsLock->unlock();
		 }
	     }
	   else
	     {
	       cout << msg << endl;
	       bzero(buffer,256);
	       sprintf(buffer, "error 2 Did not recieve a valid command from this user\n");
	       int senderSocketFD = wrkItem->getSocketFD();
	       writeCount = write(senderSocketFD,buffer,255);
	
	     }
	}
     qArgs->queueLock->unlock();
     if(qArgs->socketFDs->size() == 0 && qArgs->sessionQueue->size() == 0)
       {
	 (*qArgs->closed) = true;
	 //delete socketsLock;
	 //delete (queueArgs*)args;
       }
     usleep(10000);
    }
}
//Constructor for a spreadsheetSession. This constructor creates a spreadsheetSession on a new thread
//and handles enqueuing and dequeueing incoming spreadsheet commands.
spreadsheetSession::spreadsheetSession(std::string name, std::vector<string> *users, mutex *usersLock, vector<spreadsheetSession::spreadsheetSession*> *ssSessions)
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
  this->queueLock = new mutex();
  this->spreadsheetSessions = ssSessions;
  this->closed = false;
  
  //Set up the structures needed for the new thread
  queueArgs* qArgs = new queueArgs();
  qArgs->users = users;
  qArgs->sessionQueue = &(this->sessionQueue);
  qArgs->sessionStack = &(this->sessionStack);
  qArgs->socketFDs = &(this->socketFDs);
  qArgs->cellContentsMap = &(this->cellContentsMap);
  qArgs->dependencyGraph = &(this->dependencyGraph);
  qArgs->spreadsheetName = this->spreadsheetName;
  qArgs->queueLock = this->queueLock;
  qArgs->usersLock = usersLock;
  qArgs->closed = &this->closed;

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
std::mutex* spreadsheetSession::getQueueLock()
{
  return this->queueLock;
}
bool spreadsheetSession::isClosed()
{
  return this->closed;
}
