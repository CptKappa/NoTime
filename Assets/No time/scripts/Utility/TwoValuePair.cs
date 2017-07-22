using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a simple remake of a KeyValuePair, because it didn't want to work in my code :,(
public class TwoValuePair<T,U> {

	public T Value1{ get; set;}
	public U Value2{ get; set;}

	public TwoValuePair(T v1,U v2){
		Value1 = v1;
		Value2 = v2;
	}	
}
