﻿using CustomNetworking;
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
		// The socket used to communicate with the server.  If no connection has been
		// made yet, this is null.
		private Socket socket;

		// Register for this event to be motified when a line of text arrives.
		public event Action<String> ConnectionConfirmationEvent;

        // Register event for an incoming cell update
        public event Action<String> IncomingCellUpdateEvent;

        // Register event for an incoming cell update error message
        public event Action<String> IncomingCellUpdateErrorEvent;

        // Register event for an incoming username error message
        public event Action<String> IncomingUsernameErrorEvent;

        public event Action<String> IncomingErrorEvent;

		public event Action<String> testingevent;

		private byte[] buffer;

		public string host, clientn, spreadsheet;
		public int portn;

        private bool listen;


        /// <summary>
        /// Regex to identify incoming connected message
        /// </summary>
        Regex connectedCommand = new Regex(@"(connected)\s+[0-9]+");

        /// <summary>
        /// Regex to identify incoming cell update
        /// </summary>
        Regex cellUpdateCommand = new Regex(@"(cell)\s+[A-Z][0-9]+\s+(.)+");

        /// <summary>
        /// Regex to identify incoming invalid cell change error message
        /// </summary>
        Regex invalidCellErrorCommand = new Regex(@"(error)\s+1\s+(.)+");

        /// <summary>
        /// Regex to identify incoming invalid username error message
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
			if (socket == null)
			{
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
					IncomingErrorEvent(e.Message);
				}
			}
		}

		/// <summary>
		/// Send a line of text to the server.
		/// </summary>
		/// <param name="line"></param>
		public void SendMessage(String line)
		{
			byte[] msg = new byte[1024];
			msg = Encoding.ASCII.GetBytes(line);
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
			byte[] msg = (byte[])(result.AsyncState);
			String s = Encoding.ASCII.GetString(msg);
			int index;

			// process separate messages according to placement of \n characters
			while ((index = s.IndexOf('\n')) >= 0)
			{
				// take the string from beginning to where \n occurs
				String line = s.Substring(0, index);
				line = line.Trim();

				testingevent(line);

                // Call proper event action based on Regex match
                if (cellUpdateCommand.IsMatch(line))
                    IncomingCellUpdateEvent(line);

                else if (invalidCellErrorCommand.IsMatch(line))
                    IncomingCellUpdateErrorEvent(line);

                else if (invalidUserErrorCommand.IsMatch(line))
                    IncomingUsernameErrorEvent(line);

                else if (connectedCommand.IsMatch(line))
                    ConnectionConfirmationEvent(line);

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
			// shutdown and close the socket
			//socket.Shutdown(SocketShutdown.Both);
			socket.Close();

			IncomingErrorEvent("Connection to the server lost. It is recommended that you restart\nthe application to restart the connection.");
            listen = false;
		}

        public void Close()
        {
            if (listen)
            {
                listen = false;
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
	}
}
