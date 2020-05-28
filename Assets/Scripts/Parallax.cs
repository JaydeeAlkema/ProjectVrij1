using UnityEngine;

public class Parallax : MonoBehaviour
{
	#region Variables
	[SerializeField] private bool scrolling = true;                                         // Should the background be scrolling?
	[SerializeField] private bool parallaxing = true;                                       // Should the layers be affected by parallaxing?

	[SerializeField] private float backgroundSize = default;                                // Size of the background image.
	[SerializeField] private float parallaxSpeed = default;                                 // the speed at which the background paralaxes.

	[SerializeField] private Transform camTransform = default;                              // Reference to the camera transform.
	[SerializeField] private Transform[] layers = default;                                  // All background layers.
	[SerializeField] private float viewZone = 10;                                           // view zone of the camera.
	[SerializeField] private int leftIndex = default;                                       // Left most image index.
	[SerializeField] private int rightIndex = default;                                      // Right most image index.
	[SerializeField] private float lastCameraX = default;                                   // Last X pos of the camera.
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		camTransform = Camera.main.transform;
		lastCameraX = camTransform.position.x;
		layers = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; i++)
			layers[i] = transform.GetChild(i);

		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	private void Update()
	{
		if(!camTransform) camTransform = Camera.main.transform;

		if(parallaxing)
		{
			float deltaX = camTransform.position.x - lastCameraX;
			transform.position += Vector3.right * (deltaX * parallaxSpeed);
		}

		lastCameraX = camTransform.position.x;

		if(scrolling)
		{
			if(camTransform.position.x < (layers[leftIndex].transform.position.x + viewZone)) ScrollLeft();
			if(camTransform.position.x > (layers[rightIndex].transform.position.x - viewZone)) ScrollRight();
		}
	}
	#endregion

	#region Functions
	private void ScrollLeft()
	{
		int lastRight = rightIndex;
		layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		leftIndex = rightIndex;
		rightIndex--;
		if(rightIndex < 0) rightIndex = layers.Length - 1;
	}

	private void ScrollRight()
	{
		int lastLeft = leftIndex;
		layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
		rightIndex = leftIndex;
		leftIndex++;
		if(leftIndex == layers.Length) leftIndex = 0;
	}
	#endregion
}
