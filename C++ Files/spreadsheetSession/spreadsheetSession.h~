#include <string>
#include <vector>
#include <queue>
#include <map>
#include <stack>
#include "workItem.h"
#include "cell.h"

class spreadsheetSession
{
  private:
  std::string spreadsheetName;
  std::map<std::string,std::string> cellContentsMap;
  std::queue<workItem::workItem> sessionQueue;
  std::stack<cell::cell> sessionStack;
  std::vector<int> socketFDs;
  //Dependency graph
  
  
 public:
  spreadsheetSession(std::string);
  spreadsheetSession(const spreadsheetSession &other);
  ~spreadsheetSession();
  void createNewSession();
  void closeSession();
  void addUser();
  void removeUser();
  void sendCommand(std::string);
  std::string getspreadsheetName();
  void enqueue(workItem::workItem);
  void dequeue(workItem::workItem);
};

