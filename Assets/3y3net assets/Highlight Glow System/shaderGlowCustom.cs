using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class shaderGlowCustom : MonoBehaviour
{
	[SerializeField] Renderer[] _renderers;
	int _rendererCnt;
    public enum allowedModes { onMouseEnter, alwaysOn, pathNodeMode, FadeWhileHold, TriggeredFadeIn};
    public allowedModes glowMode;
    public bool flashing = false; //Object will flash glow
    [Range(0.1f, 4.0f)]
    public float flashSpeed = 1f; //Flash speed
    public bool noOcclusion = false; //Show glow when object is occluded
    [Range(0f,10.0f)]
    public float glowIntensity = 0.08f; //Glow intensity on screen of the object
    [Range(0.5f, 2.0f)]
    public float glowOpacity = 1f; //Glow opacity on screen of the object
	[SerializeField] Color glowColor = new Color(0.53f, 0.48f, 0.21f); //Glow color of the object

	[SerializeField] Color _connectedColor = new Color(0.95f, 0.94f, 0.61f);
	Color _tempGlowColor;

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

	bool _someoneIsRotating = false;
	bool _isConnected = false;
	Timer _reduceGlowTimerForFadeWhileHold;

	bool _hasBeenTriggered = false;
	bool _triggeringDone = false;
	bool _colorReturned = false;
	Timer _triggeredTimer;

    void Awake()
    {
        //Grab the glow shader
        //highightShaderVisibleNormal = Shader.Find("3y3net/GlowVisibleNormal");
        //highightShaderHiddenNormal = Shader.Find("3y3net/GlowHiddenNormal");
       // highightShaderVisible = Shader.Find("3y3net/GlowVisible");
        highightShaderHidden = Shader.Find("3y3net/GlowHidden");
    }
		
    void Start()
    {
        lastColor = glowColor;
		_rendererCnt = _renderers.Length;
		_newMat = new Material(highightShaderHidden);
		_materials = new Material[_rendererCnt][];

		_glowColorShaderString = Shader.PropertyToID ("_GlowColor");
		_opactiyShaderString = Shader.PropertyToID ("_Opacity");
		_outlineShaderString = Shader.PropertyToID ("_Outline");
		if (glowMode == allowedModes.FadeWhileHold) {
			_reduceGlowTimerForFadeWhileHold = new Timer (1.8f);
		} else if (glowMode == allowedModes.TriggeredFadeIn) {
			_triggeredTimer = new Timer (0.8f);
			glowOpacity = 0.0f;
			glowIntensity = glowIntensity / 2f;
			_tempGlowColor = glowColor;
			glowColor = _connectedColor;
		}
    }

	public void TriggerForFadeIn(){
		_colorReturned = false;
		glowColor = _tempGlowColor;
		_triggeringDone = false;
		_hasBeenTriggered = true;
		_triggeredTimer.Reset ();
		lightOn ();
		glowIntensity = glowIntensity * 2f;

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
			if (highlighted) {
				return;
			} 
			if (glowMode == allowedModes.onMouseEnter) {
				lightOn ();
			} else if (glowMode == allowedModes.TriggeredFadeIn) {
				if (_triggeringDone) {
					if (!_colorReturned) {
						glowIntensity = glowIntensity / 2f;
						_colorReturned = true;
						glowColor = _connectedColor;
					}
					lightOn ();
				} else if (!_hasBeenTriggered) {
					lightOn ();
				}
			}
		}
    }

	void OnMouseDown() {
		if (glowMode == allowedModes.FadeWhileHold) {
			lightOn ();
			_reduceGlowTimerForFadeWhileHold.Reset ();
		} else if (glowMode == allowedModes.TriggeredFadeIn && _hasBeenTriggered) {
			_triggeringDone = true;
		}
	}

	void OnMouseUp() {
		if (!_disablePointerEnter) {
			if (!highlighted)
				return;
			if (glowMode == allowedModes.FadeWhileHold) {
				lightOff ();
			}
		}
	}

    public void OtherPointerExit()
    {
        OnMouseExit();
    }

    void OnMouseExit()
    {
		if (!_disablePointerEnter) {
			if (!highlighted)
				return;
			if (glowMode == allowedModes.onMouseEnter) {
				lightOff ();
				glowOpacity = 1.0f;
			} else if (glowMode == allowedModes.TriggeredFadeIn && _triggeringDone) {
				lightOff ();
				glowOpacity = 1.0f;
			} else if (glowMode == allowedModes.TriggeredFadeIn && !_hasBeenTriggered) {
				lightOff ();
				glowOpacity = 1.0f;
			}
		}
    }

    public void lightOn()
    {
		if (!_isConnected) {
			for (int i = 0; i < _rendererCnt; i++) {
				if (!_renderers [i].enabled) {
					continue;
				}
				if (!_firstInitialization) {
					_materials [i] = new Material[_renderers [i].sharedMaterials.Length + 1];
					for (int f = 0; f < _renderers [i].sharedMaterials.Length; f++) {
						_materials [i] [f] = _renderers [i].sharedMaterials [f];
					}
					_renderers [i].sharedMaterials = _materials [i];
				}

				//Set glow color
				_newMat.SetColor (_glowColorShaderString, glowColor);
				_materials [i] [_materials [i].Length - 1] = _newMat;
				_renderers [i].sharedMaterials = _materials [i];
			}
			_firstInitialization = true;
			highlighted = true;
		}
    }

    public void lightOff()
    {
		if (!_isConnected) {
			for (int i = 0; i < _rendererCnt; i++) {
				if (!_renderers [i].enabled) {
					continue;
				}
				_renderers [i].materials [_renderers [i].materials.Length - 1].SetFloat (_outlineShaderString, 0.0f);
			}
			
			lastGlow = 0;
			highlighted = false;
		}
    }

	public void ConnectionIsTrue(bool isTrue){
		if (!_isConnected && isTrue) {
			for (int i = 0; i < _rendererCnt; i++) {
				if (!_renderers [i].enabled) {
					continue;
				}
				if (!_firstInitialization) {
					_materials [i] = new Material[_renderers [i].sharedMaterials.Length + 1];
					for (int f = 0; f < _renderers [i].sharedMaterials.Length; f++) {
						_materials [i] [f] = _renderers [i].sharedMaterials [f];
					}
					_renderers [i].sharedMaterials = _materials [i];
				}

				//Set glow color
				_newMat.SetColor (_glowColorShaderString, _connectedColor);
				_materials [i] [_materials [i].Length - 1] = _newMat;
				_renderers [i].sharedMaterials = _materials [i];
			}
			_firstInitialization = true;
			highlighted = true;

		}
		_isConnected = isTrue;
	}

	public void TriggerFadeIn(){
		if (glowMode == allowedModes.TriggeredFadeIn) {
			TriggerForFadeIn ();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (glowMode == allowedModes.alwaysOn && !highlighted) {
			lightOn ();
		}
		else if (glowMode == allowedModes.FadeWhileHold) {
			glowOpacity = _reduceGlowTimerForFadeWhileHold.PercentTimeLeft;
		} 
		else if (glowMode == allowedModes.TriggeredFadeIn) {
			if (_hasBeenTriggered && !_triggeringDone) {
				glowOpacity = _triggeredTimer.PercentTimePassed;
			}
		}

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
				if (_isConnected) {
					if (_connectedColor != lastColor) {
						_renderers [i].materials [tempMaterialLength].SetColor (_glowColorShaderString, _connectedColor);
					}
				} else {
					if (glowColor != lastColor) {
						_renderers [i].materials [tempMaterialLength].SetColor (_glowColorShaderString, glowColor);

					}
				}
			}
			lastOpacity = glowOpacity;
			if (_isConnected) {
				lastColor = _connectedColor;
			} else {
				lastColor = glowColor;
			}
        }
    }

	void OnEnable(){
		Events.G.AddListener<PathGlowEvent> (PathGlowEventHandler);
	}

	void OnDisable(){
		Events.G.RemoveListener<PathGlowEvent> (PathGlowEventHandler);
	}

	void PathGlowEventHandler( PathGlowEvent e){
		if (e.IsRotating != _someoneIsRotating) {
			_someoneIsRotating = !_someoneIsRotating;
			_disablePointerEnter = _someoneIsRotating;
		}
	}

}
