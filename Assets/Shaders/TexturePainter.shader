Shader "TNTC/TexturePainter"{   

    Properties{
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
    }

    SubShader{
        Cull Off ZWrite Off ZTest Off

        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float3 _PainterPosition;
            float _Radius;
            float _Hardness;
            float _Strength;
            float4 _PainterColor;
            float _PrepareUV;

            struct appdata{
                float4 vertex : POSITION;   // Position du pixel à calculer
				float2 uv : TEXCOORD0;      // Coordonnées de la texture
            };

            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            float mask(float3 position, float3 center, float radius, float hardness){
                float m = distance(center, position);
                return 1 - smoothstep(radius * hardness, radius, m);    
            }

            // Vertex shader : peux changer la position du Vertex
            v2f vert (appdata v){
                v2f o;
                // Position réelle du vertex de côté dans worldPos
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                // La texture ne change pas
                o.uv = v.uv;

                // Nouveau Vertex vide
				float4 uv = float4(0, 0, 0, 1);
                // Calcul de là on est sur l'UV map mise à plat en mappant de de 0 à 1
                uv.xy = float2(1, _ProjectionParams.x) * (v.uv.xy * float2( 2, 2) - float2(1, 1));
                // Le Vertex devient un point 2D sur l'UV map du mesh
				o.vertex = uv; 
                return o;
            }

            // Fragment/Pixel Shader :
            //  Retourne la couleur des faces entre les Vertices
            fixed4 frag (v2f i) : SV_Target{   
                if(_PrepareUV > 0 ){
                    return float4(0, 0, 1, 1);
                }         

                // Couleur du background de la texture
                float4 col = tex2D(_MainTex, i.uv);
                // Facteur d'aténuation
                float f = mask(i.worldPos, _PainterPosition, _Radius, _Hardness);
                float edge = f * _Strength;
                // On peint sur le masque avec la couleur du pinceau
                // avec une interpolation entre le background et le pinceau proportionnelle à la distance au bord
                return lerp(col, _PainterColor, edge);
            }
            ENDCG
        }
    }
}