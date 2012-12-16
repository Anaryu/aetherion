using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class d_Event : ScriptableObject {
	
	// Public values
	public eEventType eventTypeID = eEventType.None;
	public string[] sParams = {"","","","","","","","","",""};
	public float[] fParams = {0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
	public bool[] bParams = {false,false,false,false,false,false,false,false,false,false};
	public Vector3[] v3Params = {Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero,Vector3.zero};
	public Object[] oParams = {null,null,null,null,null,null,null,null,null,null};
	public eEventType buildType = eEventType.None;
	
	// Private values
}