using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    private Image damageImage;
    //  public Image deathUI;
    private Image targerImg;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    private Text PlayerHp;

    Animator anim;
    FirstPersonController fpsController;


    bool isDead;
    bool damaged;

    void Start()
    {
        targerImg = GameObject.Find("Image").GetComponent<Image>();
        damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
        PlayerHp = GameObject.Find("HpText").GetComponent<Text>();
        //  deathUI = GameObject.Find("YouDied").GetComponent<Image>();


        anim = GameObject.FindWithTag("MainCamera").GetComponent<Animator>();
        fpsController = GetComponent<FirstPersonController>();
        currentHealth = startingHealth;

        AnimationEvent evt;
        evt = new AnimationEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            PlayerHp.text = "0";
            Death();
        }
        else
        {
            PlayerHp.text = currentHealth.ToString();
        }
                     
    }

    public void HealUp()
    {
        currentHealth = 100;
        PlayerHp.text = currentHealth.ToString();
    }

    public void Death()
    {
        isDead = true;

        //   gameObject.SetActive(false);
        anim.SetTrigger("Dead");

        targerImg.enabled = false;
        fpsController.GetComponent<FirstPersonController>().enabled = false;
    }
}

  