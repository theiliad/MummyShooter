using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {
	private Animator anim;
	[SerializeField] private float health = 100f;
	public Transform target;

	private Text mummiesText;

	void Start() {
		anim = GetComponentInParent<Animator>();
		mummiesText = GameObject.Find("Mummies").GetComponent<Text>();
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
				anim.SetBool("HitPlayer", false);
				anim.CrossFadeInFixedTime("die02", 0.1f);

				Weapon weaponController = GameObject.Find("FPS-AK47").GetComponent<Weapon>();
				weaponController.numOfMummies -= 1;
        		mummiesText.text = "Mummmies: " + weaponController.numOfMummies;
				
				Destroy(gameObject, 5);
			}
		}
	}
}
