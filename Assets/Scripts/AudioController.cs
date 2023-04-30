using UnityEngine;

public class AudioController : SingletonBase<AudioController>
{
	[SerializeField]
	private AudioSource sfxAudioSource;

	[SerializeField]
	private AudioSource musicAudioSource;

	[SerializeField]
	private AudioSource ambientAudioSource;

	[SerializeField]
	private AudioClip backgroundMusic;

	[SerializeField]
	private AudioClip ambient;

	private void Start()
	{
		musicAudioSource.Play();
		ambientAudioSource.Play();
	}

	public void PlaySound(AudioClip clip, float volume = 1)
	{
		sfxAudioSource.PlayOneShot(clip, volume);
	}
}