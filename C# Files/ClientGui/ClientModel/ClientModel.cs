using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ClientModel
    {
		// The socket used to communicate with the server.  If no connection has been
		// made yet, this is null.
		private Socket socket;

		// Register for this event to be motified when a line of text arrives.
		public event Action<String> IncomingLineEvent;

		private byte[] buffer;

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
				TcpClient client = new TcpClient(hostname, port);
				socket = client.Client;

				// Send message to connect to the server
				SendMessage("connect " + clientName + " " + spreadsheetName);

				// Start listening for messages back
				buffer = new byte[1024];
				socket.BeginReceive(buffer, 0, buffer.Length,
									SocketFlags.None, LineReceived, buffer);
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
				socket.BeginSend(msg, 0,msg.Length,SocketFlags.None, null, msg);
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

				// Process this individual message using IncomingLineEvent
				if (IncomingLineEvent != null)
				{
					IncomingLineEvent(line);
				}

				// delete the completed message from what we received
				s = s.Substring(index + 1);
			}

			// Listen for more messages
			msg = new byte[1024];
			socket.BeginReceive(msg, 0, msg.Length,
				SocketFlags.None, LineReceived, msg);
		}
	}
}
