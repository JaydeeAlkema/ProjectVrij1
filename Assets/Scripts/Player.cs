using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                          // Reference to the Rigidbody2D component.
	[Space]
	[Header("Player Movement")]
	[SerializeField] private int health = 100;                                  // How much health the player has before game over occurs.
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;                   // Which key to press to Jump.
	[SerializeField] private LayerMask groundMask = default;                    // Ground layermask.
	[SerializeField] private Transform groundCheckPos = default;                // Ground Check Position.
	[SerializeField] private float moveSpeed = 5f;                              // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                             // How much force is applied to the Rigidobdy when jumping.
	[SerializeField] private bool grounded = false;                             // True when on the ground.
	[Space]
	[Header("Lantern")]
	[SerializeField] private float lanternRotationSpeed = 0.25f;                // How long the rotation lerp takes.
	[SerializeField] private Light2D[] lanternLights = default;                 // Array with all the lantern lights.
	[SerializeField] private Transform lanternPivot = default;                  // Pivot of the Lantern.
	[SerializeField] private KeyCode lanternPointLeft = default;                // Which key to press to make lantern point to the Left.
	[SerializeField] private KeyCode lanternPointRight = default;               // Which key to press to make lantern point to the Right.
	[SerializeField] private Color lanternColorBlue = new Color();              // The BLUE color of the lantern.
	[SerializeField] private Color lanternColorRed = new Color();               // The RED color of the lantern.
	[SerializeField] private Color lanternColorPurple = new Color();            // The PURPLE color of the lantern.
	[SerializeField] private Color lanternColorYellow = new Color();            // The YELLOW color of the lantern.
	[SerializeField] private int lanternDir = 1;                                // Which direction the lantern points. (0 = left, 1 = right)
	#endregion

	#region Methods
	public void Damage(int damageTaken) => health -= damageTaken;
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		ChangeLanternLight(lanternColorYellow);
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
	private void CheckIfGrounded()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask) ? true : false;
	}

	private void RotateLanternOnInput()
	{
		if(Input.GetKeyDown(lanternPointLeft)) lanternDir = 0;
		else if(Input.GetKeyDown(lanternPointRight)) lanternDir = 1;
	}

	private void RotateLantern()
	{
		if(lanternDir == 0) lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 179)), lanternRotationSpeed * Time.deltaTime);
		else if(lanternDir == 1) lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 1)), lanternRotationSpeed * Time.deltaTime);
	}

	private void ChangeLanterncolorOnInput()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)) ChangeLanternLight(lanternColorBlue);
		if(Input.GetKeyDown(KeyCode.Alpha2)) ChangeLanternLight(lanternColorRed);
		if(Input.GetKeyDown(KeyCode.Alpha3)) ChangeLanternLight(lanternColorPurple);
		if(Input.GetKeyDown(KeyCode.Alpha4)) ChangeLanternLight(lanternColorYellow);
	}

	private void ChangeLanternLight(Color color)
	{
		for(int i = 0; i < lanternLights.Length; i++)
		{
			lanternLights[i].color = color;
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