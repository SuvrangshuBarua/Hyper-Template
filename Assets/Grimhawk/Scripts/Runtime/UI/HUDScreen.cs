using grimhawk.ui;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : UIScreen
{
    [SerializeField] private AnimatableUI<Image> _currencyBG;

    public override IEnumerator PlayInAnimation()
    {
        gameObject.SetActive(true);
       
        yield return StartCoroutine(_currencyBG.PlayInAnimation(0.5f));
    }

    public override IEnumerator PlayOutAnimation()
    {
        yield return StartCoroutine(_currencyBG.PlayOutAnimation(0.5f));

        gameObject.SetActive(false);
    }

    

}
