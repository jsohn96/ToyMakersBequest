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
public class InterlockNodeStateEvent:GameEvent{
	public bool Unlock { get; private set; }
	public int SendFrom { get; private set; }
	public int SendTo { get; private set; }
	public InterlockNodeStateEvent(bool ulc, int sid, int rid){
		Unlock = ulc;
		SendFrom = sid;
		SendTo = rid;
	}
}

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

// update path node info 
// update frog info 
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

// Music Manager Event
public class MBMusicMangerEvent:GameEvent{
	public bool isMusicPlaying{ get; private set;}
	public MBMusicMangerEvent(bool imp){
		isMusicPlaying = imp;
	}
}

public class MBMusicLayerAdjustmentEvent:GameEvent {
	public MusicBoxLayer ThisMusicBoxLayer { get; private set; }
	public bool IsOn { get; private set; }
	public MBMusicLayerAdjustmentEvent(MusicBoxLayer thisMusicBoxLayer, bool isOn){
		ThisMusicBoxLayer = thisMusicBoxLayer;
		IsOn = isOn;
	}
}

// music box light manager
public class MBLightManagerEvent:GameEvent{
	public LightState activeLightState{ get; private set; }
	public MBLightManagerEvent(LightState ls){
		activeLightState = ls;
	}
}

public class MBPathIndexEvent:GameEvent{
	public int jumpToIndex{ get; private set; }
	public MBPathIndexEvent(int jmp){
		jumpToIndex = jmp;
	}
}

public class MBExitPondLoop:GameEvent{
	
}


// music box audio event 
// when the node is rotating, sending over the node index and rotate speed 
public class MBNodeRotate:GameEvent{
	public int nodeIndex{ get; private set; }
	public bool isRoating{ get; private set; }
	public float rotateSpeed{ get; private set; }
	public MBNodeRotate(int nid, bool isRot, float rspeed){
		nodeIndex = nid;
		rotateSpeed = rspeed;
		isRoating = isRot;
	}
}

// when the node is correctly connected, sending over node index 
public class MBNodeConnect:GameEvent{
	public int nodeIndex{ get; private set; }
	public MBNodeConnect(int nid){
		nodeIndex = nid;
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