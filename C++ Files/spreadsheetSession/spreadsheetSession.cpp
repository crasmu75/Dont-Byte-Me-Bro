#include "spreadsheetSession.h"
#include "xmlParser.h"
#include <fstream>
using namespace std;

spreadsheetSession::spreadsheetSession(std::string name)
{
  spreadsheetName = name;
  string xmlName = name + ".xml";

  ifstream f(name.c_str());
  if(!f.good())
    {
      this->cellContentsMap = map<string, string>();
    }
  cellContentsMap = getCells(xmlName);
  
  
}
spreadsheetSession::spreadsheetSession(const spreadsheetSession &other)
{

}
spreadsheetSession::~spreadsheetSession()
{

}

std::string spreadsheetSession::getspreadsheetName()
{
  return spreadsheetName;
}

void spreadsheetSession::enqueue(workItem::workItem item)
{
  sessionQueue.push(item);
}
