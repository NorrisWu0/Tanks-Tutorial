using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NetworkLobbyManager : NetworkManager
{
    [Scene] [SerializeField] string m_MenuScene = string.Empty;
    
    [SerializeField] int m_MinPlayerNumber = 2;

    [Header("Lobby")]
    [SerializeField] NetworkLobbyPlayerEntity m_LobbyEntityPrefab = null;

    // Setup Action for other functions to subscribe to
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;


    // Fetch the prefabs the game need to spawn from a folder
    [SerializeField] string m_NetworkPrefabPath = "NetworkPrebabs";
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>(m_NetworkPrefabPath).ToList();
    public override void OnStartClient()
    {
        var _networkPrebabs = Resources.LoadAll<GameObject>(m_NetworkPrefabPath);
        foreach (var _prefab in _networkPrebabs)
            ClientScene.RegisterPrefab(_prefab);
    }
    
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    /// <summary>
    /// Disconnect player if the lobby is full OR if the player is not at menu scene.
    /// </summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections || SceneManager.GetActiveScene().path != m_MenuScene)
            conn.Disconnect();
    }

    /// <summary>
    /// Instantiate and connect a player to the server if they are at menu scene
    /// </summary>
    public override void OnServerAddPlayer(NetworkConnection _conn)
    {
        if (SceneManager.GetActiveScene().path == m_MenuScene)
        {
            NetworkLobbyPlayerEntity _instance = Instantiate(m_LobbyEntityPrefab);
            NetworkServer.AddPlayerForConnection(_conn, _instance.gameObject);
        }
        else
            Debug.LogError("Hey hey!! you got a problem here!");
    }

}
