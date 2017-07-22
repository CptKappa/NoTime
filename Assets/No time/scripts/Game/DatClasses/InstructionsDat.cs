using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstructionsDat
{
	string[] text = {
		//start polizist 1
		"mvdat;1;4;12;9;3",
		"mvdat;1;6;-3;9;10",
		"mvdat;1;11;6;-4;3",
		//start polizist 2
		"mvdat;2;2;-8.7;-10.3;1.5",
		"ladat;2;2;-7;-10;2;0",
		"mvdat;2;4;8.7;-10;2.5",
		"mvdat;2;5;8.7;7.7;1.5",
		"mvdat;2;10;17.6;13.6;2",
		"mvdat;2;10;60;13.6;5",
		//start Polizist 3
		"mvdat;3;1;-13;-11;3",
		"mvdat;3;15;-13;18;2",
		//start general
		"mvdat;4;10;0;0;2",
		//Passant 1
		"mvdat;5;15;11;26;2",
		//Passant 2
		"mvdat;6;15;-12.5;26;2",
		//Passant 3
		"mvdat;7;10;11.5;44;3",
		//Passant 4
		"mvdat;8;15;-13;44;2",
		/*"",
		"",
		"",
		"",
		"",*/
	};
	int currLine;

	public bool EndOfStream {
		get {
			if (currLine == text.Length) {
				return true;
			} else {
				return false;
			}
		}
	}


	public InstructionsDat ()
	{
		currLine = 0;
	}

	public string ReadLine ()
	{
		if (currLine == text.Length) {
			throw new IndexOutOfRangeException ();
		}
		string line = text [currLine];
		currLine++;
		Debug.Log ("gelesen");
		return line;
	}






}
