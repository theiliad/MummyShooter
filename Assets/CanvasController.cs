using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Text intro;
	public Text health;
	public Text ammo;

	// Use this for initialization
	void Start () {
		Invoke("removeIntroText", 1);
    }

    void removeIntroText() {
        Destroy(intro, 0);
    }

	public void test() {
        Debug.Log("Hello");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
