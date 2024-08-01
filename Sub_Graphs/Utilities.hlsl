float3 RotateAboutAxis_Degrees_half (float3 In, float3 Axis, float Rotation)
{
    Axis = normalize(Axis);

    Rotation = radians(Rotation);
    float s = sin(Rotation);
    float c = cos(Rotation);
    float one_minus_c = 1.0 - c;

    float3x3 rot_mat = 
    {   one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
        one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
        one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
    };
    return mul(rot_mat,  In);
}

half3 RotateVector_half (half3 direction, half3 byDirection)
{
    // Orientation
    half3 up = byDirection;
    half3 forward = float3(1, 0, 0);
    half3 right = normalize(cross(up, forward));
    forward = cross(right, up);

    half3x3 rotMatrix = float3x3(right, up, forward);
    return mul(direction, rotMatrix);
}