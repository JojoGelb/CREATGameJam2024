Shader "MonkeyJAM/PaintEraser"{

    Properties{
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
        _EraseFeather ("Feather", Float) = 0.8
        _Radius_Radius ("Radius", Float) = 0.05
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
            float _EraseFeather;

            struct appdata{
                float4 vertex : POSITION;   // Position du pixel à calculer
				float2 uv : TEXCOORD0;      // Coordonnées de la texture
            };

            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

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
                //if(distance(i.worldPos, _PainterPosition) < _Radius)
                //{
                //    return float4(0, 0, 0, 0);
                //}
                //return tex2D(_MainTex, i.uv);

                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate the distance from the center
                float dist = distance(i.worldPos, _PainterPosition);

                // Calculate the blend factor based on the distance
                float blend = smoothstep(_Radius, _Radius + _EraseFeather, dist);

                // Blend between the texture color and the erase color
                col = lerp(float4(0,0,0,0), col, blend);

                return col;
            }
            ENDCG
        }
    }
}