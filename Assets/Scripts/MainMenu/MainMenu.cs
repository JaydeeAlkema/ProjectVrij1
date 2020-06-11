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
	[SerializeField] private CustomButton backToMainMenuButton = default;
	#endregion

	#region Monobehaviour
	private void Start()
	{
		startButton.buttonClickDelegate += StartButton_OnClick;
		howToPlayButton.buttonClickDelegate += HowToPlayButton_OnClick;
		quitButton.buttonClickDelegate += QuitButton_OnClick;
		backToMainMenuButton.buttonClickDelegate += BackToMainMenuButton_OnClick;
	}
	#endregion

	#region Functions
	public void StartButton_OnClick()
	{
		SceneManager.LoadScene(1);
	}

	public void HowToPlayButton_OnClick()
	{
		howToPlayObject.SetActive(true);
	}

	public void BackToMainMenuButton_OnClick()
	{
		howToPlayObject.SetActive(false);
	}

	public void QuitButton_OnClick()
	{
		Application.Quit();
	}
	#endregion
}
