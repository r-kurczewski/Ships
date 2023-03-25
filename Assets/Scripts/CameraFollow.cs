using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target;
	public Vector3 targetOffset;

	public float backDistance;
	public float sideOffset;
	public float fixedCameraHeight;
	public float smoothSpeed = 0.125f;

	void FixedUpdate()
	{
		var targetForward = -target.forward;

		var desiredCameraPosition = target.transform.position + targetOffset;
		desiredCameraPosition += targetForward * backDistance;
		desiredCameraPosition.y = fixedCameraHeight;
		desiredCameraPosition += target.right * sideOffset;

		var cameraPosition = Vector3.Lerp(transform.position, desiredCameraPosition, smoothSpeed);

		transform.position = cameraPosition;
		transform.LookAt(target.position);
	}

}