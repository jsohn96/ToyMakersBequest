// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33419,y:32337,varname:node_3138,prsc:2|emission-2539-OUT,custl-6439-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32469,y:32322,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2111,x:32535,y:32556,ptovrint:False,ptlb:cookie,ptin:_cookie,varname:node_2111,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:23359dc591fa74df0bc9950616a0edc1,ntxv:0,isnm:False|UVIN-6614-OUT;n:type:ShaderForge.SFN_Add,id:2539,x:32954,y:32431,varname:node_2539,prsc:2|A-7241-RGB,B-2111-RGB;n:type:ShaderForge.SFN_Tex2d,id:796,x:32011,y:32970,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:40477d77a019e49f6a6e00ca8ec610d8,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Time,id:3392,x:31734,y:32613,varname:node_3392,prsc:2;n:type:ShaderForge.SFN_Vector2,id:9562,x:31775,y:32831,varname:node_9562,prsc:2,v1:1,v2:0.1;n:type:ShaderForge.SFN_Multiply,id:8914,x:32074,y:32613,varname:node_8914,prsc:2|A-4169-OUT,B-9562-OUT;n:type:ShaderForge.SFN_Add,id:6614,x:32351,y:32556,varname:node_6614,prsc:2|A-8914-OUT,B-6531-UVOUT,C-5490-OUT;n:type:ShaderForge.SFN_TexCoord,id:6531,x:32011,y:32803,varname:node_6531,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Sin,id:4169,x:31908,y:32613,varname:node_4169,prsc:2|IN-3392-TSL;n:type:ShaderForge.SFN_Add,id:8102,x:32294,y:32840,varname:node_8102,prsc:2|A-6531-U,B-796-R;n:type:ShaderForge.SFN_Add,id:8734,x:32305,y:32972,varname:node_8734,prsc:2|A-6531-V,B-796-G,C-4169-OUT;n:type:ShaderForge.SFN_Add,id:6373,x:32552,y:32903,varname:node_6373,prsc:2|A-8102-OUT,B-8734-OUT;n:type:ShaderForge.SFN_Multiply,id:5490,x:32720,y:32950,varname:node_5490,prsc:2|A-6373-OUT,B-6078-OUT;n:type:ShaderForge.SFN_Slider,id:6078,x:32305,y:33173,ptovrint:False,ptlb:distortion power,ptin:_distortionpower,varname:node_6078,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.03999838,max:1;n:type:ShaderForge.SFN_LightAttenuation,id:8130,x:32730,y:32630,varname:node_8130,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6439,x:33032,y:32723,varname:node_6439,prsc:2|A-8130-OUT,B-5692-OUT;n:type:ShaderForge.SFN_Slider,id:5692,x:32631,y:32781,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_5692,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.04244395,max:1;proporder:7241-2111-796-6078-5692;pass:END;sub:END;*/

Shader "Shader Forge/water_caustics" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _cookie ("cookie", 2D) = "white" {}
        _noise ("noise", 2D) = "black" {}
        _distortionpower ("distortion power", Range(0, 1)) = 0.03999838
        _diffuse ("diffuse", Range(0, 1)) = 0.04244395
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _cookie; uniform float4 _cookie_ST;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _distortionpower;
            uniform float _diffuse;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 node_3392 = _Time;
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_8734 = (i.uv0.g+_noise_var.g+sin(node_3392.r));
                float2 node_6614 = ((sin(node_3392.r)*float2(1,0.1))+i.uv0+(((i.uv0.r+_noise_var.r)+node_8734)*_distortionpower));
                float4 _cookie_var = tex2D(_cookie,TRANSFORM_TEX(node_6614, _cookie));
                float3 node_2539 = (_Color.rgb+_cookie_var.rgb);
                float3 emissive = node_2539;
                float node_6439 = (attenuation*_diffuse);
                float3 finalColor = emissive + float3(node_6439,node_6439,node_6439);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _cookie; uniform float4 _cookie_ST;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _distortionpower;
            uniform float _diffuse;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_6439 = (attenuation*_diffuse);
                float3 finalColor = float3(node_6439,node_6439,node_6439);
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
