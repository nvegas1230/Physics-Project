using UnityEngine;

public class ElectricForceObject
{
    // Variables
    public float chargeA = 0;
    public float chargeB = 0;
    public float chargeC = 0;
    public float dist1 = 0;
    public float dist2 = 0;
    public bool moreThanTwo;

    private System.Random rand = new System.Random();

    // Constructor
    public ElectricForceObject(bool increase)
    {
        chargeA = rand.Next(-15, 16) * 0.000001f;
        if (chargeA == 0) { chargeA = 0.000001f; } // Set a default charge if it is equal to zero
        chargeB = rand.Next(-15, 16) * 0.000001f;
        if (chargeB == 0) { chargeB = 0.000001f; }
        dist1 = rand.Next(1, 16) / 100f; // Switch to centimeters

        moreThanTwo = increase;

        if (moreThanTwo)
            dist2 = rand.Next(1, 16) / 100f; // Switch to centimeters
            chargeC = rand.Next(-15, 16) * 0.000001f;
            if (chargeC == 0) { chargeC = 0.000001f; }
    }

    // Fe = (K * Q * q) / (r * r)
    public float CalculateNetForce(string charge)
    {
        float netForce = 0;

        if (charge == "A")
        {
            float force1 = CalculateForce(chargeA, chargeB, dist1);
            float force2 = 0;
            if (moreThanTwo)
                force2 = CalculateForce(chargeA, chargeC, dist1 + dist2);
            netForce = force1 + force2;
        }
        else if (charge == "B")
        {
            float force1 = CalculateForce(chargeA, chargeB, dist1) * -1; // Switch direction because it will be inverted due to position
            float force2 = 0;
            if (moreThanTwo)
                force2 = CalculateForce(chargeB, chargeC, dist2);
            netForce = force1 + force2;
        }
        else if (charge == "C")
        {
            float force1 = CalculateForce(chargeB, chargeC, dist2) * -1; // Switch direction because it will be inverted due to position
            float force2 = CalculateForce(chargeA, chargeC, dist1 + dist2) * -1; // Switch direction because it will be inverted due to position;
            netForce = force1 + force2;
        }

        return netForce;
    }

    // Fe = (K * Q * q) / (r * r)
    public float CalculateForce(float charge1, float charge2, float distance) // *Charge 1 needs to be the charge to the left for this to work, and it only evaluates charge 1 force
    {
        float forceMagnitude = Mathf.Abs(forceMagnitude = charge1 * charge2 * 9000000000 / (distance * distance));
        float direction = 1;
        if ((charge1 < 0 && charge2 < 0) || (charge1 > 0 && charge2 > 0)) // If same charge sign, they will repel instead of attract
            direction = -1;
        return forceMagnitude * direction;
    }
}
