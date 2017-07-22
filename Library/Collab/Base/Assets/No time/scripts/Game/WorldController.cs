using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class WorldController : MonoBehaviour
{
	
	#region Data Storage

	public float currTime= 0;

	Dictionary<int,Entity> entities = new Dictionary<int, Entity> ();
	int _frameid = 0;

	int frameid { 
		get { return _frameid; } 
		set {
			_frameid = value;
		}
	}
	//double time= 0;
	//the word 'Break' refers to pause
	public GameObject BreakScreen;

	public Delegate stuff;

	public Sprite CharacterSprite;
	public Sprite PolizistSprite;
	public Sprite GeneralSprite;
	/// <summary>
	/// Here all Enitiy Updates will be added
	/// </summary>
	public event NoNoDel UpdateTick;

	FrameManager FManager = new FrameManager (@"Assets\No time\Data\History.dat");

	public string InstructionSource;

	InstructionManager IManager;

	public float TimeToSave;

	#endregion

	#region State Properties

	bool TimeRunning = true;
	bool _MenuPause;

	bool MenuPause {
		get{ return _MenuPause; }
		set {
			_MenuPause = value;
			if (_MenuPause) {				
				BreakScreen.SetActive (true);
			} else {
				BreakScreen.SetActive (false);
			}
		}
	}

	#endregion


	GameObject user;
	GameObject polize;
	GameObject General;
	float timer = 0;
	//Assets\stuff\Assets\anzug_glatze_sprite.png

	#region monobehaviour Stuff

	// Use this for initialization
	void Start ()
	{           
		currTime = 0;
		#region IntstructionManager
		IManager = new InstructionManager (InstructionSource);
		//IManager.SaveInstructions (new InstructionMovement (2f, 0, new Vector2 (4f, 4f), 3));

		//InstructionData[] idat = new InstructionData[1];
		//idat [0] = new InstructionMovement (2f, 4, new Vector2 (3, 21), 5f);
		//IManager.SaveInstructions (idat);
		IManager.LoadInstructions ();
		#endregion
		#region user
		SpriteRenderer renderer = new SpriteRenderer ();
        

		List<InputMovement> input = new List<InputMovement> ();
		input.Add (new InputMovement (KeyCode.W, new Vector2 (0f, 1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.S, new Vector2 (0f, -1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.D, new Vector2 (1f, 0f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.A, new Vector2 (-1f, 0f), PressTypes.KeyDownKey));

		GameObject player = new GameObject ("Player",typeof(SpriteRenderer),typeof(CircleCollider2D),typeof(Rigidbody2D));
	
		player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		player.GetComponent<Rigidbody2D>().mass = 1;
		player.GetComponent<Rigidbody2D>().drag = 1000;
		player.GetComponent<Rigidbody2D>().angularDrag = 10;
		player.GetComponent<Rigidbody2D>().gravityScale = 0;

		player.GetComponent<CircleCollider2D>().radius = 1.5f;
		player.GetComponent<SpriteRenderer> ().sprite = CharacterSprite;

		AddNewPlayer (player, input);
		//Instantiate(player);
		Debug.Log ("object created");
		#endregion
		#region polize


		GameObject c = new GameObject ("Polizist",typeof(SpriteRenderer),typeof(CircleCollider2D),typeof(Rigidbody2D));

		c.transform.position = new Vector2(-4f,-5f);
		c.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		c.GetComponent<Rigidbody2D>().mass = 2;
		c.GetComponent<Rigidbody2D>().drag = 1000;
		c.GetComponent<Rigidbody2D>().angularDrag = 10;
		c.GetComponent<Rigidbody2D>().gravityScale = 0;

		c.GetComponent<CircleCollider2D>().radius = 1.5f;
		c.GetComponent<SpriteRenderer> ().sprite = PolizistSprite;


		AddNewCharacter(c);

		//Instantiate(player);
		Debug.Log ("Character created");
		#endregion
		#region general

		GameObject g = new GameObject ("General", typeof(SpriteRenderer),typeof(CircleCollider2D),typeof(Rigidbody2D));
		g.transform.position = new Vector2(8,8);
		g.GetComponent<SpriteRenderer> ().sprite = GeneralSprite;

		g.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		g.GetComponent<Rigidbody2D>().mass = 2;
		g.GetComponent<Rigidbody2D>().drag = 1000;
		g.GetComponent<Rigidbody2D>().angularDrag = 10;
		g.GetComponent<Rigidbody2D>().gravityScale = 0;

		g.GetComponent<CircleCollider2D>().radius = 1.5f;

		AddNewCharacter(g);

		//Instantiate(player);
		Debug.Log ("Character created");
		#endregion
		//kann benutzt werden um slowmos zu machen und so
		//einfach Zeitlauf verlangsamen
		//Debug.Log (Time.timeScale);

	}

	// Update is called once per frame
	void Update ()
	{
		Buttons ();
		//running all updates
		if (TimeRunning && UpdateTick != null) {
			//I call my own Update <3
			if (UpdateTick!=null) {
				UpdateTick ();
			}
			timer = timer + Time.deltaTime;
			currTime = currTime + Time.deltaTime;
			if (timer >= TimeToSave) {
				FManager.ExportByEntityData (entities);
				timer = 0;
			}
		}
		//Debug.Log ("Log");

	}

	#endregion


	#region Methodes

	void Buttons ()
	{
		if (MenuPause) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				float ident = BreakScreen.transform.GetComponentInChildren<Slider> ().value;

				//wir wollen ja nichts immer laden
				Frame f = FManager.GetFrameByID ((int)ident);
				LoadFrame (f);
			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			//Stop and start time
			TogglePause ();

			//Serialize ();
		}

	}

	void TogglePause ()
	{
		Slider slider = BreakScreen.transform.GetComponentInChildren<Slider> ();
		if (MenuPause) {
			FManager.StopPause ();
			MenuPause = false;
			TimeRunning = true;
			FManager.RevertToFrameID ((int)slider.value);
		} else {
			FManager.StartPause ();
			slider.maxValue = FManager.GetHighestID ();
			slider.value = slider.maxValue;
			MenuPause = true;
			TimeRunning = false;
		}
		Debug.Log ("TogglePause");
	}

	public void UpdateUI ()
	{
		Text step = BreakScreen.transform.GetComponentInChildren<Text> ();
		Slider slide = BreakScreen.transform.GetComponentInChildren<Slider> ();


		step.text = (slide.value * TimeToSave).ToString ();
	}

	/// <summary>
	/// Loads the frame f.
	/// This pastes the information of the Frame
	/// into the coresponding Entity
	/// </summary>
	/// <param name="f">Frame that should be loaded</param>
	//ToDo make it more efficient
	void LoadFrame (Frame f)
	{
		//this reverts the current time
		currTime = f.FrameID * TimeToSave;
		Debug.Log ("Load Frame Initialized");
		foreach (Data data in f.Info) {
			try {
				//Debug.Log("entwered Try at Load Frame");
				Entity c = entities [data.id];
				//Debug.Log("entity snached");
				c.Load (data,f.FrameID*TimeToSave);
				//Debug.Log("Entity succesfully Loaded Data");
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				//Debug.Log ("Stuff happened");
			}
		}
		Debug.Log ("Loaded Frame " + f.FrameID);
	}


	public void AddNewEntity (GameObject go)
	{        
		Entity c = new Entity (go, this, entities.Count);
		entities.Add (entities.Count, c);
	}

	public void AddNewCharacter (GameObject go)
	{        
		Entity c = new Character (go, this, entities.Count, IManager);

		entities.Add (entities.Count, c);
	}

	public void AddNewPlayer (GameObject go, List<InputMovement> move)
	{
		Entity c = new Player (go, this, entities.Count, move);
		entities.Add (entities.Count, c);
	}

	public void OnSlideChange ()
	{
		float ident = BreakScreen.transform.GetComponentInChildren<Slider> ().value;
		if (MenuPause) {
			Frame f = FManager.GetFrameByID ((int)ident);
			LoadFrame (f);
		}
	}

	#endregion




}