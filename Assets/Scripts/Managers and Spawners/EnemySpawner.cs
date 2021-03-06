﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemySpawner : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform[] spawnPositions = default;  // spawn Pos for the enemies that spawn to the right of the screen. 0 = left, 1 = right.
	[SerializeField] private GameObject[] enemiesToSpawn = default; // Array with the enemies that can spawn.. 0 = left, 1 = right.
	[Space]
	[SerializeField] private float spawnInterval = 10f; // Time between enemy spawns.
	[SerializeField] private List<GameObject> enemiesInScene = new List<GameObject>();  // List with all the enemies in the scene.
	[SerializeField] private Transform enemySpawnTransformParent = default; // Reference to the parent of the newly spawned enemies.

	private int enemyIndex = 0;
	private Transform followTransform = default;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(SpawnEnemiesAtInterval());
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
		else transform.position = followTransform.position;
	}
	#endregion

	#region Functions
	private IEnumerator SpawnEnemiesAtInterval()
	{
		while(true)
		{
			yield return new WaitForSeconds(spawnInterval);
			int randInt = Random.Range(0, 2);

			GameObject enemyGO = Instantiate(enemiesToSpawn[randInt], spawnPositions[randInt].position, Quaternion.identity, enemySpawnTransformParent);
			enemyGO.name += " [" + enemyIndex + "]";
			enemyGO.transform.position = new Vector3(enemyGO.transform.position.x, -3f, 0f);
			if(randInt == 0)
			{
				enemyGO.GetComponent<Enemy>().MovementMethod = MovementMethod.MoveTowards;
				enemyGO.GetComponent<Enemy>().MoveTime = 4f;

				SpriteRenderer[] spriteRenderers = enemyGO.GetComponentsInChildren<SpriteRenderer>();
				Light2D[] lights = enemyGO.GetComponentsInChildren<Light2D>();
				foreach(SpriteRenderer sprite in spriteRenderers) sprite.flipX = true;
				foreach(Light2D light in lights)
				{
					Vector3 scale = light.transform.localScale;
					scale.x *= -1;
					light.transform.localScale = scale;
				}
			}
			else if(randInt == 1)
			{
				enemyGO.GetComponent<Enemy>().MovementMethod = MovementMethod.CleanLerp;
				enemyGO.GetComponent<Enemy>().MoveTime = 10f;
			}
			enemyIndex++;
			enemiesInScene.Add(enemyGO);
		}
	}
	#endregion
}
