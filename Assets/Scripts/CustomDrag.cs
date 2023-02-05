using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomDrag : MonoBehaviour
{
	public Vector3 drag;

	private Rigidbody rb;
	

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		rb.AddForce(new Vector3(-rb.velocity.x * drag.x, -rb.velocity.y * drag.y, -rb.velocity.z * drag.z));
	}
}
