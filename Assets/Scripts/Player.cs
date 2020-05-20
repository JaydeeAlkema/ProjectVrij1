using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                          // Reference to the Rigidbody2D component.
	[Space]
	[Header("Player Movement")]
	[SerializeField] private int health = 100;                                  // How much health the player has before game over occurs.
	[SerializeField] private int damageToDeal = 100;                            // How much damage to deal to the enemy.
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;                   // Which key to press to Jump.
	[SerializeField] private LayerMask groundMask = default;                    // Ground layermask.
	[SerializeField] private Transform groundCheckPos = default;                // Ground Check Position.
	[SerializeField] private float moveSpeed = 5f;                              // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                             // How much force is applied to the Rigidobdy when jumping.
	[SerializeField] private bool grounded = false;                             // True when on the ground.
	[Space]
	[Header("Lantern")]
	[SerializeField] private PlayerLightArea lightArea = default;               // Reference to the Player Light Area class.
	[SerializeField] private int lanternDir = 1;                                // Which direction the lantern points. (0 = left, 1 = right)
	[SerializeField] private float timeToDefeatEnemy = 2f;                      // How long it takes before the enemy in the light area get's defeated.
	[SerializeField] private float lanternRotationSpeed = 0.25f;                // How long the rotation lerp takes.
	[Space]
	[SerializeField] private Light2D[] lanternLights = default;                 // Array with all the lantern lights.
	[SerializeField] private Transform lanternPivot = default;                  // Pivot of the Lantern.
	[Space]

	[SerializeField] private KeyCode lanternPointLeft = default;                // Which key to press to make lantern point to the Left.
	[SerializeField] private KeyCode lanternPointRight = default;               // Which key to press to make lantern point to the Right.
	[Space]
	[SerializeField] private Color[] lanternLightColors = default;              // Array with all the colors the lanter can be.
	[SerializeField] private int lanterLightColorIndex = 0;
	#endregion

	#region Methods
	public void Damage(int damageTaken) => health -= damageTaken;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		ChangeLanternLight(lanternLightColors[lanterLightColorIndex]);
	}

	private void FixedUpdate()
	{
		if(grounded)
		{
			rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
			if(Input.GetKey(jumpKey))
			{
				rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			}
		}
	}

	private void Update()
	{
		CheckIfGrounded();
		RotateLanternOnInput();
		ChangeLanterncolorOnInput();
		RotateLantern();
	}
	#endregion

	#region Functions
	/// <summary>
	/// Checks if there is a ground underneath the player.
	/// </summary>
	private void CheckIfGrounded()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask) ? true : false;
	}

	/// <summary>
	/// Gets the input and changes the lanternDir acordingly.
	/// </summary>
	private void RotateLanternOnInput()
	{
		if(Input.GetKeyDown(lanternPointLeft)) lanternDir = 0;
		else if(Input.GetKeyDown(lanternPointRight)) lanternDir = 1;
	}

	/// <summary>
	/// Rotates the lantern pivot smoothly.
	/// </summary>
	private void RotateLantern()
	{
		if(lanternDir == 0) lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 179)), lanternRotationSpeed * Time.deltaTime);
		else if(lanternDir == 1) lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 1)), lanternRotationSpeed * Time.deltaTime);
	}

	/// <summary>
	/// Change lantern light color on input.
	/// </summary>
	private void ChangeLanterncolorOnInput()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)) lanterLightColorIndex = 0;
		if(Input.GetKeyDown(KeyCode.Alpha2)) lanterLightColorIndex = 1;
		if(Input.GetKeyDown(KeyCode.Alpha3)) lanterLightColorIndex = 2;
		if(Input.GetKeyDown(KeyCode.Alpha4)) lanterLightColorIndex = 3;

		ChangeLanternLight(lanternLightColors[lanterLightColorIndex]);
	}

	/// <summary>
	/// A simple function that receives a color and sets all the lights underneath the lanter transform.
	/// It also enables and disables the light area gameobject so the OnCollisionEnter2D can be triggered again.
	/// </summary>
	/// <param name="color"></param>
	private void ChangeLanternLight(Color color)
	{
		lightArea.gameObject.SetActive(false);
		for(int i = 0; i < lanternLights.Length; i++)
		{
			lanternLights[i].color = color;
		}
		lightArea.gameObject.SetActive(true);
	}

	/// <summary>
	/// Gets called when the OnCollisionEnter2D function gets triggered on the PlayerLightArea
	/// </summary>
	/// <param name="playerLightArea"></param>
	public void OnCollisionDetected(PlayerLightArea playerLightArea)
	{
		lightArea = playerLightArea;
		StartCoroutine(DealDamageToTargetInLightArea());
	}

	/// <summary>
	/// Deals damage to the target within the light area after a certain time.
	/// </summary>
	/// <returns></returns>
	public IEnumerator DealDamageToTargetInLightArea()
	{
		if(lanterLightColorIndex == lightArea.TargetInLightArea.GetComponent<Enemy>().GetTypeIndex())
		{
			if(lightArea.TargetInLightArea.GetComponent<IDamageable>() != null)
			{
				yield return new WaitForSeconds(timeToDefeatEnemy);
				lightArea.TargetInLightArea.GetComponent<IDamageable>().Damage(damageToDeal);
			}
		}
	}
	#endregion

	#region Debugging
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, groundCheckPos.position);
	}

	#endregion
}