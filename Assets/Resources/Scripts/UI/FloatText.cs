using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FloatText : MonoBehaviour
{
    public Animator anim;
    private Text damageText;
    private void OnEnable()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damageText = anim.GetComponent<Text>();

    }

    public void SetText(string text)
    {
        //  anim.GetComponent<Text>().text = text;
        damageText.text = text;
    }

}

