using UnityEngine;
using System.Collections;

//public  class KeyGivenToPersonEvent : GameEvent {
//	public PersonId PersonId { get; private set; }
//	public KeyGivenToPersonEvent (PersonId personId){
//		PersonId = personId;
//	}
//}


//****************//
// Rotate Puzzle  //
//****************//

// for the rotating path prototype 

public class DancerFinishPath: GameEvent{
}

public class SetPathNodeEvent: GameEvent{
	public int NodeIdx { get; private set; }
	public SetPathNodeEvent(int nid){
		NodeIdx = nid;
	}
}

public class PathCompeleteEvent:GameEvent{
	
}



//***********//
// Main Box  //
//***********//

public class ClockCompletionEvent: GameEvent{
	public bool IsClockCompleted { get; private set; }
	public float MaxSpeed { get; private set; }
	public ClockCompletionEvent(bool isClockCompleted, float maxSpeed){
		IsClockCompleted = isClockCompleted;
		MaxSpeed = maxSpeed;
	}
}

