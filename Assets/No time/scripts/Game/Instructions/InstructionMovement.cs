using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

//[DataContract (Name = "Data", Namespace = "No Time")]
public class InstructionMovement : InstructionData
{

	//another updates
	public Vec3 _waypoint;
	//The Target Location of this Instruction
	public Vector3 waypoint {
		get{ return new Vector3 (_waypoint.x, _waypoint.y, _waypoint.z); }
		set {
			_waypoint.x = value.x;
			_waypoint.y = value.y;
			_waypoint.z = value.z;
		}
	}
	//How fast?

	public float speed;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionMovement"/> class.
	/// </summary>
	/// <param name="t">Trigger Time</param>
	/// <param name="TId">Target identifier.</param>
	/// <param name="target">Target Location</param>
	/// <param name="s">speed</param>
	public InstructionMovement (int TId, float t, Vector3 target, float s) : base (TId, t)
	{
		waypoint = target;
		speed = s;
	}




	public override string ToString ()
	{
		return string.Format ("[InstructionMovement: {3} waypoint={0}, speed={1}]", waypoint, speed, base.ToString ());
	}




}
