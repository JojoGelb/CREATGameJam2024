using System.Collections.Generic;
using _Script.Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace _Script
{
    public class PollutionManager : Singleton<PollutionManager>
    {
        private RenderTexture renderTexture; // La RenderTexture à modifier

        private Texture2D texture2D; // La texture 2D utilisée pour lire et écrire les pixels

        private int frame = 0;
        private int UNKNOWN = 3, POLLUTED = 0, CLEAN = 1, NEWLY_POLLUTED = 2;

        void Start()
        {
            renderTexture = GetComponent<Paintable>().getSupport();
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, 6, false);
        }

        void Update()
        {
            RenderTexture currentActiveRT = RenderTexture.active;

            frame++;
            if (frame % 100 != 0)
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
            var pixels1D = texture.GetPixelData<Color32>(0);
            //for (int i = 0; i < pixels.Length; i++)
            //{
            //    pixels[i] = new Color(1 - pixels[i].r, 1 - pixels[i].g, 1 - pixels[i].b, pixels[i].a);
            //}



            List<OilPipeGameplay> factories = OilPipeManager.Instance.GetOliPipesInGame();
            List<Vector2> factories2D = new List<Vector2>();

            foreach(OilPipeGameplay factory in factories)
            {
                //factories2D.Add(UVCoordinateFinder.FindUVPoint(factory.transform));
            }




            // 1D to 2D
            //Color[,] pixels2D = new Color[texture.width, texture.height];
            //int[,] visitStates = new int[texture.width, texture.height];
            //
            //for (int y = 0; y < texture.height; y++)
            //{
            //    for (int x = 0; x < texture.width; x++)
            //    {
            //        pixels2D[x, y] = pixels1D[y * texture.width + x];
            //    }
            //}
            //// -----------------------------------------------
            //Queue<int> toVisit;












            // -----------------------------------------------

            // 2D to 1D
            //for (int y = 0; y < texture.height; y++)
            //{
            //    for (int x = 0; x < texture.width; x++)
            //    {
            //        pixels1D[y * texture.width + x] = pixels2D[x, y];
            //    }
            //}
            //texture.SetPixelData(pixels1D);
            //texture.SetPixels(pixels1D);
            texture.Apply();
        }

        void VisitCell(int x, int y, Queue<int> toVisit, int[,] visitStates)
        {
        }

        int[] getNeighours(int x, int y)
        {
            return new[] { 1};
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

        public float GetPercentageTextureFilled()
        {
            if (texture2D == null) return 0;
            //Color[] pixels = texture2D.GetPixels();

            var pixels = texture2D.GetPixelData<Color32>(5);

            float pixelColored = 0;

            Color c = new Color(0, 0, 0, 0);

            for (int i = 0; i < pixels.Length; i++)
            {
                if (c != pixels[i])
                {
                    pixelColored++;
                }
            }

            return pixelColored / pixels.Length;
        }
    }
}