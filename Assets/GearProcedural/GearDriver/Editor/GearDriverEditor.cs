// GearDriverEditor.cs (C)2013 by Alexander Schlottau, Hamburg, Germany
//   shows a custom inspector for the GearDriver.cs script


using UnityEditor;
using UnityEngine;
[CanEditMultipleObjects, CustomEditor(typeof(GearDriver))]


public class GearDriverEditor : Editor {

	private SerializedObject gearDrivers;
	private SerializedProperty settings;
	private delegate void menu();
	private menu currentMenu;
	private Color[] selectionColor = new Color[2];
	private bool twoSelected = false;
	private float tool_MoveToDegree = 0.0f;
	private float tool_RotaDegree = 90.0f;

	private GUIContent
		menuSettingsCont	= new GUIContent("Gear Driver", "Settings of this driver."),
		menuToolCont		= new GUIContent("Tools","Layout your gears or add script to all other gears."),
		liveUpdCont			= new GUIContent("Live Update", "Updates the ratios and speeds every frame.\nNot recommented to toggle at runtime."),
		onceUpdCont			= new GUIContent("Update Speed Once", "Updates ratios and speeds one time"),
		outputListCont		= new GUIContent("Outputs connected:", "Add or delete Gears to be driven by this GearDriver."),
		toolsCenterCont		= new GUIContent("Center", "Center first selected to the others position."),
		toolsMoveDirCont	= new GUIContent("Move To", "Moves first selected to the other at shortest way.\n" +
			"Click a few times to match teeth, if not match on first click."),
		toolsMoveDegCont	= new GUIContent("Move To Degree", "Align the first selcted to the other at a given angle.\n" +
		                                  "Click a few times to match teeth, if not match on first click.");


	void OnEnable() {

		if (Selection.activeGameObject != null) {
			gearDrivers = new SerializedObject(targets);
			settings 	= gearDrivers.FindProperty("settings");
		}
		selectionColor [0] = Color.cyan;
		selectionColor [1] = Color.blue;

		if (FirstTwoSelectedGears()[1] != null)
			this.currentMenu = Menu_Tools;
		else
			this.currentMenu = Menu_Settings;
	}
	
	public override void OnInspectorGUI() {

		gearDrivers.Update ();
		this.currentMenu();
		if(gearDrivers.ApplyModifiedProperties() ||
		   (Event.current.type == EventType.ValidateCommand &&
		 Event.current.commandName == "UndoRedoPerformed"))
		foreach(GearDriver gd in targets) {
			if(PrefabUtility.GetPrefabType(gd) != PrefabType.Prefab) 
			{ }
		}
	}

	void OnSceneGUI() {
		
		GearDriver gearDriver = (GearDriver)target;
		if (gearDriver.settings.outputTo == null) return;

		Handles.color = Color.magenta;

		for (int i = 0; i<gearDriver.settings.outputTo.Count; i++) {
			if (gearDriver.settings.outputTo[i] != null) {
				DrawHandleCircles(gearDriver.settings.outputTo[i].gameObject);
			} else {
				gearDriver.settings.outputTo.RemoveAt(i);
			}
		}
	}

	private void Menu_Settings() {

		ShowMenuButtons (0);

		SerializedProperty
			isMotor = settings.FindPropertyRelative ("isMotor"),
			isShaft = settings.FindPropertyRelative ("isShaft"),
			isWorm  = settings.FindPropertyRelative ("isWorm"),
			invWorm = settings.FindPropertyRelative ("invWormOut"),
			updateLive = settings.FindPropertyRelative ("updateLive"),
			motorSpeed = settings.FindPropertyRelative ("motorSpeed"),
			outputTo = settings.FindPropertyRelative ("outputTo");

		if (outputTo == null)
			return;



		if (Selection.activeTransform != null)
			if (Selection.activeTransform.gameObject.GetComponent<ProceduralGear> () != null)
				isShaft.boolValue = false;
			else 
				if (Selection.activeTransform.gameObject.GetComponent<ProceduralWormGear>() != null) {
					isShaft.boolValue = false;
					isWorm.boolValue = true;
				}
		isShaft.boolValue = EditorGUILayout.Toggle ("Is Shaft", isShaft.boolValue);
		isWorm.boolValue = EditorGUILayout.Toggle ("Is Worm Gear", isWorm.boolValue);
		if (isWorm.boolValue) {
			invWorm.boolValue = EditorGUILayout.Toggle ("  Invert Worm Output", invWorm.boolValue);
			EditorGUILayout.Separator ();
	    }
		EditorGUILayout.BeginHorizontal ();
		isMotor.boolValue = EditorGUILayout.Toggle ("Is Motor", isMotor.boolValue);
		GUI.enabled = (!updateLive.boolValue && Application.isPlaying && isMotor.boolValue);
			ShowUpdateOnceButton ();
		GUI.enabled = true;
		EditorGUILayout.EndHorizontal ();

		if (isMotor.boolValue) {
		
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("  ", GUILayout.Width (10));
			updateLive.boolValue = EditorGUILayout.Toggle (liveUpdCont, updateLive.boolValue);
			EditorGUILayout.LabelField ("  ", GUILayout.Width (10));
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Separator ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("  ", GUILayout.Width (10));
			motorSpeed.floatValue = EditorGUILayout.FloatField ("Motor Speed (RPM)", motorSpeed.floatValue / 6.0f)*6.0f;
			EditorGUILayout.EndHorizontal ();
		} 
	
		EditorGUILayout.Separator ();
		EditorGUILayout.PropertyField(outputTo, outputListCont, true);

		ShowInfoField ();
	}

	private void Menu_Tools() {

		ShowMenuButtons (1);

		EditorGUILayout.LabelField ("Scripts",EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Separator();

		if (GUILayout.Button ("Add GearDriver Script to all gears in scene", EditorStyles.miniButton)) {
			AddGearDriverToSceneObjects();
		}
		EditorGUILayout.Separator();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();

		EditorGUILayout.LabelField ("Layout",EditorStyles.boldLabel);

		GameObject[] selectedObjects = FirstTwoSelectedGears();
		ShowTwoSelected (selectedObjects);

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Separator();

		GUI.enabled = (selectedObjects [1] != null);
		if (GUILayout.Button (toolsCenterCont, EditorStyles.miniButtonLeft)) {
			Tool_CenterObjects(selectedObjects);
			Selection.activeObject = selectedObjects[0];
		}
		if (GUILayout.Button (toolsMoveDirCont, EditorStyles.miniButtonMid)) {
			Tool_MoveTo(selectedObjects, 0.0f, false);
		}
		if (GUILayout.Button (toolsMoveDegCont, EditorStyles.miniButtonRight)) {
			Tool_MoveTo(selectedObjects, tool_MoveToDegree, true);
		}

		EditorGUILayout.Separator();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		tool_MoveToDegree = SliderFloatFieldNullable ("Angle to fit in degrees:", tool_MoveToDegree, 360.0f, false);
	
		EditorGUILayout.Separator();
	}

	private void Tool_MoveTo (GameObject[] _selectedObjects, float _degree, bool _useDegree)
	{

		ProceduralGear gear1 = _selectedObjects [0].GetComponent<ProceduralGear> ();
		ProceduralGear gear2 = _selectedObjects [1].GetComponent<ProceduralGear> ();
		Transform gear1Transform = gear1.transform;
		Transform gear2Transform = gear2.transform;

		float neededDistance = (gear1.prefs.d) + (gear2.prefs.d);
		Transform originalParentObjectA = gear1Transform.parent;
		gear1Transform.parent = gear2Transform;
		Quaternion originalObjectBRotation = gear2Transform.rotation;
		gear2Transform.rotation = Quaternion.identity;

		if (!_useDegree) {
			gear1.gameObject.transform.Translate(0.000f,0.000f,0.00001f);
			Quaternion relativRotation = Quaternion.LookRotation(gear1Transform.position - gear2Transform.position);
			_degree = relativRotation.eulerAngles.y - gear2Transform.rotation.eulerAngles.y;
		}

		Tool_CenterObjects(_selectedObjects);
		gear1Transform.Rotate(0.0f, _degree , 0.0f);
		gear1Transform.Translate(0.0f, 0.0f, neededDistance );
		gear2Transform.rotation = originalObjectBRotation;
		gear1Transform.parent = originalParentObjectA;

		float angleBetweenTwoTeethOnSource = (360.0f/gear1.prefs.teethCount);
		float angleBetweenTwoTeethOnTarget = (360.0f/gear2.prefs.teethCount);
		float ratio = angleBetweenTwoTeethOnSource / angleBetweenTwoTeethOnTarget;
	
		gear1Transform.rotation = gear2Transform.rotation;
		gear1Transform.Rotate(0.0f, _degree , 0.0f);
		gear1Transform.Rotate(0.0f, tool_RotaDegree+=90 + (angleBetweenTwoTeethOnSource*0.5f) + (_degree*ratio)  , 0.0f);

	}

	private void Tool_CenterObjects (GameObject[] _selectedObjects)
	{
		_selectedObjects[0].transform.position = _selectedObjects[1].transform.position;
		_selectedObjects[0].transform.rotation = _selectedObjects[1].transform.rotation;
	}

	private void DrawHandleCircles(GameObject _selected) {
		
		Transform t = _selected.transform;
		ProceduralGear gear = _selected.GetComponent<ProceduralGear> ();
		if (!twoSelected) {
			if (gear == null) {
				Handles.Disc (Quaternion.identity, t.position, t.up, t.localScale.x * 0.5f, true, 0f);
				return;
			}
			Handles.Disc (Quaternion.identity, t.position, t.up, gear.prefs.dk * t.localScale.x, true, 0f);
		}
	}

	private void ShowMenuButtons(int _currenMenuIndex) {

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		Color c = GUI.color;
		
		if (_currenMenuIndex == 0)
			GUI.color = Color.yellow;
		if (GUILayout.Button (menuSettingsCont, EditorStyles.miniButtonLeft))
			this.currentMenu = Menu_Settings;
		
		if (_currenMenuIndex != 1)
			GUI.color = c;
		else
			GUI.color = Color.green;
		
		if (GUILayout.Button (menuToolCont, EditorStyles.miniButtonRight))
			this.currentMenu = Menu_Tools;  
		
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space ();
		GUI.color = c;
	}

	private void ShowUpdateOnceButton() {
		
		if (GUILayout.Button (onceUpdCont)) {
			GearDriver gd = (GearDriver)target;
			gd.UpdatePowerchain ();
		}
		EditorGUILayout.Separator();
	}

	private void ShowTwoSelected(GameObject[] _selectedObjects) {

		if (_selectedObjects [1] != null) {
			GameObject objMove = _selectedObjects[0];
			objMove = (GameObject)EditorGUILayout.ObjectField ("Move this object:", objMove, typeof(GameObject),true);	
			GameObject objStay = _selectedObjects[1];
			objStay = (GameObject)EditorGUILayout.ObjectField ("to this object:", objStay, typeof(GameObject),true);
		} else 
			ShowTwoSelectionsInfo();
	}

	private void ShowTwoSelectionsInfo() {

		EditorGUILayout.LabelField ("Select two gears to align, or center position of them.");
		EditorGUILayout.LabelField ("The first one you select will be the one to move");
		EditorGUILayout.LabelField ("  towards the second selected, that will stay.");
		EditorGUILayout.LabelField ("(Use <Shift> key to multi-select in scene view.)");
		EditorGUILayout.Separator();
	}

	private GameObject[] FirstTwoSelectedGears() {
		
		GameObject[] newObjects = new GameObject[2];
		
		if (Selection.transforms.GetLength (0) > 1) {
			if (Selection.transforms [0] != Selection.activeGameObject.transform) {
				newObjects [0] = Selection.transforms [0].gameObject;
				newObjects [1] = Selection.transforms [1].gameObject;
			} else {
				newObjects [0] = Selection.transforms [1].gameObject;
				newObjects [1] = Selection.transforms [0].gameObject;
			}
			twoSelected = true;		
		} else {
			twoSelected = false;
		}
		return newObjects;
	}

	private void AddGearDriverToSceneObjects ()
	{
		ProceduralGear[] allGears = GameObject.FindObjectsOfType<ProceduralGear> ();
		int i = 0;
		foreach (ProceduralGear g in allGears) {
			if (g.gameObject.GetComponent<GearDriver>() == null) {
				g.gameObject.AddComponent<GearDriver>();
				i++;
			}		
		}

		ProceduralWormGear[] allWormGears = GameObject.FindObjectsOfType<ProceduralWormGear> ();
		foreach (ProceduralWormGear wg in allWormGears) {
			if (wg.gameObject.GetComponent<GearDriver>() == null) {
				wg.gameObject.AddComponent<GearDriver>();
				i++;
			}		
		}
		Debug.Log ("GearDriverEditor.cs : Added GearDriver.cs script to " + i.ToString () + " gameObjects in scene.");
	}
	
	private float SliderFloatFieldNullable(string _content, float _value, float _range, bool _allowNegativ) {
		
		EditorGUILayout.BeginHorizontal();
		_value = EditorGUILayout.FloatField(_content, _value);
		_value = (float)GUILayout.HorizontalSlider(_value, (_allowNegativ)?  -_range:0.0f, _range);
		_value = Mathf.Clamp(_value, (_allowNegativ)?  -_range:0.0f, _range);
		if (GUILayout.Button("0", EditorStyles.miniButton, GUILayout.MaxWidth(22f)))
			_value = 0.0f;
		GUILayout.Space(5);
		EditorGUILayout.EndHorizontal();
		return _value;
	}

	private void ShowInfoField() {

		GearDriver gearDriver = (GearDriver)target;
		if (gearDriver.settings.outputTo == null)
			return;
		Color gCol = GUI.color;
		string str = (gearDriver.settings.isShaft&&gearDriver.settings.isMotor)? "\n Motor (Shaft)" : 
			(!gearDriver.settings.isShaft && !gearDriver.settings.isMotor)? "\n Gear" : gearDriver.settings.isShaft? "\n Shaft": "\n Motor (Gear)";
		str = gearDriver.settings.isWorm? (gearDriver.settings.isMotor)? "\n Motor (Worm Gear)" : "\n Worm Gear"  : str;
		InfoField (" Outputs connected: " + gearDriver.settings.outputTo.Count.ToString() + str +
		           "\n Actual Speed: " + (-gearDriver.motorSpeedRPM).ToString()+ " rpm", new Color(0.85f,0.85f,0.85f),220f);
		GUI.color = gCol;
	}

	private void InfoField(string _text, Color _color, float _width) {
		
		GUILayout.Space(3);
		Texture2D texture = new Texture2D(1, 1);
		texture.hideFlags = HideFlags.HideAndDontSave;
		texture.SetPixels(new []{_color}); texture.Apply();
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleLeft;
		style.margin = new RectOffset(0,0,4,0);
		style.fixedWidth = _width;
		style.normal.background = texture;
		GUILayout.Box(_text, style);
	}

}
