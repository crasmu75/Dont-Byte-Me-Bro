#include "commandParser.h"
#include <iostream>
#include <sstream>
#include <boost/algorithm/string.hpp>
#include <algorithm>

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
  std::locale loc;
  std::string result = "";
  //Make cellContents uppercase
  for(int i = 0;i< s.length();i++)
  {
    result += std::toupper(s.at(i),loc);
  }
  s = result;
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
  if(s.substr(0,1).compare("=") == 0)
  {
  std::string result = "";
  std::locale loc;
  //Make cellContents uppercase
  for(int i = 0;i< s.length();i++)
  {
    result += std::toupper(s.at(i),loc);
  }
  s = result;
  }
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

std::string commandParser::parseUsername(std::string msg)
{
   std::size_t posFirstWord = msg.find(" ") + 1;
   std::string s = msg.substr(posFirstWord);
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

std::list<std::string> commandParser::parseCells(std::string s)
{
  
  char strArray[1024];
  bzero(strArray,1024);
  strcpy(strArray, s.c_str());
  bool prevLetter = false;
  bool prevNumber = false;
  std::string current_cell = "";
  std::list<std::string> cells;
  
  for(int i = 0; i < 1024; i++)
    {
      if ((strArray[i] > 64 && strArray[i] < 91) || (strArray[i] > 96 && strArray[i] < 123))
	{
	  
	  prevLetter = true;
	  if (prevLetter)
	    {
	      current_cell += strArray[i];
	      for (int j = i+1; j < 1024; j++)
		{
		  if (prevLetter || prevNumber)
		    {
		      if (strArray[j] > 47 && strArray[j] < 58)
			  {
			    prevNumber = true;
			    current_cell += strArray[j];
			  }
		       else
			  {
			    prevLetter = false;
			    prevNumber = false;
			    cells.push_back(current_cell);
			    current_cell = "";
			    break;
			  } // else
		    } // if
		} // for	
	    } //if
	} //if
    } //for
  return cells;
} //method



