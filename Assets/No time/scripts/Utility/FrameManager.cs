using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using System;

/// <summary>
/// For all your Frame-needs
/// </summary>
public class FrameManager
{
	//References and data. Just stuff
	FrameFormatter Formatter;
	//The thread that manages the the frameformatter
	Thread formatthread;
	WorldController Controller;
	List<Frame> _Frames;


	float savetime;

	//FrameIDs are automatically asigned
	//they start at 0 and only contain hole numbers

	/// <summary>
	/// List of all frames
	/// is kept up to date
	/// </summary>
	public List<Frame> Frames {
		get { 
			if (upToDate) {
				return _Frames;
			} else {
				_Frames = Formatter.DeSerialize ();
				upToDate = true;
				return _Frames;
			}
		}
	}

	/// <summary>
	/// Shows if the Framelist ist up to date
	/// in order to reduce quantity of IOs
	/// </summary>
	bool upToDate;

	/// <summary>
	/// Gets the framecount
	/// shorter than over the list
	/// </summary>
	public int Framecount { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FrameManager"/> class.
	/// </summary>
	/// <param name="p">Path to Framestore File</param>
	public FrameManager (string p)
	{
		ProcessThreadCollection alloldones = Process.GetCurrentProcess ().Threads;
		//foreach (Thread item in alloldones) {
		//	if (item.IsBackground == true) {
		//		item.Abort ();
		//	}
		//}

		Framecount = 0;

		Formatter = new FrameFormatter (p);
		Formatter.DelPath ();
		Formatter.running = true;
		formatthread = new Thread (new ThreadStart (Formatter.RunAutoSerialization));
		formatthread.Name = "Formatter";
		formatthread.IsBackground = true;
		formatthread.Start ();
	}

	/// <summary>
	/// Exports Frame f to File
	/// </summary>
	/// <param name="f">Frame to Export</param>
	public void Export (Frame f)
	{
		upToDate = false;
		Formatter.AddFrameToQueue (f);
	}

	/// <summary>
	/// Exports a list of Frames to file
	/// </summary>
	/// <param name="fs">Framelist</param>
	public void Exportlist (List<Frame> fs)
	{
		upToDate = false;
		foreach (Frame item in fs) {
			Export (item);
		}
	}

	/// <summary>
	/// Exports a int/Character Dict to a file as Frame
	/// </summary>
	/// <param name="data">The Dict that should be exported</param>
	public void ExportByEntityData (Dictionary<int,Entity> data)
	{
		if (!formatthread.IsAlive) {
			formatthread = new Thread (Formatter.RunAutoSerialization);
			formatthread.Start ();
		}
		Frame DataToSave = new Frame (Framecount, data);
		Framecount++;
		Frames.Add (DataToSave);
		upToDate = true;
		Export (DataToSave);
	}

	/// <summary>
	/// Gets the frame by I.
	/// </summary>
	/// <returns>The frame by I.</returns>
	/// <param name="id">Identifier.</param>
	public Frame GetFrameByID (int id)
	{
		foreach (Frame frame in Frames) {
			if (frame.FrameID == id) {
				return frame;
			}
		}
		return null;
	}

	/// <summary>
	/// Gets the highest ID of all previous Frames.
	/// </summary>
	/// <returns>The highest ID</returns>
	public int GetHighestID ()
	{
		int max = 0;
		foreach (Frame item in Frames) {
			if (item.FrameID > max) {
				max = item.FrameID;
			}
		}
		return max;
	}

	/// <summary>
	/// Reverts to the frame with a specific ID.
	/// </summary>
	/// <param name="ID">FrameID of frame</param>
	public void RevertToFrameID (int Id)
	{
		Framecount = Id + 1;
		Formatter.AddFrameToQueue (new Frame (-Id));
		upToDate = false;
	}

	public void StartPause ()
	{
		Formatter.running = false;
		/*formatthread.Abort ();*/
	}

	public void StopPause ()
	{
		Formatter.running = true;
		formatthread = new Thread (new ThreadStart (Formatter.RunAutoSerialization));
		formatthread.Name = "Formatter";
		formatthread.IsBackground = true;
		formatthread.Start ();
	}



}
