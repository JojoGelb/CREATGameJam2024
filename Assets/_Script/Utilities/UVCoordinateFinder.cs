using UnityEngine;

namespace _Script.Utilities
{
    public class UVCoordinateFinder
    {
        public static Vector2 FindUVPoint(Transform targetPoint)
        {
            Vector3 worldPosition = targetPoint.position;

            Debug.Log("World position: " + worldPosition);
            // Lance un rayon vers la surface de l'objet
            if (Physics.Raycast(worldPosition * 1.5f, -worldPosition*1.5f, out RaycastHit hit))
            {
                Debug.Log(hit.textureCoord2);

                return hit.textureCoord2;
            }

            return Vector2.zero;
        }
    }
}