using UnityEngine;

public class AudioManager : MonoBehaviour
{
	#region Variables
	private static AudioManager instance = default;								// Private instance of the Audio Manager.

	[SerializeField] private AudioSource audioSource = default;                 // Reference to the audio source component.
	[SerializeField] private Animator anim = default;							// Reference to the animator component. This will handle fading in the music.

	public static AudioManager Instance { get => instance; set => instance = value; }
	#endregion

	private void Awake()
	{
		if(!instance || instance != this)
			instance = this;
	}

	public void FadeInBackgroundMusic()
	{
		audioSource.enabled = true;
		anim.SetBool("FadeIn", true);
	}

	public void PlaySoundEffect(AudioClip clip, Transform pos, float volume)
	{
		AudioSource.PlayClipAtPoint(clip, pos.position, volume);
	}
}
