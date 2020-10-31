using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    [Header("Audio Setup")]
    [SerializeField] AudioSource m_EngineAudioSrc = null;
    [SerializeField] AudioClip m_EngineIdling = null;
    [SerializeField] AudioClip m_EngineDriving = null;
    [SerializeField] float m_PitchRange = 0.2f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_EngineAudioSrc.pitch;
    }


    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        PlayEngineAudio();
    }



    /// <summary>
    /// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
    /// </summary>
    private void PlayEngineAudio()
    {

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_EngineAudioSrc.clip = m_EngineDriving)
            {
                m_EngineAudioSrc.clip = m_EngineIdling;
                m_EngineAudioSrc.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_EngineAudioSrc.Play();
            }
        }
        else
        {
            if (m_EngineAudioSrc.clip = m_EngineIdling)
            {
                m_EngineAudioSrc.clip = m_EngineDriving;
                m_EngineAudioSrc.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_EngineAudioSrc.Play();
            }
        }

    }


    /// <summary>
    /// Move and turn the tank.
    /// </summary>
    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    /// <summary>
    /// Adjust the position of the tank based on the player's input.
    /// </summary>
    private void MoveTank()
    {
        Vector3 _movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + _movement);
    }

    /// <summary>
    /// Adjust the rotation of the tank based on the player's input.
    /// </summary>
    private void TurnTank()
    {
        float _turnAngle = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion _turnRotation = Quaternion.Euler(0, _turnAngle, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * _turnRotation);
    }
}