using UnityEngine;
using System.Collections;
using System.IO;

public class MetricManagerScript : MonoBehaviour {

	string createText = "";
	public static MetricManagerScript instance;

	//public int sampleMetric1, sampleMetric2;

	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);
	}
	
	//When the game quits we'll actually write the file.
	void OnApplicationQuit(){
		GenerateMetricsString ();
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
		time = time.Replace (" ", "_");
		time = time.Replace (":", "-");
		string reportFile = "GameName_Metrics_" + time + ".txt"; 


		FileInfo file = new System.IO.FileInfo (reportFile);
		File.WriteAllText (file.FullName, createText);
		//In Editor, this will show up in the project folder root (with Library, Assets, etc.)
		//In Standalone, this will show up in the same directory as your executable
	}

	void GenerateMetricsString(){
//		createText = 
//			"Number of times something happened 1: " + sampleMetric1 + "\n" +
//			"Number of times something happened 2: " + sampleMetric2;
	}

	public void LogTime(string WhatYouWantToCallIt){
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); 
		createText += WhatYouWantToCallIt + ": " + "- Time: " + time + "\r\n";
	}

	public void LogInt(string WhatYouWantToCallIt, int theIntToLog){
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); 
		createText += WhatYouWantToCallIt + ": " + theIntToLog + " - Time: " + time + "\r\n";
	}

	public void LogFloat(string WhatYouWantToCallIt, float theFloatToLog) {
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); 
		createText += WhatYouWantToCallIt + ": " + theFloatToLog + " - Time: " + time + "\r\n";
	}

	public void LogVector3(string WhatYouWantToCallIt, Vector3 theVector3ToLog) {
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); 
		createText += WhatYouWantToCallIt + ": " + "- Vector3 (" + theVector3ToLog.x + ", " + theVector3ToLog.y + ", " + theVector3ToLog.z + ") - Time: " + time + "\r\n";
	}

	//Add to your set metrics from other classes whenever you want
	// public void AddToSample1(int amtToAdd){
	// 	sampleMetric1 += amtToAdd;
	// }
}
