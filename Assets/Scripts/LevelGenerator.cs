using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The level generator script is responsibly for spawning all the level platforms allong with the Quick Time Event objects
/// This class keeps spawning platforms that seamlesly connect with eachother until the player reaches a certain point.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject[] platformPrefabs = default;            // Array with all the Platform prefabs.
	[SerializeField] private int platformsToSpawn = 10;                         // How many platforms will be spawned until the end platform is spawned.
	[SerializeField] private Vector2 platformOffset = default;                  // How much offset there is between platform spawns. (platform width / 2)
	[SerializeField] private Transform platformSpawnPoint = default;            // Where to spawn the (next) platform.
	[SerializeField] private Transform platformParent = default;                // Reference to the platforms parent.
	[SerializeField] private float distanceToDespawnPlatform = 30f;             // Distance between the Main Camera and the platforms to despawn.
	[Space]
	[SerializeField] private List<GameObject> platformsInScene = new List<GameObject>();    // List with all the Platforms that are in the scene.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		for(int i = 0; i < platformsInScene.Count; i++)
		{
			if(Vector3.Distance(platformsInScene[i].transform.position, Camera.main.transform.position) < distanceToDespawnPlatform)
				Show(platformsInScene[i]);
			else
				Hide(platformsInScene[i]);
		}
	}
	#endregion

	#region Functions
	/// <summary>
	/// Generates all the platforms needed. This is done via an enumerator because the Game Manager will have to sequence which tasks will be performed.
	/// </summary>
	/// <returns></returns>
	public IEnumerator Generate()
	{
		for(int i = 0; i < platformsToSpawn; i++)
		{
			Vector2 platformSpawnPos = new Vector2(platformSpawnPoint.position.x + platformOffset.x * i, platformOffset.y);
			GameObject newPlatformGO = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)], platformSpawnPos, Quaternion.identity, platformParent);
			platformsInScene.Add(newPlatformGO);
		}
		yield return new WaitForEndOfFrame();
	}

	/// <summary>
	/// Hides all the platforms out of view.
	/// </summary>
	private void Hide(GameObject gameObject)
	{
		gameObject.SetActive(false);

		for(int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Shows all the platforms inside the view range.
	/// </summary>
	private void Show(GameObject gameObject)
	{
		gameObject.SetActive(true);

		for(int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}
	}
	#endregion
}
