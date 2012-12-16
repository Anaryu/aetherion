using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventController : MonoBehaviour {
	
	// Public variables
	public string Name;
	public int ReadOnlySysID;
	
	// Private variables
	public int SysID;
	public List<d_EventPage> pages;
	public SceneInterpreter interpreter;
	public int maxPage;
	private int currentPage;
	private bool thisPageRun;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	
	// Update is called once per frame
	public void UpdateEvent () 
	{
		// Get the interpreter if it's null
		if (interpreter == null)
		{ 
			if (GameObject.FindGameObjectWithTag("SceneInterpreter")) { return; }
			interpreter = GameObject.FindGameObjectWithTag("SceneInterpreter").GetComponent<SceneInterpreter>(); 
		}
		// Return if the interpreter is running
		if (interpreter.InterpreterActive()) { return; }
		
		// Check our pages and process if appropriate
		int pid = pages.Count;
		int previousPage = currentPage;
		currentPage = -1;
		for (int i = pid-1; i >= 0; i--)
		{
			// Check each criteria, if they all pass, this is the current page
			bool c1 = pages[i].variable1Name == "" || pages[i].variable1Value <= interpreter.GetVariable(pages[i].variable1Name);
			bool c2 = pages[i].variable2Name == "" || pages[i].variable2Value <= interpreter.GetVariable(pages[i].variable2Name);
			bool c3 = pages[i].switch1Name == "" || pages[i].switch1Value == interpreter.GetSwitch(pages[i].switch1Name);
			bool c4 = pages[i].switch2Name == "" || pages[i].switch2Value == interpreter.GetSwitch(pages[i].switch2Name);
			// If all 4 criteria match, that's our page
			if (c1 && c2 && c3 && c4) 
			{
				if (previousPage != i) { thisPageRun = false; }
				currentPage = i;
				break;
			}
		}
		
		// If our page trigger is auto and we're not running, run us!
		if (currentPage >= 0 && pages[currentPage].trigger == eEventTrigger.Auto && !thisPageRun)
		{
			Debug.Log (pages[currentPage].pageName + ": " + currentPage);
			// Add each event to the scene interpreter
			foreach (d_Event e in pages[currentPage].events)
			{ interpreter.AddEvent(e, gameObject); }
			thisPageRun = true;
		}
	}
	
	// Get/Set for maxPage
	public int getMaxPage() { return maxPage; }
	public void setMaxPage(int newPage) { maxPage = newPage; }
	
	// Get the SysID
	public int GetSysID() 
	{ 
		SetSysID();
		return SysID; 
	}
	
	// Set the SysID
	public void SetSysID()
	{
		// Get the interpreter
		interpreter = GameObject.FindGameObjectWithTag("SceneInterpreter").GetComponent<SceneInterpreter>();
		// Set it to zero if we can't find ourselves
		if (interpreter != null && interpreter.EventObjects.Contains(gameObject))
		{ SysID = 1 + interpreter.EventObjects.FindIndex((t) => { return (t == gameObject); }); }
		else { SysID = 0; }
	}
	
}