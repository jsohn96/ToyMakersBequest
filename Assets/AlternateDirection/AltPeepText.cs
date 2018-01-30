using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltPeepText : MonoBehaviour {
	[SerializeField] TextMeshProUGUI _tmpDog;
	[TextArea]
	[SerializeField] string[] _strings;

	public void ChangeText(int peepIndex){
		_tmpDog.text = _strings [peepIndex];
	}
}
