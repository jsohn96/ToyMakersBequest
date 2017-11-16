using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPageTest : MonoBehaviour {
	Animator _bookAnim;

	// Use this for initialization
	void Start () {
		_bookAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			_bookAnim.Play ("Flip");
		}
		
	}
}
