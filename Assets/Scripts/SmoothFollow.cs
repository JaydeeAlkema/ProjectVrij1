using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform target = default;            // what to follow.
	[SerializeField] private float smoothing = default;             // How "Smooooth" the camera follows the target.
	[SerializeField] private Vector3 offset = default;              // Offset from the target transform.
	[SerializeField] private Vector2 minClamp = default;            // Minimum clamp of the Camera (How far left it can go)
	[SerializeField] private Vector2 maxClamp = default;            // Naximum clamp of the Camera (How far Right it can go)
	[SerializeField] private bool clamping = true;                  // is clamping enabled.

	private Vector3 desiredPos;
	private Vector3 smoothedPos;
	#endregion

	#region Properties
	public Transform Target { get => target; set => target = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void FixedUpdate()
	{
		desiredPos = new Vector3(offset.x + target.position.x, offset.y + target.position.y, offset.z);
		smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothing * Time.deltaTime);
		transform.position = smoothedPos;

		if(clamping)
		{
			Vector3 pos = transform.position;
			pos.x = Mathf.Clamp(pos.x, minClamp.x, maxClamp.x);
			pos.y = Mathf.Clamp(pos.y, minClamp.y, maxClamp.y);
			transform.position = pos;
		}
	}
	#endregion
}
