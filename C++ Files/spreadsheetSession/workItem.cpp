#include "workItem.h"
#include<string>

using namespace std;

workItem::workItem(){}

workItem::workItem(int _socketFD, string _command)
{
  this->socketFD = _socketFD;
  this->command = _command;
}

workItem::workItem(const workItem &other)
{
  this->socketFD = other.socketFD;
  this->command = other.command;
}

workItem::~workItem() {}

int workItem::getSocket()
{
  return this->socketFD;
}

string workItem::getCommand()
{
  return this->command;
}
