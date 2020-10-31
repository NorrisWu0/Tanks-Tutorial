using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float m_DampTime = 0.2f;
    [SerializeField] float m_ScreenEdgeBuffer = 4f;
    [SerializeField] float m_MinSize = 6.5f;                  
    //[HideInInspector]
    public Transform[] targets; 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        MoveRig();
        ZoomCamera();
    }

    /// <summary>
    /// Smooth align camera rig to desired position 
    /// </summary>
    private void MoveRig()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    /// <summary>
    /// Find the point between targets and assign it to desired position for camera to align to.
    /// </summary>
    private void FindAveragePosition()
    {
        Vector3 _averagePos = new Vector3();
        int _numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            _averagePos += targets[i].position;
            _numTargets++;
        }

        if (_numTargets > 0)
            _averagePos /= _numTargets;

        _averagePos.y = transform.position.y;

        m_DesiredPosition = _averagePos;
    }

    /// <summary>
    /// Change size of the orphographic camera to zoom in/out the view as required
    /// </summary>
    private void ZoomCamera()
    {
        float _requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, _requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    /// <summary>
    ///  
    /// </summary>
    private float FindRequiredSize()
    {
        Vector3 _desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float _size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            Vector3 _targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            Vector3 _desiredPosToTarget = _targetLocalPos - _desiredLocalPos;

            _size = Mathf.Max (_size, Mathf.Abs (_desiredPosToTarget.y));

            _size = Mathf.Max (_size, Mathf.Abs (_desiredPosToTarget.x) / m_Camera.aspect);
        }
        
        _size += m_ScreenEdgeBuffer;

        _size = Mathf.Max(_size, m_MinSize);

        return _size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}