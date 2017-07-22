using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System;
using System.Runtime.Serialization;

public class InstructionManager
{
	/// <summary>
	/// there is a List of Instructions for each Character
	/// </summary>
	Dictionary<int,List<string>> Instructions = new Dictionary<int, List<string>> ();

	//try to update
	InstructionsDat dat = new InstructionsDat ();

	public InstructionManager ()
	{
		Debug.Log ("New InstructionManager");
	}

	public void LoadInstructions ()
	{
		while (!dat.EndOfStream) {			
			try {
				string line = dat.ReadLine ();
				if (line.Length < 1) {
					continue;
				}
				if (line [0] == '/') {
					continue;
				}
				int id = int.Parse (line.Split (';') [1]);
				if (!Instructions.ContainsKey (id)) {
					Instructions.Add (id, new List<string> ());
					Debug.Log ("Neuer Eintrag im Dict");
				} else {
					Debug.Log ("Eintrag in Dict schon vorhanden");
				}

				Instructions [id].Add (line);

			} catch (Exception ex) {
				Debug.LogError (ex);
			}
		}
		#region OutDated
		/*FileStream fs = new FileStream (path, FileMode.Open);

		while (fs.Position < fs.Length) {
			try {
				Debug.Log ("Deserializing an instance of the object");
				//XmlReaderSettings xmlsetting = new XmlReaderSettings ();

				/*Debug.Log (xmlsetting.CheckCharacters);
				Debug.Log (xmlsetting.ConformanceLevel);
				Debug.Log (xmlsetting.IgnoreWhitespace);
				Debug.Log (xmlsetting.NameTable);
				Debug.Log (xmlsetting.ProhibitDtd);
				Debug.Log (xmlsetting.ValidationFlags);
				Debug.Log (xmlsetting.ValidationType);

				XmlSerializer ser = new XmlSerializer (typeof(InstructionData[]));
				Debug.Log ("Serializer");
				//XmlReader reader = XmlReader.Create (fs, xmlsetting);
				//DataContractSerializer ser = new DataContractSerializer (new InstructionData (1f, 3).GetType ());
				InstructionData[] deser = ser.Deserialize (fs) as InstructionData[];
				Debug.Log ("Type of stuff" + deser.GetType ());

				Debug.Log (string.Format ("Target: {0} TriggerTime: {1}", deser [0].TargetId, deser [0].TriggerTime));
			} catch (Exception ex) {
				Debug.LogError ("[Instruction Loading Error]:");
				Debug.LogError (ex);
			} 
		}
		fs.Close ();*/
		#endregion
		#region alt
		/*Instructions = new Dictionary<int, List<InstructionData>> ();
		if (File.Exists (path)) {

			StringReader sr = new StringReader (path);
			sr.Read ();

			//FileStream fs = new FileStream (sr, FileMode.Open);
			//XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader (fs, new XmlDictionaryReaderQuotas ());
			XmlReader xmlr = XmlReader.Create (sr);
			//DataContractSerializer dcs = new DataContractSerializer (Instructions.Values.GetType ());
			try {
				
						
				InstructionData f = (InstructionData)xmlr.ReadContentAs (new InstructionData ().GetType, new IXmlNamespaceResolver ());
				if (Instructions.ContainsKey (f.TargetId)) {
					Instructions [f.TargetId].Add (f);
				} else {
					Instructions.Add (f.TargetId, new List<InstructionData> ());
					Instructions [f.TargetId].Add (f);
				}

			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
			} finally {
				reader.Close ();
				fs.Close ();
			}

		}*/
		#endregion
	}

	public InstructionData ConvertTextToInstruction (string text)
	{
		string[] dat = text.Split (';');
		switch (dat [0]) {
		case "dadat":			
			return new InstructionData (int.Parse (dat [1]), float.Parse (dat [2]));
		case "mvdat":
			return new InstructionMovement (int.Parse (dat [1]), float.Parse (dat [2]), new Vector2 (float.Parse (dat [3]), float.Parse (dat [4])), float.Parse (dat [5]));
		case "wadat":
			return new InstructionWait (int.Parse (dat [1]), float.Parse (dat [2]), float.Parse (dat [3]), float.Parse (dat [4]));
		case "ladat":
			return new InstructionLookAt (int.Parse (dat [1]), float.Parse (dat [2]), float.Parse (dat [3]), float.Parse (dat [4]), float.Parse (dat [5]), float.Parse (dat [6]));
		default:
			Debug.LogError ("Somehow i have no clue what Instruction that is");
			return null;
		}
	}

	public LinkedList<InstructionData> GetInstructionSet (Character sender)
	{
		if (Instructions.ContainsKey (sender.id)) {
			List<string> data = Instructions [sender.id];
			//converting the List to an Instruction List
			List<InstructionData> converted = new List<InstructionData> ();
			foreach (string item in data) {
				converted.Add (ConvertTextToInstruction (item));
			}
			converted.Sort ();
			LinkedList<InstructionData> linked = new LinkedList<InstructionData> (converted);
			/*Queue<InstructionData> value = new Queue<InstructionData> ();
		foreach (InstructionData item in converted) {
			value.Enqueue (item);
		}*/
			return linked;
		}
		return new LinkedList<InstructionData> ();
	}
}
