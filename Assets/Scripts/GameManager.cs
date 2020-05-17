using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the main scene.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject cam = default;                  // Reference to the main camera in the scene. (I know Camera.main exists)
	[SerializeField] private GameObject playerPrefab = default;         // Reference to the Player Prefab.
	[SerializeField] LevelGenerator levelGenerator = default;           // Reference to the level generator in the scene.

	private GameObject playerInstance = null;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		StartCoroutine(BeginGame());
	}
	#endregion

	#region Functions
	/// <summary>
	/// Begins all the required code to start the game. This ensures the scene will always load in correctly.
	/// </summary>
	/// <returns></returns>
	private IEnumerator BeginGame()
	{
		cam.SetActive(false);
		yield return StartCoroutine(levelGenerator.Generate());

		playerInstance = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);
		playerInstance.transform.position = new Vector3(0, -5f, 0);

		cam.GetComponent<SmoothCam>().Target = playerInstance.transform;
		cam.SetActive(true);
		yield return new WaitForEndOfFrame();
	}
	#endregion
}
