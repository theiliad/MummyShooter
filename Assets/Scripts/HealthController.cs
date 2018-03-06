using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
	private Animator anim;
	[SerializeField] private float health = 100f;
	public Transform target;

	void Start() {
		anim = GetComponentInParent<Animator>();
    }

	void Update(){
		if (health > 0 ) GetComponentInParent<UnityEngine.AI.NavMeshAgent>().destination = target.position;
		else GetComponentInParent<UnityEngine.AI.NavMeshAgent>().destination = GetComponentInParent<UnityEngine.AI.NavMeshAgent>().transform.position;
	}

	public void ApplyDamage(float damage) {
		Debug.Log("GOT HIT: " + damage);

		if (health > 0) {
			anim.CrossFadeInFixedTime("takeDamage", 0.1f);
			health -= damage;

			if (health <= 0) {
				anim.CrossFadeInFixedTime("die02", 0.1f);
				Destroy(gameObject, 5);
			}
		}
	}
}
