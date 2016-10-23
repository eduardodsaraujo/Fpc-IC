
#pragma strict


	var player: GameObject;
	var weapon: GameObject;
	var enemies: GameObject[];
	var numEnemies: GameObject[];
	var numDeads: int = 0;
	var reloadTime: float;
	var health : float;
	
	var aO : AudioSource[];

	//var modifyThread: Thread ;

	var hitPoints:float;



function Start () {

	hitPoints = 100;
	player = GameObject.FindWithTag("Player");
	weapon = GameObject.FindWithTag("Weapon");
	numEnemies = GameObject.FindGameObjectsWithTag("NumEnemies");
	

	//health = player.GetComponent(PlayerHealth).health;

}

function Update () {
	 //player.GetComponent(PlayerHealth).health = health;
	

     enemies = GameObject.FindGameObjectsWithTag("Enemy");

	
	}
	


