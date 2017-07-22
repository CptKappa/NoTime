using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// this class is for controllable Player Entities
/// </summary>
public class Player : Entity
{

	public List<InputMovement> Movement;
	GameObject highlight;
	bool inInteraction = false;
	//the Gameobject you can interact with
	GameObject _partner;
	Vector2 oldSpeed;
	float fastMove = 1;

	GameObject interactionpartner {
		get { return _partner; } 
		set {
			if (_partner != value) {
				Debug.Log ("partnerInteraction");
				_partner = value;
				partnerInteraction = world.GetEntityByGameObject (interactionpartner);

			}
		}
	}

	Entity partnerInteraction;

	public Player (GameObject obj, WorldController wc, int i, List<InputMovement> move) : base (obj, wc, i)
	{
		Movement = move;
		//Debug.Log ("Player Created");
		highlight = wc.Highlight;
		highlight.SetActive (false);

	}


	public override void Update ()
	{
		//Debug.Log ("WE UPDATED");
		#region move Player
		//Checking how to move
		Vector2 vec = new Vector2 ();
		/*foreach (InputMovement move in Movement) {
			vec = vec + move.Check ();
		}*/

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			fastMove = 3;
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			fastMove = 1;
		}

		if (Input.GetKey (KeyCode.A) || Input.GetKeyDown (KeyCode.A)) {
			vec.x -= 1;
		}
		if (Input.GetKey (KeyCode.W) || Input.GetKeyDown (KeyCode.W)) {
			vec.y += 1;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKeyDown (KeyCode.S)) {
			vec.y -= 1;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKeyDown (KeyCode.D)) {
			vec.x += 1;
		}
		vec.Normalize ();
		//	Debug.Log (vec);
		/*if (oldSpeed != vec) {
			Debug.Log ("Velocity Change");
			//Utilities.RMove (vec, 4, entity);
			entity.GetComponent<Rigidbody2D> ().velocity = vec * 5;
			if (vec == new Vector2 ()) {
				entity.GetComponent<Rigidbody2D> ().drag = 1000;
			} else {
				entity.GetComponent<Rigidbody2D> ().drag = 0;
			}
			oldSpeed = vec;
		}*/


		if (vec != new Vector2 ()) {
			Utilities.Move (vec * 5 * fastMove, this.entity);
		}
		//Debug.Log(vec);
		#endregion
		#region Interaction
		if (Input.GetKeyDown (KeyCode.E) && highlight.activeSelf) {


			if (!inInteraction) {
				(partnerInteraction as Character).InteractionInteruptStart (this);

				/*(partnerInteraction as Character).Instructions.AddFirst((partnerInteraction as Character).currentInstruction);
				(partnerInteraction as Character).currentInstruction=new InstructionWait(partnerInteraction.id,world.currTime,-1);*/
				inInteraction = true;
			} else {
				(partnerInteraction as Character).InteractionInteruptEnd (this);
				/*//(((partnerInteraction) as Character).currentInstruction as InstructionWait).duration = world.currTime-(((partnerInteraction) as Character).currentInstruction as InstructionWait).TriggerTime;
				(((partnerInteraction) as Character).currentInstruction as InstructionWait).duration =0;//(((partnerInteraction) as Character).currentInstruction as InstructionWait).duration;*/
				inInteraction = false;
			}
		}
		#endregion
		#region near us
		//who is near us?
		//List<GameObject> nearus = new List<GameObject> ();
		Collider2D[] colliders = Physics2D.OverlapCircleAll (entity.transform.position, 4f);
		List<Collider2D> coll = new List<Collider2D> (colliders);
		coll.Remove (entity.GetComponent<CircleCollider2D> ());
		if (coll.Count > 0) {
			interactionpartner = coll [0].gameObject;
			highlight.SetActive (true);
			highlight.transform.position = coll [0].transform.position;
			return;
		}
		highlight.SetActive (false);
		#endregion
	}
}
