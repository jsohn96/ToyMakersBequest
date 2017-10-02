using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeText : MonoBehaviour {
	TextMesh _textMesh;
	[SerializeField] int _peepState = 1;
	[SerializeField] string _newText;

	// Use this for initialization
	void Start () {
		_textMesh = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SwapText(PickedUpGearEvent e){
		if (_peepState == e.WhichGear) {
			_textMesh.text = _newText;
		}
	}

	void OnEnable(){
		Events.G.AddListener<PickedUpGearEvent> (SwapText);
	}
	void OnDisable(){
		Events.G.RemoveListener<PickedUpGearEvent> (SwapText);
	}
}
