using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    private Animator anim;
    private AudioSource _AudioSource;

    public float range = 100f;
    public int bulletsPerLoad = 30;
    public int numOfBullets;
    public float fireRate = 0.1f;
    float fireTimer;

    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    
    public AudioClip fireSound;

    private bool isReloading;

    void Start() {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        numOfBullets = bulletsPerLoad;
        anim.SetBool("Fire", false);
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
        } if (Input.GetKeyDown(KeyCode.R)) {
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
            Debug.Log("HIT FOUND");
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