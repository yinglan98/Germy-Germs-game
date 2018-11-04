using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllButtonScript : MonoBehaviour {
	public PlayerControllter2 script;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void ClickOnPlay(){
		PlayerControllter2.mode = 2;
	}

	public void ClickOnTutroial(){
		
	}

	public void ClickOnQuit(){
		
	}

	public void ClickOnRestart(){
		PlayerControllter2.mode = 4;
		script.B_NeedToRestart = true;
	}
}
