using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotebookPage {
	public int pageNumber;
	public GameObject[] objectsInPage;
	public bool nextPageLocked;

	public NotebookPage (
		int _pageNumber,
		GameObject[] _objectsInPage,
		bool _nextPageLocked){
		pageNumber = _pageNumber;
		objectsInPage = _objectsInPage;
		nextPageLocked = _nextPageLocked;
	}
}

public class NotebookPageManager : MonoBehaviour {

	int _currentPage = 0;	// Use this for initialization


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
