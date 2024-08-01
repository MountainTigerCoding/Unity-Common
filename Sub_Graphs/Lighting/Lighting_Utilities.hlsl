//------------------------------------------------------------------------------------------------------
// Main Light
//------------------------------------------------------------------------------------------------------

/*
- Obtains the Direction, Color and Distance Atten for the Main Light.
- (DistanceAtten is either 0 or 1 for directional light, depending if the light is in the culling mask or not)
*/
void MainLight_half (float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
	Direction = half3(0.5, 0.5, 0);
	Color = 1;
	DistanceAtten = 1;
	ShadowAtten = 1;
#else
	half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
	Light mainLight = GetMainLight(shadowCoord);
	Direction = mainLight.direction;
	Color = mainLight.color;
	DistanceAtten = mainLight.distanceAttenuation;
	
#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
	ShadowAtten = 1.0h;
#endif
	ShadowAtten = mainLight.shadowAttenuation;
#endif
}

/*
- Obtains the Light Cookie assigned to the Main Light
- (For usage, You'd want to Multiply the result with your Light Colour)
- To work in an Unlit Graph, requires keywords :
	- Boolean Keyword, Global Multi-Compile "_LIGHT_COOKIES"
*/
void MainLightCookie_half (float3 WorldPos, out half Cookie)
{
	Cookie = 1;
	#if defined(_LIGHT_COOKIES)
        Cookie = SampleMainLightCookie(WorldPos).x;
    #endif
}

//------------------------------------------------------------------------------------------------------
// Main Light Shadows
//------------------------------------------------------------------------------------------------------

/*
- Samples the Shadowmap for the Main Light, based on the World Position passed in. (Position node)
- For shadows to work in the Unlit Graph, the following keywords must be defined in the blackboard :
	- Enum Keyword, Global Multi-Compile "_MAIN_LIGHT", with entries :
		- "SHADOWS"
		- "SHADOWS_CASCADE"
		- "SHADOWS_SCREEN"
	- Boolean Keyword, Global Multi-Compile "_SHADOWS_SOFT"
- For a PBR/Lit Graph, these keywords are already handled for you.
*/
/*
void MainLightShadows_half (half3 WorldPos, out half ShadowAtten)
{
	MainLightShadows_half(WorldPos, half4(1, 1, 1, 1), ShadowAtten);
}
*/

void MainLightShadows_half (half WorldPos, half4 Shadowmask, out half ShadowAtten)
{
	#ifdef SHADERGRAPH_PREVIEW
		ShadowAtten = 1;
	#else
		#if defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
			float4 shadowCoord = ComputeScreenPos(TransformWorldToHClip(WorldPos));
		#else
			float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
		#endif
		ShadowAtten = MainLightShadow(shadowCoord, WorldPos, Shadowmask, _MainLightOcclusionProbes);
	#endif
}

//------------------------------------------------------------------------------------------------------
// Main Light Layer Test
//------------------------------------------------------------------------------------------------------

#ifndef SHADERGRAPH_PREVIEW
	#if UNITY_VERSION < 202220
	/*
	GetMeshRenderingLayer() is only available in 2022.2+
	Previous versions need to use GetMeshRenderingLightLayer()
	*/
	uint GetMeshRenderingLayer(){
		return GetMeshRenderingLightLayer();
	}
	#endif
#endif

//------------------------------------------------------------------------------------------------------
// Additional Lights
//------------------------------------------------------------------------------------------------------

void AdditionalLights_float (float3 WorldPosition, float3 WorldNormal, out float3 Color, out float3 Direction, out float ShadowAttenuation)
{
	Color = 0;
    Direction = 0;
    ShadowAttenuation = 0;

#ifndef SHADERGRAPH_PREVIEW
	uint pixelLightCount = GetAdditionalLightsCount();
	uint meshRenderingLayers = GetMeshRenderingLayer();

	#if USE_FORWARD_PLUS
	for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++) {
		FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK
		Light light = GetAdditionalLight(lightIndex, WorldPosition);
	    #ifdef _LIGHT_LAYERS
		if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
	    #endif
		{
			// Add the attenuated light color
			Color += light.color * (light.distanceAttenuation * light.shadowAttenuation);
            Direction += light.direction;
            ShadowAttenuation += light.shadowAttenuation;
        }
	}
#endif

	// For Foward+ the LIGHT_LOOP_BEGIN macro will use inputData.normalizedScreenSpaceUV, inputData.positionWS, so create that:
	InputData inputData = (InputData)0;
	float4 screenPos = ComputeScreenPos(TransformWorldToHClip(WorldPosition));
	inputData.normalizedScreenSpaceUV = screenPos.xy / screenPos.w;
	inputData.positionWS = WorldPosition;

	LIGHT_LOOP_BEGIN(pixelLightCount)
		Light light = GetAdditionalLight(lightIndex, WorldPosition);
	#ifdef _LIGHT_LAYERS
		if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
	#endif
		{
			// Add the attenuated light color
			Color +=  light.color * (light.distanceAttenuation * light.shadowAttenuation);
            Direction += light.direction;
            ShadowAttenuation += light.shadowAttenuation;
        }
	LIGHT_LOOP_END
#endif
}

/* Alternative method to above. NOT TESTED
void AdditionalLights_half(half3 PositionWS, half3 NormalWS, out half3 Diffuse)
{
	half3 diffuseColor = 0;

#ifndef SHADERGRAPH_PREVIEW
NormalWS = normalize(NormalWS);
	int pixelLightCount = GetAdditionalLightsCount();
	for (int i = 0; i < pixelLightCount; ++i)
	{
		Light light = GetAdditionalLight(i, PositionWS);
		half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
		diffuseColor += LightingLambert(attenuatedLightColor, light.direction, NormalWS);
	}
#endif

	Diffuse = diffuseColor;
}
*/

//------------------------------------------------------------------------------------------------------
// Pseudo Subsurface Scattering
//------------------------------------------------------------------------------------------------------

void PseudoSubsurface_half (half3 PositionWS, half3 NormalWS, half3 SSRadius, half3 ShadowResponse, out half3 ssAmount)
{
#ifdef SHADERGRAPH_PREVIEW
	half3 color = half3(0,0,0);
	half3 atten = 1;
	half dir = half3(0.707, 0, 0.707);

#else
	//half4 shadowCoord = TransformWorldToShadowCoord(PositionWS);
	//Light mainLight = GetMainLight(shadowCoord);

	half3 dir = 0;
	half3 color = 0;
	half distAtten = 0;
	half atten = 0;
	MainLight_half(PositionWS, dir, color, distAtten, atten);

	half cookie = 0;
	MainLightCookie_half(PositionWS, cookie);
	atten *= cookie;
#endif

    half NdotL = dot(NormalWS, -1 * dir);
    half alpha = SSRadius;
    half theta_m = acos(-alpha); // boundary of the lighting function

    half theta = max(0, NdotL + alpha) - alpha;
    half normalizer = (2 + alpha) / (2 * (1 + alpha));
    half wrapped  = (pow(abs( ((theta + alpha) / (1 + alpha)) ), 1 + alpha)) * normalizer;
	half shadow = lerp (1, atten, ShadowResponse);
    ssAmount = abs(color * shadow  * wrapped);
}