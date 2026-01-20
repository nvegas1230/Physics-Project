using UnityEngine;

public class ElectricFieldObject
{
    // Variables
    public float charge = 0; // Charge (Q)
    public float strength = 0; // Electric Field Strength (E)
    public float force = 0; // Electric Force (Fe)
    public int direction = 0; // 0 - 360 degrees

    private System.Random rand = new System.Random();

    // Constructor
    public ElectricFieldObject()
    {
        charge = rand.Next(-15, 16);
        if (charge == 0) { charge = 1; }
        direction = rand.Next(0, 4) * 90; // 0: East, 90: North, 180: West, 270: South
        strength = rand.Next(1, 16);

        force = Mathf.Abs(strength * charge); // Fe = E * Q
    }

    public string GetDirection(bool FactorInForce)
    {
        string directionString = "";
        float directionInt = 1; // 1: East/Right, -1: West/Left, 2: North/Up, -2: South/Down

        if (direction == 0)
            directionInt = 1;
        else if (direction == 90)
            directionInt = 2;
        else if (direction == 180)
            directionInt = -1;
        else if (direction == 270)
            directionInt = -2;

        if (FactorInForce)
            directionInt = directionInt * (Mathf.Abs(charge) / charge);

        if (directionInt == 1)
            directionString = "to the Right";
        else if (directionInt == -1)
            directionString = "to the Left";
        else if (directionInt == 2)
            directionString = "Upwards";
        else if (directionInt == -2)
            directionString = "Downwards";

        return directionString;
    }
}
