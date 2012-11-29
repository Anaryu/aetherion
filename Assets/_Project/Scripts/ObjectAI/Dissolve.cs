using UnityEngine;
using System.Collections;

public class Dissolve : MonoBehaviour {
	
	// Public variables
	public float DissolveTime = 5f;
	public Vector3 DissolveDirections;
	
	// Private variables
	private float timer;

	// Use this for initialization
	void Start () {
	
		// Set our starting time
		timer = DissolveTime;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Dissolve per the amount over time
		transform.localScale -= DissolveDirections * (Time.deltaTime / DissolveTime);
		
		// Reduce our time and die afterwards
		timer -= Time.deltaTime;
		if (timer <= 0) { Destroy(this.gameObject); }
	}
}
