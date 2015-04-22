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

class spreadsheetSession
{
  private:
  std::mutex *queueLock;
  std::string spreadsheetName;
  std::map<std::string,std::string> cellContentsMap;
  std::queue<workItem::workItem*> sessionQueue;
  std::stack<cell::cell*> sessionStack;
  std::vector<int> socketFDs;
  DependencyGraph::DependencyGraph dependencyGraph;
  
  bool active;
  //void * doWork(void * args);
  //Dependency graph
  
  
 public:
  spreadsheetSession(std::string, std::vector<std::string>*, std::mutex*);
  spreadsheetSession(const spreadsheetSession &other);
  ~spreadsheetSession();
  void createNewSession();
  void closeSession();
  void addUser();
  void removeUser();
  void sendCommand(std::string);
  std::string getspreadsheetName();
  void enqueue(workItem::workItem*);
  void dequeue(workItem::workItem*);
  void addSocketFD(int);
  std::mutex* getQueueLock();
};

