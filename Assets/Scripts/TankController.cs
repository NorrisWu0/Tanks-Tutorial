using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankHealth))]
[RequireComponent(typeof(TankMovement))]
[RequireComponent(typeof(TankShooting))]

/// <summary>
/// Tank Controller class will handles all the input and forward it to the sub components for corresponding actions.
/// </summary>
public class TankController : MonoBehaviour
{
    public int playerNumber = 0;

    // Input Control
    private string m_VertAxis = "";
    private string m_HoriAxis = "";
    private string m_FireButton = "";

    // Movement Variables
    private float m_VertInputValue = 0;
    private float m_HoriInputValue = 0;

    // Reference to components
    private TankMovement m_Movement = null;
    private TankShooting m_Shooting = null;

    private void Awake()
    {
        m_Movement = GetComponent<TankMovement>();
        m_Shooting = GetComponent<TankShooting>();
    }

    private void OnEnable()
    {
        m_VertInputValue = 0f;
        m_HoriInputValue = 0f;
    }

    void Start()
    {
        m_VertAxis = "Vertical" + playerNumber;
        m_HoriAxis = "Horizontal" + playerNumber;
        m_FireButton = "Fire" + playerNumber;
    }

    /// <summary>
    /// Handles all the Input from user to control the tank
    /// </summary>
    private void Update()
    {
        if (m_Movement.enabled)
        {
            // Store the player's input and make sure the audio for the engine is playing.
            m_VertInputValue = Input.GetAxis(m_VertAxis);
            m_HoriInputValue = Input.GetAxis(m_HoriAxis);

            m_Movement.PlayEngineAudio(m_VertInputValue, m_HoriInputValue);
        }

        if (m_Shooting.enabled)
            m_Shooting.FiringLogic(m_FireButton);
    }


    /// <summary>
    /// Move and turn the tank.
    /// </summary>
    void FixedUpdate()
    {
        m_Movement.MoveTank(m_VertInputValue);
        m_Movement.TurnTank(m_HoriInputValue);
    }

    /// <summary>
    /// Toggle Tank components via passing a boolean value.
    /// </summary>
    /// <param name="_state1">Controls movement component</param>
    /// <param name="_state2">Controls shooting component</param>
    public void ToggleTankControl(bool _state1, bool _state2)
    {
        m_Movement.enabled = _state1;
        m_Shooting.enabled = _state2;
    }
}
