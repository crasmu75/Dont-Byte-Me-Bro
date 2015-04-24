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
#include <mutex>
#include <signal.h>

using namespace std;

void error(const char *msg)
{
	perror(msg);
	exit(1);
}

// Catches a SIGPIPE Exception if  the user's socket closes before the server can write to it.
void handler(int s)
{
	cout << "caught sigpipe exception " << endl;
}


// Provides Arguments for a thread that will remove spreadsheet sessions with no users.
typedef struct
{
	vector<spreadsheetSession::spreadsheetSession*> *spreadsheetSessions;
	mutex *sessionsLock;
} deleteArgs;

//	Provides a structure to hold the socket information of the client who has connected to a socket
typedef struct
{
	// All of the sockets currently connected to the server.
	vector<int> *sockets;
	// Client socket
	int socketFD;
	// Vector holding all of the active spreadsheet sessions.
	vector<spreadsheetSession::spreadsheetSession*> *spreadsheetSessions;
	// Vector of all registered users
	vector<string> *users;
	// Locks for the sockets, spreadsheet session and users vectors.
	mutex *socketsLock;
	mutex *sessionsLock;
	mutex *usersLock;
} socketArgs;

// This method will continually loop through the spreadsheet sessions and will remove any sessions with no active users.
void * updateSessions(void * args)
{
	// Contains the vector of active spreadsheet sessions and  the spreadsheet sessions vector lock.
	deleteArgs * dArgs = (deleteArgs *) args;
	while(true)
	{
		dArgs->sessionsLock->lock();
		for(vector<spreadsheetSession::spreadsheetSession*>::iterator it = dArgs->spreadsheetSessions->begin();it!= dArgs->spreadsheetSessions->end();it++)
		{
			// iIf the spreadsheet has an empty queue and no active users, take the spreadsheet session out of the vector.
			if((*it)->isClosed())
			{
				dArgs->spreadsheetSessions->erase(it);
				//delete (*it);
				break;
			}
		}
		dArgs->sessionsLock->unlock();
		usleep(100000);
	}
}

// Receives a connection from a user who just connected to a client. This socket waits until the user sends a 'connect' command.
// After receiving a connect command over the socket, this thread either adds the user to an existing spreadsheet session 
//  or creates a new one and adds the user.
void * receiveConnection(void * skts)
{
	int writeCount, readCount;
	// Contains all of the connected sockets, the socket of the connected user,  the vector of active spreadsheet sessions, the vector of valid users, and the appropriate mutex locks.
	socketArgs * clientSockets = (socketArgs *) skts;
	// Represent s the buffer to read and write on the socket.
	char buffer[256];
	// Socket of connected user.
	int socket = clientSockets->socketFD;
	// Create a new queue lock if a spreadsheet session is created.
	mutex *queueLock;
	// Message representing the incoming data.
	string incMsg = "";
	// Represents if the client has successfully connected.
	bool clientConnected = false;
	// While the client is not connected to a spreadsheet, continually wait for a 'connect' command from client.
	while(!clientConnected)
    {
		// Erase all data in the buffer.
		bzero(buffer, 256);
		//Read incoming message into the buffer
		size_t posEndLine = string::npos;
		incMsg = "";
		// Continually read on the socket until a newline is found.
		while(posEndLine == string::npos)
		{
			readCount = read(socket,buffer,255);
			// If no data is read from the socket, the socket has closed.
			if(readCount == 0)
			{
				cout << "read error in accepting connections." << endl;
				vector<int>::iterator it;
				// Find the socket that disconnected and remove it from the vector of client sockets.
				clientSockets->socketsLock->lock();
				it = find(clientSockets->sockets->begin(),clientSockets->sockets->end(), socket);
				close(socket);
				clientSockets->sockets->erase(it);
				clientSockets->socketsLock->unlock();
				// Stop listening for commands on this socket.
				return (void *) skts;
			}
			// Append the incoming data to a message and look for a newline.
			incMsg += buffer;
			posEndLine = incMsg.find('\n');
		}

		
		// Deal with each command in the received message.
		while(incMsg.find('\n') != string::npos)
		{
			// Parse the incoming message into commands using the newline character as a delimiter. 
			posEndLine = incMsg.find('\n');
			string msg = incMsg.substr(0,posEndLine);
			incMsg = incMsg.substr(posEndLine + 1);
			
			
			//Get the command from the incoming message.
			string commandStr = commandParser::parseCommand(msg);
			
			// If it was a connected command, check if the user is valid and add them to a spreadsheet session.
			if(commandStr.compare("connect") == 0)
			{
				// Parse client name out of command.
				string clientName = commandParser::parseClientName(msg);
				
				// Check valid client name.
				clientSockets->usersLock->lock();
				if (std::find(clientSockets->users->begin(), clientSockets->users->end(), clientName) == clientSockets->users->end())
				{
					cout << "Client is not registered on the server." << endl;
					// Build buffer to send out error message on failed connect.
					char buffer[256];
					bzero(buffer, 256);
					sprintf(buffer, "error 4 %s\n", clientName.c_str());
					writeCount = 0;
					// MAYBE ERROR?
					// Continually write to socket to ensure all data went out.
					while(writeCount < strlen(buffer))
					{
						writeCount += write(socket, buffer, strlen(buffer));
						string s = buffer;
						s = s.substr(writeCount);
						bzero(buffer,256);
						sprintf(buffer, s.c_str());
					} 
					clientSockets->usersLock->unlock();
					// Continue parsing commands from message.
					continue;
				}
				clientSockets->usersLock->unlock();
				
				// Get spreadsheet name from command.
				string spreadsheetName = commandParser::parseSpreadsheetName(msg);
				
				// Represents whether or not the spreadsheet session already exists.
				bool foundSheet = false;
				
				//Check if spreadsheet Session exist by iterating over the vector of current sessions
				clientSockets->sessionsLock->lock();
				for (vector<spreadsheetSession::spreadsheetSession*>::iterator it = clientSockets->spreadsheetSessions->begin(); it != clientSockets->spreadsheetSessions->end(); it++)
				{
					//If the spreadsheetName corresponds to an active session enqueue and add request for the user to join that session
					if(spreadsheetName.compare((*it)->spreadsheetSession::getspreadsheetName()) == 0)
					{
						queueLock = (*it)->getQueueLock();
						string addCommand = "add " + clientName;
						//Enqueue user add request
						workItem::workItem* addRequest = new workItem::workItem(socket,addCommand);
						queueLock->lock();
						(*it)->enqueue(addRequest);
						queueLock->unlock();
						// Tell thread that a sheet has been found and the client successfully connected to a spreadsheet.
						foundSheet = true;
						clientConnected = true;
					}
				}
				clientSockets->sessionsLock->unlock();
				
				//If the spreadsheet isn't found create a new session for the incoming user.
				if(!foundSheet)
				{
					cout << "creating new session....." << spreadsheetName << endl;
					spreadsheetSession::spreadsheetSession* session = new spreadsheetSession::spreadsheetSession(spreadsheetName, clientSockets->users, clientSockets->usersLock, clientSockets->spreadsheetSessions);
					string addCommand = "add " + clientName;
					//Enqueue user add request
					workItem::workItem* addRequest = new workItem::workItem(socket,addCommand);
					queueLock = session->getQueueLock();
					queueLock->lock();
					session->enqueue(addRequest);
					queueLock->unlock();
					// Add created spreadsheet session to vector of active sessions.
					clientSockets->sessionsLock->lock();
					clientSockets->spreadsheetSessions->push_back(session);
					clientSockets->sessionsLock->unlock();
					// Tell thread teh client has connected.
					clientConnected = true;
				}
			}
			// If  the command received wasn't a connect command, send the user an error.
			else
			{
				bzero(buffer, 256);
				sprintf(buffer, "error 2 Did not recieve a connect command from this user\n");
				cout << "got else" << endl;
				writeCount = write(socket,buffer,255);
			}
		}
	}
}

// Starting point for creating the server. This method will create a vector of active spreadsheet sessions, registered users,  and connected sockets.
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
	
	// Represents the length of  the client's socket.
	socklen_t clientLength;
	
	//This will make the server ignore broken pipe exceptions and continue executing code. The broken sockets should later be closed.
	signal(SIGPIPE,handler);
	
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
	
	// Zeros out server address and sets variables to accept the socket over the internet and from any IP address.
	bzero((char *) &serverAddr, sizeof(serverAddr));
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_addr.s_addr = INADDR_ANY;
	// Sets the server address to use the user's provided port.
	serverAddr.sin_port = htons(portno);
	
	// Binds the socket (referenced by the socket file descriptor) with the provided server address created above.
	if (::bind(socketFD, (struct sockaddr *) &serverAddr,
	sizeof(serverAddr)) < 0) 
	error("ERROR on binding");
	
	// Allows process to listen on socket for connections. 
	// First argument is file descriptor representing the socket to listen for connections over.
	listen(socketFD,5);
	
	// Represents a vector of all of the sockets currently connected to the server.
	vector<int> *allSockets = new vector<int>;
	// Represents arguments passed to the thread that is responsible for removing empty spreadsheet sessions.
	deleteArgs* dArgs = new deleteArgs();
	// Thread to remove empty spreadsheet sessions.
	pthread_t thread2;
	// Locks shared by the server and all sessions on the registered users, active sockets, and active spreadsheet sessions.
	mutex* usersLock = new mutex();
	mutex* socketsLock = new mutex();
	mutex* sessionsLock = new mutex();
	// Send spreadsheet sessions vector and corresponding lock  to the thread responsible for removing empty spreadsheet sessions.
	dArgs->spreadsheetSessions = spreadsheetSessions;
	dArgs->sessionsLock = sessionsLock;
	pthread_create(&thread2, 0, updateSessions, (void*) dArgs);
	pthread_detach(thread2);
	
	
	
	// Continually accepts client sockets
	while (1)
	{
		// Provides a structure for passing arguments to a method responsible for listening for connect commands from a connected client.
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
		
		//Populate the necessary structures for a new thread responsible for listening over connected socket.
		allSockets->push_back(newSocketFD);
		clientSockets->users = users;
		clientSockets->sockets = allSockets;
		clientSockets->socketFD = newSocketFD;
		clientSockets->spreadsheetSessions = spreadsheetSessions;
		clientSockets->usersLock = usersLock;
		clientSockets->socketsLock = socketsLock;
		clientSockets->sessionsLock = sessionsLock;
		
		//Spin a new thread for the incoming connection
		pthread_create(&thread, 0, receiveConnection, (void *) clientSockets);
		pthread_detach(thread);
	}
	
	// Close the socket that listens for user connections.
	close(socketFD);
	
	// Exit main gracefully.
	return 0;
}
