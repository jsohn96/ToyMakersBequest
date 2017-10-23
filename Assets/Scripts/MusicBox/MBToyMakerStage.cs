using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBToyMakerStage : MonoBehaviour {
	MBToyMaker _TM;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ResumePathWhenFinished(){
	
		Events.G.Raise (new PathResumeEvent ());
	}
}
