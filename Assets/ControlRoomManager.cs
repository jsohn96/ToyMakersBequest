using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlRoomManager : MonoBehaviour {
	
	[SerializeField] TextMeshPro _tmpInstruction;

	[SerializeField] ControlRoomAudio _controlRoomAudio;

	[SerializeField] Fading _fading;

	[SerializeField] Camera _mainCamera;
	[SerializeField] Camera _peepCamera;

	[SerializeField] AltPeepText _peepText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
