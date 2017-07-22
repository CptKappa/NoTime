using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : IEntity
{
	
	int _Id;

	public int id{ get { return _Id; } }

	public GameObject entity{ get; set; }

	public WorldController world { get; set; }

	public Entity (GameObject obj, WorldController wc, int i)
	{
		entity = obj;
		world = wc;
		_Id = i;
		wc.UpdateTick += Update;
	}

	virtual public void Update ()
	{
		Debug.Log ("Update Entity");
	}

	public virtual Data Save ()
	{
		//Debug.Log (entity.transform.rotation.eulerAngles);
		return new Data (entity.transform.position, entity.transform.rotation.eulerAngles, id);

	}

	public virtual void Load (Data d, float t)
	{
		entity.transform.position = d.location;
		Debug.Log (d.location);
		entity.transform.rotation = Quaternion.Euler (d.orientation);
		Debug.Log (d.orientation);
	}

    

}
