using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TapSoundTags {
	heavyWood,
	mediumWood,
	floorWood,
	smallWood,
	lightWood,
	smallMetal,
	heavyMetal,
	waterTank,
	waterTankTop,
	metalTrinket,
	metalLock,
	wire,
	sparkle,
	chest, // light thud
	messageBoardMetalCreak,
	mediumMetal
}

public class TapSoundPlayer : AudioSourceController {

	[SerializeField] AudioSource[] _audioSources = new AudioSource[3];

	int _audioSourceCnt = 2;

	//Medium Wood
	[SerializeField] AudioClip[] _mediumWoodClips;
	int _mediumWoodClipLength = 0;
	// Wire
	[SerializeField] AudioClip[] _wireClips;
	int _wireClipLength = 0;
	// MetalLock
	[SerializeField] AudioClip[] _metalLockClips;
	int _metalLockClipLength = 0;
	// chest
	[SerializeField] AudioClip[] _chestClips;
	int _chestClipLength = 0;
	// water tank
	[SerializeField] AudioClip[] _waterTankClips;
	int _waterTankClipLength = 0;
	// water tank top
	[SerializeField] AudioClip[] _waterTankTopClips;
	int _waterTankTopClipLength = 0;
	// heavy metal
	[SerializeField] AudioClip[] _heavyWoodClips;
	int _heavyWoodClipLength = 0;
	//floor Wood
	[SerializeField] AudioClip[] _floorWoodClips;
	int _floorWoodClipLength = 0;
	//message Board Metal Creak
	[SerializeField] AudioClip[] _metalCreakMessageBoardClips;
	int _metalCreakMessageBoardClipLength = 0;
	// gem Sparkle
	[SerializeField] AudioClip _gemSparkleClip;
	//heavy Metal
	[SerializeField] AudioClip _heavyMetalClip;
	// mediumMetal
	[SerializeField] AudioClip _mediumMetalClips;

	public bool _activateSounds = false;
	bool _needToResetPitch = false;

	public void PlayTapSound(TapSoundTags tapSoundTag){
		if (_activateSounds) {
			if (_needToResetPitch) {
				_audioSources [_audioSourceCnt].pitch = 1.0f;
				_needToResetPitch = false;
			}
			if (_audioSourceCnt >= 2) {
				_audioSourceCnt = 0;
			} else {
				_audioSourceCnt++;
			}

			switch (tapSoundTag) {
			case TapSoundTags.mediumWood:
				if (_mediumWoodClipLength == 0) {
					_mediumWoodClipLength = _mediumWoodClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _mediumWoodClips [ChooseRandomClip (_mediumWoodClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.wire:
				if (_wireClipLength == 0) {
					_wireClipLength = _wireClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _wireClips [ChooseRandomClip (_wireClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.metalLock:
				if (_metalLockClipLength == 0) {
					_metalLockClipLength = _metalLockClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _metalLockClips [ChooseRandomClip (_metalLockClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.chest:
				if (_chestClipLength == 0) {
					_chestClipLength = _chestClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _chestClips [ChooseRandomClip (_chestClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.waterTank:
				if (_waterTankClipLength == 0) {
					_waterTankClipLength = _waterTankClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _waterTankClips [ChooseRandomClip (_waterTankClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.waterTankTop:
				if (_waterTankTopClipLength == 0) {
					_waterTankTopClipLength = _waterTankTopClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _waterTankTopClips [ChooseRandomClip (_waterTankTopClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.heavyWood:
				if (_heavyWoodClipLength == 0) {
					_heavyWoodClipLength = _heavyWoodClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _heavyWoodClips [ChooseRandomClip (_heavyWoodClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.floorWood:
				if (_floorWoodClipLength == 0) {
					_floorWoodClipLength = _floorWoodClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _floorWoodClips [ChooseRandomClip (_floorWoodClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.messageBoardMetalCreak:
				if (_metalCreakMessageBoardClipLength == 0) {
					_metalCreakMessageBoardClipLength = _metalCreakMessageBoardClips.Length;
				}
				_audioSources [_audioSourceCnt].clip = _metalCreakMessageBoardClips [ChooseRandomClip (_metalCreakMessageBoardClipLength)];
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.sparkle:
				_audioSources [_audioSourceCnt].clip = _gemSparkleClip;
				_audioSources [_audioSourceCnt].pitch = RandomPitch (0.95f, 1.05f);
				_needToResetPitch = true;
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.heavyMetal:
				_audioSources [_audioSourceCnt].clip = _heavyMetalClip;
				_audioSources [_audioSourceCnt].pitch = RandomPitch (0.95f, 1.05f);
				_needToResetPitch = true;
				_audioSources [_audioSourceCnt].Play ();
				break;
			case TapSoundTags.mediumMetal:
				_audioSources [_audioSourceCnt].clip = _mediumMetalClips;
				_audioSources [_audioSourceCnt].pitch = RandomPitch (0.97f, 1.03f);
				_needToResetPitch = true;
				_audioSources [_audioSourceCnt].Play ();
				break;
			default:
				break;
			}
		}
	}

	float RandomPitch(float min, float max){
		return Random.Range (min, max);
	}

	int ChooseRandomClip(int clipLength){
		return Random.Range (0, clipLength - 1);
	}

}
	
