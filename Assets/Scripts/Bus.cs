using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour {
	public int capacity = 50; 
	public int numberOfPassenger;

	public TextMesh text;

	public List<GameObject> passengers = new List<GameObject> ();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateNumber(){
		numberOfPassenger = passengers.Count;
		text.text = numberOfPassenger + "/" + capacity;
	}

}
