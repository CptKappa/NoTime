using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// this Class is for UpdateHandeling in a different thread
/// </summary>
public class UpdateHandeler
{
	//public Queue<bool> UpdateRequest = new Queue<bool> ();
	float timeout = 0;

	public event NoNoDel ToUpdate;

	public void AddToUpdate (NoNoDel stuff)
	{
		ToUpdate += stuff;
		Debug.Log ("Update added");
	}

	public void StartRunning ()
	{
		
		Debug.Log ("some update");

		if (ToUpdate != null) {
			ToUpdate ();
			Debug.Log ("UPDATE");

			timeout = 0;
		} else {
			timeout += 0.5f;
			System.Threading.Thread.Sleep (100);
		}


		Debug.Log ("Handler died");
	}




}
