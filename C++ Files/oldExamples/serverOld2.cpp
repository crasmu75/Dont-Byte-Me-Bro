/* A simple server in the internet domain using TCP
   The port number is passed as an argument 
   Code taken from:
   www.linuxhowtos.org/c_c++/socket.htm

*/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>

void error(const char *msg)
{
    perror(msg);
    exit(1);
}


struct socketArgs
{
  int sockets[2];
};

int main(int argc, char *argv[])
{
  // Socket file descriptors and port number.
     int socketFD, newSocketFD, portno;
     // Size of the address of the client.
     socklen_t clientLength;
     // Buffer to hold characters coming into socket connection.
     char buffer[256];
     // Structure containing an internet address.
     struct sockaddr_in serverAddr, clientAddr;
     // Used to hold number of bytes to be read or written.
     int readCount, writeCount;

     struct socketArgs;

     // If a port is not provided, set it to 2000 (default).
     if (argc < 2)
       {
	 printf("Using default port 2000");
	 portno = 2000;
       }

     // Set port to provided command line argument.
     else
       portno = atoi(argv[1]);
     
     // Create new socket with Internet domain, Stream type socket, and default protocol (TCP for Stream).
     // Returns an entry into the file descriptor table, used for subsequent references to this socket.
     socketFD = socket(AF_INET, SOCK_STREAM, 0);
     if (socketFD < 0) 
        error("ERROR opening socket");

     // Sets all values in server address buffer to zero.
     bzero((char *) &serverAddr, sizeof(serverAddr));

     // Sets code family of server address to be Internet domain.
     serverAddr.sin_family = AF_INET;
     // Sets the IP address of the host.
     // INADDR_ANY gets the IP address of the machine running the server.
     serverAddr.sin_addr.s_addr = INADDR_ANY;
     // Sets the port number of the server to be the provided port.
     serverAddr.sin_port = htons(portno);

     // Binds the socket (referenced by the socket file descriptor) with the provided server address.
     if (bind(socketFD, (struct sockaddr *) &serverAddr,
              sizeof(serverAddr)) < 0) 
              error("ERROR on binding");

     // Allows process to listen on socket for connections. 
     // First argument is file descriptor referencing socket to listen on.
     // Second argument is the size of the backlog queue for number of connections that can be waiting 
     //    while process is handling a particular conneciton.
     listen(socketFD,5);

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

     // Initializes all bytes in the incoming buffer to be zero.
     bzero(buffer,256);

     // Reads bytes from the socket using the new file descriptor. This will block until there is something to read from the socket.
     // This will read the total number of characters send, or 255 (whichever is less) and place them in the buffer.
     // The read method returns the number of characters read.
     readCount = read(newSocketFD,buffer,255);

     // Throw an error if the number of characters read is less than 0.
     if (readCount < 0) error("ERROR reading from socket");

     // Prints the message received.
     printf("Here is the message: %s\n",buffer);


     // Sends a message back to the client using the new file descriptor. 
     // The first parameter is the new file descriptor.
     // The second parameter is the message to be sent
     // The third parameter is the number of characters in the message.
     // The write method returns the number of bytes successfully written.
     writeCount = write(newSocketFD,"I got your message",18);
     if (writeCount < 0) error("ERROR writing to socket");

     // Close socket connections.
     close(newSocketFD);
     close(socketFD);

     // Exit program normally.
     return 0; 
}
