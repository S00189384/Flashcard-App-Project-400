using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


//Controller to hold a list of all the language profiles the user has created and their current profile they are using on the app.

//TO DO - make class that reads / writes json from / to specific folder.
// I'm writing 3 profiles to JSON at start every time - eventually have a check for if there is no profiles on device - prompt them to make one before they can continue.
//Ensure there's at least one profile saved before allowing user to make cards, do other stuff etc. 
//Check for this in ScreenController? If no profiles - load create profile screen, otherwise load create card screen.
//Use JSONHelper methods here to read list of objects from jsonfile etc. since this script is doing that its own way. 

//NOTE: I changed the script execution order for this script so it gets called before others. 

public class LanguageProfileController : MonoBehaviour
{
    public static LanguageProfileController Instance;

    public event Action<LanguageProfile> UserSelectedNewProfileEvent;


    [Header("Storing JSON Location - Unity Editor & PC")]
    [SerializeField] private string languageProfilesListJSONPathPC;
    [SerializeField] private string currentProfileJSONPathPC;
    [Header("Storing JSON Location - Mobile")]
    [SerializeField] private string languageProfilesListJSONPathMobile;
    [SerializeField] private string currentProfileJSONPathMobile;
    [Header("User's language profile info")]
    [SerializeField] private List<LanguageProfile> userLanguageProfilesList = new List<LanguageProfile>();


    private LanguageProfile currentLanguageProfile;
    public LanguageProfile CurrentLanguageProfile
    {
        get => currentLanguageProfile;
        set
        {
            //New set ID? Update.
            if (currentLanguageProfile != value)
            {
                currentLanguageProfile = value;
                UserSelectedNewProfileEvent?.Invoke(currentLanguageProfile);
            }
        }
    }


    [Header("API Settings")]
    [SerializeField] private bool PostNewLanguageProfilesToAPI;
    private Action<LanguageProfile> OnLanguageProfileCreated;

    public List<LanguageProfile> GetUserLanguageProfiles() => userLanguageProfilesList;

    #region Start
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        if (PostNewLanguageProfilesToAPI)
            OnLanguageProfileCreated += PostLanguageProfileToAPI;

        OnLanguageProfileCreated += CheckToAssignNewCurrentLanguageProfile;
        OnLanguageProfileCreated += SaveLanguageProfileToMemory;

        LanguageProfile.LanguageProfileCreatedEvent += OnLanguageProfileCreated;
        BtnLogout.UserLoggedOutEvent += ClearLanguageProfileDataFromMemory;
    }

    private void ClearLanguageProfileDataFromMemory()
    {
        currentLanguageProfile = null;
        userLanguageProfilesList.Clear();
    }

    private void PostLanguageProfileToAPI(LanguageProfile languageProfile) => APIUtilities.Instance.PostNewLanguageProfile(UserDataHolder.Instance.CurrentUser.ID,languageProfile);
    private void SaveLanguageProfileToMemory(LanguageProfile languageProfile) => userLanguageProfilesList.Add(languageProfile);
    private void CheckToAssignNewCurrentLanguageProfile(LanguageProfile newLanguageProfile)
    {
        //New profile not marked as current profile? Don't need to do anything.
        if (newLanguageProfile.IsCurrentProfile == false)
            return;

        //Check if there is another Profile that is marked as current profile. Switch around.
        LanguageProfile previousCurrentLanguageProfile = userLanguageProfilesList.Find(profile => profile.IsCurrentProfile == true);
        if (previousCurrentLanguageProfile == null)
        {
            CurrentLanguageProfile = newLanguageProfile;
            return;
        }

        //Switch around.
        previousCurrentLanguageProfile.IsCurrentProfile = false;
        CurrentLanguageProfile = newLanguageProfile;

        //Update previous current lang profile on API side. 
        APIUtilities.Instance.ModifyLanguageProfile(previousCurrentLanguageProfile.ID,previousCurrentLanguageProfile);
    }

    public void UpdateLanguageProfilesData(List<LanguageProfile> languageProfiles)
    {
        userLanguageProfilesList = languageProfiles;
        currentLanguageProfile = languageProfiles.SingleOrDefault(profile => profile.IsCurrentProfile == true);
    }

    #endregion
    #region New Language Profile Creation / Selection

    //Select new Profile passing in languageprofile object.
    public void SelectNewProfile(LanguageProfile newProfile)
    {
        //If user selects same profile, dont have to do anything.
        if (newProfile == currentLanguageProfile)
            return;

        //If there was a current profile, set that to not be the current profile anymore and update API.
        if(currentLanguageProfile != null)
        {
            currentLanguageProfile.IsCurrentProfile = false;
            APIUtilities.Instance.ModifyLanguageProfile(currentLanguageProfile.ID, currentLanguageProfile);
        }

        //Update current profile which raises event to notify certain UI objects.
        CurrentLanguageProfile = newProfile;
        currentLanguageProfile.IsCurrentProfile = true;

        APIUtilities.Instance.ModifyLanguageProfile(currentLanguageProfile.ID, currentLanguageProfile);

        print($"New Language Profile Was Selected: {newProfile}");
    }

    //Select new Profile passing in ID. Finds profile based on ID.
    public void SelectNewProfile(string ID)
    {
        LanguageProfile newProfile = userLanguageProfilesList.Find(profile => profile.ID == ID);
        SelectNewProfile(newProfile);
    }
    #endregion
    #region Writing / Saving Data To Json
    private void CreateSampleProfilesAndSaveToJSON()
    {
        List<LanguageProfile> languageProfiles = new List<LanguageProfile>()
        {
            new LanguageProfile(nativeLanguage: new Language("en","English"),learningLanguage: new Language("ru","Russian"),IsCurrentProfile: true),
            new LanguageProfile(nativeLanguage: new Language("en","English"),learningLanguage: new Language("ja","Japanese"),IsCurrentProfile: false),
            new LanguageProfile(nativeLanguage: new Language("en","English"),learningLanguage: new Language("it","Italian"),IsCurrentProfile: false),
        };

        SaveListOfLanguageProfilesToJSON(languageProfiles);
    }
    private void SaveListOfLanguageProfilesToJSON(List<LanguageProfile> languageProfilesList)
    {
        string json = JSONHelper.ToJson(languageProfilesList);

#if UNITY_EDITOR

        if(File.Exists(Application.dataPath + languageProfilesListJSONPathPC) == false)
            File.Create(Application.dataPath + languageProfilesListJSONPathPC).Dispose();


        //Old way - File.WriteAllText(Application.dataPath + languageProfilesListJSONPathPC, json);
        using (TextWriter writer = new StreamWriter(Application.dataPath + languageProfilesListJSONPathPC, false))
        {           
            writer.WriteLine(json);
            writer.Close();
        }

#elif UNITY_ANDROID
        File.WriteAllText(Application.persistentDataPath + languageProfilesListJSONPathMobile, json);
#endif
    }
    private List<LanguageProfile> ReadLanguageProfileListFromJSON()
    {
        string json;

#if UNITY_EDITOR
        json = File.ReadAllText(Application.dataPath + languageProfilesListJSONPathPC);

#elif UNITY_ANDROID
        json = File.ReadAllText(Application.persistentDataPath + languageProfilesListJSONPathMobile);

#endif
        List<LanguageProfile> profiles = JSONHelper.FromJson<LanguageProfile>(json);
        return profiles;
    }
    private void SaveEngRuAsCurrentLanguageProfileToJson()
    {
        LanguageProfile englishRussianProfile = new LanguageProfile(new Language("en","English"),new Language("ru","Russian"),true);

        string profileAsJSON = JsonUtility.ToJson(englishRussianProfile);

        string filePath = Application.dataPath + currentProfileJSONPathPC;

        File.WriteAllText(filePath, profileAsJSON);
    }
    private LanguageProfile ReadCurrentLanguageProfileFromJSON()
    {
        LanguageProfile currentLanguageProfile = JsonUtility.FromJson<LanguageProfile>(File.ReadAllText(Application.dataPath + currentProfileJSONPathPC));
        return currentLanguageProfile;
    }
    #endregion
    #region Event Subscribing / Unsubscribing.
    private void OnEnable()
    {
        //LanguageProfile.LanguageProfileCreatedEvent += OnNewLanguageProfileCreated;
        //Set.SetCreatedEvent += OnNewSetCreated;
    }

    private void OnDisable()
    {
        //LanguageProfile.LanguageProfileCreatedEvent -= OnNewLanguageProfileCreated;
        //Set.SetCreatedEvent -= OnNewSetCreated;
    }
    #endregion

    private void OnDestroy()
    {
        LanguageProfile.LanguageProfileCreatedEvent -= OnLanguageProfileCreated;
        BtnLogout.UserLoggedOutEvent -= ClearLanguageProfileDataFromMemory;
    }

    //Application stops: Save all info to JSON?
    private void OnApplicationQuit()
    {
        //SaveListOfLanguageProfilesToJSON(userLanguageProfilesList);
    }
}