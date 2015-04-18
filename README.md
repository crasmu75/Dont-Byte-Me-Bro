We are Number 1 Team
# Dont-Byte-Me-Bro

4/17/15
Camille and Drew worked in spreadsheetSession.cpp and got the server to read xml files correctly (we used an example that contained 7 cells) and the server sent a "connected 7" command to the client successfully. It also sends each cell contents command to the client and the client updates them correctly. (The client still needs to be fixed to not split up the cell contents by spaces.) We then worked on the server in spreadsheetSession.cpp when the client sends a command to change a cell, the server parses this command but it still needs Jessie's dependency graph, so you could start by implementing the dependency graph and having the server (in spreadsheetSession.cpp, there is a comment for it) check for circular dependencies before accepting this change. Skipping this part, we have the server add the old cell contents (before the change) to the stack, and then send the cell change to each socket in the spreadsheet session.

We also made some changes in the server so you'll want to copy that file over as well. To be safe, check our commits in github to see all the C++ files that were changed that you'll want to copy over to Linux