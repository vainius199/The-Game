using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class Weapon : Photon.MonoBehaviour {

    private Animator anim;
    private Vector3 originalPosition;
    private AudioSource audioSource;
    private PhotonView photonView;
    private float fireTimer = 0;
    private float stabTimer = 0;


    private bool isReloading = false;

    private bool shootInput = false;
    private bool weaponHit = false;
    private bool stab = false;
    private bool throwGrenade = false;

    public int bulletsPerMag = 30;
    public int bulletsLeft = 200;
    public int currentBullets;

    public enum ShootMode { Auto, Semi, Knife, Grenade}
    public ShootMode shootMode;
    private CharacterState charState;
    public float range = 100;
    public float fireRate = 0.1f;
    public float damage = 20f;
    public float aimSpeed = 8f;
    public float spread = 0.1f;
    public float stabRate = 0.0f;

    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public GameObject hitParticle;
    public GameObject bulletHole;
    public Vector3 aimPosition;
    private  Text ammoText;
    public RawImage weaponTarget;

	// Use this for initialization
	void Start () {
        photonView = gameObject.GetComponentInParent<PhotonView>();
        currentBullets = bulletsPerMag;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        originalPosition = transform.localPosition;
        UpdateAmmoText();
        if(gameObject.name == "FPS-Sniper")
        {
            weaponTarget = GameObject.Find("SnipeImage").GetComponent<RawImage>();
        }
	}
	
	// Update is called once per frame
	void Update () {
 
        if (photonView.isMine || GetComponentInParent<FirstPersonController>().testing)
        {
            switch (shootMode)
            {
                case ShootMode.Auto:
                    shootInput = Input.GetButton("Fire1");
                    break;
                case ShootMode.Semi:
                    shootInput = Input.GetButtonDown("Fire1");
                    break;
                case ShootMode.Knife:
                    shootInput = false;
                    stab = Input.GetButtonDown("Fire1");
                    break;
                case ShootMode.Grenade:
                    shootInput = false;
                    throwGrenade = Input.GetButtonUp("Fire1");
                    break;
            }

            if (shootInput)
            {
                if (currentBullets > 0)
                    Fire();
                else if (bulletsLeft > 0) DoReload();
            }
            else if (stab)
            {
                Stab();
            }
            else if(throwGrenade)
            {
                ThrowGrenade();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (currentBullets < bulletsPerMag && bulletsLeft > 0)
                    DoReload();
            }

            else if (Input.GetKeyDown(KeyCode.F) && shootMode != ShootMode.Knife)
            {
                anim.CrossFadeInFixedTime("hit", 0.1f);
                GetComponentInParent<FirstPersonController>().characterState = AnimState.hit;
                RaycastHit hit;

                if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward * 0.5f, out hit, 10))
                {
                    if (hit.transform.GetComponent<HealthController>())
                    {
                        hit.transform.GetComponent<HealthController>().ApplyDamage(50);
                    }

                    else if (hit.transform.GetComponent<EnemyHealth>())
                    {
                        hit.transform.GetComponent<EnemyHealth>().DoDamageToEnemy(50, hit.point);
                    }
                }
                weaponHit = true;
            }
            if (fireTimer < fireRate) fireTimer += Time.deltaTime;
            if (stabTimer < stabRate) stabTimer += Time.deltaTime;

            Aim();
        }
	}

    private void Stab()
    {
        if (stabTimer < stabRate) return;
        anim.CrossFadeInFixedTime("stab", 0.1f);
        stabTimer = 0.0f;
        GetComponentInParent<FirstPersonController>().characterState = AnimState.stab;
    }

    private void ThrowGrenade()
    {
        GameObject grenade;
        if (PhotonNetwork.connected)
            grenade = PhotonNetwork.Instantiate("GrenadePrefab", shootPoint.transform.position, shootPoint.transform.rotation, 0);
        else
            grenade = Instantiate((GameObject)Resources.Load("GrenadePrefab"), shootPoint.transform.position, shootPoint.transform.rotation);
        grenade.transform.localScale = new Vector3(1,1,1);
        
        grenade.AddComponent<Rigidbody>();
        grenade.AddComponent<SphereCollider>();
        grenade.GetComponent<Rigidbody>().AddForce(shootPoint.forward * 500);
    }

    private void Aim()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            anim.enabled = false;
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aimSpeed);

            if (weaponTarget != null)
            {
                GetComponentInParent<Camera>().fieldOfView = 15;
                weaponTarget.enabled = true;
            }
        }
        else
        {
            if (weaponTarget != null)
                weaponTarget.enabled = false;
            GetComponentInParent<Camera>().fieldOfView = 60;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aimSpeed);
            anim.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(weaponHit.ToString());
    }

    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

        Vector3 shootDirection = shootPoint.transform.forward;
        shootDirection.x += Random.Range(-spread, spread);
        shootDirection.y += Random.Range(-spread, spread);

        RaycastHit hit;
        if(Physics.Raycast(shootPoint.position, shootDirection, out hit, range))
        {
            if (hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
               
            }
            else if (hit.transform.GetComponent<EnemyHealth>() && hit.collider.tag != "Head")
            {
                hit.transform.GetComponent<EnemyHealth>().DoDamageToEnemy(damage, hit.point);
            }
            else if (hit.collider.tag == "Head")
            {
                hit.transform.GetComponent<EnemyHealth>().DoDamageToEnemy(damage*5, hit.point);
            }
            else
            {                
                GameObject hitParticleEffect = Instantiate(hitParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                GameObject hitHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                Destroy(hitParticleEffect, 1.0f);
                Destroy(hitHoleEffect, 2.0f);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.1f);
        GetComponentInParent<FirstPersonController>().characterState = AnimState.Fire;
        muzzleFlash.Play();
        ShootSound(); 

        currentBullets--;
        UpdateAmmoText();
        fireTimer = 0.0f;
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
        UpdateAmmoText();
    }

    private void DoReload()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (isReloading) return;

        anim.CrossFadeInFixedTime("Reload", 0.01f);
        GetComponentInParent<FirstPersonController>().characterState = AnimState.Reload;
    }

    private void ShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }

    private void UpdateAmmoText()
    {
       ammoText.text = currentBullets + "/" + bulletsLeft;
    }

    private void OnEnable()
    {
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        UpdateAmmoText();
    }

    public void pickUpAmmo(int ammo)
    {
        bulletsLeft += ammo;
        UpdateAmmoText();
    }
   

}
