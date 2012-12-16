using UnityEngine;
using System.Collections;

public class Database : MonoBehaviour {
	
	// Public variables
	public TextAsset GameEvents;
	public TextAsset GameEventsPages;
	
	// Use this for initialization
	void Start () 
	{
		// Set ourselves as persistent
		GameObject.DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
