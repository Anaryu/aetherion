using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class d_EventPage : ScriptableObject {
	
	// Public values
	public bool open;
	public int pageNumber;
	public string pageName;
	public eEventTrigger trigger;
	
	public string variable1Name;
	public string variable2Name;
	public float variable1Value;
	public float variable2Value;
	public string switch1Name;
	public string switch2Name;
	public bool switch1Value;
	public bool switch2Value;
	
	public List<d_Event> events;
	
	// Private values
	
	// Public functions
	public void Init(int pNumber, string pName)
	{
		// Set default values
		pageNumber = pNumber;
		pageName = pName;
		
		// Set the default values
		variable1Name = "";
		variable2Name = "";
		switch1Name = "";
		switch2Name = "";
		
		events = new List<d_Event>();
	}
}
