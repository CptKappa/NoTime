using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

	#region provides Movement for GameObjects without rigid bodies

	/// <summary>
	/// Move Gameobject in direction X and Y with a fixed rotationspeed
	/// </summary>
	/// <param name="xDir">X Movement</param>
	/// <param name="yDir">Y Movement</param>
	/// <param name="go">Gameobject</param>
	public static void Move (float xDir, float yDir, GameObject go)
	{
		Vector3 goal = new Vector3 (xDir * Time.deltaTime, yDir * Time.deltaTime, 0);
		go.transform.Translate (goal, Space.World);
		Vector3 target = new Vector3 (xDir + go.transform.position.x, yDir + go.transform.position.y, 0);

		Vector3 vectorToTarget = target - go.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, 15 * Time.deltaTime);

		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	/// <summary>
	/// Move Gameobject in direction X and Y with a variable rotationspeed
	/// </summary>
	/// <param name="xDir">X Movement</param>
	/// <param name="yDir">Y Movement</param>
	/// <param name="go">Gameobject</param>
	/// <param name="rot">Rotation</param>
	public static void Move (float xDir, float yDir, GameObject go, float rot)
	{
		Vector3 goal = new Vector3 (xDir * Time.deltaTime, yDir * Time.deltaTime, 0);
		go.transform.Translate (goal, Space.World);
		Vector3 target = new Vector3 (xDir + go.transform.position.x, yDir + go.transform.position.y, 0);

		Vector3 vectorToTarget = target - go.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, rot * Time.deltaTime);

		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	/// <summary>
	/// Move Gameobject in the Vectors direction with a fixed rotationspeed
	/// </summary>
	/// <param name="vec">Vector</param>
	/// <param name="go">GameObject</param>
	public static void Move (Vector2 vec, GameObject go)
	{
		Vector3 goal = new Vector3 (vec.x * Time.deltaTime, vec.y * Time.deltaTime, 0);
		go.transform.Translate (goal, Space.World);
		Vector3 target = new Vector3 (vec.x + go.transform.position.x, vec.y + go.transform.position.y, 0);

		Vector3 vectorToTarget = target - go.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, 15 * Time.deltaTime);

		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	/// <summary>
	/// Move Gameobject in the Vectors direction with variable rotationspeed
	/// </summary>
	/// <param name="vec">Vector</param>
	/// <param name="go">GameObject</param>
	/// <param name="rot">Rotation</param>
	public static void Move (Vector2 vec, GameObject go, float rot)
	{
		Vector3 goal = new Vector3 (vec.x * Time.deltaTime, vec.y * Time.deltaTime, 0);
		go.transform.Translate (goal, Space.World);
		Vector3 target = new Vector3 (vec.x + go.transform.position.x, vec.y + go.transform.position.y, 0);

		Vector3 vectorToTarget = target - go.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, rot * Time.deltaTime);

		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	/// <summary>
	/// Move the GameObject the speed towards the vector
	/// Use this for Waypoints and so on
	/// </summary>
	/// <param name="vec">Vector of target location</param>
	/// <param name="go">GameObject.</param>
	/// <param name="speed">Speed</param>
	public static void Move (Vector2 vec, float speed, GameObject go)
	{
		Vector3 vecToTarget = new Vector3 (vec.x - go.transform.position.x, vec.y - go.transform.position.y, 0);

		Vector3 goal = vecToTarget;
		goal.Normalize ();
		goal = goal * speed;
		goal = goal * Time.deltaTime;
		if (goal.magnitude > vecToTarget.magnitude) {
			go.transform.Translate (vecToTarget, Space.World);
		} else {
			go.transform.Translate (goal, Space.World);
		}
		Vector3 target = new Vector3 (vecToTarget.x + go.transform.position.x, vecToTarget.y + go.transform.position.y, 0);
		if (vecToTarget.magnitude > 0.1) {
			Vector3 vectorToTarget = target - go.transform.position;
			float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, 15 * Time.deltaTime);
		}
		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	#endregion

	#region provides movement for GameObject with rigidbodies

	public static void RMove (Vector2 vec, GameObject go)
	{
		Vector3 goal = new Vector3 (vec.x * Time.deltaTime, vec.y * Time.deltaTime, 0);
		//go.GetComponent<Rigidbody2D>().AddForce (goal,ForceMode2D.Impulse);
		go.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (goal.x + go.transform.position.x, goal.y + go.transform.position.y));
		Vector3 target = new Vector3 (vec.x + go.transform.position.x, vec.y + go.transform.position.y, 0);

		Vector3 vectorToTarget = target - go.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, 15 * Time.deltaTime);

		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	public static void RMove (Vector2 vec, float speed, GameObject go)
	{
		Vector3 vecToTarget = new Vector3 (vec.x - go.transform.position.x, vec.y - go.transform.position.y, 0);

		Vector3 goal = vecToTarget;
		goal.Normalize ();
		goal = goal * speed;
		goal = goal * Time.deltaTime;
		if (goal.magnitude > vecToTarget.magnitude) {
			go.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (vecToTarget.x + go.transform.position.x, vecToTarget.y + go.transform.position.y));
		} else {
			go.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (goal.x + go.transform.position.x, goal.y + go.transform.position.y));
		}
		Vector3 target = new Vector3 (vecToTarget.x + go.transform.position.x, vecToTarget.y + go.transform.position.y, 0);
		if (vecToTarget.magnitude > 0.1) {
			Vector3 vectorToTarget = target - go.transform.position;
			float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
			go.transform.rotation = Quaternion.Slerp (go.transform.rotation, q, 15 * Time.deltaTime);
		}
		//Debug.Log ("+" + Quaternion.Euler (vectorToTarget));
		//Debug.Log ("-"+angle);
		//Debug.Log (">"+q);
	}

	#endregion

	#region LookAtRegion

	/// <summary>
	/// Somebody should lay eyes on somebody (IS IT LOVE?)
	/// </summary>
	/// <param name="sender">who should be turned</param>
	/// <param name="POI"Point of interest (BADY DONT HURT ME!)</param>
	public static void LookAtObject (GameObject sender, GameObject POI)
	{
		Vector2 vectorToTarget = POI.transform.position - sender.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		sender.transform.rotation = Quaternion.Slerp (sender.transform.rotation, q, 15 * Time.deltaTime);
	}

	public static void LookAtObject (GameObject sender, Vector2 POI)
	{
		Vector2 vectorToTarget = POI - new Vector2 (sender.transform.position.x, sender.transform.position.y);
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		sender.transform.rotation = Quaternion.Slerp (sender.transform.rotation, q, 15 * Time.deltaTime);
	}

	#endregion



}