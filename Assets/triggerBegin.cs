using UnityEngine;
using System.Collections;

public class triggerBegin : MonoBehaviour {

	GameObject scene;
	// Use this for initialization
	void Start () {
		scene = GameObject.FindWithTag ("Scene");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			scene.GetComponent<GameManager>().contarTempo= true;
		}
	}

}
