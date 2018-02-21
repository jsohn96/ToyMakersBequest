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
	public void OnPointerDown(PointerEventData pointerEventData){
		_messageReceiver.SendMessage ("OnPointerDown", _whichDirection);
	}

	public void OnPointerUp(PointerEventData pointerEventData){
		_messageReceiver.SendMessage ("OnPointerUp", _whichDirection);
	}
}