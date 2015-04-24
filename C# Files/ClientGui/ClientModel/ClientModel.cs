using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model
{
    public class ClientModel
    {
        /// <summary>
		/// The socket used to communicate with the server.  If no connection has been
		/// made yet, this is null.
        /// </summary>
		private Socket socket;

        /// <summary>
        /// Buffer to hold bytes to be sent to server
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// Sign-in credentials
        /// </summary>
        public string host, clientn, spreadsheet;

        /// <summary>
        /// Port number to connect to server
        /// </summary>
        public int portn;

        /// <summary>
        /// Boolean to listen when connected to server
        /// </summary>
        private bool listen;

        // Actions to be registered as commands are received from the server -----------------------------------------------

        /// <summary>
		/// Confirm successful connection
        /// </summary>
		public event Action<String> ConnectionConfirmationEvent;

        /// <summary>
        /// Update a cell
        /// </summary>
        public event Action<String> IncomingCellUpdateEvent;

        /// <summary>
        /// Error 0 -- Generic
        /// </summary>
        public event Action<String> IncomingGenericErrorEvent;

        /// <summary>
        /// Error 1 -- Invalid cell update
        /// </summary>
        public event Action<String> IncomingCellUpdateErrorEvent;

        /// <summary>
        /// Error 2 -- Invalid command
        /// </summary>
        public event Action<String> InvalidCommandEvent;

        /// <summary>
        /// Error 3 -- Invalid request in current state
        /// </summary>
        public event Action<String> InvalidStateErrorEvent;

        /// <summary>
        /// Error 4 -- Invalid username
        /// </summary>
        public event Action<String> IncomingUsernameErrorEvent;

        /// <summary>
        /// Alert of a lost connection
        /// </summary>
        public event Action<String> ConnectionLostErrorEvent;

        // Regex to identify incoming messages from Server -------------------------------------------------------

        /// <summary>
        /// Regex to identify incoming connected message
        /// </summary>
        Regex connectedCommand = new Regex(@"(connected)\s+[0-9]+");

        /// <summary>
        /// Regex to identify incoming cell update
        /// </summary>
        Regex cellUpdateCommand = new Regex(@"(cell)\s+[A-Z][0-9]+\s*(.)*");

        /// <summary>
        /// ERROR 0 -- Regex to identify incoming generic error message
        /// </summary>
        Regex genericErrorCommand = new Regex(@"(error)\s+0\s+(.)+");

        /// <summary>
        /// ERROR 1 -- Regex to identify incoming invalid cell change error message
        /// </summary>
        Regex invalidCellErrorCommand = new Regex(@"(error)\s+1\s+(.)+");

        /// <summary>
        /// ERROR 2 -- Regex to identify incoming invalid command
        /// </summary>
        Regex invalidCommandErrorCommand = new Regex(@"(error)\s+2\s+(.)+");

        /// <summary>
        /// ERROR 3 -- Regex to identify incoming invalid state error message
        /// </summary>
        Regex invalidStateErrorCommand = new Regex(@"(error)\s+3\s+(.)+");

        /// <summary>
        /// ERROR 4 -- Regex to identify incoming invalid username error message
        /// </summary>
        Regex invalidUserErrorCommand = new Regex(@"(error)\s+4\s+(.)+");

		/// <summary>
        /// Creates a not yet connected client model.
        /// </summary>
        public ClientModel()
        {
            socket = null;
        }

		/// <summary>
		/// Connect to the server at the given hostname and port and with the given spreadsheet 
		/// and user names.
		/// </summary>
		public void Connect(string hostname, int port, string clientName, string spreadsheetName)
		{
            // Check if socket is currently null
			if (socket == null)
			{
                // Try to connect to server via TcpClient
				try
				{
					TcpClient client = new TcpClient(hostname, port);
					socket = client.Client;
                    listen = true;

					host = hostname;
					portn = port;
					clientn = clientName;
					spreadsheet = spreadsheetName;
				
					// Send message to connect to the server
					try
					{
						SendMessage("connect " + clientName + " " + spreadsheetName + " \n");
					}
					catch(Exception e)
					{
						ServerLost();
					}

					// Start listening for messages back
					buffer = new byte[1024];
					try
					{
						socket.BeginReceive(buffer, 0, buffer.Length,
											SocketFlags.None, LineReceived, buffer);
					}
					catch(Exception e)
					{
						ServerLost();
					}
				}
				catch(Exception e)
				{
					ConnectionLostErrorEvent(e.Message);
				}
			}

			else
			{
                // Send connection message
				try
				{
					listen = true;

					host = hostname;
					portn = port;
					clientn = clientName;
					spreadsheet = spreadsheetName;

					SendMessage("connect " + clientName + " " + spreadsheetName + " \n");
				}
				catch (Exception e)
				{
					ServerLost();
				}
			}
		}

		/// <summary>
		/// Send a line of text to the server.
		/// </summary>
		/// <param name="line"></param>
		public void SendMessage(String line)
		{
            // Create ASCII encoded message
			byte[] msg = new byte[1024];
			msg = Encoding.ASCII.GetBytes(line);

            // If socket is not null, send the message
			if (socket != null)
			{
				try
				{
					socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, null, msg);
				}
				catch(Exception e)
				{
					ServerLost();
				}
			}
		}

		/// <summary>
		/// Deal with an arriving line of text.
		/// </summary>
		private void LineReceived(IAsyncResult result)
		{
            // Store incoming message as a string
			byte[] msg = (byte[])(result.AsyncState);
			String s = Encoding.ASCII.GetString(msg);
			int index;

			// process separate messages according to placement of \n characters
			while ((index = s.IndexOf('\n')) >= 0)
			{
				// take the string from beginning to where \n occurs
				String line = s.Substring(0, index);
				line = line.Trim();

                // Call proper event action based on Regex match
                if (cellUpdateCommand.IsMatch(line))
                    IncomingCellUpdateEvent(line);

                else if (invalidCellErrorCommand.IsMatch(line))
                    IncomingCellUpdateErrorEvent(line);

                else if (invalidUserErrorCommand.IsMatch(line))
                    IncomingUsernameErrorEvent(line);

                else if (connectedCommand.IsMatch(line))
                    ConnectionConfirmationEvent(line);

                else if (genericErrorCommand.IsMatch(line))
                    IncomingGenericErrorEvent(line);

                else if (invalidCommandErrorCommand.IsMatch(line))
                    InvalidCommandEvent(line);

                else if (invalidStateErrorCommand.IsMatch(line))
                    InvalidStateErrorEvent(line);

				// delete the completed message from what we received
				s = s.Substring(index + 1);
			}

			// Listen for more messages
			msg = new byte[1024];

			try
			{
                if(listen)
				    socket.BeginReceive(msg, 0, msg.Length,
					    SocketFlags.None, LineReceived, msg);
			}

			catch(Exception e)
			{
				ServerLost();
			}
		}

		/// <summary>
		/// Called when we can't send or receive to the server anymore.
		/// </summary>
		public void ServerLost()
		{
			// Close the socket
			socket.Close();

			ConnectionLostErrorEvent("Connection to the server lost. It is recommended that you restart\nthe application to restart the connection.");
            listen = false;
		}

        /// <summary>
        /// Closes socket when application closes
        /// </summary>
        public void Close()
        {
            // If server is still connected, shutdown and close
            if (listen)
            {
                listen = false;
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
	}
}
