using UnityEngine;

public enum QuickTimeEventState
{
	InActive,
	Active,
	Completed
}
public class QuickTimeEvent : MonoBehaviour
{
	#region Variables
	[SerializeField] private QuickTimeEventState state = QuickTimeEventState.InActive;  // In which state is the Quick Time Event.
	[SerializeField] private SpriteRenderer spriteRenderer = default;                   // Reference to the Sprite Renderer component.
	[SerializeField] private QuickTimeEventKey[] quickTimeEventKeys = default;          // Which keys to press for this Quick Time Event.
	[SerializeField] private int keyToPressIndex = 0;                                   // Which key has to be pressed. So if the index is 1, then we need to press the key at index one in the keysToPress Array.
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = quickTimeEventKeys[keyToPressIndex].KeySprite;
	}

	private void Update()
	{
		if(state == QuickTimeEventState.InActive) CheckIfInViewOfCamera();
		if(state == QuickTimeEventState.Active) GetUserInput();
		if(state == QuickTimeEventState.Completed) Destroy(gameObject, 5f);
		#endregion
	}

	#region Functions
	/// <summary>
	/// Checks for any key that is pressed. And compares it to the key that has to be pressed, if correct the player may continue, else it wont do anything.
	/// </summary>
	private void GetUserInput()
	{
		for(int i = 0; i < quickTimeEventKeys.Length; i++)
		{
			if(Input.anyKeyDown && keyToPressIndex < quickTimeEventKeys.Length)
			{
				if(Input.GetKeyDown(quickTimeEventKeys[keyToPressIndex].KeyToPress))
				{
					Debug.Log("Pressed the CORRECT key!");

					keyToPressIndex++;
					if(keyToPressIndex == quickTimeEventKeys.Length)
					{
						state = QuickTimeEventState.Completed;
						return;
					}
					else
						spriteRenderer.sprite = quickTimeEventKeys[keyToPressIndex].KeySprite;
				}
				else
				{
					Debug.Log("Pressed the WRONG key!");
				}
			}
		}
	}

	/// <summary>
	/// checks if this QTE is in view of the camera, if so the QTE will be set to active.
	/// </summary>
	private void CheckIfInViewOfCamera()
	{

	}
	#endregion
}

[System.Serializable]
public struct QuickTimeEventKey
{
	#region Variables
	[SerializeField] private KeyCode keyToPress;            // Which key to press for this QTE.
	[SerializeField] private Sprite keySprite;              // Which sprite to display for this QTE.
	#endregion

	#region Properties
	public KeyCode KeyToPress { get => keyToPress; set => keyToPress = value; }
	public Sprite KeySprite { get => keySprite; set => keySprite = value; }
	#endregion
}
