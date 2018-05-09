using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheatreStar : MonoBehaviour {
	[SerializeField] SpriteRenderer _spriteRenderer;
	[SerializeField] AnimationCurve _starGlowCurve;
	bool _isGlowing = false;
	bool _isClicked = false;
	Color _fullColor = new Color (1f,1f,1f,0.7f);
	Color _emptyColor = new Color(1f,1f,1f,0f);

	float _timer = 0f;
	float _rateOfGlow;

	[SerializeField] bool _connectLines = false;
	[SerializeField] TheatreStarLine _theatreStarLine;
	bool _isActivated = false;

	void Start(){
		_rateOfGlow = Random.Range (3f, 7f);
	}

	void OnTouchDown(){
		if (_isActivated) {
			if (!_isClicked) {
				if (_connectLines) {
					_theatreStarLine.AddStarToLine (transform.position);	
				}
				Events.G.Raise (new TheatreFadeOutStarsEvent ());
			}
			_isClicked = true;
			_spriteRenderer.color = _fullColor;
			_timer = 0f;
		}
	}

	void OnMouseOver(){
		if (!_isClicked) {
			_timer += Time.deltaTime / _rateOfGlow;
			_spriteRenderer.color = Color.Lerp (_emptyColor, _fullColor, _starGlowCurve.Evaluate (Mathf.PingPong (_timer, 1f)));
		}
	}

	void OnMouseExit(){
		if (!_isClicked) {
			_timer = 0f;
			_spriteRenderer.color = _emptyColor;
		}
	}

	void Update(){
		if (_isGlowing) {
			_timer += Time.deltaTime / _rateOfGlow;
			_spriteRenderer.color = Color.Lerp (_fullColor, _emptyColor, _starGlowCurve.Evaluate(Mathf.PingPong(_timer, 1f)));
		}
	}

	void ActivateStars(TheatreActivateStarsEvent e){
		_isActivated = true;
		_isGlowing = true;

	}

	void OnEnable(){
		Events.G.AddListener<TheatreActivateStarsEvent> (ActivateStars);
	}

	void OnDisable(){
		Events.G.RemoveListener<TheatreActivateStarsEvent> (ActivateStars);
	}
}
