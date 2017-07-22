using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImportDat
{
	string[] text = {
		"Polizist1st;1;2;1000;10;1.5;39.5;9",
		"Polizist2st;2;2;1000;10;1.5;-4;-5",
		"Polizist3st;3;2;1000;10;1.5;38;-11",
		"General;4;2;1000;10;1.5;8;8",
		"Passant1;5;1.5;1000;10;1.5;-11;26",
		"Passant2;6;1.5;1000;10;1.5;-12;45",
		"Passant3;7;1.5;1000;10;1.5;-41;28",
		"Passant4;8;1.5;1000;10;1.5;10;43"
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


	public ImportDat ()
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
		return line;
	}





}
