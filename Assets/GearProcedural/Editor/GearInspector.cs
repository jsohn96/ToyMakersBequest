// GearInspector.cs (C)2013 by Alexander Schlottau, Hamburg, Germany
//   shows a custom inspector for the Gear.cs script

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralGear))]

public class GearInspector : Editor {
	
	#region Tool Settings (you can change this values, if needed)
	
	private int defaultColorPalette = 1;  	// set the default colors of the points
	private int maxPoints = 200;		   	// min should by 200,
											// increase maxPoints only if you really need more points
	#endregion

	#region Private Variables
	private GUIContent
		menuPrefs    = new GUIContent("Gear", "Basic settings of this gear."),
		menuUV		 = new GUIContent("UV Mapping", "Edit the UV's of this gear here."),
		menuOpt      = new GUIContent("Options", "Edit Presets, make Prefabs and Save gears."),
		menuMat      = new GUIContent("Materials", "Edit Materials of this gear."),
		insContent   = new GUIContent("+", "duplicate this point"),
		delContent   = new GUIContent("-", "delete this point"),
		modulContent = new GUIContent("Modul", "This is the scale of the teeth."),
		tContent	 = new GUIContent("Thickness", "Thickness of the gear at the base of the teeth."),
		innContent 	 = new GUIContent("Internal Teeth","Switch from outer to internal teeth."),
		countContent = new GUIContent("Teeth Count", "How many teeth the gear will get.");
	private Vector3 pointSnap = Vector3.zero;
	
	private SerializedObject gears;
	private SerializedProperty points, prefs;
	private delegate void menu();
	private menu currentMenu;
	private int currenPartMenu = 0;
	private Color[] colors = new Color[200];
	private float colorPalette = 0.0f;
	private bool showOptInfo, showMatInfo, showPrefInfo, showUVInfo, switchInnerGearing, symmetric;
	private string path = "Assets";
	private string bSlash {get{ return (Application.platform == RuntimePlatform.OSXEditor) ? "/" : "\\"; }}
	#endregion
	
	[MenuItem("GameObject/Create Other/Procedural Gear", false, 7100)]
	
	static void CreateNewGear() {
		Vector3 position = Vector3.zero;
		if (Selection.activeTransform != null)
			position = Selection.activeTransform.position;
		GameObject go = new GameObject();
		go.name = "Gear";
		go.AddComponent(typeof(ProceduralGear));
		Selection.activeGameObject = go;
		go.transform.position = position;
		Undo.RegisterCreatedObjectUndo (go, "ProcGear Created");
	}
	
	  
	void OnEnable() {
		if (Selection.activeGameObject != null) {
			gears 	= new SerializedObject(targets);
			points 	= gears.FindProperty("points");
			prefs 	= gears.FindProperty("prefs");
			ProceduralGear g = (ProceduralGear)target;
			if (target != null) {
				switchInnerGearing = g.prefs.inner;
				if (g.points != null)
					colorPalette = g.points[g.prefs.bodyParts].uvOffX;
			}
		}
		this.currentMenu = MenuPrefs;
		SetColorPalette();
	}

	
	void OnSceneGUI() {
		
		ProceduralGear gear = (ProceduralGear)target;
		if (gear.points == null) return;

		Transform gearTransform = gear.transform;
		Vector3 baseRadius = new Vector3(gear.df * gearTransform.localScale.x,0f,0f);

		Undo.RecordObject(gear, "TM PointMove");

		Handles.Disc(Quaternion.identity, gearTransform.position,gearTransform.up, baseRadius.x, true, 0f);
		Handles.Disc(Quaternion.identity, gearTransform.position,gearTransform.up, gear.prefs.dk * gearTransform.localScale.x, true, 0f);
		int partCount = gear.prefs.bodyParts+gear.prefs.teethParts;
		for(int i = 0; i < (symmetric ? partCount+1 : gear.points.Length-3); i++) {
			if (i != gear.prefs.bodyParts) {
				string str = "";
				Vector3 oldPoint = gearTransform.TransformPoint(baseRadius + gear.points[i].offset);
				if (i > partCount) {
					 Handles.color = colors[i-partCount+1];
					 str = (i-partCount).ToString();
				} else if (i > gear.prefs.bodyParts) {
					Handles.color = colors[i-gear.prefs.bodyParts+1];
					str = (i-gear.prefs.bodyParts).ToString();
				} else {
					Handles.color = colors[i+2];
					str = (i+1).ToString(); }
				
				Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, gear.prefs.modul*0.3f, pointSnap, Handles.DotCap);
				if(oldPoint != newPoint){
					gear.points[i].offset = gearTransform.InverseTransformPoint(newPoint-baseRadius);
					if (GUI.changed) gear.UpdateGear();
				}
				Handles.Label(oldPoint, new GUIContent(str), EditorStyles.boldLabel);
			}
		}
	} 
	
	public override void OnInspectorGUI() {
	
		gears.Update();
		this.currentMenu();
		if(gears.ApplyModifiedProperties() ||
			(Event.current.type == EventType.ValidateCommand &&
			Event.current.commandName == "UndoRedoPerformed"))
			foreach(ProceduralGear g in targets) {
				if (switchInnerGearing != g.prefs.inner && targets.Length == 1) {
					g.SwitchInnerGearing();
					g.prefs.inner = !g.prefs.inner;
					switchInnerGearing = g.prefs.inner; }
				if(PrefabUtility.GetPrefabType(g) != PrefabType.Prefab) 
					g.UpdateGear();	
		}
	}

	private void MenuPrefs() {

		MenuButtons(0);
		SerializedProperty
			modul 		= prefs.FindPropertyRelative("modul"),
			teethCount 	= prefs.FindPropertyRelative("teethCount"),
			capT		= prefs.FindPropertyRelative("capT"),
			capB		= prefs.FindPropertyRelative("capB"),
			bodyParts 	= prefs.FindPropertyRelative("bodyParts"),
			teethParts 	= prefs.FindPropertyRelative("teethParts"),
			thickness 	= prefs.FindPropertyRelative("thickness"),
			slope		= prefs.FindPropertyRelative("slope"),
			inner 		= prefs.FindPropertyRelative("inner"),
			coningX		= prefs.FindPropertyRelative("coneX"),
			coningY		= prefs.FindPropertyRelative("coneY"),
			basePoint 	= points.GetArrayElementAtIndex(bodyParts.intValue),
			partCount	= basePoint.FindPropertyRelative("mat");
		if (modul == null || partCount == null) return;
		
		for (int i=0; i<3; i++) {
			EditorGUILayout.BeginHorizontal();
			if (i < 1) EditorGUILayout.PropertyField(modul, modulContent);
				else if (i == 1) EditorGUILayout.PropertyField(teethCount, countContent);
					else EditorGUILayout.PropertyField(thickness, tContent);
			GUILayout.Space(155);
			EditorGUILayout.EndHorizontal();
		}
		GUILayoutOption maxWidth = GUILayout.MaxWidth(20f);		
		if (targets.Length==1) EditorGUILayout.PropertyField(inner, innContent);
		slope.floatValue = SliderFloatFieldNullable("Slope", slope.floatValue, 3f);
		coningX.floatValue = SliderFloatFieldNullable("Coning Witdh",coningX.floatValue, 1f);
		coningY.floatValue = SliderFloatFieldNullable("Coning Radius",coningY.floatValue, 1f);
	
		if (partCount.intValue == 0){
			points.InsertArrayElementAtIndex(points.arraySize); points.InsertArrayElementAtIndex(points.arraySize);
			partCount.intValue = 1;
		} else
			symmetric = partCount.intValue == -1 ? true : false;
		if (targets.Length < 2) {
			symmetric = EditorGUILayout.Toggle("Symmetric Body", symmetric);
			partCount.intValue = symmetric ? -1 : points.arraySize-4-teethParts.intValue-bodyParts.intValue;
		}
		Color gCol = GUI.color;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Draw Caps",GUILayout.Width(145));
		if (!capT.boolValue) GUI.color = new Color(1f,1f,1f,0.6f);
		if (GUILayout.Button("Teeth", EditorStyles.miniButton)) capT.boolValue = !capT.boolValue;
		if (!capB.boolValue) GUI.color = new Color(1f,1f,1f,0.6f); else GUI.color = gCol;
		if (GUILayout.Button("Body", EditorStyles.miniButton))capB.boolValue = !capB.boolValue;
		GUI.color = gCol;
		EditorGUILayout.Space(); EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator(); EditorGUILayout.Separator();
		int j = 1;
		EditorGUILayout.LabelField("Tooth Parts");
		for(int i = bodyParts.intValue+1; i < bodyParts.intValue+teethParts.intValue+1; i++){
			SerializedProperty
				point 		= points.GetArrayElementAtIndex(i),
				offset		= point.FindPropertyRelative("offset");
			if (offset == null) return;
			EditorGUILayout.BeginHorizontal();
			InfoField(" "+j.ToString(),colors[1+j++],20f);
			EditorGUILayout.PropertyField(offset , GUIContent.none);
			if(GUILayout.Button(insContent, EditorStyles.miniButtonLeft, maxWidth)){
				points.InsertArrayElementAtIndex(i); teethParts.intValue++; }
			if (teethParts.intValue > 1) {
				if(GUILayout.Button(delContent, EditorStyles.miniButtonRight, maxWidth)){
					points.DeleteArrayElementAtIndex(i) ;teethParts.intValue--; }
			} else DeleteButtonDisabled();
			EditorGUILayout.EndHorizontal();
		}
		j = 1;
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField( symmetric ? "Body Parts" : "Body Parts Frontside");
		for(int i = 0; i < bodyParts.intValue ; i++) {
			SerializedProperty
				point 		= points.GetArrayElementAtIndex(i),
				offset		= point.FindPropertyRelative("offset");
			EditorGUILayout.BeginHorizontal();
			InfoField(" "+(j).ToString(),colors[1+j++],20f);
			EditorGUILayout.PropertyField(offset , GUIContent.none);
			if(GUILayout.Button(insContent, EditorStyles.miniButtonLeft, maxWidth)){
				points.InsertArrayElementAtIndex(i); bodyParts.intValue++; }
			if (bodyParts.intValue > 1) {
				if(GUILayout.Button(delContent, EditorStyles.miniButtonRight, maxWidth)){
					points.DeleteArrayElementAtIndex(i) ;bodyParts.intValue--; }
			} else DeleteButtonDisabled();
			EditorGUILayout.EndHorizontal();
		}
		
		if (!symmetric) {
			j = 1;
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Body Parts Backside");
			for(int i = points.arraySize-partCount.intValue-3; i < points.arraySize-3 ; i++){
				SerializedProperty
					point 		= points.GetArrayElementAtIndex(i),
					offset		= point.FindPropertyRelative("offset");
				if (offset == null) return;
				EditorGUILayout.BeginHorizontal();
				InfoField(" "+(j).ToString(),colors[1+j++],20f);
				EditorGUILayout.PropertyField(offset , GUIContent.none);
				if(GUILayout.Button(insContent, EditorStyles.miniButtonLeft, maxWidth)){
					points.InsertArrayElementAtIndex(i); partCount.intValue++; }
				if (partCount.intValue > 1) {
					if(GUILayout.Button(delContent, EditorStyles.miniButtonRight, maxWidth)){
						points.DeleteArrayElementAtIndex(i) ; partCount.intValue--; }
				} else DeleteButtonDisabled();
				EditorGUILayout.EndHorizontal();
			}
		}
		EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.BeginHorizontal();
		ShowInfoField(); GUI.color = Color.gray; EditorGUILayout.LabelField(" ");
		if (GUILayout.Button("?", GUILayout.Width(20))) showPrefInfo = !showPrefInfo;
		EditorGUILayout.EndHorizontal();
		if (showPrefInfo) ShowPrefInfo();
		EditorGUILayout.Separator();
	}

	private void MenuMat() {
		
		MenuButtons(1);
		ProceduralGear g = (ProceduralGear)target;
		SerializedProperty
			bodyParts  = prefs.FindPropertyRelative("bodyParts"),
			teethParts = prefs.FindPropertyRelative("teethParts"),
			calculateTangens = prefs.FindPropertyRelative("tangens");
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(calculateTangens, new GUIContent("Calculate Tangens","Check this, only if shader needs them.\nCalculate tangens needs more time on CPU."));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.LabelField("Textures in Renderer:");
		EditorGUILayout.BeginHorizontal();
		int matCount = 0;
		foreach(Material m in g.GetComponent<Renderer>().sharedMaterials) {
			GUILayout.Label(matCount.ToString(), GUILayout.MaxWidth(10f));
			GUILayout.Label(m.mainTexture, GUILayout.Width(50f), GUILayout.Height(50f));
			matCount++;
		}
		EditorGUILayout.EndHorizontal();
		MenuButtons_Parts(currenPartMenu);
		GUILayoutOption labelHeight = GUILayout.Height(40f);
		
		int[] indexes = GetStartEndPartIndex(bodyParts.intValue, teethParts.intValue, 0);
		for (int i=indexes[0]; i< indexes[1];i++) {
			if (i == points.arraySize-2)  EditorGUILayout.LabelField("Cap Body");
			else 
				if (i == points.arraySize-1) EditorGUILayout.LabelField("Cap Teeth");
			SerializedProperty
				point = points.GetArrayElementAtIndex(i),
				mat   = point.FindPropertyRelative("mat");
			if (mat == null) break;
			if (mat.intValue > g.GetComponent<Renderer>().sharedMaterials.Length-1) mat.intValue = 0;
			EditorGUILayout.BeginHorizontal();
			InfoField(" "+ indexes[2].ToString(),colors[++indexes[2]], 15f);
			GUILayout.Label(g.GetComponent<Renderer>().sharedMaterials[mat.intValue].mainTexture, GUILayout.Width(40f), labelHeight);
			GUILayout.Label(g.GetComponent<Renderer>().sharedMaterials[mat.intValue].name,GUILayout.Width(120f), labelHeight);
			for (int j=0; j < matCount; j++)
				if (GUILayout.Button(j.ToString(), GUILayout.MaxWidth(35f))) mat.intValue = j;
			EditorGUILayout.LabelField(" ");
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Separator(); EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		ShowInfoField();
		GUI.color = Color.gray;
		EditorGUILayout.LabelField(" ");
		if (GUILayout.Button("?", GUILayout.Width(20))) showMatInfo = !showMatInfo;
		EditorGUILayout.EndHorizontal();
		if (showMatInfo) ShowMatInfo();
		EditorGUILayout.Separator();
	}
		
	private void MenuUV() {
		
		MenuButtons(2);
		SerializedProperty
			bodyParts 	= prefs.FindPropertyRelative("bodyParts"),
			teethParts = prefs.FindPropertyRelative("teethParts"),
			autoUVscale = prefs.FindPropertyRelative("ramp"),
			automaticUV = prefs.FindPropertyRelative("autoUV"),
			toothFlanks = prefs.FindPropertyRelative("flanks");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(automaticUV, new GUIContent("Automatic Mapping","UV's will be set by the angle of the faces."));
		if(GUILayout.Button("Reset UVs", EditorStyles.miniButtonMid)) ResetUVs();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		if (automaticUV.boolValue)
			autoUVscale.floatValue = SliderFloatField("Ramp Angle", autoUVscale.floatValue, -1, 1f);
		else {
			EditorGUILayout.Separator();
			toothFlanks.boolValue = EditorGUILayout.Toggle("Tooth Flanks Planar",toothFlanks.boolValue);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			SerializedProperty		// use "unused" bodycap point.offset as storage for ranges (scale/offset)
				scalePoint 	= points.GetArrayElementAtIndex(points.arraySize-2),
				scaleValue	= scalePoint.FindPropertyRelative("offset");
			Vector3 scale = scaleValue.vector3Value;
			scale.x = EditorGUILayout.FloatField("Scale Range", scale.x);
			if (scale.x == 0) scale.x = 1.0f;
			scale.x = Mathf.Clamp(scale.x, 0.001f, 1000f);
			if (GUILayout.Button("1", EditorStyles.miniButton, GUILayout.MaxWidth(22f)))
				scale.x = 1.0f;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			scale.y = EditorGUILayout.FloatField("Offset Range", scale.y);
			if (scale.y == 0) scale.y = 1.0f;
			scale.y = Mathf.Clamp(scale.y, 0.001f, 1000f);
			if (GUILayout.Button("1", EditorStyles.miniButton, GUILayout.MaxWidth(22f)))
				scale.y = 1.0f;
			scaleValue.vector3Value = scale;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			MenuButtons_Parts(currenPartMenu);

			int[] indexes = GetStartEndPartIndex(bodyParts.intValue, teethParts.intValue, 1);
			if (indexes[3] == 3) EditorGUILayout.LabelField("Caps");
			for(int i = indexes[0]; i < indexes[1]; i++){
				SerializedProperty
					point 		= points.GetArrayElementAtIndex(i),
					uvMapping   = point.FindPropertyRelative("uvMapping"),
					uvScaleX	= point.FindPropertyRelative("uvScaleX"),
					uvScaleY	= point.FindPropertyRelative("uvScaleY"),
					uvOffsetX   = point.FindPropertyRelative("uvOffX"),
					uvOffsetY   = point.FindPropertyRelative("uvOffY");
				if (uvMapping == null) break;
				
				EditorGUILayout.Separator();
				bool b = uvMapping.boolValue;
				string s = "";
				if (indexes[3] == 1)
					s = (symmetric ? "Body ":"Body Front ")+indexes[2].ToString()+". Part";
				else if (indexes[3] == 0)
					s = "Teeth "+(indexes[2]).ToString()+". Part";
				else if (indexes[3] == 2)
					s = "Body Back "+(indexes[2]).ToString()+". Part";
				else if (i == points.arraySize-2)
					s = "Cap on Body";
				else
					s = "Cap on Teeth";
				EditorGUILayout.LabelField(new GUIContent("UV Mapping "+s,"Mapping of the UV's."));
				b = EditorGUILayout.Toggle("Cylindrical",uvMapping.boolValue);
				bool b2 = !b;
				b2 = EditorGUILayout.Toggle("Planar", b2);
				b = !b2;
				uvMapping.boolValue = b;
			
	            uvScaleX.floatValue = SliderFloatField("Scale X", uvScaleX.floatValue, ++indexes[2], scale.x);
	            uvScaleY.floatValue = SliderFloatField("Scale Y", uvScaleY.floatValue, indexes[2], scale.x);
				uvOffsetX.floatValue = SliderFloatField("Offset X", uvOffsetX.floatValue,indexes[2], scale.y);
	            uvOffsetY.floatValue = SliderFloatField("Offset Y", uvOffsetY.floatValue,indexes[2], scale.y);
			}		
		}
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		ShowInfoField();
		GUI.color = Color.gray;
		EditorGUILayout.LabelField(" ");
		if (GUILayout.Button("?", GUILayout.Width(20))) showUVInfo = !showUVInfo;
		EditorGUILayout.EndHorizontal();
		if (showUVInfo) ShowUVInfo();
		EditorGUILayout.Separator();
	}
	
	private void MenuOpt() {
		
		Color gCol = GUI.color;
		MenuButtons(3);
		ProceduralGear g = (ProceduralGear)target;
		GUI.color = gCol;
		g.name = EditorGUILayout.TextField("Name",g.name.Replace("(Clone)",""), GUILayout.Width(300),GUILayout.Height(14));
		path = EditorGUILayout.TextField("Path",path, GUILayout.Width(300),GUILayout.Height(14));
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		bool noPath = false;
		if (System.IO.File.Exists(Application.dataPath.Replace("Assets",path) + bSlash + g.name + ".asset")) {
			GUI.color = Color.red;	
			InfoField("  File exist or mesh already saved.", Color.red, 205f);
			EditorGUILayout.Space();
		} else
			if (System.IO.Directory.Exists(Application.dataPath.Replace("Assets",path)) && path != "")
				EditorGUILayout.LabelField("Save mesh to Asset folder");
			else {	
				GUI.color = Color.red;	
				InfoField("  Path does not exist.", Color.red, 150f);
				EditorGUILayout.Space();
				noPath = true;
			}
		
		GUILayoutOption buttonWidth = GUILayout.Width(120);
		if (GUILayout.Button("Save Mesh", buttonWidth))
			if (GUI.color != Color.red)
				SaveMesh();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		if (GUI.color == Color.red)	EditorGUILayout.Separator();
		if (!noPath) GUI.color = gCol;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Create a prefab in Assets folder");
		if (GUILayout.Button("Create Prefab", buttonWidth))
			if (GUI.color != Color.red) 
				CreatePrefab(false, false);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Duplicate and make this to prefab");
		if (GUILayout.Button("Duplicate", buttonWidth)) 
			if (GUI.color != Color.red)
				CreatePrefab(false, true);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Copy as object and this to prefab");
		if (GUILayout.Button("Copy Object", buttonWidth))
			if (GUI.color != Color.red)
				CreatePrefab(true, true);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Preferences:");
		GUI.color = gCol;
		EditorGUILayout.Separator();
		if (targets.Length < 2) {
			GUI.color = Color.white;
			if (g.points[g.prefs.bodyParts].uvOffX == 0) g.points[g.prefs.bodyParts].uvOffX = defaultColorPalette;
			colorPalette = Mathf.Clamp(Mathf.FloorToInt(SliderFloatField("Color Palette", g.points[g.prefs.bodyParts].uvOffX,-1, 200f)), 1, 200);
			if (g.points[g.prefs.bodyParts].uvOffX != colorPalette) {
				SetColorPalette();
				g.points[g.prefs.bodyParts].uvOffX = colorPalette;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			for (int i=2;i<20;i++) InfoField("", newColor(i), 10f);
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
		} else EditorGUILayout.LabelField("(Multi Object Editing not supported for preferences.)");
		EditorGUILayout.BeginHorizontal();
		GUI.color = Color.gray;
		EditorGUILayout.Space();
		if (GUILayout.Button("?", GUILayout.Width(20))) showOptInfo = !showOptInfo;
		EditorGUILayout.EndHorizontal();
		if (showOptInfo) ShowOptInfo();
		EditorGUILayout.Separator();
	}
	
	private void MenuButtons(int _i) {
		
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		Color c = GUI.color;
		if (_i == 0) GUI.color = Color.yellow;
		if(GUILayout.Button(menuPrefs, EditorStyles.miniButtonLeft))
			this.currentMenu = MenuPrefs;
		if (_i != 1) GUI.color = c; else GUI.color = Color.green;
		if(GUILayout.Button(menuMat, EditorStyles.miniButtonMid))
			this.currentMenu = MenuMat;
		if (_i != 2) GUI.color = c; else GUI.color = Color.magenta;
		if(GUILayout.Button(menuUV, EditorStyles.miniButtonMid))
			this.currentMenu = MenuUV;
		if (_i != 3) GUI.color = c; else GUI.color = Color.red;
		if(GUILayout.Button(menuOpt, EditorStyles.miniButtonRight))
			this.currentMenu = MenuOpt;
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
	}
		
	private void MenuButtons_Parts(int _i) {
		
		SerializedProperty
			capT = prefs.FindPropertyRelative("capT"),
			capB = prefs.FindPropertyRelative("capB");
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		GUILayoutOption maxWidth = GUILayout.MaxWidth(100f);
		Color c = GUI.color;
		Color a = new Color(0.8f,0.8f,0.8f,1f);
		GUIStyle s = EditorStyles.miniButtonRight;
		if (!symmetric) s = EditorStyles.miniButtonMid;
		if (capT.boolValue || capB.boolValue) s = EditorStyles.miniButtonMid;
		
		if (_i == 0) GUI.color = a;
		if(GUILayout.Button("Teeth", EditorStyles.miniButtonLeft,maxWidth))
			currenPartMenu = 0;
		
		if (_i != 1) GUI.color = c; else GUI.color = a;
		if(GUILayout.Button(symmetric ? "Body":"Body Front", s, maxWidth))
			currenPartMenu = 1;
		
		if (!symmetric) {
			if (_i != 2) GUI.color = c; else GUI.color = a;
			if (!capT.boolValue && !capB.boolValue) s = EditorStyles.miniButtonRight; 
			if(GUILayout.Button("Body Back", s, maxWidth))
				currenPartMenu = 2; 
		} else if (_i==2) currenPartMenu=1;
		
		if (capT.boolValue || capB.boolValue) {
			if (_i != 3) GUI.color = c; else GUI.color = a;
			if(GUILayout.Button("Caps", EditorStyles.miniButtonRight, maxWidth))
				currenPartMenu = 3;
		} else if (_i==3) currenPartMenu=0;
		
		GUI.color = c;
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
	}
	
	private void ShowInfoField() {
		
		Color gCol = GUI.color;
		long t = 0; int v = 0;
		string str  = "", str2 = "", str3 ="";
		foreach(ProceduralGear g in targets) {  
			t += g.ms; v += g.vt; 
			if (g.prefs.tangens) str3 = " (with tangens)";
		}
		if (targets.Length>1) str = "(" + targets.Length.ToString() + " gears) ";
		if (t == 0) {  str2 += "<"; t = 1;  }
		if (v != 0)
			 InfoField ("  Vertices: " + v.ToString() + "    " + str +
					  "\n  Triangles: " + (v * 3).ToString() +
					  "\n  Build time: " +  str2 + t.ToString() + "ms" + str3, new Color(0.85f,0.85f,0.85f),220f);
		else InfoField ("    Too many vertices !!!\n    Remove some parts or" +
					  "\n    reduce teeth count." , Color.yellow, 220f);
		GUI.color = gCol;
	}

	private void InfoField(string _text, Color _col, float _width) {

		GUILayout.Space(3);
		Texture2D tex = new Texture2D (1, 1);
		tex.hideFlags = HideFlags.HideAndDontSave;
		tex.SetPixels(new []{_col}); tex.Apply();
		GUIStyle style = new GUIStyle();
		style.alignment = TextAnchor.MiddleLeft;
		style.margin = new RectOffset(0,0,4,0);
		style.fixedWidth = _width;
		style.normal.background = tex;
		GUILayout.Box(_text, style);

	}
	
	private void DeleteButtonDisabled() {
		
		Color gCol = GUI.color;
		GUI.color = new Color(1f,1f,1f,0.4f);
		if(GUILayout.Button(delContent, EditorStyles.miniButtonRight, GUILayout.Width(20))){}
		GUI.color = gCol;
	}
	
	private float SliderFloatField(string _content, float _value, int _i, float _max) {
		
		EditorGUILayout.BeginHorizontal();
		if (_i > -1) InfoField(" ",colors[_i],20f);
			else if (_i == -2) InfoField(" ",Color.white,20f);
		_value = EditorGUILayout.FloatField(_content, _value);
		_value = (float)GUILayout.HorizontalSlider(_value, 0f, _max);
		_value = Mathf.Clamp(_value, 0f, _max);
		EditorGUILayout.EndHorizontal();
		return _value;
	}
	
	private float SliderFloatFieldNullable(string _content, float _value, float _range) {
		
		EditorGUILayout.BeginHorizontal();
		_value = EditorGUILayout.FloatField(_content, _value);
		_value = (float)GUILayout.HorizontalSlider(_value, -_range, _range);
		_value = Mathf.Clamp(_value, -_range, _range);
		if (GUILayout.Button("0", EditorStyles.miniButton, GUILayout.MaxWidth(22f)))
			_value = 0.0f;
		GUILayout.Space(5);
		EditorGUILayout.EndHorizontal();
		return _value;
	}
	
	private int[] GetStartEndPartIndex(int _bodyParts, int _teethParts, int _m) {
		
		int start=0, end=2, c=1, p = 0;
		if (currenPartMenu == 0) {
			start = _bodyParts+1-_m; end = _bodyParts+1+_teethParts-_m;
			EditorGUILayout.LabelField("Tooth Parts");
		}
		if (currenPartMenu == 1) {
			start = 0; end = _bodyParts;
			EditorGUILayout.LabelField(symmetric ? "Body Parts" : "Body Parts Front");
			p = 1;
		}
		if (currenPartMenu == 2) {
			start = _bodyParts+_teethParts+1; end = points.arraySize-3;
			EditorGUILayout.LabelField("Body Parts Back");
			p = 2;
		}
		if (currenPartMenu > 2)  {
			start = points.arraySize-2; end = points.arraySize;
			p = 3;
		}
		return new int[] {start, end, c, p};
	}
	
	private void SetColorPalette() {
		
		colors = new Color[maxPoints];
		if (colorPalette == 0)
			colorPalette = defaultColorPalette;
		for (int i=0; i<maxPoints; i++)
			colors[i]= newColor(i);
	}
	
	private Color newColor(int _i) {
		
		return new Color(Mathf.Sin(_i*colorPalette*3)*0.5f+0.5f,
							Mathf.Cos(_i*colorPalette)*0.5f+0.5f,
								Mathf.Tan(_i*colorPalette*2)*0.5f+0.5f);
	}
	
	private void ResetUVs() {
		
		foreach(ProceduralGear g in targets) {
			for (int i=0; i < points.arraySize;i++) {
				g.points[i].uvOffX = 0.0f; g.points[i].uvOffY = 0.0f;
				g.points[i].uvScaleX = 1.0f; g.points[i].uvScaleY = 1.0f;
			}
			g.points[points.arraySize-2].uvMapping = true;
			g.points[points.arraySize-1].uvMapping = false;
			g.prefs.autoUV = true; g.prefs.flanks = true;
			g.UpdateGear();
		}
	}
	
	private void SaveMesh() {

		ProceduralGear g = (ProceduralGear)target;
		string meshPrefabPath = "";
		meshPrefabPath = path + bSlash + g.name + ".asset";
		MeshFilter filter  = (MeshFilter)g.GetComponent(typeof(MeshFilter));
		filter.sharedMesh.hideFlags = HideFlags.None;
		Unwrapping.GenerateSecondaryUVSet(filter.sharedMesh);
		Mesh mesh = Mesh.Instantiate(filter.sharedMesh) as Mesh;
		if (!System.IO.File.Exists(Application.dataPath.Replace("Assets",path) + bSlash + g.name + ".asset"))
			AssetDatabase.CreateAsset(mesh, meshPrefabPath);
		else
			AssetDatabase.SaveAssets();
		filter.sharedMesh.hideFlags = HideFlags.HideAndDontSave;
	}
	
	private void CreatePrefab(bool _delete, bool _inst) {

		ProceduralGear g = (ProceduralGear)target;
		MeshFilter filter  = (MeshFilter)g.GetComponent(typeof(MeshFilter));
		GameObject newGO = new GameObject();
		newGO.name = g.name;
		newGO.AddComponent(typeof(MeshFilter));
		newGO.AddComponent(typeof(MeshRenderer));
		SaveMesh();
		MeshFilter f = (MeshFilter)newGO.GetComponent(typeof(MeshFilter));
		f.sharedMesh = Mesh.Instantiate(filter.sharedMesh) as Mesh;
		MeshRenderer r  = (MeshRenderer)newGO.GetComponent(typeof(MeshRenderer));
		MeshRenderer r2 = (MeshRenderer)g.GetComponent(typeof(MeshRenderer));
		r.sharedMaterials = r2.sharedMaterials;
		#pragma warning disable 618
		Object prefab = EditorUtility.CreateEmptyPrefab(path +"/"+ g.name + ".prefab");
		EditorUtility.ReplacePrefab(g.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
		#pragma warning restore 618
		if (!_delete) {
			if (_inst) {
				GameObject go = (GameObject)Instantiate(g.gameObject, g.gameObject.transform.position, g.gameObject.transform.rotation);
				go.name = go.name.Replace("(Clone)","");
			}
			DestroyImmediate(newGO, true);
		}
	} 

	private void ShowOptInfo() {
		
		EditorGUILayout.Separator();
		GUI.color = Color.white;
		EditorGUILayout.TextArea("Save Mesh:\n\n Saves only the mesh of this gear.\n You can drag this mesh onto an empty GameObject\n to get a gear without the procedural script attached to it.\n" +
									"\nCreate Prefab:\n\n This saves the mesh and saves this procedural gear as prefab.\n All settings of the Gear script will be saved.\n Now you can copy the gear gameObject in scene view.\n"+
									" Change settings on one gear and click Apply on top of the inspector\nand all other clones will update to this setting.\n" +
									"\nDuplicate:\n\n Makes a copy of this procedural gear.\n Also saves the gear mesh and makes this gear to a prefab.\n"+
									"\nCopy Object:\n\n Copy this procedural gear to a new object,\n without the procedural script.\n Also saves the gear mesh and makes this gear to a prefab"+
									"\n\nPreferences - Color Palette:\n\n Set a color palette you like for the points.\n Each gear can get a different color palette.");
	}
	
	private void ShowPrefInfo() {
		
		EditorGUILayout.Separator();
		GUI.color = Color.white;
		EditorGUILayout.TextArea("Modul, Teeth Count:\n\n Modul is the size of the teeth in a norm scale.\n You see two white circles around the gear.\n Between this circles the teeth have to be, to be in norm scale.\n" +
									" Teeth Count is the amount of teeth the gear have.\n"+
									"\nThickness:\n\n This is the thickness of the gear at the base.\n The base is the point where the body connects to the teeth.\n This value should not be used to scale the gear.\n"+
									" To scale the gear use the Transforms scale.\n Use thickness if teeth should be thinner than the body, for instance.\n"+
									"\nInternal Teeth:\n\n Switch the gear to a gear with internal teeth.\n"+
									"\nSlope, Coning:\n\n Slope for helical gearing. This will bevel the tooth flanks.\n Coning Width will change the angle of the tooth flanks.\n Coning Radius change the radial angle to get cone teeth.\n"+
									"\nSymmetric Body:\n\n Create gears with both sides have the same geometric,\n or gears with different parts on each side.\n"+						
									"\nDraw Caps:\n\n Choose if the gear should be closed at the inner body and at the teeth spikes.\n"+						
									"\nTooth and Body Parts:\n\n This are the single parts of the gear.\n Add more parts by click on the + button or delete with the - button.\n You see colored points in scene view. You can move them around with the mouse.\n"+
									" Or you can move them by going with the mouse over the X Y Z in the inspector,\n click and hold mouse and move to the left or right.\n"+
									"\n If you add a point it is a copy of the point, where you have clicked.\n Move the new point by editing values in inspector is easier to get\n straight objects.\n"+
									"\nChange Colors:\n\n Click this, if the colored points can not distinguished well,\n to get new colors.");
	}
	
	private void ShowUVInfo() {
		
		EditorGUILayout.Separator();
		GUI.color = Color.white;
		EditorGUILayout.TextArea("Automatic Mapping, Ramp Angle:\n\n All parts will be mapped automatically, if checked.\n You can set an angle, where the mapping switches from\n planar to cylindrical.\n" +
									" With this option checked, you get planar mapping on flat surfaces and\n cylindrical mapping on radial surfaces like shafts or the teeth flanks.\n"+
									"\n Without Automatic Mapping checked, you can edit each part:\n"+
									"\nTooth Flanks Planar:\n\nWith this option checked, the tooth flanks allways mapped planar,\nwhatever the top of the same teeth part is mapped."+
									"\n\nScale Range, Offset Range:\n\n Use this to increase or decrease the range of the sliders values.\n Use it if you do not get the right size of your texture within\n a range of 0 to 1.");
	}
	
	private void ShowMatInfo() {
		
		EditorGUILayout.Separator();
		GUI.color = Color.white;
		EditorGUILayout.TextArea("Calculate Tangens:\n\n Check this only if a shader needs tangens.\n This takes more time on CPU.\n"+
									"\nTextures in Renderer:\n\n Shows all textures of the materials in the MeshRenderer.\n Add materials by drag&drop them into the MeshRenderer's Materials slot.\n"+
							   		"\nTooth and Body Parts:\n\n Select the material for each part. Click the buttons to change textures.\n Do not use more materials in MeshRenderer as nessessary.");
	}
}
