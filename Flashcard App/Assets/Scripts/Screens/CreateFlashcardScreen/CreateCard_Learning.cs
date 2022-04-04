using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCard_Learning : CreateCard
{
    private void Start()
    {
        UpdateDisplayBasedOnCurrentProfile(LanguageProfileController.Instance.currentLanguageProfile);
    }

    private void UpdateDisplayBasedOnCurrentProfile(LanguageProfile currentProfile)
    {
        Sprite learningFlagSprite = Resources.Load<Sprite>($"Prefabs/Sprites/Flags/{currentProfile.LearningLanguage.ISO}");
        string placeholderText = $"Enter word in {currentProfile.LearningLanguage.Name}..";

        UpdateDisplay(learningFlagSprite, placeholderText);
    }

    //React to new profile being selected by user & update display.
    private void OnNewProfileSelected(LanguageProfile newSelectedProfile)
    {
        ClearUserInput();
        UpdateDisplayBasedOnCurrentProfile(newSelectedProfile);
    }

    private void OnEnable()
    {
        LanguageProfileController.Instance.UserSelectedNewProfileEvent += OnNewProfileSelected;

        //LanguageProfile currentProfile = LanguageProfileController.Instance.currentLanguageProfile;
        //if(profileIDToRepresent != currentProfile.ID)
            //UpdateDisplayBasedOnCurrentProfile(currentProfile);
    }

    private void OnDisable() => LanguageProfileController.Instance.UserSelectedNewProfileEvent -= OnNewProfileSelected;
}