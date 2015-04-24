 #include<string>

#ifndef WORKITEM_H
#define WORKITEM_H

//This header file creates a workItem. The workItem is used in the 
//spreadsheetSession.cpp class in order to add command request
//onto the queue and stack
class workItem
{
 public:
  workItem();
  workItem(int socketFD, std::string command);
  workItem(const workItem &other);
  ~workItem();
  int getSocket();
  std::string getCommand();
  int getSocketFD();
  workItem& operator=(const workItem&);
 private:
  int socketFD;
  std::string command;
};


#endif
