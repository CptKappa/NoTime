using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This is the Class of all Character that are not controlled by a player
/// They receive 'Instructions' from the Instruction Manager
/// Multiple 'Instructions' can be Queued up at once
/// </summary>
public class Character : Entity
{
	InstructionManager manager;
	
	public LinkedList<InstructionData> Instructions = new LinkedList<InstructionData> ();
	public InstructionData currentInstruction;
	//float is when it was finished, Instruction what was finished
	public Stack<TwoValuePair<float,InstructionData>> DoneInstructions = new Stack<TwoValuePair<float, InstructionData>> ();

	public Character (GameObject obj, WorldController wc, int i, InstructionManager m) : base (obj, wc, i)
	{		
		manager = m;
		Instructions = manager.GetInstructionSet (this);
		if (Instructions.Count > 0) {
			currentInstruction = Instructions.First.Value;
			Instructions.RemoveFirst ();
		}
	}

	public override void Update ()
	{
		if (currentInstruction == null && Instructions.Count > 0) {
			currentInstruction = Instructions.First.Value;
			Instructions.RemoveFirst ();
		}

		//TODO: do the next instruction here
		if (currentInstruction != null) {
			if (currentInstruction.TriggerTime < world.currTime) {
				//yes, now what instruction am i looking at?
				if (currentInstruction is InstructionMovement) {
					ExecMovementInst ();
				} else {
					if (currentInstruction is InstructionWait) {
						ExecWaitInst ();
					} else {
						if (currentInstruction is InstructionLookAt) {
							ExecLookAttInst ();
						} else {
							if (currentInstruction is InstructionData) {
								ExecDataInst ();
							}
						}
					}
				}

			}
		}
		//Check if i should do the instruction

		//TODO: Check if next Instruction ist up

		//TODO: If so execute curesponding to it

		//TODO: Do some more commentating about not excisting stuff


	}
	//we know that currentInstruction is of type movement:
	void ExecMovementInst ()
	{
		InstructionMovement inst = currentInstruction as InstructionMovement;
		//calculat the vector *scrapped* the class does that for us
		//Debug.Log ("I want to do something");

		Utilities.RMove (inst.waypoint, inst.speed, entity);
		if (entity.transform.position == inst.waypoint) {
			DoneInstructions.Push (new TwoValuePair<float, InstructionData> (world.currTime, currentInstruction));
			currentInstruction = null;
			Debug.Log ("Added Instruction To done");
		}
	}

	void ExecDataInst ()
	{
		//I don't think we should do anything here
		//but it makes it more understandable
	}

	void ExecWaitInst ()
	{
		InstructionWait wait = currentInstruction as InstructionWait;

		if (wait.duration != -1 && wait.endtime == 0) {
			wait.endtime = world.currTime + wait.duration;
		}
		/*if (wait.duration>0) {
			wait.timetaken += Time.deltaTime;
		}*/
		if (wait.endtime != 0 && wait.endtime < world.currTime) {
			
			DoneInstructions.Push (new TwoValuePair<float, InstructionData> (world.currTime, currentInstruction));
			currentInstruction = null;
			Debug.Log ("Added Instruction To done");
		}
	}

	void ExecLookAttInst ()
	{
		InstructionLookAt lookat = currentInstruction as InstructionLookAt;

		if (lookat.duration != -1 && lookat.endtime == 0) {
			lookat.endtime = world.currTime + lookat.duration;
		}
		/*if (wait.duration>0) {
			wait.timetaken += Time.deltaTime;
		}*/

		if (lookat.target != null) {
			Utilities.LookAtObject (entity, lookat.target);
		} else {
			Utilities.LookAtObject (entity, lookat.lookAt);
		}
			


		if (lookat.endtime != 0 && lookat.endtime < world.currTime) {

			DoneInstructions.Push (new TwoValuePair<float, InstructionData> (world.currTime, currentInstruction));
			currentInstruction = null;
			Debug.Log ("Added Instruction To done");
		}


	}


	public override void Load (Data d, float t)
	{
		base.Load (d, t);
		Debug.Log ("Now starting Time Revert thingi");
		RevertInstructionsToTime (t);
	}

	void RevertInstructionsToTime (float t)
	{
		if (DoneInstructions.Count > 0) {
			while (DoneInstructions.Count > 0 && DoneInstructions.Peek ().Value1 > t) {
				if (currentInstruction != null) {
					Instructions.AddFirst (currentInstruction);
				}
				currentInstruction = DoneInstructions.Pop ().Value2;
			}
		}
	}

	/// <summary>
	/// Interrupt of current instruction because of an interaction
	/// </summary>
	/// <param name="sender">Sender.</param>
	public void InteractionInteruptStart (Entity sender)
	{
		//splitting the last Instruction
		//we have to do different things depending on what instruction currently happens
		if (currentInstruction != null) {
			if (currentInstruction is InstructionMovement) {
				InstructionMovement full = currentInstruction as InstructionMovement;

				Instructions.AddFirst (full);
				DoneInstructions.Push (new TwoValuePair<float, InstructionData> (world.currTime, new InstructionMovement (full.TargetId, full.TriggerTime, entity.transform.position, full.speed)));
				//now it is just an infinitif wait.
			}
		}
		currentInstruction = new InstructionLookAt (id, world.currTime, 4, 5, -1, 0);
		(currentInstruction as InstructionLookAt).target = sender.entity;

	}

	public void InteractionInteruptEnd (Entity sender)
	{
		currentInstruction = null;



	}
}
