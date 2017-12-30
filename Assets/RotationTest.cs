using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour {
	private Quaternion originalRotation;
	private float  startAngle = 0;

	float relativeOffset = 0f;

	public void Start()
	{
		originalRotation = this.transform.rotation;

	}

	void OnMouseDown(){
		relativeOffset = 0f;
		InputIsDown ();
	}

	void OnMouseDrag(){
		InputIsHeld ();
	}

	public void InputIsDown()
	{
		#if UNITY_IPHONE || UNITY_ANDROID

		originalRotation = this.transform.rotation;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 tempvector = Input.mousePosition;
		Vector3 vector = tempvector - screenPos;
		startAngle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		//startAngle -= Mathf.Ata n2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;  // uncomment to pop to where mouse is 

		#else

		originalRotation = this.transform.rotation;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 vector = Input.mousePosition - screenPos;
		startAngle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		//startAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;  // uncomment to pop to where mouse is 

		#endif
	}

	public void InputIsHeld()
	{
		#if UNITY_IPHONE || UNITY_ANDROID

		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 tempVector = Input.mousePosition;
		Vector3 vector = tempVector - screenPos;
		float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

		Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle , this.transform.forward);
		newRotation.y = 0; //This and the line below, may need to be changed depending on what axis the object is rotating on. 
		newRotation.eulerAngles = new Vector3(0,0,newRotation.eulerAngles.z);
		this.transform.rotation = originalRotation *  newRotation;

		#else
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 vector = Input.mousePosition - screenPos;
		float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
		Quaternion newRotation = Quaternion.AngleAxis(angle - startAngle , this.transform.forward);
		newRotation.y = 0; //see comment from above 
		newRotation.eulerAngles = new Vector3(0,0,newRotation.eulerAngles.z);
		this.transform.rotation = originalRotation *  newRotation;


		#endif
	}

}
