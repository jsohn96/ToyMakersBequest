// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:1,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32915,y:32743,varname:node_2865,prsc:2|emission-1605-OUT;n:type:ShaderForge.SFN_ScreenPos,id:5494,x:31518,y:32484,varname:node_5494,prsc:2,sctp:0;n:type:ShaderForge.SFN_SceneColor,id:1902,x:32268,y:32621,varname:node_1902,prsc:2|UVIN-2686-OUT;n:type:ShaderForge.SFN_RemapRange,id:3351,x:31749,y:32484,varname:node_3351,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-5494-UVOUT;n:type:ShaderForge.SFN_Parallax,id:5348,x:32042,y:32621,varname:node_5348,prsc:2|UVIN-3351-OUT,HEI-517-OUT,DEP-3315-OUT,REF-9829-OUT;n:type:ShaderForge.SFN_Slider,id:517,x:31555,y:32683,ptovrint:False,ptlb:node_517node_517,ptin:_node_517node_517,varname:node_517,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3268511,max:1;n:type:ShaderForge.SFN_Slider,id:3315,x:31529,y:32774,ptovrint:False,ptlb:node_517node_517_copy,ptin:_node_517node_517_copy,varname:_node_517node_517_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8774086,max:1;n:type:ShaderForge.SFN_Slider,id:9829,x:31555,y:32880,ptovrint:False,ptlb:node_517node_517_copy_copy,ptin:_node_517node_517_copy_copy,varname:_node_517node_517_copy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2405839,max:1;n:type:ShaderForge.SFN_LightVector,id:6335,x:31584,y:33083,varname:node_6335,prsc:2;n:type:ShaderForge.SFN_ViewReflectionVector,id:2850,x:31584,y:33226,varname:node_2850,prsc:2;n:type:ShaderForge.SFN_Dot,id:4236,x:31799,y:33150,varname:node_4236,prsc:2,dt:1|A-6335-OUT,B-2850-OUT;n:type:ShaderForge.SFN_Power,id:98,x:32059,y:33154,varname:node_98,prsc:2|VAL-4236-OUT,EXP-4475-OUT;n:type:ShaderForge.SFN_Exp,id:4475,x:31799,y:33353,varname:node_4475,prsc:2,et:1|IN-7811-OUT;n:type:ShaderForge.SFN_Slider,id:7811,x:31383,y:33387,ptovrint:False,ptlb:specular,ptin:_specular,varname:node_7811,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:3.255144,max:8;n:type:ShaderForge.SFN_Multiply,id:73,x:32395,y:33158,varname:node_73,prsc:2|A-98-OUT,B-7147-OUT;n:type:ShaderForge.SFN_Add,id:1605,x:32631,y:32824,varname:node_1605,prsc:2|A-1902-RGB,B-73-OUT,C-7974-OUT;n:type:ShaderForge.SFN_Slider,id:7147,x:31994,y:33380,ptovrint:False,ptlb:glossyIntensity,ptin:_glossyIntensity,varname:node_7147,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:4;n:type:ShaderForge.SFN_Color,id:9531,x:32268,y:32978,ptovrint:False,ptlb:node_9531,ptin:_node_9531,varname:node_9531,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2027465,c2:0.5514706,c3:0.392741,c4:1;n:type:ShaderForge.SFN_NormalVector,id:8567,x:31945,y:32808,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:8356,x:31945,y:32961,varname:node_8356,prsc:2;n:type:ShaderForge.SFN_Dot,id:1253,x:32101,y:32862,varname:node_1253,prsc:2,dt:0|A-8567-OUT,B-8356-OUT;n:type:ShaderForge.SFN_OneMinus,id:5239,x:32268,y:32805,varname:node_5239,prsc:2|IN-1253-OUT;n:type:ShaderForge.SFN_Multiply,id:7974,x:32454,y:32873,varname:node_7974,prsc:2|A-5239-OUT,B-9531-RGB;n:type:ShaderForge.SFN_Tex2d,id:7836,x:32121,y:32084,ptovrint:False,ptlb:node_7836,ptin:_node_7836,varname:node_7836,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:043ef571cf5c0407197215664c13cbb7,ntxv:3,isnm:True|UVIN-6143-OUT;n:type:ShaderForge.SFN_Time,id:6565,x:31676,y:32314,varname:node_6565,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2986,x:31853,y:32156,varname:node_2986,prsc:2|A-9723-OUT,B-6565-T;n:type:ShaderForge.SFN_Add,id:2686,x:32150,y:32477,varname:node_2686,prsc:2|A-440-OUT,B-5348-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:1370,x:32351,y:32131,varname:node_1370,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7836-RGB;n:type:ShaderForge.SFN_Add,id:6143,x:32010,y:32289,varname:node_6143,prsc:2|A-2986-OUT,B-3351-OUT;n:type:ShaderForge.SFN_Vector4Property,id:3946,x:31555,y:32089,ptovrint:False,ptlb:Distort,ptin:_Distort,varname:node_3946,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:1,v3:0.01,v4:0;n:type:ShaderForge.SFN_ComponentMask,id:9723,x:31701,y:32034,varname:node_9723,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3946-XYZ;n:type:ShaderForge.SFN_Multiply,id:440,x:32341,y:32354,varname:node_440,prsc:2|A-1370-OUT,B-3946-Z;proporder:517-9829-3315-7811-7147-9531-7836-3946;pass:END;sub:END;*/

Shader "Shader Forge/waterTest01" {
    Properties {
        _node_517node_517 ("node_517node_517", Range(0, 1)) = 0.3268511
        _node_517node_517_copy_copy ("node_517node_517_copy_copy", Range(0, 1)) = 0.2405839
        _node_517node_517_copy ("node_517node_517_copy", Range(0, 1)) = 0.8774086
        _specular ("specular", Range(1, 8)) = 3.255144
        _glossyIntensity ("glossyIntensity", Range(0, 4)) = 0
        _node_9531 ("node_9531", Color) = (0.2027465,0.5514706,0.392741,1)
        _node_7836 ("node_7836", 2D) = "bump" {}
        _Distort ("Distort", Vector) = (1,1,0.01,0)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _node_517node_517;
            uniform float _node_517node_517_copy;
            uniform float _node_517node_517_copy_copy;
            uniform float _specular;
            uniform float _glossyIntensity;
            uniform float4 _node_9531;
            uniform sampler2D _node_7836; uniform float4 _node_7836_ST;
            uniform float4 _Distort;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 projPos : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float4 node_6565 = _Time;
                float2 node_3351 = ((sceneUVs * 2 - 1).rg*0.5+0.5);
                float2 node_6143 = ((_Distort.rgb.rg*node_6565.g)+node_3351);
                float3 _node_7836_var = UnpackNormal(tex2D(_node_7836,TRANSFORM_TEX(node_6143, _node_7836)));
                float3 emissive = (tex2D( _GrabTexture, ((_node_7836_var.rgb.rg*_Distort.b)+(_node_517node_517_copy*(_node_517node_517 - _node_517node_517_copy_copy)*mul(tangentTransform, viewDirection).xy + node_3351).rg)).rgb+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_specular))*_glossyIntensity)+((1.0 - dot(i.normalDir,viewDirection))*_node_9531.rgb));
                float3 finalColor = emissive;
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma only_renderers d3d9 d3d11 glcore gles metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _node_517node_517;
            uniform float _node_517node_517_copy;
            uniform float _node_517node_517_copy_copy;
            uniform float _specular;
            uniform float _glossyIntensity;
            uniform float4 _node_9531;
            uniform sampler2D _node_7836; uniform float4 _node_7836_ST;
            uniform float4 _Distort;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float3 finalColor = 0;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float _node_517node_517;
            uniform float _node_517node_517_copy;
            uniform float _node_517node_517_copy_copy;
            uniform float _specular;
            uniform float _glossyIntensity;
            uniform float4 _node_9531;
            uniform sampler2D _node_7836; uniform float4 _node_7836_ST;
            uniform float4 _Distort;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                float4 projPos : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_6565 = _Time;
                float2 node_3351 = ((sceneUVs * 2 - 1).rg*0.5+0.5);
                float2 node_6143 = ((_Distort.rgb.rg*node_6565.g)+node_3351);
                float3 _node_7836_var = UnpackNormal(tex2D(_node_7836,TRANSFORM_TEX(node_6143, _node_7836)));
                o.Emission = (tex2D( _GrabTexture, ((_node_7836_var.rgb.rg*_Distort.b)+(_node_517node_517_copy*(_node_517node_517 - _node_517node_517_copy_copy)*mul(tangentTransform, viewDirection).xy + node_3351).rg)).rgb+(pow(max(0,dot(lightDirection,viewReflectDirection)),exp2(_specular))*_glossyIntensity)+((1.0 - dot(i.normalDir,viewDirection))*_node_9531.rgb));
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
