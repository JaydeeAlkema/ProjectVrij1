using System.Collections;
using UnityEngine;

public class LightOrbSpawner : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject lightOrbPrefab = default;           // Reference to the LightOrb Prefab.
	[SerializeField] private Transform lightOrbSpawnPos = default;          // Transform position of the spawn point for the light orbs.
	[SerializeField] private float lightOrbSpawnInterval = 30f;             // How many second inbetween spawns.

	private Transform followTransform = null;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(SpawnLightOrb());
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
	private IEnumerator SpawnLightOrb()
	{
		while(true)
		{
			yield return new WaitForSeconds(lightOrbSpawnInterval);
			Instantiate(lightOrbPrefab, lightOrbSpawnPos.position, Quaternion.identity);
		}
	}
	#endregion
}
