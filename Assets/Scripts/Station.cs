using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour {

	public int id;

	public string StationName;
	public float maximumTimeBetweenArrivals;
	public int numberOfPassenger;

	public GameObject userPrefab;

	public bool eastToWest;

	public List<GameObject> users;
	List<GameObject> userRegister = new List<GameObject> ();
	public List<GameObject> usersDone;

	public bool create = true;

	GameObject controller;

	public TextMesh nameText;

	int totalTime=0;
	int timeAux = 0;

	public float serviceTime; 

	public int maxUsers;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("Controller");
		nameText.text = StationName;
		nameText.gameObject.SetActive (false);
		
	}
	public void initUserCreation(){
		timeAux = Random.Range (60, 800);
		Invoke ("createUser", (timeAux) / 100);
	}

	void createUser(){
		if (create && users.Count <= maxUsers) {
			Vector2 pos = new Vector2 (transform.position.x, transform.position.y + ((0.35f + (users.Count * 0.15f)) * (eastToWest ? 1 : -1)));
			var o = Instantiate (userPrefab, pos, Quaternion.identity);
			o.GetComponent<User> ().initialStation = id;
			o.GetComponent<User> ().finalStation = Random.Range (id + 1, (id + 1) >= 11 ? controller.GetComponent<Controller> ().numberOfStations : 12);
			o.GetComponent<User> ().arrivalTime = totalTime;
			o.GetComponent<User> ().balanceOnCard = 2500 * (Random.Range (0, 10));
			o.GetComponent<User> ().calculeTimeService (controller.GetComponent<Controller> ().points);
			users.Add (o);
			userRegister.Add (o);
			numberOfPassenger = users.Count;


			timeAux = Random.Range (60, 800);
			totalTime = timeAux;
			Invoke ("createUser", (timeAux) / 100);
		}

	}
	public void updatePositions(){
		for(int i=0;i<users.Count;i++){
			users [i].transform.position = new Vector2 (transform.position.x, transform.position.y + ((0.35f + (i * 0.15f)) * (eastToWest ? 1 : -1)));
		}
		for(int i=0;i<usersDone.Count;i++){
			usersDone [i].transform.position = new Vector2 (transform.position.x, transform.position.y + ((0.35f + ((i+users.Count) * 0.15f)) * (eastToWest ? 1 : -1)));
			usersDone [i].GetComponent<SpriteRenderer> ().enabled=true;
		}
	}

	public void clearUsers(int cont){
		//GameObject aux = users [i];
		for(int i=0;i<cont;i++){
			users.RemoveAt (0);
			//Destroy (aux);
		}
	}
	
	void OnMouseEnter(){
		nameText.gameObject.SetActive (true);
	}
	void OnMouseExit(){
		nameText.gameObject.SetActive (false);
	}
}
