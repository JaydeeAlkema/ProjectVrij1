using UnityEngine;

public enum FadeType
{
	FadeIn,
	FadeOut
}

public class SceneFader : MonoBehaviour
{
	#region Variables
	[SerializeField] private FadeType fadeType = FadeType.FadeOut;  // Wheter to fadein or fadeout.
	[SerializeField] private Animator anim = default;               // Reference to the animator that will animate the fadein image.
	#endregion

	#region Properties
	public FadeType FadeType { get => fadeType; set => fadeType = value; }
	#endregion

	#region Monobehaviour callbacks
	private void Start()
	{
		anim.enabled = false;
	}
	#endregion

	#region Functions
	public void Fade()
	{
		anim.enabled = true;
		switch(FadeType)
		{
			case FadeType.FadeIn:
				anim.SetBool("FadeOut", false);
				anim.SetBool("FadeIn", true);
				break;

			case FadeType.FadeOut:
				anim.SetBool("FadeOut", true);
				anim.SetBool("FadeIn", false);
				break;

			default:
				break;
		}
	}
	#endregion
}
