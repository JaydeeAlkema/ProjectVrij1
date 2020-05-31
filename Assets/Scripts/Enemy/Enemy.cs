using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum EnemyState
{
	Idle,
	Active,
	Dead
}

public enum EnemyType
{
	Blue,
	Red,
	Purple,
	Yellow
}

public class Enemy : MonoBehaviour, IDamageable
{
	#region Variables
	[SerializeField] private EnemyState state = EnemyState.Idle;            // The state of the enemy.
	[SerializeField] private EnemyType type = EnemyType.Blue;               // The type of enemy.
	[Space]
	[SerializeField] private int damageOnCollision = 50;                    // how much damage the enemy deals when it comes into contact with the target.
	[SerializeField] private Vector2 startingPos = default;                 // the starting position of the enemy.
	[Space]
	[SerializeField] private float health = 100;                            // How much health is left.
	[SerializeField] private Transform target = default;                    // Target of the enemy. (which will always be the player).
	[SerializeField] private SpriteRenderer bodySpriteRenderer = default;   // Reference to the SpriteRenderer component of the body.
	[SerializeField] private float lerpTime = default;                      // how long it takes in second the reach the target destination.
	[Space]
	[SerializeField] private Light2D[] lights = default;                        // Array with all the lights the enemy has to show where it is in the scene.
	[Space]
	[SerializeField] private AudioSource audioSource = default;             // Reference to the audiosource component.
	[SerializeField] private AudioClip[] onEnemyHitAudioSource = default;   // Array with all on hit sounds.

	private float timeStartedLerping;
	#endregion

	#region Properties
	public EnemyType Type { get => type; set => type = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		startingPos = transform.position;

		ChangeEyeColorLightByType();
	}

	private void Update()
	{
		if(health <= 0) state = EnemyState.Dead;

		if(state == EnemyState.Idle) CheckIfInViewOfCamera();
		else if(state == EnemyState.Active) MoveTowardsTarget();
		else if(state == EnemyState.Dead) Destroy(gameObject);
	}
	#endregion

	#region Functions
	/// <summary>
	/// checks if the body sprite renderer is in view of the camera, if so, the enemy will be set to active.
	/// </summary>
	private void CheckIfInViewOfCamera()
	{
		if(Camera.main != null)
			if(Vector3.Distance(transform.position, Camera.main.transform.position) < 25f)
			{
				target = GameManager.Instance.PlayerInstance.transform;
				timeStartedLerping = Time.time;
				state = EnemyState.Active;
			}
	}

	/// <summary>
	/// Move the enemy towards the target. Also turn the enemy to look towards the target.
	/// </summary>
	private void MoveTowardsTarget()
	{
		transform.position = CleanLerp(startingPos, target.position, timeStartedLerping, lerpTime);

		Vector3 diff = target.position - transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

	/// <summary>
	/// Cleanly lerps the enemy from the starting point to the end point.
	/// </summary>
	/// <param name="startPos"></param>
	/// <param name="endPos"></param>
	/// <param name="timeStartedLerping"></param>
	/// <param name="lerptime"></param>
	/// <returns></returns>
	private Vector3 CleanLerp(Vector3 startPos, Vector3 endPos, float timeStartedLerping, float lerptime = 1)
	{
		float timeSinceStarted = Time.time - timeStartedLerping;
		float percentageComplete = timeSinceStarted / lerptime;

		Vector3 result = Vector3.Lerp(startPos, endPos, percentageComplete);
		return result;
	}

	/// <summary>
	/// Deal damage to the player once it hits it.
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<IDamageable>() != null)
		{
			collision.GetComponent<IDamageable>().Damage(damageOnCollision);
			state = EnemyState.Dead;
		}
	}

	/// <summary>
	/// Implementation of the IDamageable interface.
	/// </summary>
	/// <param name="damageTaken"></param>
	public void Damage(int damageTaken)
	{
		health -= damageTaken;

		int index = Random.Range(0, onEnemyHitAudioSource.Length);
		AudioManager.Instance.PlaySoundEffect(onEnemyHitAudioSource[index], transform, 1f);
	}

	/// <summary>
	/// Returns and index relevant to the enemy type.
	/// </summary>
	/// <returns></returns>
	public int GetTypeIndex()
	{
		int index = 0;
		switch(type)
		{
			case EnemyType.Blue:
				index = 0;
				break;
			case EnemyType.Red:
				index = 1;
				break;
			case EnemyType.Purple:
				index = 2;
				break;
			case EnemyType.Yellow:
				index = 3;
				break;
			default:
				break;
		}
		return index;
	}

	/// <summary>
	/// </summary>
	private void ChangeEyeColorLightByType()
	{
		RandomizeEnemyType();

		for(int i = 0; i < lights.Length; i++)
		{
			switch(type)
			{
				case EnemyType.Blue:
					lights[i].color = Color.blue;
					break;
				case EnemyType.Red:
					lights[i].color = Color.red;
					break;
				case EnemyType.Purple:
					Color newCol;
					if(ColorUtility.TryParseHtmlString("FD00FF", out newCol))
					{
						lights[i].color = newCol;
					}
					break;
				case EnemyType.Yellow:
					lights[i].color = Color.yellow;
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Sets the enemy type to a (pseudo) random type
	/// </summary>
	private void RandomizeEnemyType()
	{
		int typeIndex = Random.Range(0, 4);

		switch(typeIndex)
		{
			case 0:
				type = EnemyType.Blue;
				break;
			case 1:
				type = EnemyType.Red;
				break;
			case 2:
				type = EnemyType.Purple
					;
				break;
			case 3:
				type = EnemyType.Yellow;
				break;
			default:
				break;
		}
	}
	#endregion
}
