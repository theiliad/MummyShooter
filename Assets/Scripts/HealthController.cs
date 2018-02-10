using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
	private Animator anim;
	[SerializeField] private float health = 100f;

	void Start() {
		anim = GetComponent<Animator>();
    }

	public void ApplyDamage(float damage) {
		Debug.Log("GOT HIT: " + damage);

		anim.CrossFadeInFixedTime("takeDamage", 0.1f);

		health -= damage;
	}
}
