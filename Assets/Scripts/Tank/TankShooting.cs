using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    [SerializeField] Rigidbody m_Shell = null;            
    [SerializeField] Transform m_Muzzle = null;    
    [SerializeField] Slider m_AimSlider = null;           
    [SerializeField] AudioSource m_ShootingAudio = null;  
    [SerializeField] AudioClip m_ChargingClip = null;     
    [SerializeField] AudioClip m_FireClip = null;         
    [SerializeField] float m_MinLaunchForce = 15f; 
    [SerializeField] float m_MaxLaunchForce = 30f; 
    [SerializeField] float m_MaxChargeTime = 0.75f;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    /// <summary>
    /// Track the current state of the fire button and make decisions based on the current launch force.
    /// </summary>
    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // Max charge reached, fire shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();

        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            // Pressed fire button, start charging from mininum launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            // Holding fire button to charge launch force.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            // Released button and fire shell.
            Fire();
        }

    }

    /// <summary>
    /// Instantiate and launch the shell.
    /// </summary>
    private void Fire()
    {
        m_Fired = true;

        Rigidbody _shellRigidbody = Instantiate(m_Shell, m_Muzzle.position, m_Muzzle.rotation) as Rigidbody;
        _shellRigidbody.velocity = m_CurrentLaunchForce * m_Muzzle.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}