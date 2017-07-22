using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class WorldController : MonoBehaviour {
    
	Dictionary<int,Character> entities = new Dictionary<int, Character>();
	int frameid = 0;
	//double time= 0;

    GameObject user;
    //int timer = 0;
    //Assets\stuff\Assets\anzug_glatze_sprite.png
    // Use this for initialization
    void Start () {
        string file = @"Assets\stuff\Assets\anzug_glatze_sprite.png";
        
        if (File.Exists(file))
        {
            Texture2D text = new Texture2D(2, 2);
            byte[] data = File.ReadAllBytes(file);
            text.LoadImage(data);
            Sprite sprite = Sprite.Create(text, new Rect(0f, 0f, text.width, text.height), new Vector2(0.5f, 0.5f),32);
            
            SpriteRenderer renderer = new SpriteRenderer();
            
            GameObject player = new GameObject("Player",renderer.GetType());
            
            player.GetComponent<SpriteRenderer>().sprite=sprite;
            user = player;
            AddNewEntity(player);
            //Instantiate(player);
            Debug.Log("object created");
        }
        

    }

    // Update is called once per frame
    void Update () {
		Buttons ();
		/*timer = timer + 1 * Time.deltaTime;
		if (timer>5) {
			Serialize ();
		}*/



	}

	void Buttons(){
		if (Input.anyKey)
		{
			Vector3 v = user.transform.position;
			int speed = 5;
			int y = 0;
			int x = 0;
			//creating a "vector" in order to just say how to move next.
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				x--;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				x++;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				y++;
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				y--;
			}
			//here we really move the user
			Move(x*speed,y*speed,user);
			if (Input.GetKeyDown(KeyCode.Space)) {
				Serialize ();
			}
			if (Input.GetKeyDown(KeyCode.Return)) {
				if (frameid>0) {
					Frame frame = GetRevertFrame (frameid-1);
					LoadFrame (frame);
				}
			}
		}
	}

	void Move(int xDir, int yDir,GameObject g)
    {
		Vector3 goal = new Vector3 (xDir * Time.deltaTime, yDir * Time.deltaTime, 0);
		g.transform.Translate (goal);
		Vector3 target = new Vector3 (xDir+g.transform.position.x, yDir+g.transform.position.y, 0);

		Vector3 vectorToTarget = target - g.transform.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg);
		Quaternion q = Quaternion.AngleAxis(angle,Vector3.forward);
		transform.rotation = Quaternion.Slerp(g.transform.rotation, q, Time.deltaTime * 50);
		Debug.Log (angle);
	}
    

	Frame GetRevertFrame(int id){
		List<Frame> History = DeSerialize ();
		foreach (Frame frame in History) {
			if (frame.FrameID==id) {
				return frame;
			}
		}
		return null;
	}

	void LoadFrame(Frame f){
		Debug.Log ("Load Frame Initialized");
		foreach (Data data in f.Info) {
			try {
				//Debug.Log("entered Try at Load Frame");
				Character c = entities[data.id];
				//Debug.Log("entity snached");
				c.Load(data);
				//Debug.Log("Entity succesfully Loaded Data");
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
				//Debug.Log ("Stuff happened");
			}
		}
		Debug.Log ("Loaded Frame " + f.FrameID);
	}


	void Serialize(){
		string path = "History.dat";
		if (frameid==0) {
			try {
				File.Delete (path);
			} catch (Exception ex) {
				Debug.Log ("File not Found");
			}
		}
		Debug.Log ("started Serializatione");
		FileStream fs = new FileStream (path, FileMode.Append);
		try {
			Frame instance = new Frame(frameid);
			frameid++;
			foreach (Character entity in entities.Values) {
				instance.Info.Add(entity.Save());
			}
			BinaryFormatter bf = new BinaryFormatter();

			bf.Serialize(fs,instance);

		} catch (Exception ex) {
			Debug.LogError (ex.ToString ());
		}
		finally{
			fs.Close ();
		}
		Debug.Log ("Succsesfully serialized Data");
	}

	List<Frame> DeSerialize(){
		
		string path = "History.dat";
		List<Frame> frames = new List<Frame> ();
		if (File.Exists(path)) {
			FileStream fs = new FileStream(path,FileMode.Open);

			try {			
				BinaryFormatter bf = new BinaryFormatter();
				try {
					while (fs.Position<fs.Length) {
						Frame f = (Frame)bf.Deserialize(fs);
						frames.Add(f);
					}
				} catch (Exception ex) {
					Debug.LogError (ex.ToString ());
				}
			} catch (Exception ex) {
				Debug.LogError (ex.ToString ());
			}
			fs.Close ();
		}
		return frames;
	}


    public void AddNewEntity(GameObject go)
    {        
        Character c = new Character(go, this, entities.Count);
		entities.Add(entities.Count,c);
    }






}
