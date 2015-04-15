#include<string>

#ifndef CELL_H
#define CELL_H


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
