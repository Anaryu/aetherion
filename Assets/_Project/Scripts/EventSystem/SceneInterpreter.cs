using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInterpreter : MonoBehaviour {
	
	// Public values for objects
	public GameObject WaitPrefab;
	public GameObject MessagePrefab;
	
	// Public variables
	public List<GameObject> EventObjects;
	public GameObject ActiveObject;
	public d_Event ActiveEvent;
	public bool AllowEvents;
	public bool EventActive;
	
	// Private variables
	private List<d_Event> EventQ = new List<d_Event>();
	private List<GameObject> EventGO = new List<GameObject>();
	private Dictionary<string, float> variables = new Dictionary<string, float>();
	private Dictionary<string, bool> switches = new Dictionary<string, bool>();
	private bool Running;
	private bool DeactivateEvent = false;
	private float eventGraceTime = 0f;
	
	// Use this for initialization
	void Start () 
	{
		// Initially we're okay with events happening
		Running = true;
		AllowEvents = true;
		EventActive = false;
	}
	
	// Update is called once per frame
	void Update ()
	{ }
	
	// LateUpdate is called once per frame after Update
	void LateUpdate () 
	{
		// If we're running...
		if (Running)
		{
			// If we have 'DeactivateEvent' set, clear the active state
			if (DeactivateEvent) { EventActive = false; DeactivateEvent = false; }
			
			// If our active event is done or we're not doing anything
			if (!EventActive)
			{
				// Get the next item from the list and delete it
				if (EventQ.Count > 0)
				{
					// Get our caller and event
					d_Event nextEvent = EventQ[0];
					GameObject caller = EventGO[0];
					// Process the action
					ProcessEvent(nextEvent, caller);
					eventGraceTime = 0.2f;
					// Remove them
					EventQ.RemoveAt(0);
					EventGO.RemoveAt(0);
				}
				// No queue items means we want to loop through events and find more
				else
				{
					// Update our Grace Time
					eventGraceTime -= Time.deltaTime;
					
					// Loop through all events and do their event updates
					foreach (GameObject eventController in EventObjects)
					{ eventController.GetComponent<EventController>().UpdateEvent(); }
				}
			}
		}
	}
	
	// Process a new event
	public void ProcessEvent(d_Event newEvent, GameObject caller)
	{
		// Check our event type and react based on that
		switch (newEvent.eventTypeID)
		{
			case eEventType.CallFunction:
				caller.SendMessage(newEvent.sParams[0], newEvent.sParams[1]);
				break;
			case eEventType.PlaySound:
				break;
			case eEventType.Play3DSound:
				break;
			// Wait Event
			case eEventType.Wait:
				GameObject created = (GameObject)Instantiate(WaitPrefab);
				created.GetComponent<eventWait>().TimeRemaining = newEvent.fParams[0];
				ActiveObject = created;
				EventActive = true;
				Debug.Log("Waiting: " + newEvent.fParams[0]);
				break;
			// Move Event
			case eEventType.Move:
				break;
			case eEventType.LookAt:
				break;
			case eEventType.Message:
				break;
			case eEventType.SetSwitch:
				switches[newEvent.sParams[0]] = newEvent.bParams[0];
				Debug.Log("Set Switch:" + newEvent.sParams[0]);
				break;
			case eEventType.FlipSwitch:
				switches[newEvent.sParams[0]] = !switches[newEvent.sParams[0]];
				break;
			case eEventType.SetVariable:
				variables[newEvent.sParams[0]] = newEvent.fParams[0];
				Debug.Log("Set Variable:" + newEvent.sParams[0]);
				break;
			case eEventType.IncreaseVariable:
				variables[newEvent.sParams[0]] = variables[newEvent.sParams[0]] + newEvent.fParams[0];
				break;
			case eEventType.DecreaseVariable:
				variables[newEvent.sParams[0]] = variables[newEvent.sParams[0]] - newEvent.fParams[0];
				break;
		}
	}
	
	// Get/Set for variables/switches
	public void SetVariable(string name, float val) { variables[name] = val; }
	public float GetVariable(string name) 
	{ 
		if (!variables.ContainsKey(name)) { variables[name] = 0f; }
		return variables[name]; 
	}
	public void SetSwitch(string name, bool val) { switches[name] = val; }
	public bool GetSwitch(string name) 
	{ 
		if (!switches.ContainsKey(name)) { switches[name] = false; }
		return switches[name]; 
	}
	
	// Add an item to the queue
	public bool AddEvent(d_Event newEvent, GameObject caller)
	{
		// See if we're in a state where we can add it to the queue
		if (AllowEvents)
		{
			//Debug.Log("Added event: " + newEvent.eventTypeID.ToString());
			// Add it to the end of the of the list
			EventQ.Add(newEvent);
			EventGO.Add(caller);
			return true;
		}
		// If we reached this, the event wasn't added, so let the caller know
		return false;
	}
	
	// Start/Stop/Check the run state of the interpreter
	public bool IsRunning() { return Running; }
	public void StopRunning() { Running = false; }
	public void StartRunning() { Running = true; }
	
	// The function used to check if this interpreter is in a state where
	// the game shouldn't continue other processing like Player control,
	// etc.
	public bool InterpreterActive() 
	{ 
		return (EventQ.Count > 0 || EventActive || eventGraceTime > 0f); 
	}
	
	// Messages
	// Message that the running event is complete
	void CompleteCurrentEvent() { DeactivateEvent = true; }
}