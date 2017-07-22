using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Vec3
{
	public float x;
	public float y;
	public float z;
}


[Serializable]
public class Data
{
	public int id;

	float x;
	float y;
	float rotation;

	//[NonSerialized]
	public Vector3 location {
		get{ return new Vector2 (x, y); }
		set {
			x = value.x;
			y = value.y;
		}
	}
	//[NonSerialized]
	public Vector3 orientation { get { return new Vector3 (0, 0, rotation); } set { rotation = value.z; } }

	public Data (Vector3 l, Vector3 or, int i)
	{
		location = l;
		orientation = or;
		id = i;

	}



}
