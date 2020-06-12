using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
	public void LoadScene(int buildIndex)
	{
		SceneManager.LoadScene(buildIndex);
	}
}
