using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                  // Reference to the Rigidbody2D component.
	[Space]
	[SerializeField] private string horizontalMovementAxis = "Horizontal";  // The name of the Horizontal movement axis.
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;               // Which key to press to Jump.
	[SerializeField] private LayerMask groundMask = default;                // Ground layermask.
	[SerializeField] private Transform groundCheckPos = default;            // Ground Check Position.
	[SerializeField] private bool grounded = false;                         // True when on the ground.
	[Space]
	[SerializeField] private float moveSpeed = 5f;                      // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                     // How much force is applied to the Rigidobdy when jumping.
	#endregion

	#region Monobehaviour Callbacks
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
	}
	#endregion

	#region Functions
	private void CheckIfGrounded()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask) ? true : false;
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