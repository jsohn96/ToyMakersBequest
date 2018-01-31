﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherLevelManager : LevelManager {

	public override void PickUpCharm(){
		base.PickUpCharm ();
		AltCentralControl._freedom = true;
		AltCentralControl._currentState = (AltStates)((int)AltCentralControl._currentState+1);
		InventorySystem._instance.AddItem (items.motherCharm);
		Events.G.Raise (new PickedUpItem (items.motherCharm));
	}
}
