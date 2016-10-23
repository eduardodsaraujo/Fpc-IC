using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System;
using System.IO;

public class GameManager : MonoBehaviour {
	
	
	public GameObject barReloadTime;
	public GameManagerJs jsScript; 
	public GameObject[] enemies;
	public GameObject weapon;
	public GameObject mainCamera;
	public GameObject[] numCubes;
	private static Thread modifyThread;
	public int numEnemies = 3;
	public float hitPoints = 100;
	public float reloadTime = 2;
	public float fireRate = 2;
	public float health;
	public float shotsReceveid;
	public float shots;
	public float life;
	public bool contarTempo;
	public bool comecarTransmissao= false;
	public bool gameOver = false;
	public bool fillBarReloadBool = false;
	public bool timeToReload = false;
	public GameObject spawner;
	public GameObject numDeads;
	public GameObject values;
	public GameObject rounds;
	public int numRounds = 0;
	
	public DateTime dateTime;
	public DateTime totalTime;
	public GameObject time;
	bool inicioContagemTime = false;
	public GameObject[] enemiesAlive;
	
	Thread logThread;
	StreamWriter w1;
	bool canLog = false;
	
	public GameObject player;
	
	void Awake()
	{
		reloadTime = 2;
		//Get the JavaScript component
		jsScript = this.GetComponent<GameManagerJs>();//Don't forget to place the 'JS1' file inside the 'Standard Assets' folder
	}
	
	void OnLevelWasLoad(){
		///if (w1 == null)
		//	w1 = new StreamWriter("log1.txt",true);
	}
	// Use this for initialization
	void Start () {
		weapon = jsScript.weapon;
		numCubes = jsScript.numEnemies;
		barReloadTime.SetActive (false);
		
		dateTime = new DateTime (1,1,1,0,0,0);
		totalTime = new DateTime (1,1,1,0,0,0);
		time.GetComponent<Text> ().text = "";
		//w1 = new StreamWriter("log1.txt",true);
		
		//time.GetComponent<Text>().text = String.Format ("{0:mm:ss}", dateTime);
		
	}
	
	void Update () {
		
		StartCoroutine (fillBarReload ());
		//print ("TotalTime: " + totalTime.Minute + ":" + totalTime.Second);
		if (contarTempo == true) {
			//contarTempo = false;
			numDeads.SetActive(true);
			time.SetActive(true);
			spawner.GetComponent<Spawner> ().startNewWave= true;
			//	w1.WriteLine ("-------------------------------------------------------------------------------------------");
			//	LogInicial("Begin");
		}
		
		int num = jsScript.numDeads;
		numDeads.GetComponent<Text>().text = num.ToString ();
		
		if (numRounds >= 11) {
			player.GetComponent<PlayerHealth>().health= 0;
			player.GetComponent<PlayerHealth>().ApplyDamage(500);
		}
		enemies = jsScript.enemies;
		if (player.GetComponent<PlayerHealth>().health <= 0) {
			if(gameOver==false){
				//		LogInicial("GameOver");
				////		w1.Close();
				gameOver= true;
				time.SetActive(false);
				rounds.SetActive(false);
				player.GetComponent<PlayerHealth>().health =1;
			}		
		}
		foreach (GameObject numCube in numCubes) {
			numCube.GetComponent<CubeSet>().numEnemies = numEnemies;
		}
		if (spawner.GetComponent<Spawner> ().startNewWave == true) {
			StartCoroutine(startNewWave());
		};
		
		//		if (enemys != null) {
		//				foreach (GameObject enemy in enemys) {
		//					enemy.GetComponent <EnemyDamageReceiver> ().hitPoints = hitPoints;
		//				}
		//		}
		enemiesAlive = GameObject.FindGameObjectsWithTag ("Enemy");
		
		weapon.GetComponent<GunScript>().reloadTime = reloadTime;
		weapon.GetComponent<GunScript>().emptyReloadTime = reloadTime;
		weapon.GetComponent<GunScript>().fireRate = fireRate;
		shotsReceveid = player.GetComponent<PlayerHealth> ().tiros;
		shots = weapon.GetComponent<GunScript> ().tirosDados;
		life = player.GetComponent<PlayerHealth> ().health;
		if (weapon.GetComponent<GunScript>().timeToReload == true){
			timeToReload = true;
		}

		foreach (GameObject numCube in numCubes) {
			numCube.GetComponent<CubeSet>().numEnemies = numEnemies;
		}
		
		values.GetComponent<Text> ().text = "Reload Time: " + reloadTime + "\nNumEnemies: " + numEnemies +
			"\nFireRate: " + fireRate;
		
		//StartCoroutine (mudarHealth ());
		
	}
	IEnumerator iniciarContagem(){
		if (inicioContagemTime == false) {
			inicioContagemTime = true;
			contarTempo = false;
			comecarTransmissao = true;
			dateTime = new DateTime (1,1,1,0,0,30);
			print ("Date time: "+ dateTime.Second);
			while (dateTime.Second > 0) {
				//	Log ("");
				totalTime = totalTime.AddSeconds (1);
				dateTime = dateTime.AddSeconds (-1);
				yield return new WaitForSeconds (1);
				print(dateTime.Second);
				if(gameOver != true)
					time.GetComponent<Text> ().text = String.Format ("{0:mm:ss}", dateTime);
			}
			inicioContagemTime = false;
			spawner.GetComponent<Spawner> ().startNewWave = true;
		}
	}
	IEnumerator fillBarReload(){
		if (timeToReload == true) {
			if (fillBarReloadBool == false) {
				StartCoroutine(stopBar());
				fillBarReloadBool = true;
				int i = 0;
				barReloadTime.SetActive (true);
				barReloadTime.GetComponent<Image> ().fillAmount = 0;
				
				while (barReloadTime.GetComponent<Image>().fillAmount<1f) {
					barReloadTime.GetComponent<Image> ().fillAmount += 0.018f;
					yield return new WaitForSeconds (reloadTime / 100);
					print (reloadTime/300);
				}
			}
		}
		
	}
	
	IEnumerator stopBar(){
		yield return new WaitForSeconds (reloadTime);
		barReloadTime.SetActive (false);
		fillBarReloadBool = false;			
		timeToReload = false;
		
	}
	
	IEnumerator startNewWave(){
		StartCoroutine(iniciarContagem());
		rounds.GetComponent<Text> ().text = "Round " + ++numRounds;
		float alpha = 0f;
		while (alpha < 1.5f) {
			rounds.GetComponent<Text> ().color = new Color (1, 0, 0, alpha);
			alpha = alpha + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		while (alpha > 0) {
			rounds.GetComponent<Text> ().color = new Color (1, 0, 0, alpha);
			alpha = alpha - 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}


	public void LogInicial(string logMessage)
	{
		StartCoroutine (AppendLog(logMessage));
		//		while (gameOver != true) {
		//
		//				Log ("");
		//
		//			}
		//
		//		}
		
	}
	public void Log(string logMessage)
	{
		if (canLog == true) {
			canLog = false;
			
			string temp = "EnemiesRound: " + numEnemies + "\tEnemiesAlive: " + 
				enemiesAlive.Length + "\tReloadTime: " + reloadTime + "\tFireRate: " 
					+ fireRate + "\tShots:; " + weapon.GetComponent<GunScript> ().tirosDados + 
					"\tShotsReceived: " + player.GetComponent<PlayerHealth> ().tiros;
			temp += "\tRoundNum: " + numRounds.ToString () + "\tTime: 00:" + dateTime.Second.ToString ();
			StartCoroutine (AppendLog (logMessage + temp));
			canLog= true;		
		}
	}
	IEnumerator  AppendLog(string log){
	
		//	using (w = File.AppendText ("log.txt")) {
		logThread = new Thread(o => {
			//while (true) {
			
			w1.Write ("{0} - {1} ",DateTime.Now.ToShortDateString (),
			          DateTime.Now.ToLongTimeString ());
			w1.WriteLine ("  {0}", log);
			
			print (log);
			print (w1.ToString ());
			
			
		}
		);
		logThread.Start ();
		yield return new WaitForSeconds(1);
		canLog=true;
		//w.Close ();
	}
	
	void OnGUI()
	{
		//render the JS1 'message' variable
	}
	
	void OnDestroy(){
		/////if (w1 != null) {
		///	w1.Close();		
		//}
	}
}
