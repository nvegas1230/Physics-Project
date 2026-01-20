using UnityEngine;
using System;

public class VectorComponent
{
    public float? opposite;
    public float? adjacent;
    public float? hypotenuse;
    public float? angle;

    // Constructor
    public VectorComponent(float? opp = null, float? adj = null, float? hyp = null, float? ang = null)
    {
        opposite = opp;
        adjacent = adj;
        hypotenuse = hyp;
        angle = ang;
    }

    // Made this so i dont have to remember it
    private float FloatToRadians(float degrees)
    {
        return degrees * (Mathf.PI / 180.0f);
    }

    // Made this so i dont have to remember it
    private float RadiansToFloat(float radians)
    {
        return radians * (180.0f / Mathf.PI);
    }

    // Calculate hypotenuse using equations: a^2 + b^2 = c^2 || a / cos = h || o / sin = h
    public float? CalculateHypotenuse()
    {
        if (opposite != null && adjacent != null) // a^2 + b^2 = c^2
            return Mathf.Sqrt((opposite.Value * opposite.Value) + (adjacent.Value * adjacent.Value));
        if (angle != null && adjacent != null) // a / cos = h
            return adjacent.Value / Mathf.Cos(FloatToRadians(angle.Value));
        if (angle != null && opposite != null) // o / sin = h
            return opposite.Value / Mathf.Sin(FloatToRadians(angle.Value));
        return null;
    }

    // Calculate opposite using equations: a^2 + b^2 = c^2 || h * sin = o || a * tan = o
    public float? CalculateOpposite()
    {
        if (hypotenuse != null && adjacent != null) // a^2 + b^2 = c^2
            return Mathf.Sqrt((hypotenuse.Value * hypotenuse.Value) - (adjacent.Value * adjacent.Value));
        if (angle != null && adjacent != null) // h * sin = o
            return adjacent.Value * Mathf.Tan(FloatToRadians(angle.Value));
        if (angle != null && hypotenuse != null) // a * tan = o
            return hypotenuse.Value * Mathf.Sin(FloatToRadians(angle.Value));
        return null;
    }

    // Calculate adjacent using equations: a^2 + b^2 = c^2 || h * cos = a || o / tan = a
    public float? CalculateAdjacent()
    {
        if (hypotenuse != null && opposite != null) // a^2 + b^2 = c^2
            return Mathf.Sqrt((hypotenuse.Value * hypotenuse.Value) - (opposite.Value * opposite.Value));
        if (angle != null && hypotenuse != null) // h * cos = a
            return hypotenuse.Value * Mathf.Cos(FloatToRadians(angle.Value));
        if (angle != null && opposite != null) // o / tan = a
            return opposite.Value / Mathf.Tan(FloatToRadians(angle.Value));
        return null;
    }

    // Calculate angle using equations: sin = o / h || cos = a / h || tan = o / a
    public float? CalculateAngle()
    {
        if (hypotenuse != null && opposite != null) // sin = o / h
            return RadiansToFloat(Mathf.Asin(opposite.Value / hypotenuse.Value));
        if (adjacent != null && hypotenuse != null) // cos = a / h
            return RadiansToFloat(Mathf.Acos(adjacent.Value / hypotenuse.Value));
        if (adjacent != null && opposite != null) // tan = o / a
            return RadiansToFloat(Mathf.Atan(opposite.Value / adjacent.Value));
        return null;
    }
}
