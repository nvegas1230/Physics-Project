using UnityEngine;

public class MagneticFieldObject
{
    // Variables
    public float fluxLineCount = 0; // # of flux lines (wb or vs)
    public float strength = 0; // Magnetic Field Strength (T)
    public float areaX = 0; // Area x dimension (m)
    public float areaY = 0; // Area y dimension (m)
    
    // Magnetic field from a wire variables
    public float current = 0; // Current (A)

    // Wire in a magnetic field variables
    public float length = 0; // Length of the wire (m)
    public float force = 0; // Force exerted on the wire (N)
    public float wireDirection = 0;
    public bool fieldFacingPlayer = false;

    // Charge in a magnetic field variables
    public float velocity = 0; // Velocity of particle (m/s)
    public float charge = 0; // Charge of the particle (C)

    private System.Random rand = new System.Random();

    // Constructor
    public MagneticFieldObject(int problemType)
    {
        if (problemType == 1) // Magnetic field strength from flux lines and area
        {
            areaX = rand.Next(1, 81) / 10f;
            areaY = rand.Next(1, 81) / 10f;
            fluxLineCount = rand.Next(1, 21) * 2;
            strength = fluxLineCount / (areaX * areaY);
        }
        else if (problemType == 2) // Magnetic field around a wire
        {
            areaX = rand.Next(10, 101) / 100f;
            current = rand.Next(50, 501) / 100f;
            strength = 0.0000002f * current / areaX;
        }
        else if (problemType == 3) // Wire in a magnetic field
        {
            current = rand.Next(50, 501) / 100f;
            strength = rand.Next(50, 501) / 100f;
            length = rand.Next(5, 201) / 100f;
            force = strength * current * length;
            wireDirection = rand.Next(0, 4) * 90f;
            fieldFacingPlayer = rand.Next(0, 2) == 1;
        }
        else if (problemType == 4) // Charge in a magnetic field
        {
            charge = rand.Next(1, 9);
            strength = rand.Next(50, 401) / 100f;
            velocity = rand.Next(50, 4001) / 100f;
            force = charge * velocity * strength;
            wireDirection = rand.Next(0, 4) * 90f;
            fieldFacingPlayer = rand.Next(0, 2) == 1;
        }
    }

    public string GetDirection()
    {
        string direction = "";

        if (fieldFacingPlayer)
        {
            if (wireDirection == 0)
                direction = "up";
            else if (wireDirection == 90)
                direction = "left";
            else if (wireDirection == 180)
                direction = "down";
            else
                direction = "right";
        }
        else
        {
            if (wireDirection == 0)
                direction = "down";
            else if (wireDirection == 90)
                direction = "right";
            else if (wireDirection == 180)
                direction = "up";
            else
                direction = "left";
        }

        return direction;
    }
}
