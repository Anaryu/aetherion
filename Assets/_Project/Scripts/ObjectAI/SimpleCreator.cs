using UnityEngine;
using System.Collections;

public class SimpleCreator : MonoBehaviour {
	
	// Public variables
	public GameObject [] ToCreate;
	public Vector3 CreateAt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Create the object now
	public void Create(int id)
	{ GameObject go = (GameObject)Instantiate(ToCreate[id], transform.position + CreateAt, transform.rotation); }
}