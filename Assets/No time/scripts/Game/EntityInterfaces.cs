using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IEntity 
{
	int id{ get; }
	GameObject entity{ get; set;}
	WorldController world{ get; set; }

	Data Save ();
	void Load (Data d,float t);
}

/// <summary>
/// Interface for all controllable Entities
/// </summary>
public interface IControllable{
	/// <summary>
	/// Attach methode to a Buttonpress.
	/// </summary>
	/// <param name="key">The key you want it to be attached to</param>
	/// <param name="methode">The Methode that should be triggered</param>
	void RegisterKeys(KeyCode key, Action methode);
	/// <summary>
	/// Remove a Methode from a key.
	/// </summary>
	/// <param name="key">The Key where the Methode got attached</param>
	/// <param name="methode">The Methode you want to remove</param>
	void UnregisterKeys(KeyCode key, Action methode);
}



