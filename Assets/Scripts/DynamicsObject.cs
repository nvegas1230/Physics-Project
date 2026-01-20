using UnityEngine;
using System;

public class DynamicsObject
{
    public VectorComponent upForce = new VectorComponent();
    public VectorComponent downForce = new VectorComponent();
    public VectorComponent rightForce = new VectorComponent();
    public VectorComponent leftForce = new VectorComponent();
    public VectorComponent friction = new VectorComponent();
    public VectorComponent acceleration = new VectorComponent();
    public VectorComponent netForce = new VectorComponent();
    public float frictionCoefficient = 0;
    public float mass = 0;
    public float inclineAngle = 0;

    private System.Random rand = new System.Random();

    // Constructor
    public DynamicsObject(bool isVector = false)
    {
        mass = rand.Next(3, 16);
        frictionCoefficient = rand.Next(10, 21)/100f;

        upForce.hypotenuse = mass * 9.81f;
        downForce.hypotenuse = -upForce.hypotenuse.Value;
        rightForce.hypotenuse = rand.Next(50, 251);
        leftForce.hypotenuse = -rand.Next(50, 126);
        acceleration.hypotenuse = 0;
        inclineAngle = rand.Next(10, 46);

        if (isVector)
        {
            // DownForce (gravity)
            downForce.angle = inclineAngle;
            downForce.adjacent = downForce.CalculateAdjacent(); // y value
            downForce.opposite = downForce.CalculateOpposite(); // x value

            // UpForce (its just normal force)
            upForce.adjacent = Mathf.Abs(downForce.adjacent.Value);

            // RightForce
            rightForce.angle = inclineAngle;
            rightForce.adjacent = rightForce.CalculateAdjacent(); // x value
            rightForce.opposite = rightForce.CalculateOpposite(); // y value

            // LeftForce (we will ignore it for now)
            leftForce.angle = inclineAngle;
            leftForce.adjacent = leftForce.CalculateAdjacent();
            leftForce.opposite = leftForce.CalculateOpposite();
        }
    }

    // F_f = Umg
    public void CaclulateFrictionBasic()
    {
        if (downForce.hypotenuse.HasValue)
            friction.hypotenuse = Mathf.Abs(frictionCoefficient * downForce.hypotenuse.Value);
    }

    // a = f/m
    public void CalculateAccelerationBasic()
    {
        if (netForce.adjacent.HasValue)
            acceleration.hypotenuse = netForce.adjacent.Value / mass;
    }

    // F_net = F_positives - F_negatives
    public void CalculateNetForceBasic()
    {
        if (rightForce.hypotenuse.HasValue && leftForce.hypotenuse.HasValue)
        {
            float currentNet = rightForce.hypotenuse.Value + leftForce.hypotenuse.Value;
            if (currentNet != 0 && friction.hypotenuse.HasValue)
            {
                netForce.adjacent = Mathf.Max(0, Mathf.Abs(currentNet) - friction.hypotenuse.Value) * (currentNet / Mathf.Abs(currentNet));
                if (netForce.adjacent.Value == 0)
                {
                    friction.hypotenuse = currentNet;
                }
            }
            else
            {
                netForce.adjacent = 0;
            }  
        }
        else
        {
            netForce.adjacent = 0;
        }
    }

    // This calculates all of the missing ones in order so it won't bug out
    public void CalculateAllBasic()
    {
        CaclulateFrictionBasic();
        CalculateNetForceBasic();
        CalculateAccelerationBasic();
    }

    public void CalculateFrictionVector()
    {
        if (downForce.hypotenuse.HasValue)
            friction.hypotenuse = Mathf.Abs(frictionCoefficient * upForce.adjacent.Value);
    }

    public void CalculateAccelerationVector()
    {
        acceleration.angle = inclineAngle;
        acceleration.adjacent = netForce.adjacent / mass;
        acceleration.opposite = netForce.opposite / mass;
    }

    // F_net = F_positives - F_negatives
    public void CalculateNetForceVectorX()
    {
        if (rightForce.adjacent.HasValue && leftForce.adjacent.HasValue && downForce.opposite.HasValue)
        {
            float currentNetX = rightForce.adjacent.Value + leftForce.adjacent.Value + downForce.opposite.Value; // RightForce and gravity will always be in positive direction
            Debug.Log(currentNetX);
            if (upForce.adjacent > 0) // check if friction should apply
                currentNetX = Mathf.Max(0, Mathf.Abs(currentNetX) - friction.hypotenuse.Value) * (Mathf.Abs(currentNetX) / currentNetX);
            Debug.Log(currentNetX);
            netForce.adjacent = currentNetX;
        }
    }

    public void CalculateNetForceVectorY()
    {
        if (rightForce.opposite.HasValue && leftForce.opposite.HasValue && downForce.adjacent.HasValue && upForce.adjacent.HasValue)
        {
            float currentNetY = Mathf.Max(0, Mathf.Abs(rightForce.opposite.Value) + Mathf.Abs(leftForce.opposite.Value) - Mathf.Abs(downForce.adjacent.Value)); // Applied forces will always be in positive direction, gravity in negative
            netForce.opposite = currentNetY;
        }
    }

    public void CalculateAllVector()
    {
        CalculateFrictionVector();
        CalculateNetForceVectorY();
        CalculateNetForceVectorX();
        CalculateAccelerationVector();
    }
}
