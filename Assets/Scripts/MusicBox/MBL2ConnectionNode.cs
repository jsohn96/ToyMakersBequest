using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBL2ConnectionNode : MonoBehaviour {
	bool isRotating = false;
	Quaternion originAngle;
	Quaternion finalAngle;
	float rotateAmount; 
	bool isDirectToPond = false;
	PathNode _connectionNode;
	int enterSceneTime = 0;

	// Use this for initialization
	void Start () {
		_connectionNode = GetComponent<PathNode> ();
	}


	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);

	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (PathStateManagerHandle);
		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);

	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isRotating) {
			// rotate the node
			if (Mathf.Abs (Quaternion.Angle (transform.localRotation, finalAngle)) > 0.02f) {
				transform.localRotation = Quaternion.Lerp (transform.localRotation, finalAngle, Time.deltaTime * 10f);
			} else {
				transform.localRotation = finalAngle;
				isRotating = false;
			}
		}
	}

	void RotateNode(float amount){
		// temp Rot Degree = 0, 90, 180, 270 
		//float tempRotDegree = transform.rotation.eulerAngles.z;
		if(!isRotating){
			isRotating = true;
			originAngle = transform.localRotation;
			//transform.Rotate(0,0,-90);
			finalAngle = transform.localRotation;
			finalAngle = originAngle * Quaternion.Euler (0, 0, amount);

		}


	}

	void DancerOnBoardHandle(DancerOnBoard e){
		if (e.NodeIdx == _connectionNode.readNodeInfo().index) {
			if (isDirectToPond) {
				Events.G.Raise (new MBPathIndexEvent (2));
			} else {
				Events.G.Raise (new MBPathIndexEvent (21));
			}
		}
	}

	public void SwitchDirection(){
		if (isDirectToPond) {
			// rotate 180?
			RotateNode(180);

		} else {
			// rotate -180
			RotateNode(180);
		}

	}

	void PathStateManagerHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.MB_Stage_EnterPlayScene) {
			if (!isDirectToPond) {
				isDirectToPond = true;
				SwitchDirection ();
			}
		} else if (e.activeEvent == PathState.MB_Stage_EnterPondScene) {
			if (isDirectToPond) {
				isDirectToPond = false;
				SwitchDirection ();
			}
		} else if (e.activeEvent == PathState.MB_Stage_ExitPondScene) {
			if (enterSceneTime < 1) {
				Events.G.Raise (new MBPathIndexEvent (33));
				enterSceneTime += 1;
			} else {
				Events.G.Raise (new MBPathIndexEvent (33));
			}

		
		} else if (e.activeEvent == PathState.MB_Stage_ExitPondScene) {
			if (enterSceneTime < 1) {
				Events.G.Raise (new MBPathIndexEvent (33));
				enterSceneTime += 1;
			} else {
				Events.G.Raise (new MBPathIndexEvent (33));
			}


		} 
			
	} 
}
