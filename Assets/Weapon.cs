using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    private Animator anim;
    private AudioSource _AudioSource;

    public float range = 2000f;
    public int bulletsPerLoad = 30;
    public int numOfBullets;
    public float fireRate = 0.1f;
    public float damageAmount = 10f;
    float fireTimer;

    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    
    public AudioClip fireSound;

    private bool isReloading;

    public Texture2D crosshairTexture;
    private Rect position;
    private bool OriginalOn = true;

    void Start() {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        numOfBullets = bulletsPerLoad;
        anim.SetBool("Fire", false);

        float width = crosshairTexture.width + 20;
        float height = crosshairTexture.height + 20;
        position = new Rect((Screen.width - width) / 2, (Screen.height - 
            height) /2, width , height);
    }

    void OnGUI() {
        if(OriginalOn == true) {
            GUI.DrawTexture(position, crosshairTexture);
        }
    }

    void FixedUpdate() {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Fire")) anim.SetBool("Fire", false);

        isReloading = info.IsName("Reload");
    }

    void Update() {
        if (Input.GetButton("Fire1")) {
            if (numOfBullets > 0)
                _fire();
            else
                _reloadAnimation();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            if (numOfBullets < bulletsPerLoad) {
                _reloadAnimation();
            }
        }

        if (fireTimer < fireRate) {
            fireTimer += Time.deltaTime;
        }
    }

    private void _fire() {
        if (fireTimer < fireRate || numOfBullets <= 0 || isReloading) return;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) {
            Debug.Log("HIT FOUND" + hit.collider.name);

            if (hit.collider.GetComponentInParent<HealthController>()) {
                Debug.Log("HEALTH CONTROLLER FOUND");
                hit.collider.GetComponentInParent<HealthController>().ApplyDamage(damageAmount);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.1f);
        muzzleFlash.Play();
        _playFireSound();

        // anim.SetBool("Fire", true);

        numOfBullets--;
        fireTimer = 0.0f;
    }

    private void _reloadAnimation() {
        if (isReloading) return;
        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    public void reload() {
        numOfBullets = bulletsPerLoad;
    }

    private void _playFireSound() {
        _AudioSource.PlayOneShot(fireSound);

        // _AudioSource.clip = fireSound;
        // _AudioSource.Play();
    }
}