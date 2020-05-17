using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                  // Reference to the Rigidbody2D component.
	[Space]
	[SerializeField] private string horizontalMovementAxis = "Horizontal";  // The name of the Horizontal movement axis.
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;               // Which key to press to Jump.
	[Space]
	[SerializeField] private float moveSpeed = 5f;                      // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                     // How much force is applied to the Rigidobdy when jumping.
	[Space]
	[SerializeField] private bool jumping = false;                      // Is true when jumping.
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
		GetMovementInput();
	}

	private void FixedUpdate()
	{
		rb.AddForce(transform.right * moveSpeed * Time.deltaTime);

	}
	#endregion

	#region Functions
	private void GetMovementInput()
	{
		// Regular movement
		moveSpeed *= Input.GetAxis(horizontalMovementAxis);

		// Jumping
		jumping = Input.GetKeyDown(jumpKey) ? true : false;
		if(jumping)
		{
			rb.AddForce(transform.up * jumpForce * Time.deltaTime);
		}
	}
	#endregion
}