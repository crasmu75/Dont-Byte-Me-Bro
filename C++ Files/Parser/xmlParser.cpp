#include "xmlParser.h"
#include <iostream>
#include<fstream>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/xml_parser.hpp>
#include <map>
#include<vector>
#include<string>
#include<boost/algorithm/string.hpp>
#include<boost/foreach.hpp>

using namespace std;
map<string, string> getCells(string filename)
{
  using boost::property_tree::ptree;
 
  ptree p_tree;
  string cell_name;
  string cell_contents;
  map<string, string> cell_map = map<string, string>();
  read_xml(filename, p_tree);

  BOOST_FOREACH(ptree::value_type const &val_type, p_tree.get_child("spreadsheet"))
    {
    
      if (val_type.first == "cell")
	{
      cell_name = val_type.second.get<string>("name");
   
      cell_contents = val_type.second.get<string>("contents");
   
      boost::algorithm::trim(cell_name);
      boost::algorithm::trim(cell_contents);
      cell_map[cell_name] = cell_contents;
	}
    }

  return cell_map;
}

void writeFile(map<string, string> cell_map, ostream &os)
{
  using boost::property_tree::ptree;
  ptree p_tree;
  //p_tree.add("spreadsheet", ""); 
 for (map<string, string>::iterator iter = cell_map.begin(); iter != cell_map.end(); iter++)
    {
      ptree &node = p_tree.add("spreadsheet.cell", "");

      node.put("name", iter->first);
      node.put("contents", iter->second);

    }

  write_xml(os, p_tree);
}

vector<string>* getUsers()
{
  vector<string> *userlist = new vector<string>();
  string line;
  ifstream exists("users.txt");
  if (!exists.is_open())
    {
      ofstream outstream("users.txt");
      if (outstream.is_open())
	{
	  cout << "Creating new username file." << endl;
	  outstream << "sysadmin\n";
	  outstream.close();
	}
    }
  ifstream filestream("users.txt");
      while (getline(filestream, line))
	{
	  string s = line.substr(0,line.length()-1);
	  userlist->push_back(s);
	}
	filestream.close();
    
return userlist;

  
}

void writeUsers(vector<string>* users)
{
  ofstream filestream("users.txt");
  if (filestream.is_open())
    {
      for (vector<string>::iterator iter = users->begin(); iter != users->end(); iter++)
	{
	  filestream << *iter << "\n";
	}
      filestream.close();
    }
else 
   cout << "Could not open user file for writing." << endl;

}
