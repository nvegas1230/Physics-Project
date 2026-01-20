using System;
using UnityEngine;

[Serializable]
public class KinematicsObject
{
    // Need a System.Random defined to use random number generation
    System.Random rand = new System.Random();

    // Declare all of the variables
    public float? t;
    public float? di;
    public float? df;
    public float? vi;
    public float? vf;
    public float? a;
    // Variables for work (i added after basic kinematics)
    public float m;
    public float f;
    public float w;
    // Variables for momentum/impulse
    public float pi;
    public float pf;
    public float impulse;

    // Constructor
    public KinematicsObject(bool useNegatives)
    {
        // Randomly generate values for di, vi, a, and t, because the others can be calculated
        di = 0f;
        vi = rand.Next(0, 500);
        a  = rand.Next(0, 500);
        t  = rand.Next(1, 1000);
        m = rand.Next(3, 16);

        if (useNegatives)
        {
            vi = vi*2-1000;
            a = a*2-1000;
        }

        /*if (Settings.UseWholeNumbersOnly)
        {
            vi = vi/100;
            a = a/100;
            t = t/100;
        }
        else */
        {
            vi = vi/100f;
            a = a/100f;
            t = t/100f;
        }

        CalculateDependant();
    }

    public void CalculateDependant()
    {
        df = di + vi * t + 0.5f * a * t * t;
        vf = vi + a * t;

        f = m * a.Value;
        w = f * df.Value;

        pi = vi.Value * m;
        pf = vf.Value * m;
        impulse = pf - pi;
    }

    // Uses equations: t = (df - di) / v || t = (vf - vi) / a || t = 2dt / (vi + vf)
    public float? CalculateT()
    {
        if (t != null)
            return t;

        // Case 1: t = (vf - vi) / a
        if (vf != null && vi != null && a != null && a != 0)
            return (vf.Value - vi.Value) / a.Value;

        // Needs total displacement for the remaining cases
        if (di == null || df == null)
            return null;

        float dt = df.Value - di.Value;

        // Case 2: t = 2dt / (vi + vf)
        if (vi != null && vf != null && (vi.Value + vf.Value) != 0)
            return (2f * dt) / (vi.Value + vf.Value);

        // Case 3: t = dt / vi (only when a = 0)
        if (vi != null && a == 0 && vi.Value != 0)
            return dt / vi.Value;

        // Case 4: quadratic case (vf unknown)
        if (vi != null && a != null && a != 0)
        {
            float A = 0.5f * a.Value;
            float B = vi.Value;
            float C = -dt;

            float discriminant = B * B - 4 * A * C;

            if (discriminant < 0)
                return null;

            float sqrtD = (float)Math.Sqrt(discriminant);

            float t1 = (-B + sqrtD) / (2 * A);
            float t2 = (-B - sqrtD) / (2 * A);

            // Return the time
            if (t1 > 0 && t2 > 0)
                return Math.Min(t1, t2);
            if (t1 > 0)
                return t1;
            if (t2 > 0)
                return t2;
        }

        return null;
    }

    // Uses equations: di = df - dt || dt = vt || dt = vi*t + 0.5*a*t^2 || dt = ((vi + vf) / 2) * t || dt = (vf^2 - vi^2) / (2a)
    public float? CalculateDi()
    {
        if (di != null)
            return di;

        if (df == null)
            return null;

        float? dt = null;

        // Case 1: dt = vt
        if (vi != null && t != null && a == null)
            dt = vi.Value * t.Value;

        // Case 2: dt = vi*t + 0.5*a*t^2
        else if (vi != null && t != null && a != null)
            dt = vi.Value * t.Value + 0.5f * a.Value * t.Value * t.Value;

        // Case 3: dt = ((vi + vf)/2)*t
        else if (vi != null && vf != null && t != null)
            dt = ((vi.Value + vf.Value) / 2f) * t.Value;

        // Case 4: dt = (vf^2 - vi^2)/(2a)
        else if (vi != null && vf != null && a != null && a != 0)
            dt = (vf.Value * vf.Value - vi.Value * vi.Value) / (2f * a.Value);

        if (dt != null)
            return df.Value - dt.Value;

        return null;
    }

    // Uses equations: df = di + dt || dt = vt || dt = vi*t + 0.5*a*t^2 || dt = ((vi + vf)/2)*t || dt = (vf^2 - vi^2)/(2a)
    public float? CalculateDf()
    {
        if (df != null)
            return df;

        if (di == null)
            return null;

        float? dt = null;

        // Case 1: dt = vt
        if (vi != null && t != null && a == null)
            dt = vi.Value * t.Value;

        // Case 2: dt = vi*t + 0.5*a*t^2
        else if (vi != null && t != null && a != null)
            dt = vi.Value * t.Value + 0.5f * a.Value * t.Value * t.Value;

        // Case 3: dt = ((vi + vf)/2)*t
        else if (vi != null && vf != null && t != null)
            dt = ((vi.Value + vf.Value) / 2f) * t.Value;

        // Case 4: dt = (vf^2 - vi^2)/(2a)
        else if (vi != null && vf != null && a != null && a != 0)
            dt = (vf.Value * vf.Value - vi.Value * vi.Value) / (2f * a.Value);

        if (dt != null)
            return di.Value + dt.Value;

        return null;
    }

    // Uses equations: vi = (df - di)/t || vi = vf - at || vi = (2dt / t) - vf || vi = (d - 0.5at^2)/t || vi = sqrt(vf^2 - 2ad)
    public float? CalculateVi()
    {
        if (vi != null)
            return vi;

        // Case 1: vi = (df - di)/t
        if (di != null && df != null && t != null && t != 0 && a == null)
            return (df.Value - di.Value) / t.Value;

        // Case 2: vi = vf - at
        if (vf != null && a != null && t != null)
            return vf.Value - a.Value * t.Value;

        // Case 3: vi = (2dt / t) - vf
        if (di != null && df != null && vf != null && t != null && t != 0)
        {
            float dt = df.Value - di.Value;
            return (2f * dt / t.Value) - vf.Value;
        }

        // Case 4: vi = (d - 0.5at^2)/t
        if (di != null && df != null && a != null && t != null && t != 0)
        {
            float dt = df.Value - di.Value;
            return (dt - 0.5f * a.Value * t.Value * t.Value) / t.Value;
        }

        // Case 5: vi = sqrt(vf^2 - 2ad)
        if (vf != null && a != null && di != null && df != null)
        {
            float dt = df.Value - di.Value;
            float insideSqrt = vf.Value * vf.Value - 2f * a.Value * dt;
            if (insideSqrt >= 0f)
                return Mathf.Sqrt(insideSqrt);
        }

        return null;
    }

    // Uses equations: vf = vi + a*t || vf = (df - di)/t || vf = (2dt / t) - vi || vf = sqrt(vi^2 + 2ad)
    public float? CalculateVf()
    {
        if (vf != null)
            return vf;

        // Case 1: vf = vi + a*t
        if (vi != null && a != null && t != null)
            return vi.Value + a.Value * t.Value;

        // Case 2: vf = (df - di)/t (constant velocity)
        if (di != null && df != null && t != null && t != 0 && a == null)
            return (df.Value - di.Value) / t.Value;

        // Case 3: vf = (2dt / t) - vi
        if (di != null && df != null && vi != null && t != null && t != 0)
        {
            float dt = df.Value - di.Value;
            return (2f * dt / t.Value) - vi.Value;
        }

        // Case 4: vf = sqrt(vi^2 + 2ad)
        if (vi != null && a != null && di != null && df != null)
        {
            float dt = df.Value - di.Value;
            float insideSqrt = vi.Value * vi.Value + 2f * a.Value * dt;
            if (insideSqrt >= 0f)
                return Mathf.Sqrt(insideSqrt);
        }

        return null;
    }

    // Uses equations: a = (vf - vi)/t || a = (vf^2 - vi^2)/(2*dt) || a = (d - vi*t)/(0.5*t^2)
    public float? CalculateA()
    {
        if (a != null)
            return a;

        // Case 1: a = (vf - vi) / t
        if (vf != null && vi != null && t != null && t != 0)
            return (vf.Value - vi.Value) / t.Value;

        // Case 2: a = (vf^2 - vi^2) / (2*dt)
        if (vf != null && vi != null && di != null && df != null)
        {
            float dt = df.Value - di.Value;
            if (dt != 0f)
                return (vf.Value * vf.Value - vi.Value * vi.Value) / (2f * dt);
        }

        // Case 3: a = (d - vi*t) / (0.5*t^2)
        if (di != null && df != null && vi != null && t != null && t != 0)
        {
            float dt = df.Value - di.Value;
            return (dt - vi.Value * t.Value) / (0.5f * t.Value * t.Value);
        }

        return null;
    }

    // Loops until all unknowns are found or until all of them that can be found are
    public void CalculateUnknown()
    {
        bool changed;

        do
        {
            changed = false;

            float? result;

            result = CalculateDi();
            if (di == null && result != null)
            {
                di = result;
                changed = true;
            }

            result = CalculateDf();
            if (df == null && result != null)
            {
                df = result;
                changed = true;
            }

            result = CalculateT();
            if (t == null && result != null)
            {
                t = result;
                changed = true;
            }

            result = CalculateVi();
            if (vi == null && result != null)
            {
                vi = result;
                changed = true;
            }

            result = CalculateVf();
            if (vf == null && result != null)
            {
                vf = result;
                changed = true;
            }

            result = CalculateA();
            if (a == null && result != null)
            {
                a = result;
                changed = true;
            }

        } while (changed);
    }

    // Imp. = F△t = m△v = m(vf - vi) = mvf - mvi = Pf - Pi = △P
    public void CalculateP()
    {
        
    }
}
