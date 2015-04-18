#include "spreadsheetSession.h"
#include "../Parser/xmlParser.h"
#include "../Parser/commandParser.h"
#include <fstream>
#include <pthread.h>
#include <iostream>
#include <stdio.h>
#include <string.h>


using namespace std;

typedef struct
{
  queue<workItem::workItem*> *sessionQueue;
  stack<cell::cell*> *sessionStack;
  vector<int> *socketFDs;
  map<string, string> *cellContentsMap;
  
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
 while(true)
    {
      //cout<< "in do work function " << qArgs->sessionQueue->size() << endl;
      //sleep(2);
    
  if(qArgs->sessionQueue->size() > 0)
	{
	  workItem::workItem* wrkItem;
	   cout<< "in do work function " << qArgs->sessionQueue->size() << endl;
	   wrkItem = qArgs->sessionQueue->front();
	   qArgs->sessionQueue->pop();
	   string msg = wrkItem->getCommand();
	   string command = commandParser::parseCommand(msg);

	   cout << "Command: " << wrkItem->getSocket() << "\t" << wrkItem->getCommand() << endl;
	   
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
	       
	       cout << "Write count: " << writeCount << endl;
	       
	       //vector<int> allSockets;
	       //allSockets = *(clientSockets->sockets); 

     
	       for (map<string, string>::iterator it = qArgs->cellContentsMap->begin(); it != qArgs->cellContentsMap->end(); it++)
		 {
		   bzero(buffer,256);
		   string first = it->first;
		   string second = it->second;
		   sprintf(buffer, "cell %s %s\n", first.c_str(), second.c_str());
		   writeCount = write(wrkItem->getSocket(),buffer,strlen(buffer));
		   cout << buffer << ":" << writeCount << endl;
		 } 
	       
	     }

	   if (command.compare("cell") == 0)
	     {	      
	       string cellName = commandParser::parseCellName(wrkItem->getCommand());
	       string cellContents = commandParser::parseCellContents(wrkItem->getCommand());

	       // ************************CHECK DEPENDENCY GRAPH**************************** 
	       if (qArgs->cellContentsMap->count(cellName)>0)
		 {
		   cell::cell* lastCell = new cell::cell(cellName, qArgs->cellContentsMap->operator[](cellName));
		   qArgs->sessionStack->push(lastCell);
		   cout << "Map size: " << qArgs->cellContentsMap->size() << endl;
		 }
	       qArgs->cellContentsMap->insert(pair<string, string>(cellName, cellContents));

	        
	       bzero(buffer,256);
    
	       sprintf(buffer, "cell %s %s\n", cellName.c_str(), cellContents.c_str());
     

     
	       for (vector<int>::iterator it = qArgs->socketFDs->begin(); it != qArgs->socketFDs->end(); it++)
		 {
		   writeCount = write((*it),buffer,strlen(buffer));
		 }
     
	     }
	     cout<<msg<<endl;
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
  
  queueArgs* qArgs = new queueArgs();
  qArgs->sessionQueue = &(this->sessionQueue);
  qArgs->sessionStack = &(this->sessionStack);
  qArgs->socketFDs = &(this->socketFDs);
  qArgs->cellContentsMap = &(this->cellContentsMap);

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
