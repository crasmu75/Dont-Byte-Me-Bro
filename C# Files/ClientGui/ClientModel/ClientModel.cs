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
		private StringSocket socket;

		// Register for this event to be motified when a line of text arrives.
		public event Action<String> IncomingLineEvent;

		/// <summary>
        /// Creates a not yet connected client model.
        /// </summary>
        public ClientModel()
        {
            socket = null;
        }

		/// <summary>
		/// Connect to the server at the given hostname and port and with the give name.
		/// </summary>
		public void Connect(string hostname, int port)
		{
			if (socket == null)
			{
				TcpClient client = new TcpClient(hostname, port);
				socket = new StringSocket(client.Client, Encoding.ASCII);
				socket.BeginReceive(LineReceived, null);
			}
		}

		/// <summary>
		/// Send a line of text to the server.
		/// </summary>
		/// <param name="line"></param>
		public void SendMessage(String line)
		{
			if (socket != null)
			{
				socket.BeginSend(line + "\n", (e, p) => { }, null);
			}
		}

		/// <summary>
		/// Deal with an arriving line of text.
		/// </summary>
		private void LineReceived(String s, Exception e, object p)
		{
			if (IncomingLineEvent != null)
			{
				IncomingLineEvent(s);
			}
			socket.BeginReceive(LineReceived, null);
		}
	}
}
