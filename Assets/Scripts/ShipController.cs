using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
	public float movementSpeed;
	public float rotationSpeed;

	[SerializeField]
	private Vector3 forceSourcePosition;

	[SerializeField]
	private Rigidbody rb;

	private void FixedUpdate()
	{
		var moveVer = Input.GetAxis("Vertical");
		var moveHor = Input.GetAxis("Horizontal");

		var movement = transform.TransformDirection(new Vector3(0, 0, -moveVer));
		var rotation = new Vector3(0, moveHor, 0);

		rb.AddForceAtPosition(
			movementSpeed * Time.fixedDeltaTime * movement,
			rb.transform.TransformPoint(forceSourcePosition));

		rb.AddTorque(rotationSpeed * Time.fixedDeltaTime * rotation);

		var sfx = GetComponent<SplashSFX>();
		if (sfx)
		{
			sfx.playEngineSound = movement.magnitude > 0;
		}
		else
		{
			Debug.LogWarning("No SFX component available.", this);
		}
	}

	private void OnDrawGizmos()
	{
		var size = 0.03f;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(rb.transform.TransformPoint(forceSourcePosition), size);
	}
}
