using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxManagerTemp : MonoBehaviour {
	
	[SerializeField] PathNetwork[] _musicPaths;


	// Use this for initialization
	void Awake () {


	}

	void Start(){
		//Events.G.Raise (new DancerChangeMoveEvent (DancerMove.idleDance));
		_musicPaths [0].SetPathActive (true);
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
	

	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);
	
	}

	// Update is called once per frame
	void Update () {

	

	}

	void PathStateManagerHandle(PathStateManagerEvent e){
		switch (e.activeEvent) {
		case PathState.none:
			break;
	
		}
	}
		
}
