# Team: Dont-Byte-Me-Bro (Camille Rasmussen, Jessie Delacenserie, Drew McClelland, Kameron Paulsen)

makefile commands:

make - makes the project into a an executable called 'server'.
./server - runs the server on default port number 2000.
./server (portnumber) - runs the server on the provided port number (ex: ./server 2114).

The only argument the server takes is the port number to run it off of.


********** The information below was used by us to keep track of what the team had done during that commit ************
4/23/15 -- final push
Fixed GUI bugs
Locks added
Accept all errors
Send all errors
Comments finished


4/22/15
Drew and Kameron

We fixed the opening a blank spreadsheet error
We added locking
We think that we receive all commands even with latency



STILL TO DO
*test locking and threading/blocking
*error checking
*free memory?
*test writing all data to sockets
*commenting
*readme for makefile
*submit code before checkoff


4/19/15
KILLER CREW KAMERON AND CAMILLE (Kam and Cam)

*IMPLEMENTED UNDO!!!
*GOT SINGLETON TO WORK --- Now you can close any of the forms and the others will stay open... and when 
	you close the last running one (doesn't matter which form) it will end the application.
*Client and server successfully close open sockets and/or connections when appropriate.

NEW FOUND ERRORS:
*cell doesn't display value in actual selected cell (when it's a formula)
*we do not know if the client side still needs dependency graph (when you change a cell that another depends on...)
*client still doesn't evaluate formulas
*server can't read an existing XML file that is empty

4/18/15
Relief team Kameron and Jessie worked in spreadsheetSession.cpp and server.cpp list of changes to follow
*Server now saves spreadsheet every 10 seconds
*Client can open saved spreadsheets with saved changes
*Finished dependency graph
*Looks like circular dependencies are working. Server checks and sends out circular exceptions


TODO:
Undo
Removing sockets and cleaning up stuff when stuff goes wrong and when it doesn't 
All user name stuff
Finish client corrections and changes
Make sure multithreading is bomber 
Client parser needs help


Added more methods to commandParser.cpp
4/17/15
Camille and Drew worked in spreadsheetSession.cpp and got the server to read xml files correctly (we used an example that contained 7 cells) and the server sent a "connected 7" command to the client successfully. It also sends each cell contents command to the client and the client updates them correctly. (The client still needs to be fixed to not split up the cell contents by spaces.) We then worked on the server in spreadsheetSession.cpp when the client sends a command to change a cell, the server parses this command but it still needs Jessie's dependency graph, so you could start by implementing the dependency graph and having the server (in spreadsheetSession.cpp, there is a comment for it) check for circular dependencies before accepting this change. Skipping this part, we have the server add the old cell contents (before the change) to the stack, and then send the cell change to each socket in the spreadsheet session.

We also made some changes in the server so you'll want to copy that file over as well. To be safe, check our commits in github to see all the C++ files that were changed that you'll want to copy over to Linux.

Camille needs to work on GUI. I guess Imma use Singleton.

TODO:
Undo
Add dependency graph to change cell in queue.
Implement autosaving
Remove user socket when user disconnects
Make GUI run multiple spreadsheets.
Evaluate cells in spreadsheet

reading/writing username file
Allower user to add someone to valid user list
Check for username validity

Lock Queue when adding removing
Lock Spreadsheet Session vector when adding/removing spreadsheet session.
Remove spreadsheet session when last user disconnects.
Deal with errors


