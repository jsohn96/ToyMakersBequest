using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {
	public static Fading _fading;

	public Texture2D fadeOutTexture; //the texture that will overlay the screen. Black image or loading graphic
	public float fadeSpeed= 1f; // the fading speed

	private int drawDepth = -1000; // the texture's order in the draw hierarchy: a low number means render on top
	private float alpha = 1.0f; // the texture's alpha value between 0 - 1
	private int fadeDir = -1; // the direction to fade: in = -1 or out = 1
	[SerializeField] bool _noStartFade = false;
	[SerializeField] bool _EndStateForBuild = false;

	void Awake(){
		/*if (_fading) {
			Destroy (this);
		}
		else {
			_fading = this;
			DontDestroyOnLoad (gameObject);
		}
*/
		_fading = this;
		if (_noStartFade) {
			alpha = 0.0f;
		}
	}

	void Start(){
//		Cursor.visible = false;
		SceneManager.sceneLoaded += OnLevelLoaded;
	}
	void OnGUI() {
		//fade out/ in the alpha value using a direction, a speed and TIme.deltattime to conver the operation to seconds
		alpha += fadeDir * (Time.deltaTime / fadeSpeed);
		alpha = Mathf.Clamp01 (alpha);

		//set the color of our GUI
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	// sets fadeDir to the direction paramete making the scene fade in if -1 and out if 1
	public float BeginFade(int direction){
		fadeDir = direction;
		return (fadeSpeed);
	}

	//OnLevelWasLoaded is called when a level is loaded. takes loaded level index as a parameter
	void OnLevelLoaded(Scene scene, LoadSceneMode sceneMode) {
		//alpha = 1
		if (!_noStartFade) {
			BeginFade (-1);
		}
	}

//	void WhenLevelLoads(Scene scene, LoadSceneMode mode){
//		BeginFade (-1);
//	}
//
//	void OnEnable ()
//	{
//		SceneManager.sceneLoaded += WhenLevelLoads;
//		Events.G.AddListener <Act1EndedEvent>(Fade);
//		Events.G.AddListener<DancerOnBoard> (DancerOnBoardHandle);
//	}
//
//	void OnDisable ()
//	{
//		Events.G.RemoveListener<DancerOnBoard> (DancerOnBoardHandle);
//		SceneManager.sceneLoaded -= WhenLevelLoads;
//		Events.G.AddListener <Act1EndedEvent>(Fade);
//	}
//
//	void DancerOnBoardHandle(DancerOnBoard e){
//		int nodeIndex = e.NodeIdx;
//		if(_EndStateForBuild){
//			if (nodeIndex == 234 || nodeIndex == 213) {
//				StartCoroutine (ChangeLevel ());
//			}
//		}
//	}
//
//	void Fade(Act1EndedEvent e){
//		StartCoroutine (FadetoBlack());
//	}
//
//	IEnumerator FadetoBlack(){
//		yield return new WaitForSeconds(5f);
//		BeginFade (1);
//	}
//	IEnumerator ChangeLevel(){
//		yield return new WaitForSeconds(0.5f);
//		float fadeTime = BeginFade (1);
//		yield return new WaitForSeconds(fadeTime);
//		SceneManager.LoadScene (5);
//	}


//StartCoroutine(ChangeLevel());
//IEnumerator ChangeLevel(){
//	yield return new WaitForSeconds(0.5f);
//	float fadeTime = GameObject.Find ("Main").GetComponent<fading> ().BeginFade (1);
//	yield return new WaitForSeconds(fadeTime);
//	if (Application.loadedLevel != 5) {
//		Application.LoadLevel (Application.loadedLevel + 1);
//	} else {
//		Application.LoadLevel(5);
//	}
}
