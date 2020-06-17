using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	#region Variables
	[Header("Custom Buttons")]
	[SerializeField] private CustomButton startButton = default;
	[SerializeField] private CustomButton howToPlayButton = default;
	[SerializeField] private CustomButton quitButton = default;

	[Header("HowToPlay Screen")]
	[SerializeField] private GameObject howToPlayObject = default;
	[SerializeField] private GameObject[] howToPlayScreens = default;
	[SerializeField] private CustomButton backToMainMenuButton = default;
	[SerializeField] private CustomButton nextScreenButton = default;
	[SerializeField] private CustomButton previousScreenButton = default;

	int howToPlayScreenIndex = 0;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		// Main Menu
		startButton.buttonClickDelegate += StartButton_OnClick;
		howToPlayButton.buttonClickDelegate += HowToPlayButton_OnClick;
		quitButton.buttonClickDelegate += QuitButton_OnClick;

		// How to Play menu
		backToMainMenuButton.buttonClickDelegate += BackToMainMenuButton_OnClick;
		nextScreenButton.buttonClickDelegate += HowToPlayNextButton_OnClick;
		previousScreenButton.buttonClickDelegate += HowToPlayPreviousButton_OnClick;
	}
	#endregion

	#region Functions
	// Main Menu functions
	public void StartButton_OnClick()
	{
		SceneManager.LoadScene(1);
	}

	public void BackToMainMenuButton_OnClick()
	{
		howToPlayObject.SetActive(false);
	}

	public void QuitButton_OnClick()
	{
		Application.Quit();
	}

	// How to Play functions
	public void HowToPlayButton_OnClick()
	{
		howToPlayObject.SetActive(true);
	}

	public void HowToPlayNextButton_OnClick()
	{
		if(howToPlayScreenIndex == 0) howToPlayScreenIndex = 1;
		SetHowToPlayScreen();
	}

	public void HowToPlayPreviousButton_OnClick()
	{
		if(howToPlayScreenIndex == 1) howToPlayScreenIndex = 0;
		SetHowToPlayScreen();
	}

	private void SetHowToPlayScreen()
	{
		for(int i = 0; i < howToPlayScreens.Length; i++) howToPlayScreens[i].SetActive(false);
		howToPlayScreens[howToPlayScreenIndex].SetActive(true);
	}
	#endregion
}
