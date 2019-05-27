using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour {

	public int initialStation;
	public int finalStation;

	public float balanceOnCard = 0; 

	public float arrivalTime=-1;

	public float myTimeService=-1;

	//GameObject controller;

	// Use this for initialization
	void Start () {
		//controller = GameObject.Find ("Controller");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void calculeTimeService(List<Transform> points){
		if (balanceOnCard > 2500) {
			float add = 0;
			for (int i =0; i < points.Count; i++) {
				if (points [i].tag == "Station" && points [i].GetComponent<Station> ().id > initialStation && points [i].GetComponent<Station> ().id <= finalStation) {
					add += points [i].GetComponent<Station> ().serviceTime;
				}
			}
			myTimeService = add;
		}
	}
}
