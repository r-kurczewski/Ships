using UnityEngine;

public class FoamParticle : MonoBehaviour
{
	[SerializeField]
	private new ParticleSystem particleSystem;

	[SerializeField]
	private AudioClip splashSound;

	public AnimationCurve emitCurve;
	public float emitCurveMin, emitCurveMax;

	public float emitMultiplier;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		var velocity = rb.velocity;
		velocity.y = 0;

		var emitFactor = velocity.magnitude * Time.fixedDeltaTime;
		var normalizedEmitFactor = Mathf.InverseLerp(emitCurveMin, emitCurveMax, emitFactor);

		var emitCount = (int)(emitMultiplier * emitCurve.Evaluate(normalizedEmitFactor));

		if (emitCount > 0)
		{
			particleSystem.Emit(emitCount);
		}

		//Debug.Log($"{emitFactor} in range ({emitCurveMin}, {emitCurveMax} is {normalizedEmitFactor} - emiting {(int)emitCount} particles.");
	}
}
