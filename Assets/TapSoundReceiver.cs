using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapSoundReceiver : MonoBehaviour {

	[SerializeField] TapSoundPlayer _tapSoundPlayer;
	public TapSoundTags _thisSoundTag;

	void OnTouchDownSound(){
		_tapSoundPlayer.PlayTapSound (_thisSoundTag);
	}
}
