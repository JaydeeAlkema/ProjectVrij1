using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	#region Variables
	[SerializeField] private ObstaclePrefab[] obstaclesToSpawn = default;           // Arrway with all the possible obstacle prefabs to spawn.
	[SerializeField] private Transform spawnOffset = default;                       // Spawn offset for the obstacles.
	[SerializeField] private float spawnInterval = 10f;                             // Time between spawns.
	[Space]
	[SerializeField] private List<GameObject> obstaclesInScene = new List<GameObject>();    // List with all the active obstacles in the scene.

	private Transform followTransform = default;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(SpawnObstacleAtInterval());
	}

	private void Update()
	{
		if(followTransform == null)
		{
			if(GameManager.Instance.PlayerInstance != null)
			{
				followTransform = GameManager.Instance.PlayerInstance.transform;
			}
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x = followTransform.position.x;
			transform.position = pos;
		}
	}
	#endregion

	#region Functions
	private IEnumerator SpawnObstacleAtInterval()
	{
		while(true)
		{
			yield return new WaitForSeconds(spawnInterval);

			int randIndex = Random.Range(0, obstaclesToSpawn.Length);

			GameObject newObstacle = Instantiate(
				obstaclesToSpawn[randIndex].Prefab,
				spawnOffset.position + obstaclesToSpawn[randIndex].Pos,
				Quaternion.Euler(obstaclesToSpawn[randIndex].Rot),
				obstaclesToSpawn[randIndex].Parent);

			obstaclesInScene.Add(newObstacle);
		}
	}
	#endregion
}

[System.Serializable]
public struct ObstaclePrefab
{
	[SerializeField] private string name;
	[SerializeField] private Transform parent;
	[SerializeField] private GameObject prefab;
	[SerializeField] private Vector3 pos;
	[SerializeField] private Vector3 rot;

	public Transform Parent { get => parent; set => parent = value; }
	public GameObject Prefab { get => prefab; set => prefab = value; }
	public Vector3 Pos { get => pos; set => pos = value; }
	public Vector3 Rot { get => rot; set => rot = value; }
}
