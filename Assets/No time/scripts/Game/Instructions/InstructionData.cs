using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

/// <summary>
/// This class is pure Data and no functionality
/// It is used to Save Instructions for Characters
/// </summary>
//[DataContract (Name = "Data", Namespace = "No Time")]
public class InstructionData : IComparable
{

	//there is something!
	//[DataMember ()]
	//public float TriggerTime{ get; set; }
	public float TriggerTime;
	//[DataMember ()]
	//public int TargetId{ get; set; }
	public int TargetId;

	string[] args;

	public bool done = false;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionData"/> class.
	/// </summary>
	/// <param name="t">When the Instruction should be Triggered</param>
	/// <param name="TId">Target identifier. Who should do the Instruction</param>
	public InstructionData (int TId, float t)
	{
		TriggerTime = t;
		TargetId = TId;
	}

	public InstructionData (int TId, float t, string[] args)
	{
		TriggerTime = t;
		TargetId = TId;
	}

	public InstructionData (string data)
	{
		string[] stuff = data.Split (';');
		TargetId = int.Parse (stuff [1]);
		TriggerTime = float.Parse (stuff [2]);
	}


	public override string ToString ()
	{
		return string.Format ("Data: TargetId={0} TriggerTime={1}", TargetId, TriggerTime);
	}

	public int CompareTo (object obj)
	{
		if (obj == null)
			return 1;

		InstructionData other = obj as InstructionData;
		if (other != null)
			return this.TriggerTime.CompareTo (other.TriggerTime);
		else
			throw new ArgumentException ("Object is not a Temperature");
	}

}
