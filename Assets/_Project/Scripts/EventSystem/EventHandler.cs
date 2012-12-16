using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour {
	
	// Public variables
	public d_Event eventData;
	
	// Private variables
	private bool done = false;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	// Check if it's completed
	public bool Done() { return done; }
	public void SetFinished() { done = true; }
}
