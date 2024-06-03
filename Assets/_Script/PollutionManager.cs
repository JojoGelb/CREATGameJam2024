using System;
using System.Collections.Generic;
using _Script.Utilities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Script
{
    public class PollutionManager : Singleton<PollutionManager>
    {
        public Color PollutionColor;

        private RenderTexture renderTexture; // La RenderTexture à modifier
        private Texture2D texture2D; // La texture 2D utilisée pour lire et écrire les pixels

        // Tableau utilisé pour stocker des données de la propagation de pollution
        private ushort[] dataArray;

        private int frame = 0;

        void Start()
        {
            renderTexture = GetComponent<Paintable>().getSupport();
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, 6, false);

            dataArray = new ushort[renderTexture.width * renderTexture.height];
        }

        void Update()
        {
            RenderTexture currentActiveRT = RenderTexture.active;

            frame++;

            if(frame % 20 != 0) {
                RenderTexture.active = renderTexture;
                texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            }

            if (frame % 200 != 0)
            {
                return;
            }


            //texture2D.Apply();

            DilutePollution();
            ApplyTexture();

            RenderTexture.active = currentActiveRT;
        }

        void DilutePollution()
        {
            // -----------------
            // DATA SETUP
            // -----------------
            var pixels = texture2D.GetPixelData<Color32>(0);
            ushort l = (ushort)Math.Sqrt(pixels.Length);

            List<OilPipeGameplay> factories = OilPipeManager.Instance?.GetOliPipesInGame();

            //var states = new byte[l, l];
            byte NO_VISITED = 0, ALREADY_PROCESSED = 1, NEWLY_POLLUTED = 2, IN_QUEUE = 3;

            Queue<ushort> toVisit_x = new Queue<ushort>();
            Queue<ushort> toVisit_y = new Queue<ushort>();

            // -----------------
            // ALGO
            // -----------------
            if (factories == null) return;
            foreach (OilPipeGameplay factory in factories)
            {
                Vector2 fact = GetTextureCoordsOnPlanet(factory.transform.position);
                //if (states[(ushort)fact.x, (ushort)fact.y] == ALREADY_PROCESSED)
                if (dataArray[(int)(fact.x + l * fact.y)] == ALREADY_PROCESSED)
                    continue;
                toVisit_x.Enqueue((ushort)fact.x);
                toVisit_y.Enqueue((ushort)fact.y);

                //Debug.Log(fact.x + ", " + fact.y);

                while (toVisit_x.Count != 0)
                {
                    ushort x = toVisit_x.Dequeue();
                    ushort y = toVisit_y.Dequeue();

                    // HANDLE CURRENT CELL
                    var color = pixels[x + l * y];

                    var isTansparent = color.a == 0;
                    var isSemiTransparent = color.a != 0 && color.a < 150;

                    if (isTansparent || isSemiTransparent)
                    {
                        // No pollution
                        pixels[x + l * y] = new Color32(
                            (byte)(PollutionColor.r * 255),
                            (byte)(PollutionColor.g * 255),
                            (byte)(PollutionColor.b * 255),
                            255
                        );
                    }

                    if (isTansparent)
                    {
                        //states[x, y] = NEWLY_POLLUTED;
                        dataArray[x + l * y] = NEWLY_POLLUTED;
                    }
                    else
                    {
                        // Pollution
                        //states[x, y] = ALREADY_PROCESSED;
                        dataArray[x + l * y] = ALREADY_PROCESSED;

                        // LEFT
                        ushort dx = (x == 0) ? (ushort)(l - 1) : (ushort)(x - 1);
                        HandleLinkedCell(dx, y);

                        // RIGHT
                        dx = (x == l - 1) ? (ushort)(0) : (ushort)(x + 1);
                        HandleLinkedCell(dx, y);

                        // TOP
                        ushort dy = (y == 0) ? (ushort)(l - 1) : (ushort)(y - 1);
                        HandleLinkedCell(x, dy);

                        // BOTTOM
                        dy = (y == l - 1) ? (ushort)(0) : (ushort)(y + 1);
                        HandleLinkedCell(x, dy);
                    }
                }
            }

            texture2D.Apply();

            // ------------------------
            // DATA CLEANUP
            // ------------------------
            Array.Clear(dataArray, 0, dataArray.Length);

            // ------------------------
            // SUB FUNCTION
            // ------------------------
            void HandleLinkedCell(ushort x, ushort y)
            {
                //if (states[x, y] == NO_VISITED)
                if (dataArray[x + l * y] == NO_VISITED)
                {
                    toVisit_x.Enqueue(x);
                    toVisit_y.Enqueue(y);
                    //states[x, y] = IN_QUEUE;
                    dataArray[x + l * y] = IN_QUEUE;
                }
            }
        }


        void ApplyTexture()
        {
            //GetComponent<Renderer>().material.SetTexture("_MaskTexture", texture2D);
            RenderTexture.active = renderTexture;
            Graphics.Blit(texture2D, renderTexture);
            Graphics.Blit(texture2D, GetComponent<Paintable>().getSupport());
            Graphics.Blit(texture2D, GetComponent<Paintable>().getExtend());
            Graphics.Blit(texture2D, GetComponent<Paintable>().getMask());
            Graphics.Blit(texture2D, GetComponent<Paintable>().getUVIslands());
            RenderTexture.active = null;
        }

        public float GetPercentageTextureFilled()
        {
            if (texture2D == null) return 0;

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

        public Color GetColorAtPosition(RaycastHit hit)
        {
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= texture2D.width;
            pixelUV.y *= texture2D.height;

            return texture2D.GetPixel((int)pixelUV.x, (int)pixelUV.y);
        }

        public Vector2 GetTextureCoordsOnPlanet(Vector3 position)
        {
            RaycastHit hit;
            if (!Physics.Raycast(position * 1.2f, -position, out hit, Mathf.Infinity, 1 << 7))
            {
                Debug.LogError("Planet Not found");
                return Vector2.zero;
            }

            Vector2 pixelUV = hit.textureCoord;
            return new Vector2((int)(pixelUV.x * texture2D.width), (int)(pixelUV.y * texture2D.height));
        }
    }
}