﻿using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable
{
	#region Variables
	[SerializeField] private Rigidbody2D rb = default;                          // Reference to the Rigidbody2D component.
	[SerializeField] private Animator anim = default;                           // Reference to the Animator Component.
	[SerializeField] private CapsuleCollider2D capsuleCollider = default;
	[Space]
	[Header("Player Movement")]
	[SerializeField] private int health = 100;                                  // How much health the player has before game over occurs.
	[SerializeField] private int damageToDeal = 100;                            // How much damage to deal to the enemy.
	[SerializeField] private KeyCode jumpKey = KeyCode.W;                       // Which key to press to Jump.
	[SerializeField] private KeyCode slideKey = KeyCode.S;                      // Which key to press to Jump.
	[SerializeField] private LayerMask groundMask = default;                    // Ground layermask.
	[SerializeField] private Transform groundCheckPos = default;                // Ground Check Position.
	[SerializeField] private float moveSpeed = 5f;                              // How fast the character moves at the max speed.
	[SerializeField] private float jumpForce = 10f;                             // How much force is applied to the Rigidobdy when jumping.
	[SerializeField] private bool grounded = false;                             // True when on the ground.
	[SerializeField] private bool canSlide = true;                              // True when sliding on the ground.
	[SerializeField] private bool canJump = true;                               // If the player can jump.
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
	[SerializeField] private int lanternLightColorIndex = 0;
	[Header("Audio Clips & Sources")]
	[SerializeField] private AudioClip onPlayerJumpAudioClip = default;         // Audio Clip to play when player jumps.
	[SerializeField] private AudioClip onPlayerJumpLandAudioClip = default;     // Audio Clip to play when player lands after jumping.
	[SerializeField] private AudioClip onPlayerSlideAudioClip = default;        // Audio Clip to play when player starts sliding.
	[SerializeField] private AudioClip onPlayerHitAudioClip = default;          // Audio clip to play when player gets hit.
	[SerializeField] private AudioClip onPlayerCollectAudioClip = default;      // Audio clip to play when player Collects an light orb.
	[SerializeField] private AudioClip onLanternColorChange = default;          // Audio Clip to play when lantern color changes.
	[SerializeField] private AudioClip onLanternDirChange = default;            // Audio Clip to play when lantern changes direction.
	[SerializeField] private AudioSource onLowHealthGhostlyWhispers = default;  // Audio Clip of GhostlyWhispers that gets louder and louder the lower the players health gets.
	[SerializeField] private AudioSource onLowHealthHearthBeat = default;       // Audio Clip of a HearthBeat that gets louder and louder the lower the players health gets.
	#endregion

	#region Properties
	public Rigidbody2D Rb { get => rb; set => rb = value; }
	#endregion

	#region Monobehaviour Callbacks
	private void Start()
	{
		ChangeLanternLight(lanternLightColors[0], false);
	}

	private void FixedUpdate()
	{
		if(GameManager.Instance.GameState == GameState.Active)
			rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
	}

	private void Update()
	{
		if(GameManager.Instance.GameState == GameState.Active)
		{
			if(health <= 0)
			{
				GameManager.Instance.GameOver();
				StopAllCoroutines();
			}

			CheckIfGrounded();
			RotateLanternOnInput();
			ChangeLanterncolorOnInput();
			RotateLantern();

			if(Input.GetKeyDown(jumpKey))
				JumpEvent();

			if(Input.GetKeyDown(slideKey))
				SlideEvent();

			anim.SetBool("Grounded", grounded);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Obstacle"))
		{
			Debug.Log("Hit by: " + collision.name);
			SetHealth(-20);

			AudioManager.Instance.PlaySoundEffect(onPlayerHitAudioClip, transform, 0.5f);
			GameManager.Instance.ChangeVignetteIntensity(0.20f);

			collision.GetComponent<CapsuleCollider2D>().enabled = false;
			SetVolumeOfLowHealthAudioClips();
		}
		else if(collision.GetComponent<ICollectable>() != null)
		{
			SetHealth(35);

			collision.GetComponent<ICollectable>().Collect();
			AudioManager.Instance.PlaySoundEffect(onPlayerCollectAudioClip, transform, 0.5f);
			GameManager.Instance.ChangeVignetteIntensity(-0.35f);

			SetVolumeOfLowHealthAudioClips();
			Destroy(collision.gameObject);
		}
	}
	#endregion

	#region Functions
	/// <summary>
	/// A simple Health setter.
	/// Clamps the health value to 100.
	/// </summary>
	/// <param name="value"></param>
	private void SetHealth(int value)
	{
		health += value;

		if(health >= 100) health = 100;
	}

	/// <summary>
	/// Changes the volume of the ghostly whispers and hearthbeat when health is getting low.
	/// </summary>
	private void SetVolumeOfLowHealthAudioClips()
	{
		float newVolume = health / 100f;
		newVolume -= 1f;
		newVolume = newVolume / 100f;
		newVolume = Mathf.Abs(newVolume);
		onLowHealthGhostlyWhispers.volume = newVolume;
		onLowHealthHearthBeat.volume = newVolume * 10f;
	}

	/// <summary>
	/// Checks if there is a ground underneath the player.
	/// </summary>
	private void CheckIfGrounded()
	{
		//grounded = Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask) ? true : false;

		if(Physics2D.Linecast(transform.position, groundCheckPos.position, groundMask))
		{
			if(grounded == false)
			{
				AudioManager.Instance.PlaySoundEffect(onPlayerJumpLandAudioClip, transform, 1f);
				grounded = true;
			}
		}
		else
		{
			grounded = false;
		}
	}

	/// <summary>
	/// Starts the Slide Event.
	/// </summary>
	private void SlideEvent()
	{
		if(grounded && canSlide)
		{
			canSlide = false;
			AudioManager.Instance.PlaySoundEffect(onPlayerSlideAudioClip, transform, 1f);
			AudioManager.Instance.PlaySoundEffect(onPlayerJumpAudioClip, transform, 1f);
			anim.SetBool("Sliding", true);
			capsuleCollider.size = new Vector2(capsuleCollider.size.x, 0.5f);
			capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, -1f);
			StartCoroutine(SlideCooldown());
		}
	}

	/// <summary>
	/// Starts the Slide Cooldown.
	/// </summary>
	/// <returns></returns>
	private IEnumerator SlideCooldown()
	{
		yield return new WaitForSeconds(1.15f);
		anim.SetBool("Sliding", false);
		capsuleCollider.size = new Vector2(capsuleCollider.size.x, 2.75f);
		capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, 0f);
		yield return new WaitForSeconds(1.25f);
		canSlide = true;
	}

	/// <summary>
	/// Starts the Jump Event.
	/// </summary>
	private void JumpEvent()
	{
		if(grounded && canJump)
		{
			canJump = false;
			AudioManager.Instance.PlaySoundEffect(onPlayerJumpAudioClip, transform, 1f);
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			StartCoroutine(JumpCooldown());
		}
	}

	/// <summary>
	/// Starts the Jump Cooldown.
	/// </summary>
	/// <returns></returns>
	private IEnumerator JumpCooldown()
	{
		yield return new WaitForSeconds(0.1f);
		canJump = true;
	}

	/// <summary>
	/// Gets the input and changes the lanternDir acordingly.
	/// </summary>
	private void RotateLanternOnInput()
	{
		if(Input.GetKeyDown(lanternPointLeft)) { lanternDir = 0; AudioManager.Instance.PlaySoundEffect(onLanternDirChange, transform, 0.6f); }
		else if(Input.GetKeyDown(lanternPointRight)) { lanternDir = 1; AudioManager.Instance.PlaySoundEffect(onLanternDirChange, transform, 0.6f); }
	}

	/// <summary>
	/// Rotates the lantern pivot smoothly.
	/// </summary>
	private void RotateLantern()
	{
		if(lanternDir == 0)
		{
			lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 179)), lanternRotationSpeed * Time.deltaTime);
			lanternPivot.GetComponentInChildren<SpriteRenderer>().flipY = true;
		}
		else if(lanternDir == 1)
		{
			lanternPivot.rotation = Quaternion.Lerp(lanternPivot.rotation, Quaternion.Euler(new Vector3(0, 0, 1)), lanternRotationSpeed * Time.deltaTime);
			lanternPivot.GetComponentInChildren<SpriteRenderer>().flipY = false;
		}
	}

	/// <summary>
	/// Change lantern light color on input.
	/// </summary>
	private void ChangeLanterncolorOnInput()
	{
		if(Input.GetKeyDown(KeyCode.Q)) { ChangeLanternLight(lanternLightColors[0]); lanternLightColorIndex = 0; }
		if(Input.GetKeyDown(KeyCode.W)) { ChangeLanternLight(lanternLightColors[1]); lanternLightColorIndex = 1; }
		if(Input.GetKeyDown(KeyCode.E)) { ChangeLanternLight(lanternLightColors[2]); lanternLightColorIndex = 2; }
		if(Input.GetKeyDown(KeyCode.R)) { ChangeLanternLight(lanternLightColors[3]); lanternLightColorIndex = 3; }
	}

	/// <summary>
	/// A simple function that receives a color and sets all the lights underneath the lanter transform.
	/// It also enables and disables the light area gameobject so the OnCollisionEnter2D can be triggered again.
	/// </summary>
	/// <param name="color"></param>
	private void ChangeLanternLight(Color color, bool playAudio = true)
	{
		lightArea.gameObject.SetActive(false);

		for(int i = 0; i < lanternLights.Length; i++)
			lanternLights[i].color = color;

		if(playAudio)
			AudioManager.Instance.PlaySoundEffect(onLanternColorChange, transform, 0.5f);

		lightArea.gameObject.SetActive(true);
	}

	/// <summary>
	/// Gets called when the OnCollisionEnter2D function gets triggered on the PlayerLightArea
	/// </summary>
	/// <param name="playerLightArea"></param>
	public void OnCollisionDetected(PlayerLightArea playerLightArea)
	{
		lightArea = playerLightArea;
		Enemy enemyInSpotlight = lightArea.TargetInLightArea.GetComponent<Enemy>();
		StartCoroutine(DealDamageToTargetInLightArea(enemyInSpotlight, enemyInSpotlight.GetTypeIndex()));
	}

	/// <summary>
	/// Deals damage to the target within the light area after a certain time.
	/// </summary>
	/// <returns></returns>
	public IEnumerator DealDamageToTargetInLightArea(Enemy enemy, int enemyTypeIndex)
	{
		yield return new WaitForSeconds(timeToDefeatEnemy);
		if(lightArea.TargetInLightArea != null)
		{
			if(lanternLightColorIndex == enemyTypeIndex)
			{
				enemy.GetComponent<IDamageable>()?.Damage(damageToDeal);
			}
		}
		yield return null;
	}

	/// <summary>
	/// Implementation of the IDamageable interface Damage Void.
	/// </summary>
	/// <param name="damageTaken"></param>
	/// <param name="audioSource"></param>
	/// <param name="audioClip"></param>
	public void Damage(int damageTaken)
	{
		health -= damageTaken;
		SetVolumeOfLowHealthAudioClips();
		AudioManager.Instance.PlaySoundEffect(onPlayerHitAudioClip, transform, 0.5f);
		GameManager.Instance.ChangeVignetteIntensity(damageTaken / 100f);
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