using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldLadyLevelManager : LevelManager {

	public override void PickUpCharm(){
		base.PickUpCharm ();
		AltCentralControl._regret = true;
		AltCentralControl._currentState = (AltStates)((int)AltCentralControl._currentState+1);
		InventorySystem._instance.AddItem (items.oldLadyCharm);
		Events.G.Raise (new PickedUpItem (items.oldLadyCharm));
	}
}
