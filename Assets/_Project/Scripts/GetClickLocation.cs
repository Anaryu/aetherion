using UnityEngine;
using System.Collections;

public class GetClickLocation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// The ray we're casting
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		// To hold the hit data
		RaycastHit hit;
		
		// If the player clicked, find where and move the cursor there
		if (Input.GetButton("Fire1"))
		{ if (Physics.Raycast(ray, out hit, 100.0f)) { cursor().transform.position = hit.point; } }
	
	}
	
	// Return the cursor object
	GameObject cursor()
	{ return GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>().Cursor; }
	
}
