using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private Animator anim;

	void Start() {
		anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "MainCamera") {
			Debug.Log("COLLIDED With Camera");

			anim.SetBool("HitPlayer", true);
		}
    }

	void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "MainCamera") {
			Debug.Log("COLLIDEDSTAYED*** With Camera");

			anim.SetBool("HitPlayer", true);
		}
    }

	void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "MainCamera") {
			Debug.Log("COLLIDEDEXITEDxxx With Camera");

			anim.SetBool("HitPlayer", false);

			anim.CrossFadeInFixedTime("walk", 0.1f);
		}
    }
}
