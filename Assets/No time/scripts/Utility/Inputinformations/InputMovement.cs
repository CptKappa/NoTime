using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This File contains all the Input Information Classes
//These contain Keys + Something additional

/// <summary>
/// In this Class you get a Key + the coresponding direction
/// </summary>
public class InputMovement {

	public PressTypes Press;
	public KeyCode Key{ get; set;}
	public Vector2 Direction;

	//The Directions
	public float X {get{return Direction.x;}}
	public float Y {get{return Direction.y;}}

	public InputMovement(KeyCode k, Vector2 dir,PressTypes type){
		Key = k;
		Direction = dir;
		Press = type;
	}

	public Vector2 Check(){
		//Debug.Log (Press);
		switch (Press) {
		case PressTypes.Key:
			if (Input.GetKey(Key)) {
				return Direction;
			}
			break;
		case PressTypes.KeyDown:
			if (Input.GetKeyDown(Key)) {
				return Direction;
			}
			break;
		case PressTypes.KeyUp:
			if (Input.GetKeyUp(Key)) {
				return Direction;
			}
			break;
		case PressTypes.KeyDownKey:
			if (Input.GetKey(Key)||Input.GetKeyDown(Key)) {
				return Direction;
			}
			break;
		}
		return new Vector2();
	}

}
