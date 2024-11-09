
    using UnityEngine;

    public static class TransformExtensions
    {
        public static int GetYSortingOrder(this Transform transform, float yOffset = 0.0f)
        {
            return -(int)((transform.position.y + yOffset) * 1000);
        }
    }
