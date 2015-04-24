#include"xmlParser.h"
#include<string>
#include<iostream>
#include<map>
#include<fstream>
#include<boost/algorithm/string.hpp>


using namespace std;

int main (int argc, char* argv[])
{
  /*
  if (argc < 3)
    {
      cout << "you must provide at least two file names." << endl;
      return 0;
    }


  // Populate a map from cells in the first input filename xml
  string filename = argv[1];
  map<string, string> cell_map = getCells(filename);
 

  cout << "Input Sheet Read: " << endl;
    for (map<string, string>::iterator iter = cell_map.begin(); iter != cell_map.end(); iter++)
    {
      cout << iter->first << "\t" << iter->second << endl;

    }

    // Add extra cell to map.
    cell_map["Z12"] = "HELLO THERE";

  */

    // Close file.
  vector<string>* vec = getUsers();
  /*
  string msg = "register balsdfjks 1weirfow\n";

  std::size_t posFirstWord = msg.find(" ") + 1;
  std::string s = msg.substr(posFirstWord);
  boost::algorithm::trim(s);
  cout << s << endl;
  */
    /*
    // Populate a map from cells in the file created by the previous write.
    string filename2 = argv[2];
    map<string, string> cell_map2 = getCells(filename2);
    


      cout << endl << endl << "Generated Sheet Read: " << endl;
   for (map<string, string>::iterator iters = cell_map2.begin(); iters != cell_map2.end(); iters++)
    {
   
      cout << iters->first << "\t" << iters->second << endl;

    }
    */
    
  return 0;
}
