#ifndef XMLPARSER_H
#define XMLPARSER_H
#include<map>
#include<string>


   std::map<std::string, std::string> getCells(std::string);
   std::string getVersion(const std::string &filename);
   void writeFile(std::map<std::string, std::string>, std::ostream & os);

#endif
