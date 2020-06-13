using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum GameState
{
	Active,
	GameOver,
	Transitioning
}

/// <summary>
/// Manages the main scene.
/// </summary>
public class GameManager : MonoBehaviour
{
	#region Variables
	private static GameManager instance = default;                      // Instance of this.

	[SerializeField] private GameState gameState = default;
	[Space]
	[SerializeField] private GameObject cam = default;                  // Reference to the main camera in the scene. (I know Camera.main exists)
	[Space]
	[SerializeField] private SceneFader sceneFader = default;           // Reference to the scene fader.
	[SerializeField] private GameObject playerPrefab = default;         // Reference to the Player Prefab.
	[SerializeField] private Transform playerPrefabSpawnPos = default;  // Where the player will be spawned.
	[Space]
	[SerializeField] LevelGenerator levelGenerator = default;           // Reference to the level generator in the scene.
	[Space]
	[SerializeField] private Volume postProcessVolume = default;        // Reference to the post process volume.
	[Space]
	[SerializeField] private AudioSource[] audioSourcesInScene = default; // Array with all the active audio sources in the scene.
	[SerializeField] private GameObject treeLeavesParticleSystem = default; // Reference to the tree Leaves Particle System in the scene.

	private GameObject playerInstance = null;
	#endregion

	#region Properties
	public static GameManager Instance { get => instance; set => instance = value; }
	public GameObject PlayerInstance { get => playerInstance; set => playerInstance = value; }
	public Volume PostProcessVolume { get => postProcessVolume; set => postProcessVolume = value; }
	public GameState GameState { get => gameState; set => gameState = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Awake()
	{
		if(!instance || instance != this) instance = this;
	}

	private void Start()
	{
		StartCoroutine(BeginGame());
		treeLeavesParticleSystem.SetActive(false);
	}

	private void Update()
	{
		if(playerInstance != null)
		{
			if(playerInstance.transform.position.x >= 310 && gameState == GameState.Active)
				StartCoroutine(GoToCutscene());

			if(playerInstance.transform.position.x > 150)
				treeLeavesParticleSystem.SetActive(true);
		}
	}
	#endregion

	#region Functions
	/// <summary>
	/// Begins all the required code to start the game. This ensures the scene will always load in correctly.
	/// </summary>
	/// <returns></returns>
	private IEnumerator BeginGame()
	{
		gameState = GameState.Active;
		cam.SetActive(false);
		yield return StartCoroutine(levelGenerator.Generate());

		playerInstance = Instantiate(playerPrefab, playerPrefabSpawnPos.position, Quaternion.identity);

		cam.GetComponent<SmoothFollow>().Target = playerInstance.transform;
		cam.SetActive(true);

		AudioManager.Instance.FadeInBackgroundMusic();
		sceneFader.FadeType = FadeType.FadeOut;
		sceneFader.Fade();

		audioSourcesInScene = FindObjectsOfType<AudioSource>();

		yield return new WaitForEndOfFrame();
	}

	public void ChangeVignetteIntensity(float amount)
	{
		postProcessVolume.profile.TryGet(out Vignette vignette);
		vignette.intensity.value += amount / 3f;

		// Clamp value to a certain point.
		if(vignette.intensity.value >= 0.3f) vignette.intensity.value = 0.3f;
	}

	public IEnumerator GoToCutscene()
	{
		gameState = GameState.Transitioning;

		foreach(AudioSource audioSource in audioSourcesInScene)
		{
			audioSource.enabled = false;
			audioSource.volume = 0;
		}

		sceneFader.FadeType = FadeType.FadeIn;
		sceneFader.Fade();
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(2);

	}

	public void GameOver()
	{
		StartCoroutine(GameOverEvent());
	}

	private IEnumerator GameOverEvent()
	{
		gameState = GameState.GameOver;

		foreach(AudioSource audioSource in audioSourcesInScene)
		{
			audioSource.enabled = false;
			audioSource.volume = 0;
		}

		sceneFader.FadeType = FadeType.FadeIn;
		sceneFader.Fade();

		yield return new WaitForSeconds(3f);

		SceneManager.LoadScene(0);
	}
	#endregion
}
