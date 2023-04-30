using Ships.Extensions;
using System.Collections;
using UnityEngine;

public class SplashSFX : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem foamParticle;

	[SerializeField]
	private AudioClip splashSound, splashEngineSound;

	public bool playEngineSound = false;

	[SerializeField]
	private AnimationCurve splashVolume;

	[SerializeField]
	private float splashVelocityMin;

	[SerializeField]
	private float splashVelocityMax;

	private float engineSoundInterval = 0.15f;

	private float engineSoundRandomInterval = 0.02f;

	private Rigidbody rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		StartCoroutine(PlayEngineSFX(engineSoundInterval, engineSoundRandomInterval));
		StartCoroutine(PlaySplashSFX());
	}

	private IEnumerator PlayEngineSFX(float minDelay, float randomDelay)
	{
		float delay;
		while (true)
		{
			if (playEngineSound)
			{
				AudioController.instance.PlaySound(splashEngineSound, 0.4f);
			}

			delay = minDelay + randomDelay * Random.value;
			yield return new WaitForSeconds(delay);
		}
	}

	private IEnumerator PlaySplashSFX()
	{
		Vector3 prevVelocity = rb.velocity;
		while (true)
		{
			// Max height reached
			if (prevVelocity.y > 0 && rb.velocity.y <= 0)
			{
				var volumeFactor = Mathf.InverseLerp(splashVelocityMin, splashVelocityMax, Mathf.Abs(rb.velocity.y));
				var volume = splashVolume.Evaluate(volumeFactor);
				AudioController.instance.PlaySound(splashSound, volume);
			}

			prevVelocity = rb.velocity;
			yield return null;
		}
	}
}
