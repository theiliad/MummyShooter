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
    public int oldNumOfbulletPacks = 3;
    public int numOfbulletPacks = 3;
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
    
    private bool _devMode = false;

    public Text healthText;
    private Text ammoText;
    private Text mummiesText;
    private Text loserText;
    private Text winnerText;

    public double playerHealth = 100;
    public double oldHealth = 100;

    public int numOfMummies = 1;

    private GameObject originalMummy;

    void spawnMummy() {
        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(0.0f, 90.0f), 0, UnityEngine.Random.Range(0.0f, 90.0f));
        originalMummy.SetActive(true);
        if (originalMummy != null) {
            GameObject newMummy = Instantiate(GameObject.Find("mummy_rig"), randomPos, Quaternion.identity);
            newMummy.transform.Find("BloodSprayEffect").gameObject.SetActive(false);

            numOfMummies++;
            mummiesText.text = "Mummmies: " + numOfMummies;
        }
        originalMummy.SetActive(false);
    }

    void spawnAmmoAndHealth() {
        Vector3 ammoPos = new Vector3(UnityEngine.Random.Range(0.0f, 90.0f), 0.167324f, UnityEngine.Random.Range(0.0f, 90.0f));
        Vector3 healthPos = new Vector3(UnityEngine.Random.Range(0.0f, 90.0f), 0.167324f, UnityEngine.Random.Range(0.0f, 90.0f));
        GameObject ammoObject = Instantiate(GameObject.Find("AMMO_FBX"), ammoPos, Quaternion.identity);
        GameObject healthObject = Instantiate(GameObject.Find("HP_FBX"), healthPos, Quaternion.identity);

        ammoObject.transform.Rotate(-90, 0, 180);
        healthObject.transform.Rotate(-90, 0, 180);

        Rigidbody ammoRigid = ammoObject.GetComponent<Rigidbody>();
        Rigidbody healthRigid = healthObject.GetComponent<Rigidbody>();

        ammoRigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        healthRigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
    }
    
    void Start() {
        anim = GetComponent<Animator>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        ammoText = GameObject.Find("Ammo").GetComponent<Text>();
        mummiesText = GameObject.Find("Mummies").GetComponent<Text>();
        loserText = GameObject.Find("Loser").GetComponent<Text>();
        winnerText = GameObject.Find("Winner").GetComponent<Text>();

        winnerText.gameObject.SetActive(false);
        loserText.gameObject.SetActive(false);

        originalMummy = GameObject.Find("mummy_rig");
        originalMummy.SetActive(false);

        for (int i = 0; i < 5; i++) {
            spawnMummy();

            if (i < 3)
                spawnAmmoAndHealth();
        }
        InvokeRepeating("spawnMummy", 5.0f, 5.0f);
        InvokeRepeating("spawnAmmoAndHealth", 30.0f, 30.0f);
        
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
            else if (numOfbulletPacks > 0)
                _reloadAnimation();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            if (numOfBullets < bulletsPerLoad && numOfbulletPacks > 0) {
                _reloadAnimation();
            }
        }

        if (fireTimer < fireRate) {
            fireTimer += Time.deltaTime;
        }

        if (oldHealth != playerHealth) {
            healthText.text = "Health: " + (int) Math.Round(playerHealth, 2) + "%";
            oldHealth = playerHealth;
        }

        if (oldNumOfbulletPacks != numOfbulletPacks) {
            ammoText.text = "Ammo (" + numOfBullets + "/" + numOfbulletPacks * bulletsPerLoad + ")";
            oldNumOfbulletPacks = numOfbulletPacks;
        }

        if (playerHealth > 0 && playerHealth < 81) {
            if (playerHealth >= 70) {
                healthText.color = new Color(1f, 0.5f, 0.8f);
            } else if (playerHealth >= 40) {
                healthText.color = new Color(1f, 0.56f, 0f);
            } else {
                healthText.color = new Color(1f, 0, 0);
            }
        }

        if (playerHealth > 81) {
            healthText.color = new Color(1f, 0.5f, 0.8f);
        }

        if (playerHealth <= 0) {
            gameOver(false);
        }

        if (numOfMummies == 0) {
            gameOver(true);
        }
    }

    private void gameOver(bool userWon) {
        Time.timeScale = 0;

        if (userWon) {
            winnerText.gameObject.SetActive(true);
        } else {
            loserText.gameObject.SetActive(true);
        }
    }

    IEnumerator _hideBloodSplatter(float delay, GameObject mummyObject) {
        yield return new WaitForSeconds(delay);
        mummyObject.transform.Find("BloodSprayEffect").gameObject.SetActive(false);
    }

    private void _fire() {
        if (fireTimer < fireRate || numOfBullets <= 0 || isReloading) return;

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) {
            Debug.Log("HIT FOUND" + hit.collider.name);

            if (hit.collider.GetComponentInParent<HealthController>()) {
                Debug.Log("HEALTH CONTROLLER FOUND");
                hit.collider.GetComponentInParent<HealthController>().ApplyDamage(damageAmount);


                GameObject mummyObject = hit.collider.transform.parent.gameObject;
                mummyObject.transform.Find("BloodSprayEffect").gameObject.SetActive(true);

                StartCoroutine(_hideBloodSplatter(0.5f, mummyObject));
            } else {
                GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(hitParticleEffect, 1f);

                GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Destroy(bulletHole, 30f);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.1f);
        muzzleFlash.Play();
        _playFireSound();

        // anim.SetBool("Fire", true);

        numOfBullets--;
        ammoText.text = "Ammo (" + numOfBullets + "/" + numOfbulletPacks * bulletsPerLoad + ")";
        fireTimer = 0.0f;
    }

    private void _reloadAnimation() {
        if (isReloading) return;
        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }

    public void reload() {
        if (numOfbulletPacks == 0) return;
        numOfBullets = bulletsPerLoad;
        numOfbulletPacks--;

        ammoText.text = "Ammo (" + numOfBullets + "/" + numOfbulletPacks * bulletsPerLoad  + ")";
    }

    private void _playFireSound() {
        _AudioSources[0].PlayOneShot(fireSound);

        // _AudioSource.clip = fireSound;
        // _AudioSource.Play();
    }
}