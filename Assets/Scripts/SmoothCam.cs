using UnityEngine;

public class SmoothCam : MonoBehaviour
{
	#region Variables
	[SerializeField] private Transform target = default;            // what to follow.
	[SerializeField] private float smoothing = default;             // How "Smooooth" the camera follows the target.
	[SerializeField] private Vector3 offset = default;              // Offset from the target transform.

	private Vector3 desiredPos;
	private Vector3 smoothedPos;
	#endregion

	#region Properties
	public Transform Target { get => target; set => target = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Update()
	{
	}

	private void FixedUpdate()
	{
		desiredPos = new Vector3(offset.x + target.position.x, offset.y, -10);
		smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothing * Time.deltaTime);
		transform.position = smoothedPos;
	}
	#endregion
}
