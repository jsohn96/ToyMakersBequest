using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction{
	up,
	down,
	right,
	left
}

public class ButtonSystem: MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	[SerializeField] Direction _whichDirection;
	[SerializeField]GameObject _messageReceiver;

	Vector2 _inBound;
	Vector2 _outBound;

	RectTransform _rectTransform;

	bool _pointerDown = false;

	void Start(){
		_rectTransform = GetComponent<RectTransform> ();
		_inBound = _rectTransform.anchoredPosition;
		_outBound = _inBound;
		switch (_whichDirection) {
		case Direction.down:
			_inBound.y -= 8f;
			_outBound.y += 8f;
			break;
		case Direction.left:
			_outBound.x += 8f;
			_inBound.x -= 8f;
			break;
		case Direction.right:
			_outBound.x -= 8f;
			_inBound.x += 8f;
			break;
		case Direction.up:
			_outBound.y -= 8f;
			_inBound.y += 8f;
			break;
		default:
			break;
		}
	}

	public void OnPointerDown(PointerEventData pointerEventData){
		_messageReceiver.SendMessage ("OnPointerDown", _whichDirection);
		_pointerDown = true;
		_rectTransform.anchoredPosition = _inBound;
	}

	public void OnPointerUp(PointerEventData pointerEventData){
		_messageReceiver.SendMessage ("OnPointerUp", _whichDirection);
		_pointerDown = false;
	}
		

	public void AnimateArrow(){
		if (!_pointerDown) {
			_rectTransform.anchoredPosition = Vector2.Lerp (_inBound, _outBound, Mathf.PingPong (Time.time, 1f));
		}
	}
}