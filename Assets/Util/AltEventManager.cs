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