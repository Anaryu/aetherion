using UnityEngine;
using System.Collections;

public class BlobShadowClipper : MonoBehaviour {
	
	// Public variables
	public float MaxDistance = 50;
	
	// Private variables
	private Projector projector;
	
	// Use this for initialization
	void Start () {
		// Get our projector component
		projector = GetComponent<Projector>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// Find the first hit point and set our clipping plane to that
		RaycastHit hit;
		int layerMask1 = 1 << 8;
		int layerMask2 = 1 << 9;
		int layerMask3 = 1 << 12;
		int layerMask = layerMask1 | layerMask2 | layerMask3;
		
		if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance, layerMask))
		{ 
			//projector.nearClipPlane = transform.position.y - hit.point.y;
			//projector.nearClipPlane = (transform.position.y - hit.point.y + 0.3f);
			projector.farClipPlane = (transform.position.y - hit.point.y + 0.1f);
			print (hit.collider.gameObject.name);
		}
	}
}
