using UnityEngine;
using System.Collections;
using System.IO;

public class MetricManagerScript : MonoBehaviour {
	/*
	The script works by calling the appropriate public function when you want to log a line in the txt file.
	This script will continue to append each line into a string. This string will be used to generate the text file upon Application Quit.
	ex) if I want to log the position of the main character
	MetricsManagerScript.instance.LogVector3("Main character position", transform.position);

	**Where to place the script:
	Place the script in an empty gameobject, and place it in the first scene of the game
	You may also put this script in every scene you plan on having metrics logged
	if you plan on not starting from the same scene everytime.
	*/

	// The string storing all the logged data
	string createText = "";
	// The script is made into public static so that it may be accessible from any script
	public static MetricManagerScript instance = null; 

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
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
		time = time.Replace (" ", "_"); //empty spaces also don't seem to work with Unity
		time = time.Replace (":", "-");
		string reportFile = "GameName_Metrics_" + time + ".txt"; 


		FileInfo file = new System.IO.FileInfo (reportFile);
		File.WriteAllText (file.FullName, createText);
		//In Editor, this will show up in the project folder root (with Library, Assets, etc.)
		//In Standalone, this will show up in the same directory as your executable
	}

	// Logs a timestamp (use for logging an event being triggered)
	public void LogTime(string logTitle){
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString ();
		time = time.Replace ("/", "-"); 
		createText += logTitle + ": " + " - Time: " + time + "\r\n";
	}

	// Logs a Int with timestamp
	public void LogInt(string logTitle, int intToLog){
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString ();
		time = time.Replace ("/", "-"); 
		createText += logTitle + ": " + intToLog + " - Time: " + time + "\r\n";
	}

	// Logs a Float with timestamp
	public void LogFloat(string logTitle, float floatToLog) {
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString ();
		time = time.Replace ("/", "-"); 
		createText += logTitle + ": " + floatToLog + " - Time: " + time + "\r\n";
	}

	// Logs a Vector3 with timestamp
	public void LogVector3(string logTitle, Vector3 vector3ToLog) {
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString ();
		time = time.Replace ("/", "-"); 
		createText += logTitle + ": " + "- Vector3 (" + vector3ToLog.x + ", " + vector3ToLog.y + ", " + vector3ToLog.z + ") - Time: " + time + "\r\n";
	}

	/* Feel free to add more functions as needed 
	ex) 
	public void *NAME OF THE FUNCTION(string logTitle, *THE VARIABLE TO LOG) {
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); 
		createText += logTitle + ": " + "*THE VARIABLE TO LOG" - Time: " + time + "\r\n";
	}
	*/
}
