using UnityEngine.Audio;
using UnityEngine;

public class AudioManager: MonoBehaviour
{
	public AudioSource[] AudioSources;
	
	
	//public Sound[] sounds;
	public void Play(string audioName)
	{
		for (int i = 0; i < AudioSources.Length; i++) 
		{
			if (AudioSources[i].clip.name == audioName) 
			{
				AudioSources[i].Play();
			}
		}
	}
}
