using UnityEngine;
using UnityEngine.UIElements;

public class Floater : MonoBehaviour
{
	private const float epsilon = 0.01f;
	private Rigidbody rb;
	private int floatersCount;

	[SerializeField]
	private float difference;

	[SerializeField]
	private bool bouyancyActive;

	[SerializeField]
	private float floatingPower;

	private void Start()
	{
		rb = GetComponentInParent<Rigidbody>();
		floatersCount = GetComponentsInParent<Floater>().Length;
	}

	private void FixedUpdate()
	{
		ApplyWaterBuoyancy();
	}

	private void ApplyWaterBuoyancy()
	{
		var waterHeight = WaveManager.instance.GetWaveHeight(transform.position.x, transform.position.z);
		difference = waterHeight - transform.position.y;

		var force = Vector3.up * floatingPower * difference;
		rb.AddForceAtPosition(force, transform.position, ForceMode.Acceleration);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position, 0.01f);
	}
}
