using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class shaderGlowCustom : MonoBehaviour
{
	[SerializeField] Renderer[] _renderers;
	[SerializeField] bool _isUsingMultipleRenderers;
	int _rendererCnt;
    public enum allowedModes { onMouseEnter, alwaysOn, userCallFunctions };
    public allowedModes glowMode;
    public bool flashing = false; //Object will flash glow
    [Range(0.1f, 4.0f)]
    public float flashSpeed = 1f; //Flash speed
    public bool noOcclusion = false; //Show glow when object is occluded
    [Range(0f,10.0f)]
    public float glowIntensity = 1f; //Glow intensity on screen of the object
    [Range(0.5f, 2.0f)]
    public float glowOpacity = 1f; //Glow opacity on screen of the object
    public Color glowColor = Color.red; //Glow color of the object

    private float clipGlow = 0.04f; //Min real allowed glow
    private float maxGlow = 0.25f; //Max real allowed glow
    private float lastGlow = 0f; //Last glow based on distance. Incrase or decrease glow based on distance to crete the constant glow effect
    private float lastOpacity = 0f; //Last glow based on distance. Incrase or decrease glow based on distance to crete the constant glow effect
    private Color lastColor = Color.white;
    private float clipDistance = 1.0f; //Min distance. Closer the glow does not decrease
    private float maxDistance = 10f; //Max distance. Further the glow does not increase
                                     //private List<Shader> originalObjetsShader;
    private Shader highightShaderVisible = null;    //Shader used to glow object
    private Shader highightShaderHidden = null;	//Shader used to glow object
    private Shader highightShaderVisibleNormal = null;    //Shader used to glow object
    private Shader highightShaderHiddenNormal = null; //Shader used to glow object
    private bool highlighted = false;
    private float flashPosition = 1f;
    private bool flashDirectionUp = true;

	bool _disablePointerEnter = false;
	Material _newMat;
	Material[][] _materials;
	bool _firstInitialization = false;

	int _glowColorShaderString;
	int _opactiyShaderString;
	int _outlineShaderString;
	float _tempGlow;

    void Awake()
    {
        //Grab the glow shader
        //highightShaderVisibleNormal = Shader.Find("3y3net/GlowVisibleNormal");
        //highightShaderHiddenNormal = Shader.Find("3y3net/GlowHiddenNormal");
       // highightShaderVisible = Shader.Find("3y3net/GlowVisible");
        highightShaderHidden = Shader.Find("3y3net/GlowHidden");
    }

  

    // Use this for initialization
    void Start()
    {
        lastColor = glowColor;
		_rendererCnt = _renderers.Length;
		_newMat = new Material(highightShaderHidden);
		_materials = new Material[_rendererCnt][];

		_glowColorShaderString = Shader.PropertyToID ("_GlowColor");
		_opactiyShaderString = Shader.PropertyToID ("_Opacity");
		_outlineShaderString = Shader.PropertyToID ("_Outline");
    }

	public void DisablePointerEnter(bool disabled){
		_disablePointerEnter = disabled;
	}

    public void OtherPointerEnter()
    {
		if (!_disablePointerEnter) {
			OnMouseEnter ();
		}
    }

    void OnMouseEnter()
    {
		if (!_disablePointerEnter) {
			if (highlighted)
				return;
			if (glowMode == allowedModes.onMouseEnter)
				lightOn ();
		}
    }

    public void OtherPointerExit()
    {
        OnMouseExit();
    }

    void OnMouseExit()
    {
        if (!highlighted)
            return;
        if (glowMode == allowedModes.onMouseEnter)
            lightOff();
    }

    public void lightOn()
    {
		for (int i = 0; i < _rendererCnt; i++) {
			if (!_renderers[i].enabled) {
				continue;
			}
			if (!_firstInitialization) {
				_materials[i] = new Material[_renderers[i].sharedMaterials.Length+1];
				_materials[i] = _renderers [i].sharedMaterials;
				_renderers [i].sharedMaterials = _materials[i];
			}

			//Set glow color
			_newMat.SetColor(_glowColorShaderString, glowColor);
			_materials [i] [_materials [i].Length - 1] = _newMat;
			_renderers [i].sharedMaterials = _materials [i];
		}
		_firstInitialization = true;
		highlighted = true;
    }

    public void lightOff()
    {
        
		for (int i = 0; i < _rendererCnt; i++) {
			if (!_renderers[i].enabled) {
				continue;
			}
			_materials [i] [_materials [i].Length - 1] = null;
			_renderers [i].sharedMaterials = _materials[i];
		}
			
        lastGlow = 0;
        highlighted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (glowMode == allowedModes.alwaysOn && !highlighted)
            lightOn();

        if (highlighted)
        {
            if (flashing)
            {
                if (flashDirectionUp)
                {
                    flashPosition += flashSpeed * Time.deltaTime;
					if (flashPosition > 1.5f)
                    {
                        flashDirectionUp = false;
                        flashPosition = 1.5f;
                    }
                }
                else
                {
                    flashPosition -= flashSpeed * Time.deltaTime;
                    if (flashPosition < 0.75f)
                    {
                        flashDirectionUp = true;
                        flashPosition = 0.75f;
                    }
                }
            }
     

			for (int i = 0; i < _rendererCnt; i++) {
				int tempMaterialLength = _renderers [i].materials.Length - 1;

				if (!_renderers [i].enabled) {
					continue;
				}
					_tempGlow = glowIntensity * flashPosition;
					_renderers [i].materials [tempMaterialLength].SetFloat (_outlineShaderString, _tempGlow);
				if (glowOpacity != lastOpacity)
				{
					_renderers [i].materials [tempMaterialLength].SetFloat(_opactiyShaderString, glowOpacity);
				}

				if (glowColor != lastColor)
				{
					_renderers [i].materials [tempMaterialLength].SetColor(_glowColorShaderString, glowColor);

				}
			}
			lastOpacity = glowOpacity;
			lastColor = glowColor;
        }
    }


}
