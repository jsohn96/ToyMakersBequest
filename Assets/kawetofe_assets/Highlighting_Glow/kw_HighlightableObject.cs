using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Kawetofe_highlightable object.
/// </summary>
public class kw_HighlightableObject : MonoBehaviour {
	
	/// <summary>
	/// Setting the variables
	/// </summary>
	private Shader highlightShader;
	private List<Material> presetMaterials;
	private List<Shader> presetShaders;
	public bool bouncing = false;
	public float bouncingDuration = 1.0f;
	public bool isHighlightedAtStart = false;
	private Color highlightedColor = Color.white; // color when highlighted
	public Color rimColor = Color.cyan; // Rim Color
	[Range(1.0f,10f)] public float rimPower = 3.0f; // Rim power
    public bool usePBRShader = true;
    public bool useOutline = true;
    public Color outlineColor = Color.black;
    [Range(0.000f,0.02f)] public float outlineThickness = 0.005f;
    [Range(-0.03f, 0.03f)] public float outlineShift = 0f;
	
	
	
	
	/// <summary>
	/// Searches the GameObject and its children for used materials and the preset shaders.
	/// </summary>
	void Start () {
        if (usePBRShader && useOutline)
            highlightShader = Shader.Find("kawetofe/highlightShaderUnity5WithOutline"); //HighlightShader directory
        else if (usePBRShader && !useOutline)
            highlightShader = Shader.Find("kawetofe/highlightShaderUnity5");
        else if (!usePBRShader && !useOutline)
            highlightShader = Shader.Find("kawetofe/HighlightShader");
        else if (!usePBRShader && useOutline)
            highlightShader = Shader.Find("kawetofe/HighlightShaderWithOutline");
        if (highlightShader == null) {
			Debug.Log("HighlightShaders not found, HighlightShaders must be in /kawetofe/ folder.");		
		}

        // Outline effect is not supported on android

        presetMaterials = new List<Material>(); //list of all preset materials 
		presetShaders = new List<Shader>(); //list of all used shaders
		GetPresetMaterials(gameObject); // get the materials of the parent GameObject
		Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
		foreach( Transform child in allChildren){
		GetPresetMaterials(child.gameObject);//get the children materials

		}

	}
	
	
	/// <summary>
	/// Main highlight function. Use this to Highlight the GameObject
	/// </summary>
		
	public void Highlight(){
		UpdatePresetMaterials ();
		foreach (Material mat in presetMaterials){
			
			mat.shader = highlightShader;
			mat.SetColor("_MainColor",highlightedColor);
			mat.SetColor("_RimColor",rimColor);
			mat.SetFloat("_RimPower",rimPower);
            mat.SetColor("_outlineColor", outlineColor);
            mat.SetFloat("_outlineThickness", outlineThickness);
            mat.SetFloat("_outlineShift", outlineShift);
		}
	}


	public void UpdatePresetMaterials(){
		//presetMaterials.Clear ();
		//presetShaders.Clear ();
		GetPresetMaterialsOnRuntime(gameObject); // get the materials of the parent GameObject
		Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
		foreach( Transform child in allChildren){
			GetPresetMaterialsOnRuntime(child.gameObject);//get the children materials
			
		}
	}

	/// <summary>
	/// Set the GameObject to its default material values.
	/// </summary>
	
	public void DeleteHighlight(){
		for(int i=0; i < presetMaterials.Count ;i++){
			presetMaterials[i].shader = presetShaders[i];
		}
		isHighlightedAtStart = false;
	}
	
	
	/// <summary>
	/// Highlight with Bouncing effect.
	/// Use This Method to Bounce the Highlighting of the Object
	/// </summary>
	public void BouncingHighlight(){
			bouncing = true;		
			Highlight();
	}
	
	
	/// <summary>
	/// Gets the preset materials.
	/// </summary>
	/// <param name='obj'>
	/// GameObject.
	/// </param>
	
	private void GetPresetMaterials(GameObject obj){
		if(obj.GetComponent<Renderer>() != null){
			foreach(Material mat in obj.GetComponent<Renderer>().materials){

				if(mat.HasProperty("_Color")){
					highlightedColor = mat.color;
				} else {
					highlightedColor = Color.white;
				}
					presetMaterials.Add ( mat);
					presetShaders.Add (mat.shader);
				}
				
			}
		}

	private void GetPresetMaterialsOnRuntime(GameObject obj){
		int i = 0;
		if(obj.GetComponent<Renderer>() != null){
			foreach(Material mat in obj.GetComponent<Renderer>().materials){
				
				if(mat.HasProperty("_Color")){
					float alpha = 1.0f;
					highlightedColor = new Color(mat.color.r,mat.color.g,mat.color.b,alpha);
				} else {
					highlightedColor = Color.white;

				}
				if(mat.shader == this.highlightShader){
					presetMaterials[i] = presetMaterials[i];
					presetShaders[i] = presetShaders[i];
				} else {
					presetMaterials[i] = mat;
					presetShaders[i] =  mat.shader;
				}
				i++;	
			}

		
		}
	}

	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
		// Bouncing effect
		if(bouncing){
			float lerp = Mathf.PingPong(Time.time, bouncingDuration) / bouncingDuration;
			rimColor.a = lerp;
		}
		
		if(isHighlightedAtStart){
			Highlight();
		}


	}
	
	
	
}
