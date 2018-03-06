using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    private Animator anim;

    public AudioSource[] _AudioSources;
    private AudioSource _AudioSource;

    public float range = 2000f;
    public int bulletsPerLoad = 30;
    public int numOfBullets;
    public float fireRate = 0.1f;
    public float damageAmount = 10f;
    float fireTimer;

    [Header("Weapon Config")]
    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public GameObject hitParticles;
    public GameObject bulletImpact;
    
    public AudioClip fireSound;
    public AudioClip backgroundMusic;

    private bool isReloading;

    public Texture2D crosshairTexture;
    private Rect position;
    
    private bool _devMode = true;

    private Text healthText;
    private Text ammoText;
    private Text mummiesText;

    public double playerHealth = 100;
    public double oldHealth = 100;

    void spawnMummy() {
        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(0.0f, 90.0f), 0, UnityEngine.Random.Range(0.0f, 90.0f));
        Instantiate(GameObject.Find("mummy_rig"), randomPos, Quaternion.identity);

        String resultString = Regex.Match(mummiesText.text, @"\d+").Value;
        mummiesText.text = "Mummmies: " + (Int32.Parse(resultString) + 1);
    }
    
    void Start() {
        anim = GetComponent<Animator>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
        mummiesText = GameObject.Find("Mummies").GetComponent<Text>();

        for (int i = 1; i <= 5; i++) {
            spawnMummy();
        }
        InvokeRepeating("spawnMummy", 5.0f, 5.0f);
        
        _AudioSources = GetComponents<AudioSource>();
        if (!_devMode) {
            _AudioSources[1].clip = backgroundMusic;
            _AudioSources[1].Play();
        }
        
        numOfBullets = bulletsPerLoad;
        anim.SetBool("Fire", false);

        float width = crosshairTexture.width + 20;
        float height = crosshairTexture.height + 20;
        position = new Rect((Screen.width - width) / 2, (Screen.height - 
            height) /2, width , height);
    }

    void OnGUI() {
        GUI.DrawTexture(position, crosshairTexture);
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

        if (oldHealth != playerHealth) {
            healthText.text = "Health: " + Math.Round(playerHealth, 2);
            oldHealth = playerHealth;
        }

        if (playerHealth < 81) {
            if (playerHealth >= 70) {
                healthText.color = new Color(1f, 0.5f, 0.8f);
            } else if (playerHealth >= 40) {
                healthText.color = new Color(1f, 0.56f, 0f);
            } else {
                healthText.color = new Color(1f, 0, 0);
            }
        }
    }

    private void _fire() {
        if (fireTimer < fireRate || numOfBullets <= 0 || isReloading) return;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) {
            Debug.Log("HIT FOUND" + hit.collider.name);

            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Destroy(hitParticleEffect, 1f);

            if (hit.collider.GetComponentInParent<HealthController>()) {
                Debug.Log("HEALTH CONTROLLER FOUND");
                hit.collider.GetComponentInParent<HealthController>().ApplyDamage(damageAmount);
            } else {
                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(bulletHole, 30f);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.1f);
        muzzleFlash.Play();
        _playFireSound();

        // anim.SetBool("Fire", true);

        numOfBullets--;
        ammoText.text = "Ammo (" + numOfBullets + "/∞)";
        fireTimer = 0.0f;
    }

    private void _reloadAnimation() {
        if (isReloading) return;
        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    public void reload() {
        numOfBullets = bulletsPerLoad;
        ammoText.text = "Ammo (" + numOfBullets + "/∞)";
    }

    private void _playFireSound() {
        _AudioSources[0].PlayOneShot(fireSound);

        // _AudioSource.clip = fireSound;
        // _AudioSource.Play();
    }
}