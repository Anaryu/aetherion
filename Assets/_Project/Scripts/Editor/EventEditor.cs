using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EventEditor : EditorWindow
{
	// Variables
	//bool myBool = true;
	//float myFloat = 1.23f;
	List<bool> foldout = new List<bool>();
	List<d_EventPage> pages;
	Vector2 scrollView = new Vector2(0f,0f);
	SceneInterpreter interpreter;

	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Event Editor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(EventEditor));
	}
	
	// Repaint when our selection changes
	void OnSelectionChange () 
	{ 
		// Reset our trigger for the event's pages
		GameObject selected = Selection.activeGameObject;
		TriggerPageChanges(selected);
		
		// Repaint the editor window
		Repaint(); 
	}

	void OnGUI()
	{
		// Do our basic checks to return if any fail
		if (Selection.activeGameObject == null) { return; }
		
		// Object Selected in the editor
		GameObject selected = Selection.activeGameObject;
		
		// Get the scene interpreter and database
		interpreter = GameObject.FindGameObjectWithTag("SceneInterpreter").GetComponent<SceneInterpreter>();
		
		// Event controller and Helper
		EventController eCont;
		EventHelper eHelp;
		
		// Reset our changed state
		GUI.changed = false;
		
		// Check if we have a created event, if not and it's a valid object
		if (interpreter == null) { GUILayout.Label ("Missing the SceneInterpreter"); return; }
		else if (selected == null) { GUILayout.Label ("No GameObject is Selected"); return; }
		else if (selected.GetComponent<EventController>() != null)
		{
			// Get the EventController
			eCont = selected.GetComponent<EventController>();
			eHelp = selected.GetComponent<EventHelper>();
			
			// Show if we've created an event for this yet
			if (eCont.GetSysID() <= 0)
			{
				GUILayout.Label ("This event hasn't been created yet", EditorStyles.boldLabel);
				if (GUILayout.Button("Click here to Create the Event"))
				{
					// Add it to the SceneInterpreter
					interpreter.EventObjects.Add(selected);
				}
				if (GUILayout.Button("Click here to Refresh SysID"))
				{ eCont.GetSysID(); }
			}
			else
			{
				// Display the Event System ID
				GUILayout.Label ("Event System ID: " + eCont.GetSysID(), EditorStyles.boldLabel);
				
				// Set us up as a scroll area
				scrollView = EditorGUILayout.BeginScrollView(scrollView);
				
				for (int i = 0; i < eCont.pages.Count; i++)
				{
					// Update the open setting on the page to whatever the foldout control
					// is, which is updated by the player
					eCont.pages[i].open = foldout[i];
					
					// This is the vertical for each Page
					EditorGUILayout.BeginVertical("Button");
					
					// These are the 'closed' controls
					EditorGUILayout.BeginHorizontal("TextArea");
					foldout[i] = EditorGUILayout.Foldout(foldout[i], eCont.pages[i].pageNumber + ": " + eCont.pages[i].pageName);
					GUILayout.Label("", GUILayout.Width(200f)); 
					// Display the number of events on this object
					if (eCont.pages[i].events == null) 
					{ 
						GUILayout.Label("Events: None"); 
						GUILayout.Label("Trigger: None"); 
					}
					else 
					{ 
						GUILayout.Label("Events: " + eCont.pages[i].events.Count); 
						GUILayout.Label("Trigger: " + eCont.pages[i].trigger.ToString());
					}
					// Move Up Control
					if (GUILayout.Button("Move Up"))
					{ 
						int curIndex = eCont.pages.IndexOf(eCont.pages[i]);
						if (curIndex > 0)
						{
							d_EventPage curr = eCont.pages[curIndex];
							eCont.pages.RemoveAt(curIndex);
							eCont.pages.Insert(curIndex-1, curr);
							TriggerPageChanges(selected);
						}
					}
					// Move Down Control
					if (GUILayout.Button("Move Down"))
					{ 
						int curIndex = eCont.pages.IndexOf(eCont.pages[i]);
						if (curIndex < eCont.pages.Count-1)
						{
							d_EventPage curr = eCont.pages[curIndex];
							eCont.pages.RemoveAt(curIndex);
							eCont.pages.Insert(curIndex+1, curr);
							TriggerPageChanges(selected);
						}
					}
					EditorGUILayout.EndHorizontal();
					
					// These are all the 'open' controls
					if (foldout[i])
					{
						// Page Name
						EditorGUILayout.BeginHorizontal("Label");
						GUILayout.Label("Trigger:", GUILayout.Width(70f));
						eCont.pages[i].trigger = (eEventTrigger)EditorGUILayout.EnumPopup(eCont.pages[i].trigger, GUILayout.Width(120f));
						GUILayout.Label("Page Name:", GUILayout.Width(70f));
						eCont.pages[i].pageName = GUILayout.TextField(eCont.pages[i].pageName);
						EditorGUILayout.EndHorizontal();
						// Page Variable Triggers
							// Var1
						EditorGUILayout.BeginHorizontal("Label");
						GUILayout.Label("Var1:",GUILayout.Width(30f));
						eCont.pages[i].variable1Name = GUILayout.TextField(eCont.pages[i].variable1Name, GUILayout.Width(140f));
						eCont.pages[i].variable1Value = EditorGUILayout.FloatField(eCont.pages[i].variable1Value, GUILayout.Width(100f));
							// Var2
						GUILayout.Label("Var2:", GUILayout.Width(30f));
						eCont.pages[i].variable2Name = GUILayout.TextField(eCont.pages[i].variable2Name, GUILayout.Width(140f));
						eCont.pages[i].variable2Value = EditorGUILayout.FloatField(eCont.pages[i].variable2Value, GUILayout.Width(100f));
						EditorGUILayout.EndHorizontal();
						// Page Switch Triggers
							// Switch1
						EditorGUILayout.BeginHorizontal("Label");
						GUILayout.Label("Swt1:",GUILayout.Width(30f));
						eCont.pages[i].switch1Name = GUILayout.TextField(eCont.pages[i].switch1Name, GUILayout.Width(140f));
						eCont.pages[i].switch1Value = GUILayout.Toggle(eCont.pages[i].switch1Value, "", GUILayout.Width(100f));
							// Switch2
						GUILayout.Label("Swt2:", GUILayout.Width(30f));
						eCont.pages[i].switch2Name = GUILayout.TextField(eCont.pages[i].switch2Name, GUILayout.Width(140f));
						eCont.pages[i].switch2Value = GUILayout.Toggle(eCont.pages[i].switch2Value, "", GUILayout.Width(100f));
						EditorGUILayout.EndHorizontal();
						
						// Display the current "Events"
						GUILayout.Space(5f);
						EditorGUILayout.BeginVertical("Label");
						
						
							// Loop through events on the page
						GUILayout.Label("Events:");
						int j = 0;
						foreach (d_Event e in eCont.pages[i].events)
						{
							EditorGUILayout.BeginVertical("Label");
							EditorGUILayout.BeginHorizontal("Box");
							GUILayout.Label(j.ToString(), GUILayout.Width(10f));
							// Show the controls and their current value based on it's type
							e.eventTypeID = (eEventType)EditorGUILayout.EnumPopup(e.eventTypeID, GUILayout.Width(120f));
							// Buttons to move the event up/down in the list
							if (GUILayout.Button("Up", GUILayout.Width(40f)))
							{ 
								int curIndex = eCont.pages[i].events.IndexOf(e);
								if (curIndex > 0)
								{
									d_Event curr = eCont.pages[i].events[curIndex];
									eCont.pages[i].events.RemoveAt(curIndex);
									eCont.pages[i].events.Insert(curIndex-1, curr);
									TriggerPageChanges(selected);
									break;
								}
							}
							// Move Down Control
							if (GUILayout.Button("Down", GUILayout.Width(40f)))
							{ 
								int curIndex = eCont.pages[i].events.IndexOf(e);
								if (curIndex < eCont.pages[i].events.Count-1)
								{
									d_Event curr = eCont.pages[i].events[curIndex];
									eCont.pages[i].events.RemoveAt(curIndex);
									eCont.pages[i].events.Insert(curIndex+1, curr);
									TriggerPageChanges(selected);
									break;
								}
							}
							// Delete Event Control
							GUILayout.Label("", GUILayout.Width(20f));
							if (GUILayout.Button("X", GUILayout.Width(26f)))
							{ 
								eCont.pages[i].events.Remove(e);
								TriggerPageChanges(selected);
								break;
							}
							
							// Display the event-specific fields
							EditorGUILayout.EndHorizontal();
							
							DisplayEventControls(e);
							
							// End our groupings
							EditorGUILayout.EndVertical();
							GUILayout.Space(5f);
							j += 1; // increase our simple counter
						}
						
						
						EditorGUILayout.EndVertical();
						GUILayout.Space(5f);
						
						// Display the controls to add a new event
						//EditorGUILayout.BeginHorizontal("TextField");
						//GUILayout.Label("Add a new Event to this page:", GUILayout.Width(200f));
						if (GUILayout.Button("Add New Event", GUILayout.Width(150f)))
						{
							// Add a new element to the list of pages
							d_Event newEvent = ScriptableObject.CreateInstance<d_Event>();
							eCont.pages[i].events.Add(newEvent);
							TriggerPageChanges(selected);
						}
						//EditorGUILayout.EndHorizontal();
						
						// Delete Page Control
						EditorGUILayout.BeginHorizontal("Label");
						GUILayout.Label("Be careful! A deleted page cannot be recovered!");
						GUILayout.Label("");
						if (GUILayout.Button("Delete Page"))
						{ 
							eCont.pages.Remove(eCont.pages[i]);
							TriggerPageChanges(selected);
						}
						EditorGUILayout.EndHorizontal();
					}
					
					// This is th end of a single page
					EditorGUILayout.EndVertical();
					GUILayout.Space(10f);
				}
				
				// All the controls outside the pages are below
				
				// Button to add a new page
				if (GUILayout.Button("Add New Page"))
				{
					// Add a new element to the list of pages
					d_EventPage newPage = ScriptableObject.CreateInstance<d_EventPage>();
					newPage.Init(eCont.getMaxPage()+1, "");
					eCont.setMaxPage(newPage.pageNumber);
					eCont.pages.Add(newPage);
					TriggerPageChanges(selected);
				}
				
				EditorGUILayout.EndScrollView();
			}
		}
		// Show it doesn't have an EventController and allow the user to add one
		else
		{
			GUILayout.Label ("Object doesn't have an 'EventController'", EditorStyles.boldLabel);
			if (GUILayout.Button("Click here to Add the EventController"))
			{
				selected.AddComponent<EventController>(); 
				selected.AddComponent<EventHelper>();
			}
		}
		
		// If anything changed...
		if (GUI.changed) 
		{ }
	}
	
	// Run this when we add or remove a page
	void TriggerPageChanges(GameObject current)
	{
		// Return if we aren't valid
		if (current == null) { return; }
		if (current.GetComponent<EventController>() == null) { return; }
		// Get our pages and reset the foldout
		EventController eCont = current.GetComponent<EventController>();
		foldout = new List<bool>();
		if (eCont != null) { foreach (d_EventPage page in eCont.pages) { foldout.Add(page.open); } }
	}
	
	// Display the appropriate values for your event
	void DisplayEventControls(d_Event e)
	{
		EditorGUILayout.BeginHorizontal("Label");
		switch(e.eventTypeID)
		{
			case eEventType.None:
				break;
			case eEventType.CallFunction:
				GUILayout.Label("Function:", GUILayout.Width(50f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Params:", GUILayout.Width(50f));
				e.sParams[1] = GUILayout.TextField(e.sParams[1]);
				break;
			case eEventType.PlaySound:
				GUILayout.Label("Audio:", GUILayout.Width(50f));
				e.oParams[0] = EditorGUILayout.ObjectField(e.oParams[0], typeof(AudioClip), true, GUILayout.Width(200f));
				GUILayout.Label("Volume:", GUILayout.Width(50f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(50f));
				GUILayout.Label("Pitch:", GUILayout.Width(50f));
				e.fParams[1] = EditorGUILayout.FloatField(e.fParams[1], GUILayout.Width(50f));
				break;
			case eEventType.Play3DSound:
				GUILayout.Label("Audio:", GUILayout.Width(50f));
				e.oParams[0] = EditorGUILayout.ObjectField(e.oParams[0], typeof(AudioClip), true, GUILayout.Width(200f));
				GUILayout.Label("Volume:", GUILayout.Width(50f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(50f));
				GUILayout.Label("Pitch:", GUILayout.Width(50f));
				e.fParams[1] = EditorGUILayout.FloatField(e.fParams[1], GUILayout.Width(50f));
				GUILayout.Label("Source:", GUILayout.Width(50f));
				e.oParams[1] = EditorGUILayout.ObjectField(e.oParams[1], typeof(GameObject), true, GUILayout.Width(200f));
				break;
			case eEventType.Wait:
				GUILayout.Label("Wait (in sec):", GUILayout.Width(80f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(50f));
				break;
			case eEventType.Message:
				GUILayout.Label("Portrait:", GUILayout.Width(60f));
				e.oParams[0] = EditorGUILayout.ObjectField(e.oParams[0], typeof(Texture2D), true, GUILayout.Width(150f));
				GUILayout.Label("Title:", GUILayout.Width(60f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Name:", GUILayout.Width(60f));
				e.sParams[1] = GUILayout.TextField(e.sParams[1]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal("Label");
				GUILayout.Label("Message:", GUILayout.Width(60f));
				e.sParams[2] = GUILayout.TextArea(e.sParams[2], GUILayout.Height(60f));
				break;
			case eEventType.Move:
				GUILayout.Label("Speed:", GUILayout.Width(60f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(50f));
				GUILayout.Label("Object:", GUILayout.Width(60f));
				e.oParams[0] = EditorGUILayout.ObjectField(e.oParams[0], typeof(GameObject), true, GUILayout.Width(150f));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal("Label");
				e.v3Params[0] = EditorGUILayout.Vector3Field("Location:", e.v3Params[0], GUILayout.Width(200f));
				break;
			case eEventType.LookAt:
				GUILayout.Label("Speed:", GUILayout.Width(60f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(50f));
				GUILayout.Label("Looker:", GUILayout.Width(60f));
				e.oParams[0] = EditorGUILayout.ObjectField(e.oParams[0], typeof(GameObject), true, GUILayout.Width(150f));
				GUILayout.Label("Look At:", GUILayout.Width(60f));
				e.oParams[1] = EditorGUILayout.ObjectField(e.oParams[1], typeof(GameObject), true, GUILayout.Width(150f));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal("Label");
				e.v3Params[0] = EditorGUILayout.Vector3Field("Location:", e.v3Params[0], GUILayout.Width(200f));
				break;
			case eEventType.SetSwitch:
				GUILayout.Label("Switch Name:", GUILayout.Width(80f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Value:", GUILayout.Width(50f));
				e.bParams[0] = GUILayout.Toggle(e.bParams[0], "");
				break;
			case eEventType.FlipSwitch:
				GUILayout.Label("Switch Name:", GUILayout.Width(80f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				break;
			case eEventType.SetVariable:
				GUILayout.Label("Variable Name:", GUILayout.Width(80f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Value:", GUILayout.Width(50f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(80f));
				break;
			case eEventType.IncreaseVariable:
				GUILayout.Label("Variable Name:", GUILayout.Width(80f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Value:", GUILayout.Width(50f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(80f));
				break;
			case eEventType.DecreaseVariable:
				GUILayout.Label("Variable Name:", GUILayout.Width(80f));
				e.sParams[0] = GUILayout.TextField(e.sParams[0]);
				GUILayout.Label("Value:", GUILayout.Width(50f));
				e.fParams[0] = EditorGUILayout.FloatField(e.fParams[0], GUILayout.Width(80f));
				break;
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
}