using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBGate : MonoBehaviour {
	[SerializeField] GameObject LeftGate;
	[SerializeField] GameObject RightGate;
	[SerializeField] GameObject LeftLock;
	[SerializeField] GameObject RightLock;
	[SerializeField] GameObject LockNode;

	Quaternion originAngle;
	Quaternion finalAngle;
	bool isRotating = false;
	bool isLock = true;
	bool isLockActivated = false;

	Animator _anim;
	[SerializeField] BoxCollider _gateBoxCollider;


	float errorVal = 0.2f;
	float rotateSpeed = 4f;

	float progress;

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (DoorLockHandle);
		Events.G.AddListener<DancerOnBoard> (MakeGateInteractive);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (DoorLockHandle);
		Events.G.RemoveListener<DancerOnBoard> (MakeGateInteractive);
	}
 
	// Use this for initialization
	void Awake () {
		_anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay (LockNode.transform.position, -LockNode.transform.right);
		if(isRotating && isLock){
			// rotate the node 
			if(Quaternion.Angle(LockNode.transform.localRotation,finalAngle) > errorVal*10f){
				LockNode.transform.localRotation = Quaternion.Lerp(LockNode.transform.localRotation, finalAngle, Time.deltaTime * rotateSpeed);
			} else {
				//print ("####### reset boolean #######");
				LockNode.transform.localRotation = finalAngle;
				isRotating = false;
				isLock = false;
			}
		}
		if(Input.GetKeyDown(KeyCode.P)){
			RotateLock ();
		}

		// can click on the lock after the dancer is at the gate 
		if (isLockActivated) {
			ClickWithMouse ();
			// enable light 
		}

		if (!isLock && isLockActivated) {
			// when the doors are unlocked --> trigger door opening animation 
			isLockActivated = false;
			LeftLock.transform.parent = LeftGate.transform;
			RightLock.transform.parent = RightGate.transform;
			//ResumePath ();
			_anim.Play("OpenGate");

		}

		//LockNode.transform.RotateAround(LockNode.transform.position, LockNode.transform.right, Time.deltaTime * 90f);
		
	}

	// rotate 90 degrees clockwise 
	void RotateLock(){
		// temp Rot Degree = 0, 90, 180, 270 
		//float tempRotDegree = transform.localRotation.eulerAngles.z;

		if(!isRotating){
			isRotating = true;
			originAngle = LockNode.transform.localRotation;
			//transform.Rotate(0,0,-90);
			Vector3 worldAxis = transform.TransformDirection(LockNode.transform.up);
			worldAxis.Normalize ();
			//worldAxis *= 90;
			//Quaternion temp = Quaternion.AngleAxis(-90, LockNode.transform.right);
			//finalAngle =originAngle * Quaternion.AngleAxis(-90, LockNode.transform.right);
			finalAngle =  originAngle * Quaternion.AngleAxis(-90, Vector3.right) ;
			//finalAngle *= temp;

		}
		_gateBoxCollider.enabled = false;
	}

	void ClickWithMouse(){
		Vector3 forward = Camera.main.transform.TransformDirection (Vector3.forward);
		if(Input.GetMouseButtonDown(0)){
			Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			//dragPreviousMousePos = Input.mousePosition;
			bool isHit = Physics.Raycast (mousePositionRay, out hit);
			if (isHit && hit.collider.gameObject.tag == "Lock") {
				//print ("git the circle: " + hit.point);

				RotateLock();
					
				//Events.G.Raise(new MBTurnColorCircle(_ControlColor, _nodeIndex));

			}
		}
	}

	void DoorLockHandle(PathStateManagerEvent e){
		if (e.activeEvent == PathState.open_gate) {
			isLockActivated = true;

		}

	}

	void ResumePath(){
		Events.G.Raise (new PathResumeEvent ());
	}

	void MakeGateInteractive(DancerOnBoard e){
		if (e.NodeIdx == 6) {
			isLockActivated = true;
		}
	}
}
