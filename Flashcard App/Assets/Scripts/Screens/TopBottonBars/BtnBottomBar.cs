using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnBottomBar : BtnPushScreen
{
    public event Action<BtnBottomBar> BottomBarClickedEvent;

    [Header("Features of Button")]
    [SerializeField] private TextMeshProUGUI txtButtonDescription;
    [SerializeField] private Image imgButton;

    [Header("Deactivating")]
    [Range(0,1)]
    [SerializeField] private float notActiveAlpha;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnButtonClick()
    {
        BottomBarClickedEvent?.Invoke(this);

        base.OnButtonClick();
    }

    public void SetMaxAlpha()
    {
        //Set alpha to max.
        txtButtonDescription.alpha = 1f;
        imgButton.CrossFadeAlpha(1f, 0f, false);
    }

    public void SetLowAlpha()
    {
        //Decrease alpha.
        txtButtonDescription.alpha = notActiveAlpha;
        imgButton.CrossFadeAlpha(notActiveAlpha, 0f, false);
    }
}