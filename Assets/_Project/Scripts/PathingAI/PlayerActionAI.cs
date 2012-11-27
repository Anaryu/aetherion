using UnityEngine;
using System.Collections;

public class PlayerActionAI : MonoBehaviour {
	
	// Public variables
	public float Speed = 10f;
	public float VisionDistance = 10f;
	public float HearingDistance = 10f;
	public GameObject HostileTarget;
	public float AttackRange = 2f;
	public float FollowRange = 10f;
	public PlayerController pController;
	
	// Private variables

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Update speed
	public void UpdateSpeed()
	{
		// Calculate the speed and set it
		float speed = Speed;
		this.GetComponent<AIPath>().speed = speed;
		this.GetComponent<AIFollow>().speed = speed;
	}
	
	// Perform active player logic
	public void UpdateActiveAI()
	{
		// Ensure AIPath is activated and we're following the cursor
		this.GetComponent<AIPath>().canMove = true;
		this.GetComponent<AIPath>().canSearch = true;
		this.GetComponent<AIPath>().target = pController.Cursor.transform;
	}
	
	// Perform targeting logic
	public void UpdateCombatAI()
	{
	}
	
	// Perform the logic for our pathing AI
	public void UpdatePathAI()
	{
		// Return false if the player controller isn't set yet
		if (pController == null) { return; }
		
		// If we have a hostile target, attack it!
		if (HostileTarget != null)
		{
			// Ensure we're not using AIPath
			this.GetComponent<AIPath>().canMove = false;
			this.GetComponent<AIPath>().canSearch = false;
			
			// Follow the target and set our distances
			this.GetComponent<AIFollow>().canMove = true;
			this.GetComponent<AIFollow>().canSearch = true;
			this.GetComponent<AIFollow>().pickNextWaypointDistance = AttackRange;
			this.GetComponent<AIFollow>().target = HostileTarget.transform;
		}
		// Otherwise, start following the player again
		else
		{
			// Ensure we're not using AIPath
			this.GetComponent<AIPath>().canMove = false;
			this.GetComponent<AIPath>().canSearch = false;
			
			// Follow the target and set our distances
			this.GetComponent<AIFollow>().canMove = true;
			this.GetComponent<AIFollow>().canSearch = true;
			this.GetComponent<AIFollow>().pickNextWaypointDistance = FollowRange;
			this.GetComponent<AIFollow>().target = pController.Player.transform;
		}
	}
}
