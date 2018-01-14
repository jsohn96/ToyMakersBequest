using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(PathNetwork))]
public class NodeDataEditor : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PathNetwork myScript = (PathNetwork)target;
		if(GUILayout.Button("Save To Json"))
		{
			myScript.SaveNodeOrderData ();
		}
	}
//	public NodeOrderData nodeOrderData;
//
//	[MenuItem ("Window/Node Data Editor")]
//	static void Init(){
//		EditorWindow.GetWindow (typeof(NodeDataEditor)).Show ();
//	}
//
//	void OnGUI()
//	{
//		if (nodeOrderData != null) 
//		{
//			SerializedObject serializedObject = new SerializedObject (this);
//			SerializedProperty serializedProperty = serializedObject.FindProperty ("nodeOrderData");
//			EditorGUILayout.PropertyField (serializedProperty, true);
//
//			serializedObject.ApplyModifiedProperties ();
//
//			if (GUILayout.Button ("Save data"))
//			{
//				SaveNodeOrderData ();
//			}
//		}
//
//		if (GUILayout.Button ("Load data"))
//		{
//			LoadNodeOrderData();
//		}
//	}
//
//	private void LoadNodeOrderData(){
//		string path = Application.dataPath + "/LevelDesignData/NodeData/NodeOrderData1.json";
//		if (File.Exists (path)) {
//			string jsonData = File.ReadAllText (path);
//			nodeOrderData = JsonUtility.FromJson<NodeOrderData> (jsonData);
//		} else {
//			Debug.LogError ("Can't find file ");
//			nodeOrderData = new NodeOrderData();
//		}
//		
//	}
//
//	private void SaveNodeOrderData(){
//		string dataAsJson = JsonUtility.ToJson (nodeOrderData, true);
//		string path = Application.dataPath + "/LevelDesignData/NodeData/NodeData1.json";
//		File.WriteAllText (path, dataAsJson);
//	}


}
