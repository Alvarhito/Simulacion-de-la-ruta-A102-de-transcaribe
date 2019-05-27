using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.IO;
using System;


public class Data{
	public InitialData[] data;
}
[System.Serializable]
public class InitialData{
	public float passenger;
	public float Arrive;

	/*public static InitialData CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<InitialData>(jsonString);
	}*/

	// Given JSON input:
	// {"name":"Dr Charles","lives":3,"health":0.8}
	// this example will return a PlayerInfo object with
	// name == "Dr Charles", lives == 3, and health == 0.8f.
}

public class Controller : MonoBehaviour {

	public GameObject busPrefab;
	public float speed=0.05f;
	public int numberOfStations;

	public List<Transform> points;
	GameObject bus;

	Data newData;

	// Use this for initialization
	void Start () {
		bus = Instantiate (busPrefab,points[0].position, Quaternion.identity);


		getData();
		//initSimulation ();
		Invoke("initSimulation",1f);
		Invoke ("startMove", (700 / 100));


	}
	void getData(){
		newData = JsonUtility.FromJson<Data> (fetch ());

		//Debug.Log (aux1.data [0].Arrive);
	}

	string fetch(){
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5000/linear");
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		StreamReader reader = new StreamReader(response.GetResponseStream());

		var jsonResponse = reader.ReadToEnd ();
		return jsonResponse;
	}

	void startMove(){
		StartCoroutine(move());
	}
	void initSimulation(){
		int cont = 0;
		for (int i = 0; i < points.Count; i++) {
			if (points [i].tag == "Station") {
				//points [i].GetComponent<Station> ().maximumTimeBetweenArrivals = Random.Range (40, 500);
				numberOfStations+=1;
				points [i].GetComponent<Station> ().id = points [i].name == "Portal" ? 0 : cont;
				points [i].GetComponent<Station> ().maxUsers = newData.data [numberOfStations-1].passenger > 0 ? (int)(newData.data [numberOfStations-1].passenger / 3) : 0;
				points [i].GetComponent<Station> ().serviceTime = Math.Abs (newData.data [numberOfStations - 1].Arrive);
				cont += 1;
				//points [i].GetComponent<Station> ().initUserCreation ();


			}
		}
		for (int i = 0; i < points.Count; i++) {
			if (points [i].tag == "Station") {
				points [i].GetComponent<Station> ().initUserCreation ();
			}
		}
	}

	IEnumerator move(){
		updatePassenger (0);
		Debug.Log (points.Count);
		for (int i = 1; i < points.Count; i++) {
			yield return StartCoroutine (moveBus(i));
			if (points [i].tag == "Station") {
				updatePassenger (i);

			}
		}
	}

	void updatePassenger(int station){
		Bus classBus = bus.GetComponent<Bus> ();
		Station classStation = points [station].GetComponent<Station> ();

		classStation.create = false;
		for (int i = 0; i < classBus.passengers.Count; i++) {
			if (classBus.passengers [i].GetComponent<User> ().finalStation == classStation.id) {
				classBus.passengers [i].GetComponent<SpriteRenderer> ().color = Color.green;
				classStation.usersDone.Add (classBus.passengers [i]);
				classBus.passengers.RemoveAt (i);
				i--;
			}
		}

		int auxCont = 0;
		for (int i = 0; i < classStation.users.Count; i++) {
			if (classBus.passengers.Count < classBus.capacity) {
				if (classStation.users [i].GetComponent<User> ().balanceOnCard > 0) {
					classStation.users [i].GetComponent<SpriteRenderer> ().enabled = false;
					classBus.passengers.Add (classStation.users [i]);
					auxCont += 1;
				} else {
					classStation.users [i].GetComponent<SpriteRenderer> ().color = Color.red;
				}
			}else{ 
				break;
			}
		}
		classStation.clearUsers (auxCont);

		classBus.updateNumber ();
		classStation.updatePositions ();
		if (station == 0) {
			classStation.id = numberOfStations - 1;
			classStation.serviceTime = 0;
		}

	}

	IEnumerator moveBus(int j){
		while(true){
			Vector2 aux = Vector2.MoveTowards (bus.transform.position, points[j].transform.position, speed);
			bus.transform.position = new Vector3 (aux.x, aux.y, -2);
			if ((points[j].transform.position - bus.transform.position).magnitude < speed) {
				bus.transform.position = points[j].transform.position;
				break;
			}
			if ((bus.transform.position.x == points [j].transform.position.x) && (bus.transform.position.y == points [j].transform.position.y)) {
				break;
			}
			yield return null;
		}
	}
}
