Shader "Custom/Unit_Lava_Simple"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 0.5, 0.2, 1) // Un color base más claro y vibrante
        _EmissionColor("Emission Color", Color) = (1, 0.7, 0.3, 1) // Un color de emisión más brillante
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Speed("Speed", Float) = 1.0
        _DistortionStrength("Distortion Strength", Float) = 0.1
        _EmissionPower("Emission Power", Float) = 2.0 // Aumentamos la potencia de emisión para un brillo más intenso
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }
            LOD 300

            Pass
            {
                Name "ForwardLit"
                Tags { "LightMode" = "UniversalForward" }
                HLSLPROGRAM
                #pragma prefer_hlslcc gles
                #pragma vertex vert
                #pragma fragment frag

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normalOS : NORMAL;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normalWS : TEXCOORD1;
                };

                // Variables
                float4 _BaseColor;
                float4 _EmissionColor;
                float _Speed;
                float _DistortionStrength;
                float _EmissionPower;
                sampler2D _NoiseTex;
                sampler2D _NormalMap;

                // Vertex function
                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS); // Mantener float4
                    OUT.uv = IN.uv;
                    OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                    return OUT;
                }

                // Fragment function
                half4 frag(Varyings IN) : SV_Target
                {
                    // Animación del desplazamiento en las UV
                    float time = _Time.y * _Speed;
                    float2 uvOffset = IN.uv + float2(sin(time), cos(time)) * _DistortionStrength;

                    // Textura de ruido para distorsión
                    float noise = tex2D(_NoiseTex, uvOffset).r;

                    // Aplicar un mapa de normales
                    float3 normalMap = UnpackNormal(tex2D(_NormalMap, IN.uv));

                    // Color base de la lava (menos oscurecido por el ruido)
                    half4 baseColor = _BaseColor * (1.0 - (noise * 0.5)); // Reducir el oscurecimiento por el ruido

                    // Color de la emisión con mayor control y brillo
                    half4 emission = _EmissionColor * pow(noise, _EmissionPower);

                    // Obtener la luz principal de la escena en URP
                    Light mainLight = GetMainLight();
                    half3 lightDir = normalize(mainLight.direction);
                    half3 normal = normalize(IN.normalWS + normalMap);

                    // Luz simple para interacción con la superficie de lava
                    float NdotL = max(dot(normal, lightDir), 0.0);
                    baseColor.rgb *= (NdotL * 0.6 + 0.4); // Mantener algo de luz incluso en áreas menos iluminadas

                    // Mezcla de la luz con la emisión
                    return baseColor + emission;
                }
                ENDHLSL
            }
        }
            FallBack "Diffuse"
}
