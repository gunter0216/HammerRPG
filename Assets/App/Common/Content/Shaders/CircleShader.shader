Shader "Custom/CircleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
//        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                // col.rgb = 1 - col.rgb;
                return col;
            }

            void createTriangle(v2f input, float x, float yOffset, float uvX, float uvY, inout TriangleStream<v2f> output)
            {
                v2f element;
                float4 vertex = input.vertex;
                vertex.x = x;
                vertex.y += yOffset;
                element.vertex = vertex;
                element.uv = float2(uvX, uvY);
                output.Append(element);
            }
            
            [maxvertexcount(120)]
            void geom(triangle v2f input[3] : SV_POSITION, inout TriangleStream<v2f> output)
            {
                const int countPieces = 10;
                
                if (input[0].uv.x == 0 && input[0].uv.y == 0)
                {
                    float startYOffset = -0.5;
                    float yOffsetOffset = startYOffset / countPieces;
                    float firstXMin= input[0].vertex.x;
                    float firstXMax = (firstXMin + input[2].vertex.x) / 2;
                    float xOffset = (firstXMax - firstXMin) / countPieces;
                    float xUvOffset = 0.5 / countPieces;

                    for (int i = 0; i < countPieces; ++i)
                    {
                        float temp = i * yOffsetOffset;
                        float minYOffset = -(temp) * temp;
                        float maxYOffset = minYOffset + yOffsetOffset;
                        float xMin = firstXMin + i * xOffset;
                        float xMax = xMin + xOffset;
                        float xUvMin = i * xUvOffset;
                        float xUvMax = xUvMin + xUvOffset;
                        
                        createTriangle(input[0], xMin, minYOffset, xUvMin, 0, output);
                        createTriangle(input[1], xMin, minYOffset, xUvMin, 1, output);
                        createTriangle(input[2], xMax, maxYOffset, xUvMax, 1, output);

                        output.RestartStrip();

                        createTriangle(input[0], xMin, minYOffset, xUvMin, 0, output);
                        createTriangle(input[2], xMax, maxYOffset, xUvMax, 1, output);
                        createTriangle(input[0], xMax, maxYOffset, xUvMax, 0, output);

                        output.RestartStrip();
                    }
                }
                else
                {
                    float yOffset = -0.2;
                    float xMax = input[0].vertex.x;
                    float xMin = (input[2].vertex.x + xMax) / 2;

                    createTriangle(input[0], xMax, 0, 1, 1, output);
                    
                    createTriangle(input[1], xMax, 0, 1, 0, output);
                    
                    createTriangle(input[2], xMin, yOffset, 0.5, 0, output);
                    
                    output.RestartStrip();

                    createTriangle(input[0], xMax, 0, 1, 1, output);
                    
                    createTriangle(input[2], xMin, yOffset, 0.5, 0, output);
                    
                    createTriangle(input[0], xMin, yOffset, 0.5, 1, output);
                    
                    output.RestartStrip();
                }
            }
            
            ENDCG
        }
    }
}
