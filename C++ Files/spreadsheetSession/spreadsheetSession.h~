#include <string>
#include <vector>
#include <queue>
#include <map>
#include <stack>
#include "workItem.h"
#include "cell.h"
#include<vector>
#include "../DependencyGraph/DependencyGraph.h"
#include <mutex>

//This class creates a spreadsheet session. The session should
//be responsible for spinning all the new threads necessary for incoming
//users to the session. It ensures non blocking asynchronous communication
//between the users and the server, by putting each new socket on a thread.
//A queue is used to process commands, and a stack is used to keep track of
//previous commands for undo. The cellContentsMap holds the contents of all
//the cells in the spreadsheet.

class spreadsheetSession
{
  private:
  std::mutex *queueLock;
  std::string spreadsheetName;
  //Holds all information about cells in the spreadsheet
  std::map<std::string,std::string> cellContentsMap;
  //Used to process request
  std::queue<workItem::workItem*> sessionQueue;
  //used to process undo
  std::stack<cell::cell*> sessionStack;
  //used to keep track of all active sockets
  std::vector<int> socketFDs;
  //keeps track of circular dependencies between cells
  DependencyGraph::DependencyGraph dependencyGraph;
  //List of all active spreadsheetSessions
  std::vector<spreadsheetSession*> *spreadsheetSessions;
  bool closed;
  bool active;
 
  
  
 public:
  spreadsheetSession(std::string, std::vector<std::string>*, std::mutex*, std::vector<spreadsheetSession::spreadsheetSession*>*);
  spreadsheetSession(const spreadsheetSession &other);
  ~spreadsheetSession();
  std::string getspreadsheetName();
  void enqueue(workItem::workItem*);
  void dequeue(workItem::workItem*);
  void addSocketFD(int);
  std::mutex* getQueueLock();
};

