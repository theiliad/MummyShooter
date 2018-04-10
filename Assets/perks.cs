using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perks : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "MainCamera") {
			Debug.Log("PERK*** - COLLIDED With Camera");

			Destroy(gameObject);

			Weapon weaponController = collision.gameObject.GetComponentInChildren<Weapon>();
			if (gameObject.tag == "PerkHealth") {
				weaponController.playerHealth = 100f;
			} else if (gameObject.tag == "PerkAmmo") {
				weaponController.numOfbulletPacks++;
			}
		}
    }
}
