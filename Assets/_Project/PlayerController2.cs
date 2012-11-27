using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour {

	Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		anim.SetFloat("Speed", h*h+v*v);
		anim.SetFloat("Direction", h, 0.25f, Time.deltaTime);
	}
}
