using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This is used to capture and store player name before connecting/hosting a network lobby.
/// </summary>
public class PlayerName : MonoBehaviour
{
    [SerializeField] TMP_InputField m_PlayerNameInputField = null;
    [SerializeField] Button m_ContinueButton = null;

    /// <summary>
    /// Return stored name for this player
    /// </summary>
    public static string DisplayName { get; private set; }

    private const string m_PlayerPrefsKey_Name = "PlayerName";

    private void Start() => FetchSavedName();

    /// <summary>
    /// Fetch saved name from PlayerPrefs
    /// </summary>
    public void FetchSavedName()
    {
        if (PlayerPrefs.HasKey(m_PlayerPrefsKey_Name))
        {
            DisplayName = PlayerPrefs.GetString(m_PlayerPrefsKey_Name);
            m_PlayerNameInputField.text = DisplayName;
            CheckInputField();
        }

    }

    /// <summary>
    /// If the parameter is not empty, enable confirm name button.
    /// </summary>
    public void CheckInputField() => m_ContinueButton.interactable = (m_PlayerNameInputField.text != "") ? true : false;

    // TODO: Name Rules


    /// <summary>
    /// Fetch text from input field and save it on PlayerPrefs.
    /// </summary>
    public void SavePlayerName()
    {
        DisplayName = m_PlayerNameInputField.text;
        PlayerPrefs.SetString(m_PlayerPrefsKey_Name, DisplayName);
    }


}
