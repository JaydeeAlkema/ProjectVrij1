using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the main scene.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Variables
	[SerializeField] LevelGenerator levelGenerator = default;           // Reference to the level generator in the scene.
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(BeginGame());
	}
	#endregion

	#region Functions
	private IEnumerator BeginGame()
	{
		yield return StartCoroutine(levelGenerator.Generate());
	}
	#endregion
}
