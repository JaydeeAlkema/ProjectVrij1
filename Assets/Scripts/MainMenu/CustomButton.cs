using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	#region Variables
	[SerializeField] private Image mainImage = default;
	[Space]
	[SerializeField] private Color normalColor = new Color(255f, 255f, 255f, 255f);
	[SerializeField] private Color mouseHoverColor = new Color(180f, 180f, 180f, 255f);
	[SerializeField] private Color mouseOnClickColor = new Color(140f, 140f, 140f, 255f);
	[Space]
	[SerializeField] private AudioSource audioSource = default;
	[SerializeField] private AudioClip onClickAudioClip = default;
	#endregion

	#region Functions
	public void OnPointerEnter(PointerEventData eventData)
	{
		mainImage.color = mouseHoverColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mainImage.color = normalColor;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		mainImage.color = mouseOnClickColor;
		PlayAudio(onClickAudioClip);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		mainImage.color = mouseHoverColor;
	}

	/// <summary>
	/// Plays a audio clip from the audio source.
	/// </summary>
	/// <param name="audioClip"></param>
	private void PlayAudio(AudioClip audioClip)
	{
		audioSource.PlayOneShot(audioClip);
	}

	#endregion
}
