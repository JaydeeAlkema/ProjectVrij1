using UnityEngine;

public enum EnemyState
{
	Idle,
	Active,
	Dead
}

public class Enemy : MonoBehaviour
{
	#region Variables
	[SerializeField] private EnemyState state = EnemyState.Idle;            // The state of the enemy.
	[Space]
	[SerializeField] private Transform target = default;                    // Target of the enemy. (which will always be the player).
	[SerializeField] private SpriteRenderer bodySpriteRenderer = default;   // Reference to the SpriteRenderer component of the body.
	[SerializeField] private float movementSpeed = default;                 // How fast the enemy moves towards the target.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
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
		transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

		Vector3 diff = target.position - transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

	/// <summary>
	/// Deal damage to the player once it hits it.
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<IDamagable>() != null)
		{
			collision.GetComponent<IDamagable>().Damage(1);
			state = EnemyState.Dead;
		}
	}
	#endregion
}
