#include <iostream>
#include <cstring> //Needed for memset
#include <sys/socket.h> //Needed for sockets
#include <netdb.h> //Needed for sockets
#include <string>

//Tutorial:
//http://codebase.eu/tutorial/linux-socket-programming-c/

int main(int argc, char* argv[])
{
	int status;
	//getaddrinfo() will use this structure
	struct addrinfo hostInfo;
	//Pointer to linked list of hostinfo
	struct addrinfo *hostInfoList;
	
	//Zero out memory
	memset(&hostInfo, 0, sizeof hostInfo);
	
	std::cout<< "Setting up structs..." << std::endl;
	
	//IP Specification. Can be both
	hostInfo.ai_family = AF_UNSPEC;
	//Use SOCK_STREAM for TCP
	hostInfo.ai_socktype = SOCK_STREAM;
	
	//hostInfoList structure now holds google's adress info
	status  = getaddrinfo("155.98.111.60", argv[1], &hostInfo, &hostInfoList);
	if(status !=0)
    {
		std::cout<< "getaddrinfo error" << std::endl;
	}
	
	
	//Connecting the socket
	std::cout<< "Creating socket" << std::endl;
	int socketfd;
	socketfd = socket(hostInfoList->ai_family, hostInfoList->ai_socktype, hostInfoList->ai_protocol);
	hostInfoList->ai_protocol;
	if(socketfd == -1)
    {
		std::cout<< "socket error" << std::endl;
	}
	
	std::cout << "Connecting.." << std::endl;
	status = connect(socketfd, hostInfoList->ai_addr, hostInfoList->ai_addrlen);
	if(status == -1)
    {
		std::cout<< "connection error" << std:: endl;
	}
	
	
	char *msg = "Hello Server\nconn";
	int length;
	ssize_t bytesSent;
	length = strlen(msg);
	
	
	bytesSent = send(socketfd,msg,length,0);
	
	
	
	
	if(bytesSent == length)
    {
		std::cout<< "Client Sent " << bytesSent << "bytes " << std::endl;
	}
	
	
	std::string s;
	std::cin >> s;
	
	
	
	char *secondmsg = "ect sysadmin alkjffdsaf\n";
	int secondlength;
	ssize_t secondbytesSent;
	secondlength = strlen(secondmsg);
	
	
	secondbytesSent = send(socketfd,secondmsg,secondlength,0);
	
	
	
	std::cout <<"Recieving data..." << std::endl;
	ssize_t bytesRecieved;
	char incomingDataBuffer[1000];
	bytesRecieved = recv(socketfd, incomingDataBuffer, 1000, 0);
	if(bytesRecieved == 0)
    {
		std::cout << " server host shut down." << std::endl;
	}
	if(bytesRecieved == -1)
    {
		std::cout<< " client recieve error" << std::endl;
	}
	
	
	std::cout<<incomingDataBuffer << std::endl;
	
	
	char *msg2 = "cell A1 asdf\ncell A11 teeehee sjldfjlds\ncell B";
	int length2;
	ssize_t bytesSent2;
	length2 = strlen(msg2);
	
	
	bytesSent2 = send(socketfd,msg2,length2,0);
	
	
	
	
	if(bytesSent2 == length)
    {
		std::cout<< "Client Sent " << bytesSent2 << "bytes " << std::endl;
	}
	
	char *msg3 = "3 21\n";
	int length3;
	ssize_t bytesSent3;
	length3 = strlen(msg3);
	
	
	bytesSent3 = send(socketfd,msg3,length3,0);
	
	
	
	
	if(bytesSent3 == length)
    {
		std::cout<< "Client Sent " << bytesSent3 << "bytes " << std::endl;
	}
	
	std::cout << "Receiving complete. Closing Socket" << std::endl;
	
	
	freeaddrinfo(hostInfoList);
	close(socketfd);
	
}
