using UnityEngine;

public class WaveManager : MonoBehaviour
{
	public static WaveManager instance;

	public Material waterMaterial;
	public Transform waterMesh;
	public bool showGizmos;
	public float gizmosSize;

	[SerializeField]
	private float waveSpeed, waveHeight, waviness;
	[SerializeField]
	private Vector2 waveDirection;

	public float WaterBaseHeight => waterMesh.position.y;

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
		float y = WaterBaseHeight;
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
			for (int x = -count; x <= count; x++)
			{
				for (int z = -count; z <= count; z++)
				{
					if (x == 0 && z == 0) continue;
					var pointX = transform.position.x + x * gizmosSize * 100 / count;
					var pointZ = transform.position.z + z * gizmosSize * 100 / count;
					var pointPos = WavePos(pointX, pointZ);
					Gizmos.DrawSphere(pointPos, gizmosSize);
				}
			}
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 2 * gizmosSize);
		}
#endif
	}
}
