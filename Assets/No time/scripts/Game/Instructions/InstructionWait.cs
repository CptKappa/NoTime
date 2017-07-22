using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionWait:InstructionData
{

	public float duration;
	public float timetaken = 0;
	public float endtime;

	/// <summary>
	/// Initializes a new instance of the <see cref="InstructionWait"/> class.
	/// </summary>
	/// <param name="TId">Target Id</param>
	/// <param name="t">Trigger Time</param>
	/// <param name="waitduration">how long to wait (value=0 disables this)</param>
	/// <param name="end">when it should end (this Value will automatically be set on first start, but can be set manually)</param>
	public InstructionWait (int TId, float t, float waitduration, float end = 0) : base (TId, t)
	{
		duration = waitduration;
		if (end != 0) {
			endtime = end;
		}
	}
}
