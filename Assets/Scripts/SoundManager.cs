using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
 
	public static SoundManager instance;

	private void Awake()
	{
		if (instance == null)
		{ 
			instance = this;
		}
		else
		{ }
	}

	public void PlayClip(AudioClip clip, AudioSource source)
	{
		source.clip = clip;
		source.Play();
	}

}
