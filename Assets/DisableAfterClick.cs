using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterClick : MonoBehaviour {

	public void DisableThisGameObject(){
		this.gameObject.SetActive(false);
	}
}
