using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [SerializeField] float m_StartingHealth = 100f;          
    [SerializeField] Slider m_Slider = null;                        
    [SerializeField] Image m_FillImage = null;
    [SerializeField] Gradient m_HealthGradient = null;  
    [SerializeField] GameObject m_ExplosionPrefab = null;
    
    private AudioSource m_ExplosionAudio = null;
    private ParticleSystem m_ExplosionParticles = null;   
    private float m_CurrentHealth = 0.0f;
    private bool m_Dead = false;            


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    /// <summary>
    /// Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
    /// </summary>
    public void TakeDamage(float _amount)
    {
        m_CurrentHealth -= _amount;

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead)
            OnDeath();
    }

    /// <summary>
    /// Adjust the value and colour of the slider.
    /// </summary>
    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = m_HealthGradient.Evaluate(m_CurrentHealth / m_StartingHealth);
    }

    /// <summary>
    /// Play the effects for the death of the tank and deactivate it.
    /// </summary>
    private void OnDeath()
    {
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}