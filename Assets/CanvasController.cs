using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Text intro;
	public Text health;
	public Text ammo;
	public Text loser;

	// Use this for initialization
	void Start () {
		Invoke("removeIntroText", 1);

		// loser.gameObject.SetActive(false);
    }

    void removeIntroText() {
        Destroy(intro, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
