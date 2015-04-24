 #include<string>

#ifndef WORKITEM_H
#define WORKITEM_H

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
