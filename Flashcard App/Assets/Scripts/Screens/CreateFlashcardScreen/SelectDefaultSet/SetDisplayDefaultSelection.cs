using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetDisplayDefaultSelection : SetDisplay
{
    public Action<SetDisplayDefaultSelection> SetDisplaySelectedEvent;

    public override void Start() => btnSelectSet.onClick.AddListener(OnSelectSetButtonPressed);

    private void OnSelectSetButtonPressed()
    {
        //Set default set as one just pressed.
        LanguageProfileController.Instance.currentLanguageProfile.DefaultSetID = setIDToRepresent;
        SetDefaultIconImage(enabled: true);

        SetDisplaySelectedEvent?.Invoke(this);
    }
}