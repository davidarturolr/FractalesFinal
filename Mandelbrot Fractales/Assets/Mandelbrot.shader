Shader "Explorer/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("Area", vector) = (0,0,4,4)
        _Angle ("Angle", range(-3.1415,3.1415))=0
        _MaxIter("Iterations", range(4,1000))=255
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            float4 _Area;
            float _Angle, _MaxIter;
            sampler2D _MainTex;


            float2 rot(float2 p, float2 pivot, float a)
            {   float s = sin(a);
                float c = cos(a);
                p -= pivot; //para que el pivote este en 0,0
                p = float2(p.x*c-p.y*s,p.x*s+p.y*c); // lo rotamos
                p += pivot; 

                   return p;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                float2 c = _Area.xy + (i.uv -.5)*_Area.zw; //las coordenadas en los vectrores van xyzw por eso la w
                c = rot(c, _Area.xy, _Angle); //rotacion
                float2 z; // para saber donde está saltando el pixel en la pantalla
                float iter;
                for (iter = 0; iter < _MaxIter; iter ++){ //controla el numero de iteraciones
                z = float2(z.x*z.x-z.y*z.y, 2*z.x*z.y) + c ; // actualiza el valor c, basado en el valor pasado de c.                
                if (length(z)> 2) break; // la distancia del vector al origen 

            }
            float m = sqrt(iter/_MaxIter);
            float4 col = sin(float4(.3,.45,.61,1)*m*20)*.5+.5;
            return col;
            }
            ENDCG
        }
    }
}
