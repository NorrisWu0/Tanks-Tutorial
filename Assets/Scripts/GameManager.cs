using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] int m_NumRoundsToWin = 5;        
    [SerializeField] float m_StartDelay = 3f;         
    [SerializeField] float m_EndDelay = 3f;           
    [SerializeField] CameraControl m_CameraControl = null;
    [SerializeField] TextMeshProUGUI m_MessageText = null;
    [SerializeField] GameObject m_TankPrefab = null;
    [SerializeField] TankManager[] m_Tanks = null;           


    private int m_RoundNumber = 0;              
    private WaitForSeconds m_StartWait = null;
    private WaitForSeconds m_EndWait = null;
    private TankManager m_RoundWinner = null;
    private TankManager m_GameWinner = null;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    /// <summary>
    /// Instantiate tanks in the scene based on assigned spawn points.
    /// </summary>
    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].instance =
                Instantiate(m_TankPrefab, m_Tanks[i].spawnPoint.position, m_Tanks[i].spawnPoint.rotation) as GameObject;
            m_Tanks[i].playerNumber = i + 1;
            m_Tanks[i].Setup();
        }
    }

    /// <summary>
    /// Assign targets to Camera Controller to control camera behaviour
    /// </summary>
    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].instance.transform;
        }

        m_CameraControl.targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
            SceneManager.LoadScene(0);
        else
            StartCoroutine(GameLoop());
    }

    /// <summary>
    /// Prepare the level to when the game is starting.
    /// </summary>
    private IEnumerator RoundStarting()
    {
        // Setup tanks
        ResetAllTanks();

        // Disable tanks before game actually started.
        DisableTankControl();

        // Setup camera transform
        m_CameraControl.SetStartPositionAndSize();

        // Display correct round number
        m_RoundNumber++;
        m_MessageText.SetText("Round " + m_RoundNumber);
        
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // Release tank control to the player
        EnableTankControl();

        // Hide message UI
        m_MessageText.SetText(string.Empty);

        // Wait until there is a winner
        while (!CheckIfOneTankLeft())
            yield return null;

    }


    private IEnumerator RoundEnding()
    {
        // Remove tank control from player
        DisableTankControl();

        // Clear and set winner
        m_RoundWinner = null;
        m_RoundWinner = GetRoundWinner();

        // Add win count for winner
        if (m_RoundWinner != null)
            m_RoundWinner.wins++;

        Debug.Log(m_RoundWinner.instance.gameObject);
        Debug.Log(m_RoundWinner.wins);

        // Check if there is a game winner
        m_GameWinner = GetGameWinner();

        // Display round result
        m_MessageText.SetText(GetEndMessage());

        yield return m_EndWait;
    }


    private bool CheckIfOneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
            if (m_Tanks[i].instance.activeSelf)
                numTanksLeft++;

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
            if (m_Tanks[i].instance.activeSelf)
                return m_Tanks[i];

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
            if (m_Tanks[i].wins == m_NumRoundsToWin)
                return m_Tanks[i];

        return null;
    }


    private string GetEndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.coloredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
            message += m_Tanks[i].coloredPlayerText + ": " + m_Tanks[i].wins + " WINS\n";

        if (m_GameWinner != null)
            message = m_GameWinner.coloredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
            m_Tanks[i].Reset();
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
            m_Tanks[i].EnableControl();
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
            m_Tanks[i].DisableControl();
    }
}