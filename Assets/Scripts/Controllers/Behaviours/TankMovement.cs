using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [Header("Engine Setup")]
    [SerializeField] float m_MoveSpeed = 12f;
    [SerializeField] float m_TurnSpeed = 180f;

    [Header("Audio Setup")]
    [SerializeField] AudioSource m_EngineAudioSrc = null;
    [SerializeField] AudioClip m_EngineIdling = null;
    [SerializeField] AudioClip m_EngineDriving = null;
    [SerializeField] float m_PitchRange = 0.2f;

    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;


    private void Awake() => m_Rigidbody = GetComponent<Rigidbody>();
    private void OnEnable() => m_Rigidbody.isKinematic = false;
    private void OnDisable() => m_Rigidbody.isKinematic = true;
    private void Start() => m_OriginalPitch = m_EngineAudioSrc.pitch;

    /// <summary>
    /// Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
    /// </summary>
    public void PlayEngineAudio(float _vertInput, float _horiInput)
    {
        if (Mathf.Abs(_vertInput) < 0.1f && Mathf.Abs(_horiInput) < 0.1f)
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
    /// Adjust the position of the tank based on the player's input.
    /// </summary>
    public void MoveTank(float _vertInput)
    {
        Vector3 _movement = transform.forward * _vertInput * m_MoveSpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + _movement);
    }

    /// <summary>
    /// Adjust the rotation of the tank based on the player's input.
    /// </summary>
    public void TurnTank(float _horiInput)
    {
        float _turnAngle = _horiInput * m_TurnSpeed * Time.deltaTime;
        Quaternion _turnRotation = Quaternion.Euler(0, _turnAngle, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * _turnRotation);
    }
}