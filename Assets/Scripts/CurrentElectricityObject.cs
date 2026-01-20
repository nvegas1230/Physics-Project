using UnityEngine;

public class CurrentElectricityObject
{
    // Variables
    public float charge = 0; // Charge (Q)
    public float current = 0; // Current (I)
    public float time = 0; // Time (t)

    private System.Random rand = new System.Random();

    // Constructor
    public CurrentElectricityObject()
    {
        int multi = rand.Next(1, 5); // This is to increase the amount by a random amount but keep the ratio similar
        charge = rand.Next(10, 200) * multi;
        time = rand.Next(1, 20) * multi;
        current = charge / time;
    }
}
