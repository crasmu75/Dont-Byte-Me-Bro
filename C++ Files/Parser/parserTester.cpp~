#include "commandParser.h"
#include <iostream>


int main(int argv, char** arvc)
{
  
  std::string msg = "connected 3";
  std::cout << "Message:\t" << msg << std::endl;
  std::cout << "Command:\t" << commandParser::parseCommand(msg) << std::endl;
  std::cout << "Number of cells:\t" << commandParser::parseNumberOfCells(msg) <<std::endl;
  std::string msg2 = "Cell cell_name cell_contents";
  std::cout << "Message:\t" << msg2 << std::endl;
  std::cout << "Command:\t" << commandParser::parseCommand(msg2) << std::endl;
  std::cout << "Cell name:\t" << commandParser::parseCellName(msg2) <<std::endl;
  std::cout << "Cell contents:\t" << commandParser::parseCellContents(msg2) << std::endl;
  std::string strerror = "error 3 there was an error";
  std::cout << "Message:\t" << msg << std::endl;
  std::cout<< "Error ID:\t" << commandParser::parseErrorID(strerror) << std::endl;
  std::cout<< "Error Info:\t" << commandParser::parseAdditionalInformation(strerror) << std::endl;
   msg = "connect client_name spreadsheet_name";
  std::cout << "Message:\t" << msg << std::endl;
  std::cout << "Command:\t" << commandParser::parseCommand(msg) << std::endl;
  std::cout << "Client Name:\t" << commandParser::parseClientName(msg) <<std::endl;
  std::cout<< "Spreadsheet:\t" << commandParser::parseSpreadsheetName(msg) << std::endl;
   msg = "register client_name";
  std::cout << "Message:\t" << msg << std::endl;
  std::cout << "Command:\t" << commandParser::parseCommand(msg) << std::endl;
  std::cout << "Client Name:\t" << commandParser::parseClientName(msg) <<std::endl;
   msg = "cell cell_name cell_contents";
  std::cout << "Message:\t" << msg << std::endl;
  std::cout << "Command:\t" << commandParser::parseCommand(msg) << std::endl;
  std::cout << "Cell name:\t" << commandParser::parseClientName(msg) <<std::endl;
  std::cout<< "Contents:\t" << commandParser::parseSpreadsheetName(msg) << std::endl;
  
  
}
