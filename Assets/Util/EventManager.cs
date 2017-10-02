﻿using UnityEngine;
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
	public int NodeIdx { get; private set; }
	public DancerFinishPath(int nid){
		NodeIdx = nid;
	}
}

public class SetPathNodeEvent: GameEvent{
	public int NodeIdx { get; private set; }
	public SetPathNodeEvent(int nid){
		NodeIdx = nid;
	}
}

public class PathCompeleteEvent:GameEvent{
	
}


public class DancerOnBoard: GameEvent{
	public int NodeIdx { get; private set; }
	public DancerOnBoard(int nid){
		NodeIdx = nid;
	}
}

public class DancerChangeMoveEvent:GameEvent{
	public DancerMove Move{ get; private set;}
	public DancerChangeMoveEvent(DancerMove dm){
		Move = dm;
	}
}

public class PathStateManagerEvent:GameEvent{
	public PathState activeEvent{ get; private set;}
	public PathStateManagerEvent(PathState ate){
		activeEvent = ate;
	}
}

public class PathResumeEvent:GameEvent{
	
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


//***********//
// Peephole  //
//***********//

public class InsidePeepHoleEvent: GameEvent {
	public bool IsInsidePeepHole { get; private set; }
	public InsidePeepHoleEvent(bool isInsidePeepHole){
		IsInsidePeepHole = isInsidePeepHole;
	}
}

public class PickedUpGearEvent: GameEvent {
	public int WhichGear { get; private set; }
	public PickedUpGearEvent(int whichGear){
		WhichGear = whichGear;
	}
}

public class GearsReadyForPickupEvent: GameEvent {
}