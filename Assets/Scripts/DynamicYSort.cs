using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    [Tooltip("Uses a given transform as a marker for the Y offset.")]
    public Transform offsetMarker;
    
    [Tooltip("Y offset to be used in the absence of an offset marker.")]
    public float yOffset;
    
    private Renderer _renderer;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        yOffset = offsetMarker == null ? yOffset : offsetMarker.position.y - transform.position.y;
    }

    private void Update()
    {
        _renderer.sortingOrder = transform.GetYSortingOrder(yOffset);       
    }
}