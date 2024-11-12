using UnityEngine;

public class YSort : MonoBehaviour
{
    [Tooltip("Uses a given transform as a marker for the Y offset.")]
    public Transform offsetMarker;
    
    [Tooltip("Y offset to be used in the absence of an offset marker.")]
    public float yOffset;

    private void Start()
    {
        yOffset = offsetMarker == null ? yOffset : offsetMarker.position.y - transform.position.y;
        GetComponent<Renderer>().sortingOrder = transform.GetYSortingOrder(yOffset);
    }
}