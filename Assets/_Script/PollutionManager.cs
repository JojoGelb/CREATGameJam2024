using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using _Script.Utilities;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace _Script
{
    public class PollutionManager : Singleton<PollutionManager>
    {
        public Color PollutionColor;

        private RenderTexture renderTexture; // La RenderTexture à modifier
        private Texture2D texture2D; // La texture 2D utilisée pour lire et écrire les pixels

        public int FramesBetweenDilatationPass = 150;


        // Tableau utilisé pour stocker des données de la propagation de pollution
        private ushort[] dataArray;

        private int frame = 0;

        void Start()
        {
            renderTexture = GetComponent<Paintable>().getSupport();
            texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, 6, false);

            dataArray = new ushort[renderTexture.width * renderTexture.height];
        }

        void Update()
        {
            RenderTexture currentActiveRT = RenderTexture.active;

            frame++;

            if (frame % 20 != 0 || frame % FramesBetweenDilatationPass != 0)
            {
                RenderTexture.active = renderTexture;
                texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            }

            if (frame % FramesBetweenDilatationPass != 0)
            {
                return;
            }

            DilutePollution();

            texture2D.Apply();
            Graphics.Blit(texture2D, renderTexture);

            RenderTexture.active = currentActiveRT;
        }

        void DilutePollution()
        {
            // DEBUG - MESUREMENT START
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            // -----------------
            // DATA SETUP
            // -----------------
            var pixels = texture2D.GetPixelData<Color32>(0);
            ushort l = (ushort)Math.Sqrt(pixels.Length);

            List<OilPipeGameplay> factories = OilPipeManager.Instance?.GetOliPipesInGame();
            byte NO_VISITED = 0, ALREADY_PROCESSED = 1, NEWLY_POLLUTED = 2, IN_QUEUE = 3;

            var pollutionColor = new Color32(
                (byte)(PollutionColor.r * 255),
                (byte)(PollutionColor.g * 255),
                (byte)(PollutionColor.b * 255),
                255
            );

            // -----------------
            // ALGO
            // -----------------
            if (factories == null) return;

            var options = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            Parallel.ForEach(factories, options, factory =>
                {
                    Queue<ushort> toVisit_x = new Queue<ushort>();
                    Queue<ushort> toVisit_y = new Queue<ushort>();

                    Vector2 fact = factory.GetCoordsOnPlanetTexture;
                    if (dataArray[(int)(fact.x + l * fact.y)] == ALREADY_PROCESSED)
                        return;
                    toVisit_x.Enqueue((ushort)fact.x);
                    toVisit_y.Enqueue((ushort)fact.y);

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
                            pixels[x + l * y] = pollutionColor;
                        }

                        if (isTansparent)
                        {
                            dataArray[x + l * y] = NEWLY_POLLUTED;
                        }
                        else
                        {
                            // Pollution
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

                    // ------------------------
                    // SUB FUNCTION
                    // ------------------------
                    void HandleLinkedCell(ushort x, ushort y)
                    {
                        ref ushort state = ref dataArray[x + l * y];
                        if (state == NO_VISITED)
                        {
                            toVisit_x.Enqueue(x);
                            toVisit_y.Enqueue(y);
                            state = IN_QUEUE;
                        }
                    }
                }
            );


            // ------------------------
            // DATA CLEANUP
            // ------------------------
            Array.Clear(dataArray, 0, dataArray.Length);


            // DEBUG - MESUREMENT STOP
            //stopwatch.Stop();
            //Debug.Log("DilutePollution execution time: " + stopwatch.ElapsedMilliseconds + " ms");
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