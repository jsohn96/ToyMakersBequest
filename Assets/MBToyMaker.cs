using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBToyMaker : MonoBehaviour {
	[SerializeField] Transform _dancerTransform;
	[SerializeField] Light _spotLight;

	Timer _descendTimer;

	// Use this for initialization
	void Awake () {
		_spotLight.intensity = 0;
		_descendTimer = new Timer (3f);
		
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (DancerHoldHandEvent);

	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (DancerHoldHandEvent);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DancerHoldHandEvent(PathStateManagerEvent e){
		
	}
}
