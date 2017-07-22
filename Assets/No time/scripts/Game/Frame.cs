using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Frame
{
	int _FrameID;

	public int FrameID{ get { return _FrameID; } private set { _FrameID = value; } }

	public List<Data> Info { get; set; }

	public Frame (int id)
	{
		FrameID = id;
		Info = new List<Data> ();
	}

	public Frame (int id, List<Data> data)
	{
		FrameID = id;
		Info = data;
	}

	public Frame (int id, Dictionary<int,Entity> data)
	{
		FrameID = id;
		Info = new List<Data> ();
		foreach (KeyValuePair<int,Entity> i in data) {
			Info.Add (i.Value.Save ());
		}
	}

}


