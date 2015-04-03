#include <iostream>
#include <cstring>      // Needed for memset
#include <sys/socket.h> // Needed for the socket functions
#include <netdb.h>     //Needed for socket functions

//Tutorial from:
//http://codebase.eu/tutorial/linux-socket-programming-c/

int main()
{

  int status;
  struct addrinfo hostInfo;
  struct addrinfo *hostInfoList;
  

  memset(&hostInfo, 0, sizeof hostInfo);
  
  std::cout << "Setting up structures" << std::endl;
  hostInfo.ai_family = AF_UNSPEC;
  hostInfo.ai_socktype = SOCK_STREAM;
  hostInfo.ai_flags = AI_PASSIVE;
  status = getaddrinfo(NULL, "5555", &hostInfo, &hostInfoList);
  

  std::cout << "Creating socket...." << std::endl;
  int socketfd;
  socketfd = socket(hostInfoList->ai_family, hostInfoList->ai_socktype, hostInfoList->ai_protocol);
  if(socketfd == -1)
    {
      std::cout<< "error creating socket" << std::endl;
    }

  
  std::cout << "Binding Socket..." << std::endl;
  int yes = 1;
  status = setsockopt(socketfd, SOL_SOCKET,SO_REUSEADDR, &yes, sizeof(int) == -1);
  status = bind(socketfd, hostInfoList->ai_addr, hostInfoList->ai_addrlen);
  if(status == -1)
    {
      std::cout << "bind error " << std::endl;
      freeaddrinfo(hostInfoList);
      close(socketfd);
    }
  

  std::cout<< "Listening for connections..." << std::endl;
  status = listen(socketfd, 1);
  if(status == -1)
    {
      std::cout << "listen error" << std::endl;
    }

  int newSocket;
  struct sockaddr_storage clientAddr;
  socklen_t addrSize = sizeof(clientAddr);
  newSocket = accept(socketfd, (struct sockaddr *)&clientAddr, &addrSize);
  if(newSocket == -1)
  {
    std::cout << "listen error" << std::endl;
  }
  else
    {
      std::cout<< "Connection accepted. Using the new socket"<< std::endl;
    }

  std::cout << "Waiting to recieve data...." << std::endl;
  ssize_t bytesRecieved;
  char incommingDataBuffer[1000];
  bytesRecieved = recv(newSocket, incommingDataBuffer, 1000, 0);
  if(bytesRecieved == 0)
    {
      std::cout << "Client host shut down." << std::endl;
    }
  if(bytesRecieved == -1)
    {
      std::cout<< "server recieve error" << std::endl;
    }
  

  std::cout << bytesRecieved <<" sever bytes recieved: "<<  std::endl;
  incommingDataBuffer[bytesRecieved] = '\0';
  std::cout << incommingDataBuffer << std::endl;

    std::cout<< "Sending back a message...." << std::endl;
    char *msg = "thanks";
    int length;
    ssize_t bytesSent;
    length = strlen(msg);
    bytesSent = send(newSocket, msg, length, 0);
    std::cout<< "Stopping Server... "<< std::endl;
    freeaddrinfo(hostInfoList);
    close(newSocket);
    close(socketfd);
 

  
}
