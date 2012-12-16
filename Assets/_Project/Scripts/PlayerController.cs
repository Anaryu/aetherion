using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Public variables
	public GameObject Player;
	public GameObject Cursor;
	public GameObject Party;
	public GameObject[] Players;
	
	// Private variables
	private SceneInterpreter interpreter;
	private bool cameraActive;
	
	// Use this for initialization
	void Start () {
		
		// Set ourselves as persistent
		//Object.DontDestroyOnLoad(this);
		GameObject.DontDestroyOnLoad(this.gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// Set our interprefer if it's not
		if (interpreter == null && GameObject.FindGameObjectWithTag("SceneInterpreter"))
		{ interpreter = GameObject.FindGameObjectWithTag("SceneInterpreter").GetComponent<SceneInterpreter>(); }
		
		// Update the Camera
		UpdateCamera();
		
		// Update Players
		UpdatePlayers();
		
		// Update Inputs
		UpdateInput();
	
	}
	
	// Updates the Camera
	void UpdateCamera ()
	{
		if (Player != null)
		{ Camera.mainCamera.GetComponent<CharacterOrbit>().target = Player.transform; }
	}
	
	// Updates the basic character functions
	void UpdatePlayers ()
	{
	// Update all appropriate player objects
		foreach (GameObject player in Players)
		{
			// Set the animator speed
			//AIPath aiPath = player.GetComponent<AIPath>();
			/*if(aiPath.velocity != null)
			{ 
				anim.SetFloat("Speed", aiPath.velocity / aiPath.speed);
				anim.SetFloat("Speed", seek.GetCurrentPath().speed); 
				//print (seek.GetCurrentPath().speed);
			}*/
			//animation.SetFloat("Speed", h*h+v*v);
			
			// Change our control to if the Interpreter is active or not
			if (interpreter != null)
			{
				// If the interpreter is working actively, we can't control our character
				CharacterMotor motor = player.GetComponent<CharacterMotor>();
				if (interpreter.InterpreterActive()) 
				{ motor.SetControllable(false); }
				else if (cameraActive)
				{ motor.SetControllable(false); }
				else 
				{ motor.SetControllable(true); }
			}
			
			// Get the Animator
			Animator anim = player.GetComponent<Animator>();
			
			// Create and calculate our variables for animation
			Vector3 velocity;
			float cSpeed;
			velocity = player.GetComponent<CharacterController>().velocity;
			if (player.GetComponent<CharacterMotor>().MaxSpeedInDirection(velocity) > 0f)
			{ cSpeed = velocity.magnitude / player.GetComponent<CharacterMotor>().MaxSpeedInDirection(velocity); }
			else
			{ cSpeed = 0f; }
			
			// Set the variables needed for animation
			anim.SetFloat("Speed", cSpeed);
			anim.SetFloat("VSpeed", velocity.y);
			anim.SetBool("OnGround", player.GetComponent<CharacterMotor>().IsGrounded());
			anim.SetBool("Jumping", Player.GetComponent<CharacterMotor>().IsJumping());
			
			// Fix the weight on the Additional layers
			anim.SetLayerWeight(1, 1.0f);
			
			// Ensure the player has us set as their player controller
			//PlayerActionAI pAI = player.GetComponent<PlayerActionAI>();
			//pAI.pController = this;
			//pAI.UpdateSpeed();
			
			// If it's not the active player...
			/*
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
			*/
		}
	}
	
	// Checks user input and perform common actions
	void UpdateInput ()
	{
		// Check the user inputs
		
		// Fire1 == Run the SimpleCreator on the current Player GameObject
		if (Input.GetButtonDown("Fire1"))
		{
			SimpleCreator sc = Player.GetComponent<SimpleCreator>();
			if (sc != null) { sc.Create(0); }
		}
	}
	
	// Update our "Camera" status
	public void SetCameraStatus(bool active)
	{ cameraActive = active; }
}
