#include "spreadsheetSession.h"
#include "../Parser/xmlParser.h"
#include <fstream>
#include <pthread.h>
#include <iostream>


using namespace std;

typedef struct
{
  queue<workItem::workItem> *sessionQueue;
  stack<cell::cell> *sessionStack;
  vector<int> *socketFDs;
} queueArgs;
void * doWork(void * args)
{
  queueArgs *qArgs = (queueArgs *) args;
  
  while(true)
    {
      cout<< "We up in las vegas. They hate us." << endl;
      sleep(1);
    }
  
  
}
spreadsheetSession::spreadsheetSession(std::string name)
{
  spreadsheetName = name;
  string xmlName = name + ".xml";
  this->active = true;
  ifstream f(name.c_str());
  cout << "they hate us" << endl;
  if(!f.good())
    {
      this->cellContentsMap = map<string, string>();
      ofstream output(xmlName.c_str());
      writeFile(cellContentsMap,output);
    }
  else
    {
      cellContentsMap = getCells(xmlName);
    }
  //Populate dependency graph
  this->sessionQueue = queue<workItem::workItem>();
  this->sessionStack = stack<cell::cell>();
  this->socketFDs = vector<int>();
  
  queueArgs* qArgs = new queueArgs();
  qArgs->sessionQueue = &(this->sessionQueue);
  qArgs->sessionStack = &(this->sessionStack);
  qArgs->socketFDs = &(this->socketFDs);

  pthread_t thread;
  pthread_create(&thread, 0, doWork,  (void *) qArgs);
  pthread_detach(thread);
  
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

void spreadsheetSession::enqueue(workItem::workItem item)
{
  cout << "in enqueue function " << endl;
  sessionQueue.push(item);
}
