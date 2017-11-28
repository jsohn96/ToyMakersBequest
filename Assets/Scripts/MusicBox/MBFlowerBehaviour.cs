using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBFlowerBehaviour : MonoBehaviour {
	[SerializeField] int _resposeToNodeIdx;
	Animator _flowerAnimator;
	bool _isBlossom = false;

	void OnEnable(){
		Events.G.AddListener<MBLotusFlower> (LotusBlossomHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<MBLotusFlower> (LotusBlossomHandle);
	}

	// Use this for initialization
	void Awake () {
		_isBlossom = false;
		_flowerAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void LotusBlossomHandle(MBLotusFlower e){
		print ("REcv from " + e.sendFromNode);
		if (e.sendFromNode == _resposeToNodeIdx && e.isBlossom!= _isBlossom) {
			_isBlossom = e.isBlossom;
			if (_isBlossom) {
				_flowerAnimator.Play ("open");
			} else {
				_flowerAnimator.Play ("close");
			}
		}
	}
}
