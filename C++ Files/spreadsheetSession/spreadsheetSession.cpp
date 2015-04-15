#include "spreadsheetSession.h"


spreadsheetSession::spreadsheetSession(std::string name)
{
  spreadsheetName = name;
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


