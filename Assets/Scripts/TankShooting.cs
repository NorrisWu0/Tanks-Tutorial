using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    [Header("Cannon Stats")]
    [SerializeField] float m_MinLaunchForce = 15f; 
    [SerializeField] float m_MaxLaunchForce = 30f; 
    [SerializeField] float m_MaxChargeTime = 0.75f;

    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         

    [Header("Cannon Setup")]
    [SerializeField] Rigidbody m_Shell = null;            
    [SerializeField] Transform m_Muzzle = null;    
    [SerializeField] Slider m_AimSlider = null;
    private bool m_Fired;                
    
    [Header("Audio Setup")]
    [SerializeField] AudioSource m_ShootingAudio = null;  
    [SerializeField] AudioClip m_ChargingClip = null;     
    [SerializeField] AudioClip m_FireClip = null;         
     


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    /// <summary>
    /// Track the current state of the fire button and make decisions based on the current launch force.
    /// </summary>
    public void FiringLogic(string _fireButton)
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // Max charge reached, fire shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            FireShell();

        }
        else if (Input.GetButtonDown(_fireButton))
        {
            // Pressed fire button, start charging from mininum launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(_fireButton) && !m_Fired)
        {
            // Holding fire button to charge launch force.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(_fireButton) && !m_Fired)
        {
            // Released button and fire shell.
            FireShell();
        }

    }


    /// <summary>
    /// Instantiate and launch the shell.
    /// </summary>
    private void FireShell()
    {
        m_Fired = true;

        Rigidbody _shellRigidbody = Instantiate(m_Shell, m_Muzzle.position, m_Muzzle.rotation) as Rigidbody;
        _shellRigidbody.velocity = m_CurrentLaunchForce * m_Muzzle.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}