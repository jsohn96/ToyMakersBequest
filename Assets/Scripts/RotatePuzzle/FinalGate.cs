using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour {
	Animator _Anim;

	// Use this for initialization
	void Awake () {
		_Anim = GetComponent<Animator> ();
	}

	void OnEnable(){
		Events.G.AddListener<PathCompeleteEvent> (CompeleteHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathCompeleteEvent> (CompeleteHandle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CompeleteHandle(PathCompeleteEvent e){
		_Anim.Play ("OpenWindoe");
	}
}
