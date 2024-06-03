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
            if (Physics.Raycast(worldPosition * 1.5f,
                    -worldPosition * 1.2f,
                    out RaycastHit hit,
                    Mathf.Infinity,
                    LayerMask.GetMask("Planet")))
            {
                Debug.Log("2D textures : " + hit.textureCoord);
                Debug.Log("2D textures : " + hit.textureCoord2);
                Debug.Log("Triangles idx : " + hit.triangleIndex);

                Vector2 textureCoord2 =
                    GetTextureCoord2(hit.collider as MeshCollider, hit.triangleIndex, hit.textureCoord);

                Debug.Log("Texture Coord2: " + textureCoord2);

                return hit.textureCoord2;
            }

            Debug.LogError("Error during detection UV Coords of oil factory");
            return Vector2.zero;
        }

        static Vector2 GetTextureCoord2(MeshCollider meshCollider, int triangleIndex, Vector2 textureCoord)
        {
            Mesh mesh = meshCollider.sharedMesh;
            int[] triangles = mesh.triangles;
            Vector2[]
                uv2s = mesh.uv2;

            int vertexIndex1 = triangles[triangleIndex * 3];
            int vertexIndex2 = triangles[triangleIndex * 3 + 1];
            int vertexIndex3 = triangles[triangleIndex * 3 + 2];

            Vector2 uv2_1 = uv2s[vertexIndex1];
            Vector2 uv2_2 = uv2s[vertexIndex2];
            Vector2 uv2_3 = uv2s[vertexIndex3];

            Vector2 uv2 = uv2_1 * textureCoord.x + uv2_2 * textureCoord.y +
                          uv2_3 * (1 - textureCoord.x - textureCoord.y);

            return uv2;
        }
    }
}