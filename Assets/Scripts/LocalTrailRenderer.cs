using UnityEngine;

public class LocalTrailRenderer : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private Transform _parentTransform;
    private Vector3 _localPositionOffset;

    void Start()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        
    }

    void Update()
    {
        
    }
}