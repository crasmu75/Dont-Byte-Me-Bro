using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientModel
{
    public class ClientModel
    {
		// The socket used to communicate with the server.  If no connection has been
		// made yet, this is null.
		private Socket socket;

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
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
				byte[] bytes = Encoding.UTF8.GetBytes(line + "\n");
				socket.BeginSend(bytes, 0, bytes.Length, 0, (e) => { }, null);
			}
		}

		/// <summary>
		/// Deal with an arriving line of text.
		/// </summary>
		private void LineReceived(byte[] bytes, Exception e, object p)
		{
			if (IncomingLineEvent != null)
			{
				string s = System.Text.Encoding.UTF8.GetString(bytes);
				IncomingLineEvent(s);
			}
			socket.BeginReceive(LineReceived, null);
		}  
    }
}
