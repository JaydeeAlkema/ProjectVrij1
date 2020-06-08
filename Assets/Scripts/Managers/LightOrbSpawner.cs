using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrbSpawner : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform highSpawnTransform = default;
	[SerializeField] private Transform lowSpawnTransform = default;
	[Space]
	[SerializeField] private GameObject lightOrbPrefab = default;
	[SerializeField] private List<GameObject> lightOrbsInScene = new List<GameObject>();
	#endregion

	#region Functions

	#endregion
}
