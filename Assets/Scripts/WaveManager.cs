using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public Material waterMaterial;
	public bool showGizmos;

	[SerializeField]
	private float waveSpeed, waveHeight, waviness;
	[SerializeField]
	private Vector2 waveDirection;

	void Start()
	{
		waveSpeed = waterMaterial.GetFloat("_Speed");
		waveHeight = waterMaterial.GetFloat("_Height");
		waviness = waterMaterial.GetFloat("_Waviness");

		var directionVector = waterMaterial.GetVector("_Direction");
		waveDirection = (Vector2)directionVector;
	}

	private void Update()
	{
		transform.position = WavePos(transform.position.x, transform.position.z);
	}

	private float GetWaveHeight(float x, float z)
	{
		float y = 0;
		var time = Time.time;
		var direction = waveDirection.normalized * waviness;

		y += waveHeight * Mathf.Sin(direction.x * x + waveSpeed * time);
		y += waveHeight * Mathf.Sin(direction.y * z + waveSpeed * time);
		return y;
	}

	private Vector3 WavePos(float x, float z)
	{
		return new Vector3(x, GetWaveHeight(x, z), z);
	}

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Application.isPlaying && showGizmos)
		{
			int count = 10;
			for (int x = -count; x <= count; x++)
			{
				for (int z = -count; z <= count; z++)
				{
					Gizmos.DrawSphere(WavePos((float)x / count, (float)z / count), 0.01f);
				}
			}
		}
#endif
	}
}
