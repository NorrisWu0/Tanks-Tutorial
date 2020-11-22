using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

/// <summary>
/// This is the player entity on the network lobby.
/// </summary>
public class NetworkLobbyPlayerEntity : NetworkBehaviour
{
    [Header("Menu Setup")]
    [SerializeField] GameObject m_LobbyCanvas = null;
    [SerializeField] TMP_Text[] m_PlayerNameTexts = new TMP_Text[4];
    [SerializeField] TMP_Text[] m_PlayerReadyTexts = new TMP_Text[4];
    [SerializeField] Button m_StartGameBtn = null;

    private bool m_IsHost = false;
    public bool isHost
    {
        set
        {
            m_IsHost = value;
            m_StartGameBtn.gameObject.SetActive(true);
        }
    }
    [SyncVar(hook = nameof(HandleDisplayNameChange))]
    public string displayName = "Joining...";
    [SyncVar(hook = nameof(HandleReadyStatusChange))]
    public bool isReady = false;

    private NetworkLobbyManager room;

    public void HandleDisplayNameChange(string _old, string _new) => UpdateCanvas();
    public void HandleReadyStatusChange(bool _old, bool _new) => UpdateCanvas();

    private void UpdateCanvas()
    {
        if (!hasAuthority)
        {

        }

    }

}
