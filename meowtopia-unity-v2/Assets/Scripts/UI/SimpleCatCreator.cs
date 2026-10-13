// Assets/Scripts/UI/SimpleCatCreator.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SimpleCatCreator : MonoBehaviour {
    [Header("Fields")]
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Dropdown breedDropdown;
    [SerializeField] TMP_Dropdown genderDropdown;
    [SerializeField] TMP_Dropdown eyeColorDropdown;

    public void OnCreate() {
        if (SaveSystem.Current == null) SaveSystem.LoadOrCreate();

        var cat = new CatData {
            CatId = System.Guid.NewGuid().ToString(),
            Name = string.IsNullOrWhiteSpace(nameInput.text) ? "Mochi" : nameInput.text.Trim(),
            Breed = breedDropdown.options[breedDropdown.value].text,
            BirthUnix = SaveSystem.Now(),
            Level = 1,
            Hunger = 100,
            Energy = 100,
            Happiness = 100,
            Hygiene = 100,
            Health = 100,
            LastUpdateUnix = SaveSystem.Now(),
            BaseColor = "Grey",
            PatternType = "Tabby",
            PatternColor = "DarkGrey",
            EyeColor = eyeColorDropdown.options[eyeColorDropdown.value].text
        };

        SaveSystem.Current.Cat = cat;
        SaveSystem.Current.User.CatId = cat.CatId;
        SaveSystem.Write();
        SceneManager.LoadSceneAsync(SceneNames.Home); // or "02_Home"
    }
}
