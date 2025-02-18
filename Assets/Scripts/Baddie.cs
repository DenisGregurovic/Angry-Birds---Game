using UnityEngine;

public class Baddie : MonoBehaviour
{
	[SerializeField] private float maxHealth = 3f;
	[SerializeField] private float damageThreashold = 0.2f;
	[SerializeField] private GameObject baddieDeathParticle;
	[SerializeField] private AudioClip deathClip;

	private float currentHealth;

	private void Awake()
	{
		currentHealth = maxHealth;
	}

	public void DamageBaddie(float damageAmount)
	{
		currentHealth-=damageAmount;

		if (currentHealth <= 0f)
		{
			Die();
		}
	}

	private void Die()
	{
		GameManager.instance.RemoveBaddie(this);

		Instantiate(baddieDeathParticle, transform.position, Quaternion.identity);
		AudioSource.PlayClipAtPoint(deathClip,transform.position);
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		float impactVelocity = collision.relativeVelocity.magnitude;
		if (impactVelocity > damageThreashold)
		{
			DamageBaddie(impactVelocity);
		}
	}
}
