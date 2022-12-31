using System;
using UnityEngine;

public class Floater : MonoBehaviour
{
	private const int buoyancyMultiplier = 40;
	private Rigidbody rb;

	[SerializeField]
	private float baseHeight;

	[SerializeField]
	private float displacement;

	[SerializeField]
	private float submergeFactor;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		//ApplyGravity();
		ApplyWaterBuoyancy();
	}

	private void ApplyWaterBuoyancy()
	{
		var waterHeight = WaveManager.instance.GetWaveHeight(transform.position.x, transform.position.z);
		displacement = waterHeight + baseHeight - transform.position.y;
		if (displacement > 0)
		{
			submergeFactor = Mathf.Sqrt(displacement * buoyancyMultiplier);
		}
		else submergeFactor = 0;

		rb.AddForceAtPosition(Vector3.up * submergeFactor, transform.position, ForceMode.Acceleration);
	}

	private void ApplyGravity()
	{
		rb.AddForce(Physics.gravity, ForceMode.Force);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position, 0.01f);
	}
}
