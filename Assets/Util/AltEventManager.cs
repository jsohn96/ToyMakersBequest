using UnityEngine;
using System.Collections;

//public  class KeyGivenToPersonEvent : GameEvent {
//	public PersonId PersonId { get; private set; }
//	public KeyGivenToPersonEvent (PersonId personId){
//		PersonId = personId;
//	}
//}

public class PickedUpItem:GameEvent{
	public items WhichItem { get; private set; }
	public PickedUpItem(items whichItem){
		WhichItem = whichItem;
	}
}

public class PeepHoleActivationCheck: GameEvent {
}

public class SlidingDoorFinished: GameEvent {
	public bool IsOpen { get; private set; }
	public SlidingDoorFinished (bool isOpen){
		IsOpen = isOpen;	
	}
}


public class TheatreFrogClickEvent:GameEvent{
	public int frogIdx { get; private set; }
	public TheatreFrogClickEvent (int index){
		frogIdx = index;	
	}
}