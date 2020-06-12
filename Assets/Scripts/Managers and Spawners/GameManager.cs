using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Manages the main scene.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Variables
	private static GameManager instance = default;                      // Instance of this.

	[SerializeField] private GameObject cam = default;                  // Reference to the main camera in the scene. (I know Camera.main exists)
	[Space]
	[SerializeField] private SceneFader sceneFader = default;           // Reference to the scene fader.
	[SerializeField] private GameObject playerPrefab = default;         // Reference to the Player Prefab.
	[SerializeField] private Transform playerPrefabSpawnPos = default;  // Where the player will be spawned.
	[Space]
	[SerializeField] LevelGenerator levelGenerator = default;           // Reference to the level generator in the scene.
	[Space]
	[SerializeField] private Volume postProcessVolume = default;        // Reference to the post process volume.

	private GameObject playerInstance = null;
	#endregion

	#region Properties
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject PlayerInstance { get => playerInstance; set => playerInstance = value; }
	public Volume PostProcessVolume { get => postProcessVolume; set => postProcessVolume = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this) instance = this;
	}

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

		playerInstance = Instantiate(playerPrefab, playerPrefabSpawnPos.position, Quaternion.identity);

		cam.GetComponent<SmoothFollow>().Target = playerInstance.transform;
		cam.SetActive(true);

		AudioManager.Instance.FadeInBackgroundMusic();
		sceneFader.Fade();
		yield return new WaitForEndOfFrame();
	}

	public void ChangeVignetteIntensity(float amount)
	{
		postProcessVolume.profile.TryGet(out Vignette vignette);
		vignette.intensity.value += amount / 3f;

		// Clamp value to a certain point.
		if(vignette.intensity.value >= 0.3f) vignette.intensity.value = 0.3f;
	}
	#endregion
}
