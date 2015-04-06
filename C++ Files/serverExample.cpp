#include <iostream>
#include <cstring>      // Needed for memset
#include <sys/socket.h> // Needed for the socket functions
#include <netdb.h>     //Needed for socket functions

//Tutorial from:
//http://codebase.eu/tutorial/linux-socket-programming-c/

int main()
{

  char sendbuf[512] = "Received";
 char recvbuf[512] = "";
 wchar_t wbuf[512] = L"";
 int size;
 int bytesRecv;
 int bytesSent;
 WSADATA wsaData;
 SOCKET listenSocket;
 SOCKET clientSocket;
 SOCKADDR_IN listenSocketAddress;
 SOCKADDR_IN clientSocketAddress;
  
  
  WSAStartup(MAKEWORD(2,0), &wsaData);

 listenSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
 listenSocketAddress.sin_family = AF_INET;
 listenSocketAddress.sin_port = 5555;
 listenSocketAddress.sin_addr.S_un.S_addr = INADDR_ANY;

  
  bind(listenSocket, (struct sockaddr*)&listenSocketAddress, sizeof (listenSocketAddress));
 listen(listenSocket, SOMAXCONN);
 size = sizeof(clientSocketAddress);
 clientSocket = accept(listenSocket, (struct sockaddr*)&clientSocketAddress, &size);


  

  bool stop = false;

  while(stop = false){
  
	 bytesRecv = recv( clientSocket, recvbuf, sizeof (recvbuf), 0 );
	 memset(wbuf, 0, sizeof (wbuf));
	 mbstowcs(wbuf, recvbuf, strlen(recvbuf));
	 OutputDebugString(wbuf);
	 bytesSent = send( clientSocket, sendbuf, strlen(sendbuf), 0 );
	 memset(wbuf, 0, sizeof (wbuf));
	 mbstowcs(wbuf, sendbuf, strlen(sendbuf));
	 OutputDebugString(wbuf);
  }
  
    std::cout<< "Message Sent... "<< std::endl;
  
 

  
}
