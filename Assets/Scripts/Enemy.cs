using UnityEngine;

public enum EnemyState
{
	Idle,
	Active,
	Dead
}

public class Enemy : MonoBehaviour, IDamageable
{
	#region Variables
	[SerializeField] private EnemyState state = EnemyState.Idle;            // The state of the enemy.
	[SerializeField] private int damageOnCollision = 50;                    // how much damage the enemy deals when it comes into contact with the target.
	[SerializeField] private Vector2 startingPos = default;                 // the starting position of the enemy.
	[Space]
	[SerializeField] private float health = 100;                            // How much health is left.
	[SerializeField] private Transform target = default;                    // Target of the enemy. (which will always be the player).
	[SerializeField] private SpriteRenderer bodySpriteRenderer = default;   // Reference to the SpriteRenderer component of the body.
	[SerializeField] private float lerpTime = default;                      // how long it takes in second the reach the target destination.

	private float timeStartedLerping;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		startingPos = transform.position;
		timeStartedLerping = Time.time;
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
		if(bodySpriteRenderer.isVisible)
		{
			state = EnemyState.Active;
			target = GameManager.Instance.PlayerInstance.transform;
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
	public void Damage(int damageTaken) => health -= damageTaken;
	#endregion
}
