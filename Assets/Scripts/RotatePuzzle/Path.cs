﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
	BezierSpline[] _bspline;

	Vector3[] _pathLinkPositions;

	// Use this for initialization
	void Awake () {
		if (GetComponentsInChildren<PathLink> () != null) {
			PathLink[] tempObjs = GetComponentsInChildren<PathLink> ();
			_pathLinkPositions = new Vector3[tempObjs.Length];
			for (int i = 0; i < _pathLinkPositions.Length; i++) {
				_pathLinkPositions [i] = tempObjs [i].GetLinkPosition ();
			}
		} else {
			print ("Error: No Valid Path");
		}


		// change to splines 
		if (GetComponentsInChildren<BezierSpline> () != null) {
			_bspline = GetComponentsInChildren<BezierSpline> ();
		} else {
			print ("Error: No Valid Path");
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3[] GetPathInfo(){
		return _pathLinkPositions;
	}

	public BezierSpline[] GetPathSpline(){
		return _bspline;
	}
}
