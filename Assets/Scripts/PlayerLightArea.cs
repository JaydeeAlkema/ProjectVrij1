using UnityEngine;

public class PlayerLightArea : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject targetInLightArea = default;        // GameObject in the light area
	#endregion

	#region Properties
	public GameObject TargetInLightArea { get => targetInLightArea; set => targetInLightArea = value; }
	#endregion

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<IDamageable>() != null)
		{
			targetInLightArea = collision.gameObject;
			transform.GetComponentInParent<Player>().OnCollisionDetected(this);
			Debug.Log("Target in Light Area!");
		}
	}
}
