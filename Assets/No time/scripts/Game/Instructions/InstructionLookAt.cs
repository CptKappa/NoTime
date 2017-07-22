using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionLookAt :InstructionData
{
	public Vector2 lookAt;
	public float duration;
	public float timetaken = 0;
	public float endtime;
	public GameObject target;

	public InstructionLookAt (int TId, float t, float x, float y, float waitduration, float end = 0) : base (TId, t)
	{
		lookAt = new Vector2 (x, y);
		duration = waitduration;
		if (end != 0) {
			endtime = end;
		}
	}




}
