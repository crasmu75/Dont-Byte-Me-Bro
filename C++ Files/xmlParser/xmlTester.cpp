#include"xmlParser.h"
#include<string>
#include<iostream>
#include<map>
#include<fstream>


using namespace std;

int main (int argc, char* argv[])
{
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

    // Write xml file from map to second filename parameter
    ofstream output(argv[2]);
    writeFile(cell_map, output);
    // Close file.
    output.close();

   

    // Populate a map from cells in the file created by the previous write.
    string filename2 = argv[2];
    map<string, string> cell_map2 = getCells(filename2);
    


      cout << endl << endl << "Generated Sheet Read: " << endl;
   for (map<string, string>::iterator iters = cell_map2.begin(); iters != cell_map2.end(); iters++)
    {
   
      cout << iters->first << "\t" << iters->second << endl;

    }
    
  return 0;
}
