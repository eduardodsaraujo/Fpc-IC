using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using SimpleJSON;

public class TCPCopia2 : MonoBehaviour
{
	//String Host = "192.168.0.100";
	String Host = "127.0.0.1";
	public Int32 Port = 50025;
	public static bool teste = false;
	public float reloadTime = 2;
	public int numEnemies = 1;
	public static String spacing2;
	public float spacing;
	public float fireRate;
	public int enemiesAlive;
	public float shotsReceived;
	public float shots;
	public int roundNum;
	public int time;
	public string timeString;
	public float life;
	public int twoMessages = 0;
	public string twoStrings = "";
	
	public static String received = "";
	public static bool receivedServer;
	public bool beginTcp;
	private static Thread tcpThread;
	private static Thread connectThread;
	private IPHostEntry ipHostInfo;
	private IPAddress ipAddress;
	private IPEndPoint remoteEP;
	private Socket sender;
	public bool birdDead;
	byte[] bytes;
	bool gameOver;
	bool gameOverGameManager;
	bool stopThread;
	bool tryConnect;
	
	
	void Start()
	{
		beginTcp = false;
		teste = false;
		receivedServer = false;
		gameOver = false;
		bytes = new byte[1024];
		//    countColision = GetComponent<BirdMovement>().countColision;
		
		StartCoroutine(connectSocket());
		reloadTime = GetComponent<GameManager>().reloadTime;
		//setupSocket ();
		//StartCoroutine (changeTeste ());
		
	}
	void Update()
	{
		
		gameOverGameManager = GetComponent<GameManager> ().gameOver;
		if (!sender.Connected)
		{	
			if (tryConnect == false) {
				tryConnect = true;

				StartCoroutine(connectSocket());
			}
		}
		
		reloadTime = GetComponent<GameManager>().reloadTime;
		numEnemies = GetComponent<GameManager>().numEnemies;
		fireRate = GetComponent<GameManager>().fireRate;
		enemiesAlive = GetComponent<GameManager> ().enemiesAlive.Length;
		shotsReceived = GetComponent<GameManager> ().shotsReceveid;
		roundNum = GetComponent<GameManager> ().numRounds;
		time  = GetComponent<GameManager> ().totalTime.Minute*60 + GetComponent<GameManager> ().totalTime.Second ;
		timeString  = ""+GetComponent<GameManager> ().totalTime.Minute+":" + GetComponent<GameManager> ().totalTime.Second ;
		shots  = GetComponent<GameManager> ().shots;
		life  = GetComponent<GameManager> ().life;
		
		
		if (GetComponent<GameManager>().comecarTransmissao == true)
		{
			print ("startou client");
			
			StartCoroutine(StartClient());
			
			print(received);
			if (received != "")
			{
				var receivedJson = JSONNode.Parse(received);
				if (receivedJson != null)
				{
					GetComponent<GameManager>().reloadTime = float.Parse(receivedJson["reloadTime"]);
					GetComponent<GameManager>().numEnemies = int.Parse(receivedJson["numEnemies"]);
					GetComponent<GameManager>().fireRate = float.Parse(receivedJson["fireRate"]);
				}
			}
		}
		
	}
	
	
	IEnumerator connectSocket()
	{
		connectThread = new Thread (o => {

						try {
								// Establish the remote endpoint for the socket.
								// This example uses port 11000 on the local computer.

								//sender.Bind(remoteEP); 

								// start listening
								//sender.Listen(1);

								ipHostInfo = Dns.Resolve (Dns.GetHostName ());
								//IPAddress ipAddress = ipHostInfo.AddressList[0];
								ipAddress = System.Net.IPAddress.Parse (Host);
								remoteEP = new IPEndPoint (ipAddress, Port);

								// Create a TCP/IP  socket.
								//sender = new Socket (AddressFamily.InterNetwork, 
								//		SocketType.Stream, ProtocolType.Tcp);

								sender = new Socket (AddressFamily.InterNetwork,
	                    SocketType.Stream, ProtocolType.Tcp);
								sender.Connect (remoteEP);


						} catch (Exception e) {
								Debug.Log (e.ToString ());
						}
				});
		connectThread.Start();
		yield return new WaitForSeconds (2);
		tryConnect = false;
		
	}
	// **********************************************
	IEnumerator StartClient()
	{
		//if(teste == false){
		// Data buffer for incoming data.
		if (receivedServer == false)
		{
			if(stopThread == false){
				receivedServer = true;
				tcpThread = new Thread(o => {
					try
					{
						
						print("Socket connected to" +
						      sender.RemoteEndPoint.ToString());
						byte[] msg;
						int bytesRec;
						
						if (gameOverGameManager == true)
						{
							gameOver = true;
							msg = Encoding.ASCII.GetBytes("Game Over");
							int bytesSent = sender.Send(msg);
							stopThread = true;

							try {
								sender.Disconnect(false);

								sender.Shutdown (SocketShutdown.Both);
														sender.Close ();

							} catch (SocketException se) {
															Debug.Log ("SocketException : " + se.ToString ());
														}
							
						}
						else
						{
							//msg = Encoding.ASCII.GetBytes ("{ countColision: " + countColision + "}");
//							msg = Encoding.ASCII.GetBytes("{EnemiesAlive: " + enemiesAlive + ", Shots:" + shots +
//							                              ", ShotsReceived: " + shotsReceived + ", life: " +  life + 
//							                              ", ReloadTime: " + reloadTime.ToString() + ", fireRate: " + fireRate.ToString()+
//							                              ", RoundNum: " + roundNum + ", Time: " + time + "}") ;
//							msg = Encoding.ASCII.GetBytes(enemiesAlive + "-EnemiesAlive:" + shots + "-Shots:" +
//							                              shotsReceived + "-ShotsReceived:" + life  + "-Life:" + 
//							                              reloadTime.ToString() + "-ReloadTime:" + fireRate.ToString() + "-FireRate:" +
//							                              roundNum + "-RoundNum:" + time + "-Time:") ;
							msg = Encoding.ASCII.GetBytes("EnemiesAlive:" + enemiesAlive + "-Shots:" + shots +
							                              "-ShotsReceived:" + shotsReceived + "-Life:" +  life + 
							                              "-ReloadTime:" + reloadTime.ToString() + "-FireRate:" + fireRate.ToString()+
							                              "-RoundNum:" + roundNum + "-Time:" + time+ "@" ) ;

//							twoMessages++;
//							twoStrings  += "EnemiesAlive:" + enemiesAlive + "-Shots:" + shots +
//			                              "-ShotsReceived:" + shotsReceived + "-Life:" +  life + 
//		                              	  "-ReloadTime:" + reloadTime.ToString() + "-FireRate:" + fireRate.ToString()+
//									"-RoundNum:" + roundNum + "-Time:" + time+ "@";
//							if(twoMessages > 1)
//							{
								//int bytesSent = sender.Send(msg);
								//msg = Encoding.ASCII.GetBytes(twoStrings);
							int bytesSent = sender.Send(msg);
							//	twoMessages = 0;
							//	twoStrings = "";
							//}
							bytesRec = sender.Receive(bytes);
							received = Encoding.ASCII.GetString(bytes, 0, bytesRec);

						}
					}
					catch (ArgumentNullException ane)
					{
						Debug.Log("ArgumentNullException :" + ane.ToString());
					}
					catch (SocketException se)
					{
						Debug.Log("SocketException : " + se.ToString());
					}
					catch (Exception e)
					{
						Debug.Log("Unexpected exception :" + e.ToString());
					}
				});
				
				tcpThread.Start();
				yield return new WaitForSeconds(1);
				receivedServer = false;
			}
		}
		
		//}
	}
	
	
	
	void OnDestroy()
	{

	}
	
} // end class TCP
