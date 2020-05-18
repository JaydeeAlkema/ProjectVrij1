using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                          // Reference to the Rigidbody2D component.
	[Space]
	[Header("Player Movement")]
	[SerializeField] private string horizontalMovementAxis = "Horizontal";      // The name of the Horizontal movement axis.
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;                   // Which key to press to Jump.
	[SerializeField] private LayerMask groundMask = default;                    // Ground layermask.
	[SerializeField] private Transform groundCheckPos = default;                // Ground Check Position.
	[SerializeField] private float moveSpeed = 5f;                              // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                             // How much force is applied to the Rigidobdy when jumping.
	[SerializeField] private bool grounded = false;                             // True when on the ground.
	[Space]
	[Header("Lantern")]
	[SerializeField] private float lanternRotationSpeed = 0.25f;                // How long the rotation lerp takes.
	[SerializeField] private Light2D[] lanternLights = default;                   // Array with all the lantern lights.
	[SerializeField] private Transform lanternPivot = default;                  // Pivot of the Lantern.
	[SerializeField] private KeyCode lanternPointLeft = default;                // Which key to press to make lantern point to the Left.
	[SerializeField] private KeyCode lanternPointRight = default;               // Which key to press to make lantern point to the Right.
	[SerializeField] private Color lanternColorBlue = new Color();              // The BLUE color of the lantern.
	[SerializeField] private Color lanternColorRed = new Color();               // The RED color of the lantern.
	[SerializeField] private Color lanternColorPurple = new Color();            // The PURPLE color of the lantern.
	[SerializeField] private Color lanternColorYellow = new Color();            // The YELLOW color of the lantern.
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
			//if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			//{
			//rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
			//}
			//else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			//{
			//	rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
			//}
			//else
			//{
			//	rb.velocity = new Vector2(0, rb.velocity.y);
			//}

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
	}
	#endregion

	#region Functions
	private void CheckIfGrounded()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask) ? true : false;
	}

	private void RotateLanternOnInput()
	{
		if(Input.GetKeyDown(lanternPointLeft))
		{
			lanternPivot.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
		}
		else if(Input.GetKeyDown(lanternPointRight))
		{
			lanternPivot.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}
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