using UnityEngine;
using System.Collections;

public class CharacterOrbit : MonoBehaviour {
	
	// Public variables
	public Transform target;
	public float distance = 10.0f;
	
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	// Private variables
	private float x = 0.0f;
	private float y = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
	    x = angles.y;
	    y = angles.x;

		// Make the rigid body not change rotation
	   	if (rigidbody) { rigidbody.freezeRotation = true; }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () 
	{
	    if (target) 
		{
	        //x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
	        //y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			PlayerController pc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
			if (Input.GetButton("Camera"))
			{
				x += Input.GetAxis("Horizontal") * xSpeed * 0.02f;
				y -= Input.GetAxis("Vertical") * ySpeed * 0.02f;
				pc.SetCameraStatus(true);
			}
			else
			{ pc.SetCameraStatus(false); }
	 		
	 		// Limit our y offset
	 		y = ClampAngle(y, yMinLimit, yMaxLimit);
	 		       
	 		// Calculate the rotation
	        Quaternion rotation = Quaternion.Euler(y, x, 0f);
	        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
	        
	        // Set our end values
	        transform.rotation = rotation;
	        transform.position = position;
	    }
	}
	
	// ClampAngle will ensure we're never outside certain ranges
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360f) { angle += 360f; }
		if (angle > 360f) { angle -= 360f; }
		return Mathf.Clamp (angle, min, max);
	}
}
