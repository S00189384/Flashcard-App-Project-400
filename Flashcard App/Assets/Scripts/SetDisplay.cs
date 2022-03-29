using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Base script which displays a set with its information.
//Default set selection display and library set display inherit from this. 

public class SetDisplay : MonoBehaviour
{
    //public Action<SetDisplay> SetDisplaySelected;
    public string setIDToRepresent;

    [Header("Components")]
    [SerializeField] protected Button btnSelectSet;
    [SerializeField] protected TextMeshProUGUI txtSetName;
    [SerializeField] protected TextMeshProUGUI txtCardCount;
    [SerializeField] protected TextMeshProUGUI txtSubsetCount;
    [SerializeField] protected Image imgDefaultSetIcon;

    //Start.
    public virtual void Awake() { }
    public virtual void Start() { } 

    //private void OnSelectSetButtonPressed()
    //{
    //    //SetDisplaySelected?.Invoke(this);
    //}

    public virtual void UpdateDisplay(string setID, string setName)
    {
        setIDToRepresent = setID;
        txtSetName.text = setName;

        imgDefaultSetIcon.enabled = LanguageProfileController.Instance.currentLanguageProfile.DefaultSetID == setIDToRepresent ? true : false;
    }

    public void SetDefaultIconImage(bool enabled) => imgDefaultSetIcon.enabled = enabled;

}