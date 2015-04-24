#include<string>

#ifndef CELL_H
#define CELL_H

//This class creates a cell object used to hold information about contents
//and the cell's name for the spreadsheet
class cell
{
 public:
  cell();
  cell(std::string name, std::string contents);
  cell(const cell &other);
  ~cell();
  std::string getName();
  std::string getContents();
  void setContents(std::string contents);
 private:
  std::string name;
  std::string contents;
};

#endif
