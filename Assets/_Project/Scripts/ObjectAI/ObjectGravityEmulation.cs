using UnityEngine;
using System.Collections;

public class ObjectGravityEmulation : MonoBehaviour {
	
	// Public variables
	public Vector3 Fall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Vector3 mag = (Vector3.Scale(Fall, transform.localScale) / 2f);
		Vector3 dir = transform.TransformDirection(Fall);
		Debug.DrawRay(transform.position, dir);
		if (!Physics.Raycast(transform.position, dir, mag.magnitude))
		{ transform.Translate(Fall * Time.deltaTime); }
	}
}