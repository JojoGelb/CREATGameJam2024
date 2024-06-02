using UnityEngine;

namespace _Script
{
    public class PollutionManager : MonoBehaviour
    {
        public Paintable Planet;

        // Pollution
        private RenderTexture renderTexture;
        Texture2D texture;


        void Start()
        {
            renderTexture = Planet.GetComponent<Paintable>().getMask();

            texture = new Texture2D(renderTexture.width, renderTexture.height);
        }

        void Update()
        {
            //Graphics.CopyTexture(RenderTexture.active, texture);
            RenderTexture.active = renderTexture;

            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            for (int i = 0; i < renderTexture.width * .2f; i++)
            for (int j = 0; j < renderTexture.height; j++)
            {
                texture.SetPixel(i, j, new Color(1, 0, 0, 1));
            }

            texture.Apply();
            RenderTexture.active = null;
        }
    }
}