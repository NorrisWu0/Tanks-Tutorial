using UnityEngine;
using UnityEngine.Analytics;

public class ShellExplosion : MonoBehaviour
{
    [SerializeField] LayerMask m_TankMask = new LayerMask();
    [SerializeField] ParticleSystem m_ExplosionParticles = null;
    [SerializeField] AudioSource m_ExplosionAudio = null; 
    [SerializeField] float m_MaxDamage = 100f;                  
    [SerializeField] float m_ExplosionForce = 1000f;            
    [SerializeField] float m_MaxLifeTime = 2f;                  
    [SerializeField] float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    /// <summary>
    /// Find all the tanks in an area around the shell and damage them.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Collider[] _colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < _colliders.Length; i++)
        {
            // Find rigidbody object and add force to it
            Rigidbody _targetRigidbody = _colliders[i].GetComponent<Rigidbody>();
            if (!_targetRigidbody)
                continue;
            _targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            // Find tank and apply damage to it
            TankHealth _targetHealth = _targetRigidbody.GetComponent<TankHealth>();
            if (!_targetHealth)
                continue;
            _targetHealth.TakeDamage(CalculateDamage(_targetRigidbody.position));
        }

        // Play audio and effects
        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        Destroy(gameObject);
    }

    /// <summary>
    /// Calculate the amount of damage a target should take based on it's position.
    /// </summary>
    private float CalculateDamage(Vector3 _targetPosition)
    {
        // Find the distance between the target and the explosion
        Vector3 _explosionToTarget = _targetPosition - transform.position;
        float _explosionDistance = _explosionToTarget.magnitude;

        // Calculate the magnitude of impact to target
        float _relativeDistance = (m_ExplosionRadius - _explosionDistance) / m_ExplosionRadius;
        float _damage = m_MaxDamage * _relativeDistance;

        _damage = Mathf.Max(0f, _damage);

        return _damage;
    }
}