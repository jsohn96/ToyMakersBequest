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

public class MBPlayModeEvent:GameEvent{
	public PlayMode activePlayMode{ get; private set;}
	public MBPlayModeEvent(PlayMode plm){
		activePlayMode = plm;
	}
}

public class MBTurnColorCircle:GameEvent{
	public ButtonColor activeColor{ get; private set;}
	public int activeIdx{ get; private set;}
	public MBTurnColorCircle(ButtonColor btnClr, int idx){
		activeColor = btnClr;
		activeIdx = idx;
	}
}

//***********//
//    UI     //
//***********//

public class CircleTurnButtonPressEvent:GameEvent {
	public ButtonColor WhichCircleColor { get; private set; }
	public CircleTurnButtonPressEvent(ButtonColor whichCirlceColor) {
		WhichCircleColor = whichCirlceColor;
	}
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

public class CamerafovAmountChange:GameEvent {
	public float FovAmount { get; private set; }
	public CamerafovAmountChange(float fovAmount){
		FovAmount = fovAmount;
	}
}

public class WorkshopItemClicked:GameEvent {
	public bool Zoom { get; private set; }
	public WorkshopItemClicked(bool zoom) {
		Zoom = zoom;
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