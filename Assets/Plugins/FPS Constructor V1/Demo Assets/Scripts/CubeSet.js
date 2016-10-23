#pragma strict
var cubes : Transform[];
var amts : int[];
var numEnemies:int;
var cube:Transform;

function SpawnCS (pos : Transform, w : Waypoint, t : float, qntEnemies: int) {
	var spawned : Transform;
	
	for(var j : int = 0; j < qntEnemies; j++){
			cube = cubes[Random.Range(0,cubes.length)];
			print(Random.Range(0,cubes.length));
			spawned = Instantiate(cube, pos.position+Vector3(0,4,0)*j, pos.rotation);
			EnemyMovement.enemies++;
			spawned.GetComponent(EnemyMovement).waypoint = w.transform;
			yield new WaitForSeconds(t);
	}
}