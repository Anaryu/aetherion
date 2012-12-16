using UnityEngine;
using System.Collections;

public class eventWait : MonoBehaviour {
	
	// Public variable
	public float TimeRemaining;
	private bool completed = false;

	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () 
	{
		// Keep waiting...
		TimeRemaining -= Time.deltaTime;
		// if we're done...
		if (TimeRemaining <= 0 && !completed)
		{
			// Resume the interpreter and destroy ourselves
			GameObject interpreter = GameObject.FindGameObjectWithTag("SceneInterpreter");
			if (interpreter != null)
			{
				completed = true;
				interpreter.SendMessage("CompleteCurrentEvent");
				Destroy(gameObject);
			}
		}
	}
}
