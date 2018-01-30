using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherLevelManager : MonoBehaviour {

	void PickedUpMotherCharm(){
		AltCentralControl._freedom = true;
		InventorySystem._instance.AddItem (items.motherCharm);
	}
}
