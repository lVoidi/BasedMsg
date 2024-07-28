using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server {

class Program {

// Main Method
static void Main(string[] args)
{
  if (args[0] == "-port"){
	  ExecuteServer(Int32.Parse(args[1]));
  }
  else if (args[0] == "-msg"){
    ExecuteClient(Int32.Parse(args[1]), args[2]);
  }
}

// ExecuteClient() Method
static void ExecuteClient(int port, string msg)
{
 
    try {
         
        // Establish the remote endpoint 
        // for the socket. This example 
        // uses port 11111 on the local 
        // computer.
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);
 
        // Creation TCP/IP Socket using 
        // Socket Class Constructor
        Socket sender = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
 
        try {
             
            // Connect Socket to the remote 
            // endpoint using method Connect()
            sender.Connect(localEndPoint);
 
            // We print EndPoint information 
            // that we are connected
            Console.WriteLine("Socket connected to -> {0} ",
                          sender.RemoteEndPoint.ToString());
 
            // Creation of message that
            // we will send to Server
            byte[] messageSent = Encoding.ASCII.GetBytes(msg + "<EOF>");
            int byteSent = sender.Send(messageSent);
 
            // Data buffer
            byte[] messageReceived = new byte[1024];
 
            // We receive the message using 
            // the method Receive(). This 
            // method returns number of bytes
            // received, that we'll use to 
            // convert them to string
            int byteRecv = sender.Receive(messageReceived);
            Console.WriteLine("Message from Server -> {0}", 
                  Encoding.ASCII.GetString(messageReceived, 
                                             0, byteRecv));
 
            // Close Socket using 
            // the method Close()
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
         
        // Manage of Socket's Exceptions
        catch (ArgumentNullException ane) {
             
            Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        }
         
        catch (SocketException se) {
             
            Console.WriteLine("SocketException : {0}", se.ToString());
        }
         
        catch (Exception e) {
            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        }
    }
     
    catch (Exception e) {
         
        Console.WriteLine(e.ToString());
    }
}
public static void ExecuteServer(int port)
{
	// Establish the local endpoint 
	// for the socket. Dns.GetHostName
	// returns the name of the host 
	// running the application.
	IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
	IPAddress ipAddr = ipHost.AddressList[0];
	IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

	// Creation TCP/IP Socket using 
	// Socket Class Constructor
	Socket listener = new Socket(ipAddr.AddressFamily,
				SocketType.Stream, ProtocolType.Tcp);

	try {
		
		// Using Bind() method we associate a
		// network address to the Server Socket
		// All client that will connect to this 
		// Server Socket must know this network
		// Address
		listener.Bind(localEndPoint);

		// Using Listen() method we create 
		// the Client list that will want
		// to connect to Server
		listener.Listen(10);

		while (true) {
			
			Console.WriteLine("Waiting connection ... ");

			// Suspend while waiting for
			// incoming connection Using 
			// Accept() method the server 
			// will accept connection of client
			Socket clientSocket = listener.Accept();

			// Data buffer
			byte[] bytes = new Byte[1024];
			string data = null;

			while (true) {

				int numByte = clientSocket.Receive(bytes);
				
				data += Encoding.ASCII.GetString(bytes,
										0, numByte);
											
				if (data.IndexOf("<EOF>") > -1)
					break;
			}

			Console.WriteLine("Text received -> {0} ", data);
			byte[] message = Encoding.ASCII.GetBytes("Test Server");

			// Send a message to Client 
			// using Send() method
			clientSocket.Send(message);

			// Close client Socket using the
			// Close() method. After closing,
			// we can use the closed Socket 
			// for a new Client Connection
			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
		}
	}
	
	catch (Exception e) {
		Console.WriteLine(e.ToString());
	}
}
}
}
