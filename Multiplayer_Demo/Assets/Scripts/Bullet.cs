using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//[HideInInspactor]
	public GameObject playerFrom;

	void OnCollisionEnter(Collision collision)
	{
		var hit = collision.gameObject;
		var health = hit.GetComponent<Health>();
		if (health != null)
		{
			health.TakeDamage(playerFrom, 10);
		}

		Destroy(gameObject);
	}
}
