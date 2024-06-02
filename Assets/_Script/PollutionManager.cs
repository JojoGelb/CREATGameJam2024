using UnityEngine;

namespace _Script
{
    public class PollutionManager : MonoBehaviour
    {
        private RenderTexture renderTexture; // La RenderTexture à modifier

        private Texture2D texture2D; // La texture 2D utilisée pour lire et écrire les pixels
        private Renderer objectRenderer; // Le Renderer de l'objet

        private int frame = 0;

        void Start()
        {
            renderTexture = GetComponent<Paintable>().getSupport();
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            objectRenderer = GetComponent<Renderer>();
        }

        void Update()
        {
            RenderTexture currentActiveRT = RenderTexture.active;

            frame++;
            if (frame % 10 != 0)
            {
                return;
            }

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            ModifyTexture(texture2D);
            ApplyTexture();

            RenderTexture.active = currentActiveRT;
        }

        void ModifyTexture(Texture2D texture)
        {
            // Exemple simple de modification des pixels : inverser les couleurs
            Color[] pixels = texture.GetPixels();
            //for (int i = 0; i < pixels.Length; i++)
            //{
            //    pixels[i] = new Color(1 - pixels[i].r, 1 - pixels[i].g, 1 - pixels[i].b, pixels[i].a);
            //}








            texture.SetPixels(pixels);
            texture.Apply();
        }

        void ApplyTexture()
        {
            //objectRenderer.material.SetTexture("_MaskTexture", texture2D);
            RenderTexture.active = renderTexture;
            Graphics.Blit(texture2D, renderTexture);
            Graphics.Blit(texture2D, GetComponent<Paintable>().getSupport());
            Graphics.Blit(texture2D, GetComponent<Paintable>().getExtend());
            RenderTexture.active = null;
        }
    }
}