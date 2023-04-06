using System;
using Unity.Mathematics;
using UnityEngine;

public class WaterFoam : MonoBehaviour
{
	[SerializeField]
	private new ParticleSystem particleSystem;

	[SerializeField]
	private Rigidbody rb;

	public AnimationCurve emitCurve;
	public float emitCurveMin, emitCurveMax;

	public float emitMultiplier;


	private void FixedUpdate()
	{
		var velocity = rb.velocity;
		velocity.y = 0;

		var emitFactor = velocity.magnitude * Time.fixedDeltaTime;
		var normalizedEmitFactor = (emitFactor - emitCurveMin) / (emitCurveMax - emitCurveMin);
		var emitCount = emitMultiplier * emitCurve.Evaluate(normalizedEmitFactor);
		particleSystem.Emit((int)emitCount);
		//Debug.Log($"{emitFactor} in range ({emitCurveMin}, {emitCurveMax} is {normalizedEmitFactor} - emiting {(int)emitCount} particles.");
	}
}
