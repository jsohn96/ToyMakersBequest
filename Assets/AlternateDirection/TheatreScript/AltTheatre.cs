using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use part of old music box script 


public enum TheatreState{
	waitingToStart = -1,
	startShow = 0, //toymaker to Life
	readyForDancerTank,
	dancerInTank,
	magicianBoardTank,
	magicianBeginShow,
	waterTankDescend,
	magicianPrepareFrog,
	magicianLeft,
	frogJump,
	lookDownIntoTank,
	CloseTankDoors,
	CloseDoorOne,//new
	CloseDoorTwo,//new
	TurnHeartCrank, //new
	OpenTank,
	LookUpAtStageAgain, //new
	magicianRight,
	dancerShowUp,
	dancerStartPath,
	dancerKissing,
	audienceLeave1,
	audienceLeave2,
	audienceLeave3,
	magicianReturnToPosition,
	restartPerformanceNextDay,
	readyForDancerTank2,
	dancerDescend,
	lookDownIntoTank2,
	CloseTankDoors2,
	CloseDoorOne2, //new
	CloseDoorTwo2, //new
	TurnHeartCrank2, //new
	dancerLocked,
	theatreEnd,
	BeginPart2,
	DancerReturnToPosition,
	BringDancerBackUp,
	DancerMeetsMagician,
	DancerDancesWithMagician,
	LiftUpAboveWhileDancing,
	CountStars
}

public class AltTheatre : LevelManager {
	[SerializeField] TheatreCameraControl _theatreCameraControl;

	public static TheatreState currentSate = TheatreState.waitingToStart;
	[SerializeField] TheatreMagician magician;
	[SerializeField] TheatreDancer _dancer;	
	TheatreChest chest;
	TheatreCabinet cabinet;
	PathNetwork network;

	[Header("Water Tank")]
	[SerializeField] Transform _watertank;
	[SerializeField] TheatreWaterTank _theatreWaterTank;
	float _waterTankDuration = 6f;
	[SerializeField] Vector3 _tankTopPos, _tankBottomPos;
	[Header("Starting Platform")]
	[SerializeField] Transform _startPlatform;
	float _platformDuration = 7f;
	[SerializeField] Vector3 _platformBeginPos, _platformEndPos;

	[Header("Interactive Scene Objects")]
	[SerializeField] TheatreCabinet _theatreCabinet;
	[SerializeField] TheatreChest _theatreChest;

	[SerializeField] TheatreWaterTankDoors _tankDoor1, _tankDoor2;

	[SerializeField] TheatreMusic _theatreMusic;

	[SerializeField] TraversalUI _traversalUI;
	[SerializeField] TheatreLighting _theatreLighting;
	[SerializeField] TheatreSound _theatreSound;

	[SerializeField] TheatreCoin _theatreCoin;

	//there should be 0-2
	[SerializeField] TheatreAudience[] _theaterAudiences = new TheatreAudience[3];


	[SerializeField] TheatreText _theatreText;
	[SerializeField] frogSwirlTest _frogSwirl;
	[SerializeField] TheatreFrog _theatreFrog;
	[SerializeField] StartSlider _startSlider;

	[SerializeField] TheatreHeartTimer _heartTimer1, _heartTimer2;
	[SerializeField] SpriteRenderer _heartArrowInstructions;

	[SerializeField] TheatreRotation _theatreRotation;

	int _doorCloseCnt = 0;

	public bool _trueEnding = false;

	// Use this for initialization
	void Awake () {
		currentSate = TheatreState.waitingToStart;
		chest = FindObjectOfType<TheatreChest> ().GetComponent<TheatreChest> ();
		cabinet = FindObjectOfType<TheatreCabinet> ().GetComponent<TheatreCabinet> ();
		network = FindObjectOfType<PathNetwork> ().GetComponent<PathNetwork> ();
	}

	public override void PickUpCharm(){
		base.PickUpCharm ();
		AltCentralControl._freedom = true;
		AltCentralControl._currentState = (AltStates)((int)AltCentralControl._currentState+1);
		InventorySystem._instance.AddItem (items.dancerCharm);
		Events.G.Raise (new PickedUpItem (items.dancerCharm));
	}

	void Start(){
//		_watertank.localPosition = _tankTopPos;
		_startPlatform.localPosition = _platformBeginPos;
	}


	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
//		if(Input.GetKeyDown(KeyCode.Space)){
//			if(currentSate < TheatreState.theatreEnd){
//				currentSate += 1;
//				CheckStateMachine();
//				Debug.Log("Current Theatre State: " + currentSate);
//			}
//
//		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			Time.timeScale = 5.0f;
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			Time.timeScale = 1.0f;
		}

		if(Input.GetKeyDown(KeyCode.X)){
			Time.timeScale = 30.0f;
		} else if (Input.GetKeyUp(KeyCode.X)){
			Time.timeScale = 1.0f;
		}
		#endif

		//CheckStateUpdate ();
		
	}

	public void Initialize(){
		_theatreLighting.Set1 ();
		_theatreSound.PlayLightSwitch ();
		_theatreText.TriggerText (0);
	}

	public void MoveToNext(){
		if (!_trueEnding && currentSate < TheatreState.theatreEnd) {
			currentSate += 1;
			CheckStateMachine ();
			Debug.Log ("Current Theatre State: " + currentSate);
		} else if (_trueEnding && currentSate < TheatreState.CountStars) {
//			if (currentSate < TheatreState.theatreEnd) {
//				currentSate = TheatreState.theatreEnd;
//			}
			Debug.Log ("Current Theatre State: " + currentSate);
			currentSate += 1;
			CheckStateMachine ();
		}
	}

	void CheckStateUpdate(){
		if (currentSate == TheatreState.magicianLeft) {
			Debug.Log ("Waiting for player input to continue");
		} else if (currentSate == TheatreState.magicianRight) {
			Debug.Log ("Waiting for player input to continue");
		} else if (currentSate == TheatreState.dancerShowUp) {
			Debug.Log ("Waiting for player to connect nodes");
		}
	}

	public void BringInMusic(){
		_theatreMusic.BeginMusic ();
	}

	public void SetLight(int index){
		_theatreSound.PlayLightSwitch ();
		if (index == 0) {
			_theatreLighting.DisableAll ();
		}
		else if (index == 2) {
			_theatreLighting.Set2 ();
		} else {
			_theatreLighting.Set4 ();
		}
	}

	public void MagicianFinishGreet(){
		magician.PointToCenter (true);
	}

	public void CheckStateMachine(){
		Debug.Log (currentSate);
		_startSlider.SetSliderState ((int)currentSate, 35f);

		if (!_trueEnding) {
			switch (currentSate) {
			case TheatreState.waitingToStart:
				_theatreCameraControl.Activate ();
				_theatreRotation.StartInitRotation ();
				break;
			case TheatreState.startShow:
				magician.InitMagician ();
				_theatreText.TriggerText (2);
//			_theatreSound.PlayLightSwitch ();
			//magician.GoToStart ();

				StartCoroutine (LerpPosition (_startPlatform, _platformBeginPos, _platformEndPos, _platformDuration, 4f, 2f, () => {
					magician.BowDown ();
				}));
				magician.GoToStart ();
//			StartCoroutine (DelayedSelfCall (18f));
			// call back function? 
				StartCoroutine (DelayAction (17.5f, () => {
					_theatreWaterTank.Activate (true);
				}));
				StartCoroutine (LerpPosition (_watertank, _watertank.localPosition, _tankTopPos, 6f, 1.5f, 0f, () => {
					MoveToNext ();
				}));
				break;
			case TheatreState.readyForDancerTank:
			
				break;
			case TheatreState.dancerInTank:
			//prevent the lid from closing
				_theatreWaterTank.DisableLid (true);
				_dancer.FirstDancerEnterTank ();

				_theatreText.TriggerText (6);
				break;
			case TheatreState.magicianBoardTank:
			
				_theatreWaterTank.Activate (false);
				_theatreWaterTank.OpenLid (false);
				magician.StepOnTank ();
				magician.PointToCenter (false);
				_theatreCameraControl.EnableScrollFOV ();
				break;
			case TheatreState.magicianBeginShow:
				magician.BeginShow (true);
				break;
			case TheatreState.waterTankDescend:
			
				_theatreSound.PlayLightSwitch ();
				_theatreLighting.Set3 ();
				_theaterAudiences [0].AudienceEnter ();
				magician.BeginShow (false);
				Invoke ("PlayWaterTankMoveSound", 1.5f);
				StartCoroutine (LerpPosition (_watertank, _tankTopPos, _tankBottomPos, _waterTankDuration, 1.5f, 0f, () => {
					MoveToNext ();
				}));
				break;
			case TheatreState.magicianPrepareFrog:
				magician.StepOffTank ();
				break;
			case TheatreState.magicianLeft:
//			_traversalUI.FadeIn ();
				_theatreCameraControl.EnableScrollFOV ();

				magician.PointToLeft (true);
				chest.Activate (true);
				break;
			case TheatreState.frogJump:
				_theatreSound.PlayLightSwitch ();
				_theatreLighting.Set4 ();
				_theaterAudiences [1].AudienceEnter ();

				magician.PointToLeft (false);
				_theatreWaterTank.OpenLid (true);
			// frog.jumpout
			//magician go back to center position 
			//MoveToNext();
				break;
			case TheatreState.lookDownIntoTank:
//			_traversalUI.FadeOut ();
				_theatreCameraControl.MoveCameraToLookAtTank ();
			//MoveToNext();
				_theatreText.TriggerText (7);
				break;
			case TheatreState.CloseTankDoors:
				_theatreWaterTank.OpenLid (false);
				_tankDoor1.Activate (true);
				_tankDoor2.Activate (true);
//			_tankDoor1.DisableTouchInput (true);
//			_tankDoor2.DisableTouchInput (true);
			//MoveToNext();

				break;
			case TheatreState.CloseDoorOne:
				break;
			case TheatreState.CloseDoorTwo:
				_heartTimer1.ActivateHeart ();
				_heartTimer2.ActivateHeart ();
				break;
			case TheatreState.TurnHeartCrank:
				_theatreText.TriggerText (9);
				break;
			case TheatreState.OpenTank:
//			_theatreChest.GiveBackControl ();
//			_theatreCameraControl.EnableScrollFOV();
//			_theatreText.TriggerText (13);
				break;
			case TheatreState.LookUpAtStageAgain:
				_theatreCameraControl.MoveCameraToLookAtStage2 ();
				break;
			case TheatreState.magicianRight:
				_tankDoor1.Activate (false);
				_tankDoor2.Activate (false);

//			_traversalUI.FadeIn ();
//			_theatreCameraControl.EnableScrollFOV();
				magician.PointToRight (true);
			// dancer.enterScene();
				cabinet.Activate (true);

				break;
			case TheatreState.dancerShowUp:
			// dancer shows up play dancer aniamtion
				_theatreText.TriggerText (13);
				_theatreSound.PlayLightSwitch ();
				_theatreLighting.Set5 ();
//			_theaterAudiences [2].AudienceEnter ();

				magician.PointToRight (false);
				_dancer.ExitCloset ();
			// dancer.showUp();
			//_dancer.HideDancer (false);
			// enable network connection 
				break;
			case TheatreState.dancerStartPath:
			// after animation ends, activate path
				magician.EnterKissPosition ();
				network.SetPathActive (true);
				break;
			case TheatreState.dancerKissing:
				StartCoroutine (KissSepuence ());
				break;
			case TheatreState.audienceLeave1:
			// dancer|magician return to idle 
				magician.ExitKissPosition ();
				_dancer.EndKiss ();
			// dancer go to center 
			// magician back to original 
//			_theatreLighting.Set4 ();
//			_theaterAudiences [2].AudienceLeave ();

//			StartCoroutine(DelayedSelfCall(2f));
				break;
			case TheatreState.audienceLeave2:
//			_theatreLighting.Set3 ();
//			_theaterAudiences [1].AudienceLeave ();

//			StartCoroutine(DelayedSelfCall(2f));
				break;
			case TheatreState.audienceLeave3:
//			_theatreLighting.Set6 ();
//			_theaterAudiences [0].AudienceLeave ();

				_dancer.ElevateTankPlatform ();
//			StartCoroutine(DelayedSelfCall(2));
				break;
			case TheatreState.magicianReturnToPosition:
			//magician.ExitKissPosition ();
				_frogSwirl.ShrinkFrog ();
				StartCoroutine (DelayedSelfCall (5));
				break;
			case TheatreState.restartPerformanceNextDay:
				_theatreSound.PlayLightSwitch ();
				_theatreLighting.Set2 ();
				_theatreText.TriggerText (25);
				magician.BeginShow (true, true);
				break;
			case TheatreState.readyForDancerTank2:
				_theatreWaterTank.Activate (true);
				break;
			case TheatreState.dancerDescend:
//			_theatreLighting.Set6 ();
//			_theatreSound.PlayLightSwitch ();
//			_theatreWaterTank.OpenLid (true);
				magician.BeginShow (false);
				_theatreText.TriggerText (27);
				_dancer.SecondDancerEnterTank ();

				break;
			case TheatreState.lookDownIntoTank2:
				_theatreWaterTank.OpenLid (false);
//			_traversalUI.FadeOut ();
				_theatreCameraControl.MoveCameraToLookAtTank ();
				break;
			case TheatreState.CloseTankDoors2:
//			_tankDoor1.FinalActivation (true);
//			_tankDoor2.FinalActivation (true);
				_tankDoor1.FinalActivation (true);
				_tankDoor2.FinalActivation (true);
//			_tankDoor1.DisableTouchInput (true);
//			_tankDoor2.DisableTouchInput (true);

//			StartCoroutine (DelayedSelfCall (1.5f));
				break;
			case TheatreState.CloseDoorOne2:
				break;
			case TheatreState.CloseDoorTwo2:
				_heartTimer1.ActivateHeart ();
				_heartTimer2.ActivateHeart ();
				break;
			case TheatreState.TurnHeartCrank2:
				_theatreText.TriggerText (30);
				_dancer.StopMovement (true);
				break;
			case TheatreState.dancerLocked:
				_dancer.Drown ();
//				_dancer.StopMovement (true);
				break;
			case TheatreState.theatreEnd:
				
				_theatreCoin.BeginGlow ();
				_traversalUI.FadeIn ();
				_theatreCameraControl.EnableScrollFOV ();
			//Stop Dancer Here
//			_dancer.HideDancer (true);
				_tankDoor1.OpenTankCall ();
				_tankDoor2.OpenTankCall ();
				_theatreMusic.EndMusic ();
				_theatreText.TriggerText (35);
				break;
			}
		} else {
			switch (currentSate) {
			case TheatreState.BeginPart2:
//				_theatreCameraControl.MoveCameraToLookAtStage2 ();
//				_theatreCameraControl.MoveCameraToLookAtTank(5f);
				_theatreCameraControl.ZoomIn ();
//				_theatreRotation.StartInitRotation ();
				break;
			case TheatreState.DancerReturnToPosition:
//				_dancer.DancerEnterWater ();
//				_dancer.ElevateTankPlatform ();
				break;
			case TheatreState.BringDancerBackUp:
				_dancer.ElevateTankPlatform (6f);
				break;
			case TheatreState.DancerMeetsMagician:
//				_theatreCameraControl.Activate ();
//				_theatreRotation.StartInitRotation ();
				break;
			case TheatreState.DancerDancesWithMagician:
//				_theatreCameraControl.Activate ();
//				_theatreRotation.StartInitRotation ();
				break;
			case TheatreState.LiftUpAboveWhileDancing:
//				_theatreCameraControl.Activate ();
//				_theatreRotation.StartInitRotation ();
				break;
			case TheatreState.CountStars:
//				_theatreCameraControl.Activate ();
//				_theatreRotation.StartInitRotation ();
				break;
			}
		}
	}

	IEnumerator KissSepuence(){
		magician.Kiss ();
		_dancer.Kiss ();
		yield return new WaitForSeconds (5f);
		_theatreSound.PlayKissSound ();
		//_dancer.PlayKiss ();
		_theatreText.TriggerText (17);
		yield return 0;
	}

	void DancerMoveOnPathHandle(DancerMoveOnPathEvent e){
		if (currentSate == TheatreState.dancerShowUp) {
			MoveToNext ();
		}
	}

	IEnumerator DelayedSelfCall(float duration){
		yield return new WaitForSeconds (duration);
		MoveToNext ();
	}

	public void ActivateBothTankDoors(){
		_tankDoor1.DisableTouchInput (false);
		_tankDoor2.DisableTouchInput (false);
	}

	public void HideDancer(){
		_doorCloseCnt++;
		if (_doorCloseCnt >= 2) {
			_dancer.HideDancer (true);
		}
	}

	public void PrepForFrog(){
		_theatreChest.FrogPrep ();
	}

	public void CallFrog(){
		_theatreFrog.FrogJumpIntoWater ();
	}

	void PlayWaterTankMoveSound(){
		_theatreSound.WaterTankMoveSound ();
		_theatreCameraControl.MoveCameraToLookAtStage ();
	}

	IEnumerator LerpPosition (Transform transform, Vector3 origin, Vector3 goal, float duration, float initialDelay = 0f, float laterDelay = 0f, System.Action action = null){
		yield return new WaitForSeconds (initialDelay);
		float timer = 0f;
		while (duration > timer) {
			timer += Time.deltaTime; 
			transform.localPosition = Vector3.Slerp (origin, goal, timer / duration);
			yield return null;
		}
		transform.localPosition = goal;
		yield return new WaitForSeconds (laterDelay);
		if (action != null) {
			action ();
		}
	}

	IEnumerator DelayAction(float delayDuration, System.Action action){
		yield return new WaitForSeconds (delayDuration);
		if (action != null) {
			action ();
		}
	}

	void TriggerKiss(PathStateManagerEvent e){
		if (e.activeEvent == PathState.ReachPathEnd) {
			if (currentSate == TheatreState.dancerKissing - 1) {
				currentSate += 1;
				CheckStateMachine ();
				Debug.Log ("Current Theatre State: " + currentSate);
			}
		}
	}

	void OnEnable(){
		Events.G.AddListener<PathStateManagerEvent> (TriggerKiss);
		Events.G.AddListener<DancerMoveOnPathEvent> (DancerMoveOnPathHandle);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathStateManagerEvent> (TriggerKiss);
		Events.G.RemoveListener<DancerMoveOnPathEvent> (DancerMoveOnPathHandle);
	}
}




