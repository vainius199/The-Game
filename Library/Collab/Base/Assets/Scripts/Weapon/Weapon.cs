using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private Animator anim;
    private Vector3 originalPosition;
    private AudioSource audioSource;

    private bool isReloading = false;
    private bool shootInput = false; 

    public int bulletsPerMag = 30;
    public int bulletsLeft = 200;
    public int currentBullets;

    public enum ShootMode { Auto, Semi}
    public ShootMode shootMode;

    public float range = 100;
    public float fireRate = 0.1f;
    public float damage = 20f;
    public float aimSpeed = 8f;
    float fireTimer = 0;

    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public GameObject hitParticle;
    public GameObject bulletHole;
    public Vector3 aimPosition;

	// Use this for initialization
	void Start () {
        currentBullets = bulletsPerMag;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
        switch(shootMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;
            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        
        }
        if(shootInput)
        {
            if (currentBullets > 0)
                Fire();
            else if(bulletsLeft > 0 )DoReload();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(currentBullets < bulletsPerMag && bulletsLeft > 0)
                DoReload();
        }
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;

        Aim();
	}


    private void Aim()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aimSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aimSpeed);
        }
    }
    private void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");

    }

    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;

        //  anim.SetBool("Fire", true);
     

        RaycastHit hit;
        if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            GameObject hitParticleEffect = Instantiate(hitParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject hitHoleEffect = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            Destroy(hitParticleEffect, 1.0f);
            Destroy(hitHoleEffect, 2.0f);

            if(hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
            }
        }

        anim.CrossFadeInFixedTime("Fire", 0.1f);
        muzzleFlash.Play();
        ShootSound(); 

        currentBullets--;
        fireTimer = 0.0f;
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }

    private void DoReload()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (isReloading) return;

        anim.CrossFadeInFixedTime("Reload", 0.01f);


    }

    private void ShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
