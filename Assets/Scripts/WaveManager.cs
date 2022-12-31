using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public static WaveManager instance;

	public Material waterMaterial;
	public bool showGizmos;

	[SerializeField]
	private float waveSpeed, waveHeight, waviness;
	[SerializeField]
	private Vector2 waveDirection;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning($"{GetType().Name} duplicate");
			Destroy(instance.gameObject);
		}
		instance = this;
	}

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

	public float GetWaveHeight(float x, float z)
	{
		float y = 0;
		var time = Time.time;
		var direction = waveDirection.normalized * waviness;

		y += waveHeight * Mathf.Sin(direction.x * x + waveSpeed * time);
		y += waveHeight * Mathf.Sin(direction.y * z + waveSpeed * time);
		return y;
	}

	public Vector3 WavePos(float x, float z)
	{
		return new Vector3(x, GetWaveHeight(x, z), z);
	}

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Application.isPlaying && showGizmos)
		{
			const int count = 10;
			const float size = 0.01f;
			for (int x = -count; x <= count; x++)
			{
				for (int z = -count; z <= count; z++)
				{
					if (x == 0 && z == 0) continue;
					var pointX = transform.position.x + (float)x / count;
					var pointZ = transform.position.z + (float)z / count;
					var pointPos = WavePos(pointX, pointZ);
					Gizmos.DrawSphere(pointPos, size);
				}
			}
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 2 * size);
		}
#endif
	}
}
