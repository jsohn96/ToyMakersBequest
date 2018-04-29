using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreStarLine : MonoBehaviour {
	LineRenderer _lineRenderer;
	int _currentCnt = 0;

	int _tempLineCnt = 0;



	void Start () {
		_lineRenderer = GetComponent<LineRenderer> ();
	}
	
	public void AddStarToLine(Vector3 pos){
		_lineRenderer.positionCount = _currentCnt + 1;
		pos.z += 0.001f;
		_lineRenderer.SetPosition (_currentCnt, pos);
		_currentCnt++;
	}
		
}
