#include "cell.h"
#include <string>

using namespace std;

//This class creates a cell object that will be used in the spreadsheet
//to keep track of contents and the name of the cell
cell::cell()
{
  this->name = "";
  this->contents = "";
}

cell::cell(string cell_name, string cell_contents)
{
  this->name = cell_name;
  this->contents = cell_contents;
}

cell::cell(const cell &other)
{
  this->name = other.name;
  this->contents = other.contents;
}

cell::~cell(){}

std::string cell::getName()
{
  return this->name;
}

std::string cell::getContents()
{
  return this->contents;
}

void cell::setContents(string new_contents)
{
  this->contents = new_contents;
}
