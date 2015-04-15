#include <stdio.h>
#include <iostream>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>
#include <pthread.h>
#include <unistd.h>
#include <vector>
#include "Parser/commandParser.h"
#include "spreadsheetSession/spreadsheetSession.h"
#include "spreadsheetSession/workItem.h"

using namespace std;

void error(const char *msg)
{
  perror(msg);
  exit(1);
}



typedef struct
{
  vector<int> *sockets;
  int socketFD;
} socketArgs;

void * receiveConnection(void * skts)
{
  pthread_t thread;
  int writeCount, readCount;
  socketArgs * clientSockets = (socketArgs *) skts;
  vector<spreadsheetSession::spreadsheetSession> *spreadsheetSessions;
  
  char buffer[256];
  char buffer2[256];
  int socket = clientSockets->socketFD;
  while(1)
    {
      bzero(buffer, 256);
      readCount = read(socket,buffer,255);
      cout<< buffer << endl;
      string commandStr = commandParser::parseCommand(buffer);
      if(commandStr.compare("connect"))
	{
	  string clientName = commandParser::parseClientName(buffer);
	  string spreadsheetName = commandParser::parseSpreadsheetName(buffer);
	  //Check if spreadsheet Session exist
	  for (vector<spreadsheetSession::spreadsheetSession>::iterator it = spreadsheetSessions->begin(); it != spreadsheetSessions->end(); it++)
	    {
	      if(spreadsheetName.compare(it->spreadsheetSession::getspreadsheetName()))
	      {
		string addCommand = "add " + clientName;
		workItem::workItem addRequest(socket,addCommand);
		it->enqueue(addRequest);
	      }
	   //Doesn't exist 
	      else
	      {
		//Construct ss session
		spreadsheetSession::spreadsheetSession session(spreadsheetName);
		string addCommand = "add " + clientName;
		workItem::workItem addRequest(socket,addCommand);
		it->enqueue(addRequest);
		spreadsheetSessions->push_back(session);
	      }
	    }
	  break;
	}
	







      
     bzero(buffer,256);
     bzero(buffer2,256);
     sprintf(buffer, "connected 7\n");
     sprintf(buffer2, "cell A1 dontfreakoutcamille\n");
     vector<int> allSockets;
     allSockets = *(clientSockets->sockets); 
     for (vector<int>::iterator it = allSockets.begin(); it != allSockets.end(); it++)
       {
	 writeCount = write((*it),buffer,strlen(buffer));
	 writeCount = write((*it),buffer2,strlen(buffer2));
       }
    }
}

int main(int argc, char *argv[])
{
  int socketFD, newSocketFD, portno;
  char buffer[256];
  struct sockaddr_in serverAddr,clientAddr;
  pthread_t thread;
  socklen_t clientLength;
  
  if(argc < 2)
    {
      cout << "Using default port 2000" << endl;
      portno = 2000;
    }
  else
    {
      portno = atoi(argv[1]);
    }

  socketFD = socket(AF_INET, SOCK_STREAM, 0);
  if(socketFD < 0)
    {
      error("Error opening socket");
    }
  
  bzero((char *) &serverAddr, sizeof(serverAddr));
  serverAddr.sin_family = AF_INET;
  serverAddr.sin_addr.s_addr = INADDR_ANY;
  serverAddr.sin_port = htons(portno);

  // Binds the socket (referenced by the socket file descriptor) with the provided server address.
  if (bind(socketFD, (struct sockaddr *) &serverAddr,
       sizeof(serverAddr)) < 0) 
       error("ERROR on binding");

  // Allows process to listen on socket for connections. 
  // First argument is file descriptor ref
  listen(socketFD,5);
  int numSockets = 0;
  vector<int> *allSockets = new vector<int>;
  while (1)
  {
     socketArgs* clientSockets = new socketArgs();
     // Gets the size of the client address.
     clientLength = sizeof(clientAddr);
     
     // Forces process to block until client connects to the server. When a connection is successfully established the process continues.
     // The accept call returns a file descriptor referencing a new socket connected to the client.
     // All further communication on this connection should use the new file descriptor.
     // The second argument is a reference to the address of the client.
     newSocketFD = accept(socketFD, 
                 (struct sockaddr *) &clientAddr, 
                 &clientLength);

     // Throws an error if the connection could not be established.
     if (newSocketFD < 0) 
          error("ERROR on accept");

     allSockets->push_back(newSocketFD);
     clientSockets->sockets = allSockets;
     clientSockets->socketFD = newSocketFD;
     pthread_create(&thread, 0, receiveConnection, (void *) clientSockets);
     pthread_detach(thread);

  }
  close(socketFD);



 
}
