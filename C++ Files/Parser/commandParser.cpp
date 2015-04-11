#include "commandParser.h"
#include <iostream>
#include <sstream>
#include <boost/algorithm/string.hpp>
/*Filename: commandParser.cpp
/*The goal of this class is to be able to call static methods from
 *the sever to help with the parssing of messages.Refer to the command
 *section of the Protocol to see how commands are structured.
 */
commandParser::commandParser()
{

}

commandParser::commandParser(const commandParser &other)
{


}

commandParser::~commandParser()
{

}

//Returns the first word of a message, this is what command it is.
std::string commandParser::parseCommand(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ");
  std::string s = msg.substr(0,posFirstWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns the client name for the Connect and Register commands
std::string commandParser::parseClientName(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = msg.substr(posFirstWord,posSecondWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns a spreadsheet name for the Connect command
std::string commandParser::parseSpreadsheetName(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = s.substr(posSecondWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns a cell name for the Cell command
std::string commandParser::parseCellName(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = msg.substr(posFirstWord,posSecondWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns the cell contents in a Cell command
std::string commandParser::parseCellContents(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = s.substr(posSecondWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns the number of cells in Confirm Connection command
int commandParser::parseNumberOfCells(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = msg.substr(posFirstWord);
  boost::algorithm::trim(s);
  std::stringstream ss;
  int i;
  ss << s;
  ss >> i;
  return i;
}
//Returns the additional info in an Error command
std::string commandParser::parseAdditionalInformation(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = s.substr(posSecondWord);
  boost::algorithm::trim(s);
  return s;
}
//Returns the error id of an Error command
int commandParser::parseErrorID(std::string msg)
{
  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  std::size_t posSecondWord = s.find(" ") + 1;
  s = msg.substr(posFirstWord);
  boost::algorithm::trim(s);
  std::stringstream ss;
  int i;
  ss << s;
  ss >> i;
  return i;
}


