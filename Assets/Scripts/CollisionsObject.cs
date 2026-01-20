using UnityEngine;
using System;

public class CollisionsObject
{
    // Need a System.Random defined to use random number generation
    System.Random rand = new System.Random();

    // Declare all of the variables
    public float m1;
    public float v1i;
    public float v1f;

    public float m2;
    public float v2i;
    public float v2f;

    public float e; // Appearently there is something called a "coefficient of restitution", which calculates how the objects will collide (elastic, inelastic, etc.)
    // Before i forget, e = 0: completely inelastic, 0 < e < 1: inelastic, e = 1: elastic, e > 1, explosive

    public float explosionVi; // Used for explosions

    // Constructor
    public CollisionsObject()
    {
        m1 = rand.Next(1, 11);
        m2 = rand.Next(1, 11); 
        v1i = rand.Next(1, 11);
        v2i = rand.Next(-5, 6);

        if (v2i > v1i)
            v2i = 0f;

        e = rand.Next(0, 2);
        explosionVi = 0f;

        CaluclateFinalVelocities();
    }

    // I made this into a seperate function so e value can be changed around and then recalculate the output
    public void CaluclateFinalVelocities()
    {
        v1f = (m1 * v1i + m2 * v2i - m2 * e * (v1i - v2i)) / (m1 + m2);
        v2f = (m1 * v1i + m2 * v2i + m1 * e * (v1i - v2i)) / (m1 + m2);
    }

    // This is to make explosion collisions, as they need to be created differently if i want the intial velocity to be zero for both
    public void CalculateExplosion()
    {
        explosionVi = rand.Next(1, 6);

        // Pf = Pi, mvi = m1v1f + m2v2f, v2f = mvi - m1v1f / m2

        v1i = 0;        
        v2i = 0;

        v1f = -(rand.Next(1, 8) + explosionVi * ((m1 + m2) / m1));
        v2f = Mathf.Abs((((m1 + m2) * explosionVi) - (m1 * v1f)) / m2);
    }
}
