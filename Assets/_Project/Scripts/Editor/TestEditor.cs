using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class TestEditor : EditorWindow
{
	// Variables
	//bool myBool = true;
	//float myFloat = 1.23f;

	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/Test Editor")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(TestEditor));
	}
	
	// Repaint when our selection changes
	void OnSelectionChange () 
	{ 
		// Repaint the editor window
		Repaint(); 
	}

	void OnGUI()
	{
		if (Selection.activeGameObject == null) { return; }
		// Object Selected in the editor
		GameObject selected = Selection.activeGameObject;
		// Get the scene interpreter
		TestBehavior test;
		
		// Reset our changed state
		GUI.changed = false;
		
		// Check if we have a created event, if not and it's a valid object
		if (selected.GetComponent<TestBehavior>() != null)
		{
			// Get the EventController
			test = selected.GetComponent<TestBehavior>();
			
			// Show if we've created an event for this yet
			if (test.test != null) { GUILayout.Label(test.test.test.ToString()); }
			if (GUILayout.Button("Create and set d_Test"))
			{
				if (test.test == null)
				{
					test.test = ScriptableObject.CreateInstance<d_Test>();
					test.test.test = 10;
				}
				else
				{
					test.test.test += 1;
				}
			}
			if (GUILayout.Button("Add one to the list!"))
			{
				d_Test toAdd = ScriptableObject.CreateInstance<d_Test>();
				toAdd.test = Random.Range(0,100);
				test.list.Add(toAdd);
			}
			// Do the list part now
			EditorGUILayout.BeginVertical("Button");
			foreach (d_Test t in test.list)
			{
				GUILayout.Label(t.test.ToString());
			}
			EditorGUILayout.EndVertical();
		}
		
		// If anything changed...
		if (GUI.changed) 
		{ 	
			test = selected.GetComponent<TestBehavior>();
			//EditorUtility.SetDirty(selected); 
			//EditorUtility.SetDirty(test);
			/*
			if (eCont != null)
			{
				foreach (d_EventPage page in eCont.pages)
				{
					EditorUtility.SetDirty(page);
				}
			}
			*/
		}
	}
}