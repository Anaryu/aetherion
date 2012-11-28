using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Public variables
	public GameObject Player;
	public GameObject Cursor;
	public GameObject[] Players;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// Update all appropriate player objects
		foreach (GameObject player in Players)
		{
			// Set the animator speed
			AIPath aiPath = player.GetComponent<AIPath>();
			Animator anim = player.GetComponent<Animator>();
			/*if(aiPath.velocity != null)
			{ 
				anim.SetFloat("Speed", aiPath.velocity / aiPath.speed);
				anim.SetFloat("Speed", seek.GetCurrentPath().speed); 
				//print (seek.GetCurrentPath().speed);
			}*/
			//animation.SetFloat("Speed", h*h+v*v);
			
			Vector3 velocity;
			float cSpeed;
			velocity = player.GetComponent<CharacterController>().velocity;
			if (player.GetComponent<CharacterMotor>().MaxSpeedInDirection(velocity) > 0f)
			{ cSpeed = velocity.magnitude / player.GetComponent<CharacterMotor>().MaxSpeedInDirection(velocity); }
			else
			{ cSpeed = 0f; }
			anim.SetFloat("Speed", cSpeed);
			anim.SetFloat("VSpeed", velocity.y);
			anim.SetBool("OnGround", player.GetComponent<CharacterMotor>().IsGrounded());
			
			print (cSpeed);
			//print (velocity.y);
			//print (player.GetComponent<CharacterMotor>().IsGrounded());
			
			// Ensure the player has us set as their player controller
			PlayerActionAI pAI = player.GetComponent<PlayerActionAI>();
			pAI.pController = this;
			pAI.UpdateSpeed();
			
			// If it's not the active player...
			if (player != Player)
			{
				// Call the player to update their own logic
				pAI.UpdateCombatAI();
				pAI.UpdatePathAI();
			}
			else
			{
				// Call the active player AI
				pAI.UpdateActiveAI();
			}
		}
	
	}
}
