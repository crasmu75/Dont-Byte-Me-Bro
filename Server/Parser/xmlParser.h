#ifndef XMLPARSER_H
#define XMLPARSER_H
#include<map>
#include<vector>
#include<string>


   std::map<std::string, std::string> getCells(std::string);
   void writeFile(std::map<std::string, std::string>, std::ostream & os);
std::vector<std::string>* getUsers();
void writeUsers(std::vector<std::string>*);

#endif
