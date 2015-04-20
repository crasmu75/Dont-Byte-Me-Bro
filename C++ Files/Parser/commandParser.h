/*
 *Filename: commandParser.h
 *This class contains helper methods for parsing messages being
 *sent between the clients and servers.
 *
 *The goal of this class is to be able to call static methods from
 *the sever to help with the parssing of messages.Refer to the command
 *section of the Protocol to see how commands are structured.
 */

#include<string>
#include<list>

class commandParser
{
 public:
  commandParser();
  commandParser(const commandParser &other);
  ~commandParser();
  //Returns the first word of the message. This is the command
  static std::string parseCommand(std::string);
  //Returns the client name from the given command
  static std::string parseClientName(std::string);
  //Returns the spreadsheet name from a Connect command
  static std::string parseSpreadsheetName(std::string);
  //Returns the Cell name from a given command
  static std::string parseCellName(std::string);
  //Returns the cell contents from a given command
  static std::string parseCellContents(std::string);
  //Returns the number of cells in a Confirm Connection command
  static int parseNumberOfCells(std::string);
  //Returns additional error info from an Error command
  static std::string parseAdditionalInformation(std::string);
  //Returns username from register command
  static std::string parseUsername(std::string);
  //Returns the error id from an Error command
  static int parseErrorID(std::string);
  static std::list<std::string> parseCells(std::string);
  
 private:
};


