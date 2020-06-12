using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightOrb : MonoBehaviour, ICollectable
{
	#region Variables
	[SerializeField] private SpriteRenderer spriteRenderer = default;           // Reference to the Sprite Renderer component.
	[SerializeField] private Light2D light2D = default;                         // Reference to the light 2d component.
	[SerializeField] private CircleCollider2D circleCollider = default;         // Reference to the CircleCollider2D component.
	[SerializeField] private GameObject onCollectParticlesPrefab = default;     // Particle system prefab that spawns when collected.
	#endregion

	#region ICollectable Methods
	public void Collect()
	{
		spriteRenderer.enabled = false;
		light2D.enabled = false;
		circleCollider.enabled = false;
		Instantiate(onCollectParticlesPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject, 2f);
	}
	#endregion
}
