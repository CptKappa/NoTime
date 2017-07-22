using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Spawn all Characters at the beginning of the game
/// </summary>
public class Spawner
{
	ImportDat dat = new ImportDat ();
	WorldController world;

	public Spawner (WorldController wc)
	{
		world = wc;
		SummonAll ();
	}

	void SummonAll ()
	{
		ImportDat sr = dat;
		while (!sr.EndOfStream) {
			string line = sr.ReadLine ();
			if (line [0] == '/' || line.Length < 1) {
				continue;
			}
			string[] parts = line.Split (';');
			GameObject go = new GameObject (parts [0], typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D));
			go.transform.position = new Vector2 (float.Parse (parts [6]), float.Parse (parts [7]));
			go.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
			go.GetComponent<Rigidbody2D> ().mass = float.Parse (parts [2]);
			go.GetComponent<Rigidbody2D> ().drag = float.Parse (parts [3]);
			go.GetComponent<Rigidbody2D> ().angularDrag = float.Parse (parts [4]);
			go.GetComponent<Rigidbody2D> ().gravityScale = 0f;
			go.GetComponent<CircleCollider2D> ().radius = float.Parse (parts [5]);
			go.GetComponent<SpriteRenderer> ().sprite = world.CharacterSprites [int.Parse (parts [1])];
			world.AddNewCharacter (go);
		}
	}



}
