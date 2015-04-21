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
#include<algorithm>
#include <vector>
#include "Parser/commandParser.h"
#include "Parser/xmlParser.h"
#include "spreadsheetSession/spreadsheetSession.h"
#include "spreadsheetSession/workItem.h"

using namespace std;

void error(const char *msg)
{
  perror(msg);
  exit(1);
}


/*
  Provides a structure to hold the socket information of the client who has connected to a socket
*/
typedef struct
{
  // All of the sockets currently connected to the server.
  vector<int> *sockets;
  int socketFD;
  vector<spreadsheetSession::spreadsheetSession*> *spreadsheetSessions;
  vector<string> *users;
} socketArgs;


// Receives a connection from a user who just connected to a client. This socket waits until the user sends a 'connect' command.
// After receiving a connect command over the socket, this thread either adds the user to an exisitng spreadsheet session 
//  or creates a new one and adds the user.
void * receiveConnection(void * skts)
{
  pthread_t thread;
  int writeCount, readCount;
  socketArgs * clientSockets = (socketArgs *) skts;
 
  char buffer[256];
  char buffer2[256];
  int socket = clientSockets->socketFD;
  while(1)
    {
      ///Buffer needs to be zero'd in c++
      bzero(buffer, 256);
      //Read incoming message into the buffer
      readCount = read(socket,buffer,255);
      if(readCount == 0)
	{
	  cout << clientSockets->sockets->size() << endl;
	  cout << "read error in accepting connections." << endl;
	  vector<int>::iterator it;
	  it = find(clientSockets->sockets->begin(),clientSockets->sockets->end(), socket);
	  close(socket);
	  clientSockets->sockets->erase(it);
	  cout << clientSockets->sockets->size() << endl;
	  return (void *) skts;
	}
      //Get the command from the incoming message buffer
      string commandStr = commandParser::parseCommand(buffer);
      if(commandStr.compare("register") == 0)
	{
	  string clientName = commandParser::parseClientName(buffer);
	  clientSockets->users->push_back(clientName);
	}
      if(commandStr.compare("connect") == 0)
	{
	  string clientName = commandParser::parseClientName(buffer);
	  
	  // Check valid client name.
	  if (std::find(clientSockets->users->begin(), clientSockets->users->end(), clientName) == clientSockets->users->end())
	    {
	      cout << "Client has not been added to server." << endl;
	      char buffer[256];
	      bzero(buffer, 256);
	      sprintf(buffer, "error 4 %s\n", clientName.c_str());
	      writeCount = write(socket, buffer, strlen(buffer));
	      continue;
	    }

	  // Get spreadsheet name from command.
	  string spreadsheetName = commandParser::parseSpreadsheetName(buffer);
	  bool foundSheet = false;
	  //Check if spreadsheet Session exist by iterating over the vector of current sessions
	  for (vector<spreadsheetSession::spreadsheetSession*>::iterator it = clientSockets->spreadsheetSessions->begin(); it != clientSockets->spreadsheetSessions->end(); it++)
	    {
	      //If the spreadsheetName corresponds to an active session enque and add request for the user to
	      //join that session
	      if(spreadsheetName.compare((*it)->spreadsheetSession::getspreadsheetName()) == 0)
	      {
		string addCommand = "add " + clientName;
		//Enqueue add request
		workItem::workItem* addRequest = new workItem::workItem(socket,addCommand);
		(*it)->enqueue(addRequest);
		foundSheet = true;
	      }
	    }
	  //If the spreadsheet isn't found create a new session for the incoming user.
	  if(!foundSheet)
	    {
	        cout << " creating new session....." << endl;
	      	spreadsheetSession::spreadsheetSession* session = new spreadsheetSession::spreadsheetSession(spreadsheetName, clientSockets->users);
		string addCommand = "add " + clientName;
		cout<< "addCommand: " << addCommand << endl;
		//Enqueue add request
		workItem::workItem* addRequest = new workItem::workItem(socket,addCommand);
		session->enqueue(addRequest);
		//Maybe lock this? what if two connection try to push onto the spreadsheetSessions?
		clientSockets->spreadsheetSessions->push_back(session);
	    }
	  break;
	}
    }

}

int main(int argc, char *argv[])
{
  // Provides a vector of spreadsheet session pointers representing the active spreadsheet sessions.
  vector<spreadsheetSession::spreadsheetSession*> *spreadsheetSessions = new vector<spreadsheetSession::spreadsheetSession*>();

  // Represents the Users that are registered on the server.
  vector<string> *users = getUsers();
  
  
  // Represents the socket and port number the server will listen over for connection requests.
  int socketFD, portno;

  // Represents the socket of a user who connects to the server.
  int newSocketFD;

  // Represents the server and client addresses of the socket connection.
  struct sockaddr_in serverAddr,clientAddr;

  // Provides a thread to listen for received messages from a connected user.
  pthread_t thread;

  socklen_t clientLength;
  
  // If the user doesn't provide a port, the server uses 2000 as default.
  if(argc < 2)
    {
      cout << "Using default port 2000" << endl;
      portno = 2000;
    }

  // Set the port number to the provided command line argument to receive sockets on. 
  else
    {
      portno = atoi(argv[1]);
    }

  // Opens a socket to begin accepting users.
  socketFD = socket(AF_INET, SOCK_STREAM, 0);
  if(socketFD < 0)
    {
      error("Error opening socket");
    }
  
  // Zeros out server address and sets variables to accept the socket over the internet and from any ip address.
  bzero((char *) &serverAddr, sizeof(serverAddr));
  serverAddr.sin_family = AF_INET;
  serverAddr.sin_addr.s_addr = INADDR_ANY;
  // Sets the server address to use the user's provided port.
  serverAddr.sin_port = htons(portno);

  // Binds the socket (referenced by the socket file descriptor) with the provided server address created above.
  if (bind(socketFD, (struct sockaddr *) &serverAddr,
       sizeof(serverAddr)) < 0) 
       error("ERROR on binding");

  // Allows process to listen on socket for connections. 
  // First argument is file descriptor representing the socket to listen for connections over.
  listen(socketFD,5);
 
  // Represents a vector of all of the sockets currently connected to the server.
  vector<int> *allSockets = new vector<int>;

  // Continually accepts client sockets
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

     //Populate the necessary structures for the new thread
     allSockets->push_back(newSocketFD);
     clientSockets->users = users;
     clientSockets->sockets = allSockets;
     clientSockets->socketFD = newSocketFD;
     clientSockets->spreadsheetSessions = spreadsheetSessions;
     //Spin a new thread for the incoming connection
     pthread_create(&thread, 0, receiveConnection, (void *) clientSockets);
     pthread_detach(thread);
  }
  close(socketFD);
}
