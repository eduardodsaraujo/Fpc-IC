#pragma strict
 import System.Threading;


var curWave : int = 0;
var waypoints : Waypoint[];
var waves : Wave[];
var spawners : Transform[];
var spawnDelay : float = 0;
var newWave : boolean = false;
var startNewWave : boolean;

var spawnTime : float = .2;
private var spawning : boolean = false;
private var nextSpawnTimme : float;
var enemies: GameObject[];
var inimigosVivos: int = 0;
var restoInimigos: int = 0;
var inimigosRodada: int = 0;

function Spawn () {

//var thread = new Thread(MyAwesomeFunction);
//thread.Start(0);
//
//}
//
//function MyAwesomeFunction(MyVar : int){


	var w : Wave;
	var cs : CubeSet;
		var j : int = 0;
		newWave = true;	
		w = waves[curWave];
		cs = w.cubeSets[0];
		restoInimigos = cs.numEnemies % 3;
		for(var i : int = 0; i < 3; i++){
			inimigosRodada = cs.numEnemies/3;
			if(restoInimigos > 0){
				inimigosRodada += 1;
				restoInimigos--; 
			}
			print("Inimigos rodadA: "+ inimigosRodada);

			cs.SpawnCS(spawners[i], waypoints[i], spawnTime, inimigosRodada);
			    yield WaitForSeconds (0.5);

		}
		
					//cs.SpawnCS(spawners[0], waypoints[0], spawnTime, cs.numEnemies);

		while(j < 28){
				yield new WaitForSeconds (1);
				j++;
				print("Enemies :" + EnemyMovement.enemies);
				print (j);
		}
		yield new WaitForSeconds (0.5);

		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		
		for(var enemy : GameObject in enemies){
			enemy.GetComponent(EnemyDamageReceiver).ApplyDamage(100);
		}
		
		curWave++;
		//yield new WaitForSeconds(spawnDelay + 1*curWave);
		if(curWave >= waves.length)
			curWave = 0;
}

function OnUpdate(){
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

	inimigosVivos = enemies.Length;
}

function Update(){
	print(" putz " + startNewWave);
	if(startNewWave){
		startNewWave = false;
		spawning = true;
		Spawn();
	}

}
function OnTriggerEnter (other : Collider) {
	//if(other.tag == "Player")
//	if(!spawning){
//		spawning = true;
//		Spawn();
//	}
}