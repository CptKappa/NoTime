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

	public float currTime = 0;

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

	public GameObject Highlight;

	public Delegate stuff;

	public Sprite[] CharacterSprites;

	/// <summary>
	/// Here all Enitiy Updates will be added
	/// </summary>
	public event NoNoDel UpdateTick;

	FrameManager FManager = new FrameManager ("History.dat");

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

	float timer = 0;
	//Assets\stuff\Assets\anzug_glatze_sprite.png

	#region monobehaviour Stuff

	// Use this for initialization
	void Start ()
	{           
		currTime = 0;
		#region IntstructionManager
		IManager = new InstructionManager ();
		//IManager.SaveInstructions (new InstructionMovement (2f, 0, new Vector2 (4f, 4f), 3));

		//InstructionData[] idat = new InstructionData[1];
		//idat [0] = new InstructionMovement (2f, 4, new Vector2 (3, 21), 5f);
		//IManager.SaveInstructions (idat);
		IManager.LoadInstructions ();
		#endregion
		#region user

		List<InputMovement> input = new List<InputMovement> ();
		input.Add (new InputMovement (KeyCode.W, new Vector2 (0f, 1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.S, new Vector2 (0f, -1f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.D, new Vector2 (1f, 0f), PressTypes.KeyDownKey));
		input.Add (new InputMovement (KeyCode.A, new Vector2 (-1f, 0f), PressTypes.KeyDownKey));

		GameObject player = new GameObject ("Player", typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D));

		player.GetComponent<Rigidbody2D> ().isKinematic = true;
		player.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
		player.GetComponent<Rigidbody2D> ().mass = 1;
		player.GetComponent<Rigidbody2D> ().drag = 1000;
		player.GetComponent<Rigidbody2D> ().angularDrag = 10;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0;

		player.GetComponent<CircleCollider2D> ().radius = 1.5f;
		player.GetComponent<SpriteRenderer> ().sprite = CharacterSprites [0];

		AddNewPlayer (player, input);
		//Instantiate(player);
		Debug.Log ("object created");
		#endregion 

		new Spawner (this);

		/*
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
*/
		//kann benutzt werden um slowmos zu machen und so
		//einfach Zeitlauf verlangsamen
		//Debug.Log (Time.timeScale);
		Camera.main.gameObject.GetComponent<Camera_Behaivour> ().target = entities [0].entity.transform;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		Buttons ();
		//running all updates
		if (TimeRunning && UpdateTick != null) {
			//I call my own Update <3
			if (UpdateTick != null) {
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

	void FixedUpdate ()
	{
		
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
			Highlight.SetActive (true);
			FManager.StopPause ();
			MenuPause = false;
			TimeRunning = true;
			FManager.RevertToFrameID ((int)slider.value);
		} else {
			Highlight.SetActive (false);
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
				c.Load (data, f.FrameID * TimeToSave);
				//Debug.Log("Entity succesfully Loaded Data");
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				//Debug.Log ("Stuff happened");
			}
		}
		Debug.Log ("Loaded Frame " + f.FrameID);
	}

	/// <summary>
	/// Adds the new entity.
	/// </summary>
	/// <param name="go">GameObject</param>
	public void AddNewEntity (GameObject go)
	{        
		Entity c = new Entity (go, this, entities.Count);
		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Adds the new character.
	/// </summary>
	/// <param name="go">GameObject</param>
	public void AddNewCharacter (GameObject go)
	{        
		Entity c = new Character (go, this, entities.Count, IManager);

		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Adds the new player.
	/// </summary>
	/// <param name="go">GameObject</param>
	/// <param name="move">Movement Inputs</param>
	public void AddNewPlayer (GameObject go, List<InputMovement> move)
	{
		Entity c = new Player (go, this, entities.Count, move);
		entities.Add (entities.Count, c);
	}

	/// <summary>
	/// Raises the slide change event.
	/// </summary>
	public void OnSlideChange ()
	{
		float ident = BreakScreen.transform.GetComponentInChildren<Slider> ().value;
		if (MenuPause) {
			Frame f = FManager.GetFrameByID ((int)ident);
			LoadFrame (f);
		}
	}

	/// <summary>
	/// Gets the entity by game object.
	/// </summary>
	/// <returns>The entity by game object.</returns>
	/// <param name="Go">GameObject</param>
	public Entity GetEntityByGameObject (GameObject Go)
	{
		foreach (KeyValuePair<int,Entity> item in entities) {
			if (item.Value.entity == Go) {
				return item.Value;
			}
		}
		return null;
	}

	#endregion
}