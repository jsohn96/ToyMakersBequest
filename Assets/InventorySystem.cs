using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum items {
	dancerCharm,
	motherCharm,
	oldLadyCharm,
	finalKey
}

public class InventorySystem : MonoBehaviour {

	[SerializeField] List<items> _currentInventory = new List<items> ();
	public static InventorySystem _instance;

	void Awake () {
		//assign an instance of this gameobject if it hasn't been assigned before
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}


	public void AddItem(items item){
		if (!_currentInventory.Contains (item)) {
			_currentInventory.Add (item);
		}
	}

	void RemoveItem(items item){
		if (_currentInventory.Contains (item)) {
			_currentInventory.Remove (item);
		}
	}

	public bool CheckIfItemHeld(items item){
		return _currentInventory.Contains (item);
	}
}
