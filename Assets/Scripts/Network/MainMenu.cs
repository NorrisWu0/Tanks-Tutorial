using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] NetworkLobbyManager m_NetworkLobbyManager = null;
    
    [SerializeField] GameObject m_MainMenuPanel = null;
    [SerializeField] TMP_InputField m_IPAddressInputField = null;
    [SerializeField] Button m_JoinButton = null;

    public void HostLobby() => m_NetworkLobbyManager.StartHost();

    private void OnEnable()
    {
        NetworkLobbyManager.OnClientConnected += HandleClientConnected;
        NetworkLobbyManager.OnClientConnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkLobbyManager.OnClientConnected -= HandleClientConnected;
        NetworkLobbyManager.OnClientConnected -= HandleClientDisconnected;
    }

    public void CheckIPInputField() => m_JoinButton.interactable = (m_IPAddressInputField.text != string.Empty) ? true : false;

    /// <summary>
    /// Validate IP Address then join the network lobby
    /// </summary>
    public void JoinNetworkLobby()
    {
        string _ip; 
        if (ValidateIPAddress())
        {
            _ip = m_IPAddressInputField.text;

            m_NetworkLobbyManager.networkAddress = _ip;
            m_NetworkLobbyManager.StartClient();

            m_JoinButton.interactable = false;
        }
    }

    public bool ValidateIPAddress()
    {
        // TODO: Validate IP Address
        return true;
    }

    /// <summary>
    /// Enable the join lobby button then hide the main menu.
    /// </summary>
    private void HandleClientConnected()
    {
        m_JoinButton.interactable = true;
        m_MainMenuPanel.SetActive(false);
    }

    /// <summary>
    /// Re-enable the join lobby button.
    /// </summary>
    private void HandleClientDisconnected() => m_JoinButton.interactable = true;

}
