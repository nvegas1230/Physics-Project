using UnityEngine;
using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class PracticeProblems : MonoBehaviour
{
    // Need a System.Random defined to use random number generation
    System.Random rand = new System.Random();

    // Variables for basic kinematics practice questions ui
    GameObject BasicKinematicsContainer;
    GameObject BasicKinematicsSolutionContainer;
    GameObject MotionGraphsContainer;
    GameObject MotionGraphsSolutionContainer;
    GameObject ComponentKinematicsContainer;
    GameObject ComponentKinematicsSolutionContainer;
    GameObject BasicDynamicsContainer;
    GameObject BasicDynamicsSolutionContainer;
    GameObject ComponentDynamicsContainer;
    GameObject ComponentDynamicsSolutionContainer;
    GameObject TensionContainer;
    GameObject TensionSolutionContainer;
    GameObject CentreOfMassContainer;
    GameObject CentreOfMassSolutionContainer;
    GameObject WorkContainer;
    GameObject WorkSolutionContainer;
    GameObject CollisionsContainer;
    GameObject CollisionsSolutionContainer;
    GameObject ElectricForceContainer;
    GameObject ElectricForceSolutionContainer;
    GameObject ElectricFieldsContainer;
    GameObject ElectricFieldsSolutionContainer;
    GameObject CurrentElectricityContainer;
    GameObject CurrentElectricitySolutionContainer;
    GameObject MagneticFieldStrengthContainer;
    GameObject MagneticFieldStrengthSolutionContainer;
    GameObject WireinaMagneticFieldContainer;
    GameObject WireinaMagneticFieldSolutionContainer;
    GameObject ChargeinaMagneticFieldContainer;
    GameObject ChargeinaMagneticFieldSolutionContainer;
    GameObject MomentumImpulseContainer;
    GameObject MomentumImpulseSolutionContainer;

    // ############################################# Conversion/Comparative Functions #############################################
    // Converts a float to a string
    public string ToDisplayString(float? value, string format = "0.##")
    {
        return value?.ToString(format) ?? "?";
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

    // Makes subscript work for the text in unity
    public string FirstCharWithSubSecond(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        if (input.Length == 1)
            return input[0].ToString();

        return $"{input[0]}<sub>{input[1]}</sub>";
    }

    // Makes subscript work for the text in unity
    public string ScientificNotation(float value)
    {
        float currentNumber = value;
        int exponent = 0;

        while (currentNumber >= 10)
        {
            currentNumber = currentNumber / 10f;
            exponent++;
        }

        while (currentNumber < 1)
        {
            currentNumber = currentNumber * 10f;
            exponent--;
        }

        return $"{ToDisplayString(currentNumber)}x10<sup>{ToDisplayString(exponent)}</sup>";
    }

    // Removes characters (like m/s) from an answer and returns it as a flaot
    public float? FilterAnswer(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;

        StringBuilder cleaned = new StringBuilder();
        bool dotUsed = false;
        bool negativeUsed = false;

        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                cleaned.Append(c);
            }
            else if (c == '-' && !negativeUsed)
            {
                cleaned.Append(c);
                negativeUsed = true;
            }
            else if (c == '.' && !dotUsed)
            {
                cleaned.Append(c);
                dotUsed = true;
            }
        }

        if (cleaned.Length == 0)
            return null;

        if (float.TryParse(
                cleaned.ToString(),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float result))
        {
            return result;
        }

        return null;
    }

    // Compares floats for checking answers within a range, with a default of 2
    public bool FloatsEqual(float? a, float? b, float? range = 2f)
    {
        if (!a.HasValue && !b.HasValue)
            return true;

        if (!a.HasValue || !b.HasValue)
            return false;

        return Mathf.Abs(a.Value - b.Value) <= range;
    }

    // ############################################# Basic Kinematics Functions #############################################
    // Inits for BasicKinematics problems
    List<string> unknowns = new List<string>();
    KinematicsObject currentBasicKinematicsProblem;

    // Make a practice problem
    private KinematicsObject CreateBasicKinematicsProblem()
    {
        unknowns.Clear();
        System.Random rand = new System.Random();
        KinematicsObject problem = new KinematicsObject(true);

        // Randomly remove some unknowns to solve for
        int unknownsToRemove = Settings.SolveForOne ? 1 : 2;
        int attempts = 0;

        while (unknownsToRemove > 0 && attempts < 10)
        {
            attempts++;
            int choice = rand.Next(0, 6);

            switch (choice)
            {
                case 0: if (problem.t != null) { problem.t = null; unknowns.Add("t"); unknownsToRemove--; } break;
                case 1: if (problem.di != null && problem.df != null) { problem.di = null; unknowns.Add("di"); unknownsToRemove--; } break;
                case 2: if (problem.di != null && problem.df != null) { problem.df = null; unknowns.Add("df"); unknownsToRemove--; } break;
                case 3: if (problem.vi != null) { problem.vi = null; unknowns.Add("vi"); unknownsToRemove--; } break;
                case 4: if (problem.vf != null) { problem.vf = null; unknowns.Add("vf"); unknownsToRemove--; } break;
                case 5: if (problem.a != null)  { problem.a = null; unknowns.Add("a"); unknownsToRemove--; } break;
            }
        }

        return problem;
    }

    // Setup the practice problem on users side
    public void SetKinematicsProblem(KinematicsObject problem)
    {
        // Set all of the variables
        currentBasicKinematicsProblem = problem;
        TMP_Text QuestionText = BasicKinematicsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        TMP_Text VariableText = BasicKinematicsContainer.transform.Find("VariableContainer").Find("Image").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        TMP_Text AnswerText = BasicKinematicsContainer.transform.Find("AnswerContainer").Find("Image").Find("Container").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>();
        //TMP_InputField Answer1Box = BasicKinematicsContainer.transform.Find("AnswerContainer").Find("Image").Find("Container").Find("InputField1").gameObject.GetComponent<TMP_InputField>();
        //TMP_InputField Answer2Box = BasicKinematicsContainer.transform.Find("AnswerContainer").Find("Image").Find("Container").Find("InputField2").gameObject.GetComponent<TMP_InputField>();

        // Set question/answer text
        if (unknowns.Count==2)
        {   
            // Question text
            QuestionText.text = "Find " + FirstCharWithSubSecond(unknowns[0]) + " and " + FirstCharWithSubSecond(unknowns[1]) + ".";
            // Answer text
            //AnswerText.text = $"{FirstCharWithSubSecond(unknowns[0])} =\n{FirstCharWithSubSecond(unknowns[1])} =";
            //if (!Answer2Box.gameObject.activeInHierarchy) Answer2Box.gameObject.SetActive(true);
        }
        else
        {
            // Question text
            QuestionText.text = "Find " + FirstCharWithSubSecond(unknowns[0]) + ".";
            // Answer text
            //AnswerText.text = $"{FirstCharWithSubSecond(unknowns[0])} =";
            //if (Answer2Box.gameObject.activeInHierarchy) Answer2Box.gameObject.SetActive(false);
        }

        string variableTextChain = "";
        if (problem.t.HasValue) { variableTextChain = variableTextChain + $"t = {ToDisplayString(problem.t, "0.00")}s"; } else { variableTextChain = variableTextChain + $"t = ?"; }
        if (problem.di.HasValue) { variableTextChain = variableTextChain + $"\nd<sub>i</sub> = {ToDisplayString(problem.di, "0.00")}m"; } else { variableTextChain = variableTextChain + $"\nd<sub>i</sub> = ?"; }
        if (problem.df.HasValue) { variableTextChain = variableTextChain + $"\nd<sub>f</sub> = {ToDisplayString(problem.df, "0.00")}m"; } else { variableTextChain = variableTextChain + $"\nd<sub>f</sub> = ?"; }
        if (problem.vi.HasValue) { variableTextChain = variableTextChain + $"\nv<sub>i</sub> = {ToDisplayString(problem.vi, "0.00")}m/s"; } else { variableTextChain = variableTextChain + $"\nv<sub>i</sub> = ?"; }
        if (problem.vf.HasValue) { variableTextChain = variableTextChain + $"\nv<sub>f</sub> = {ToDisplayString(problem.vf, "0.00")}m/s"; } else { variableTextChain = variableTextChain + $"\nv<sub>f</sub> = ?"; }
        if (problem.a.HasValue) { variableTextChain = variableTextChain + $"\na = {ToDisplayString(problem.a, "0.00")}m/s<sup>2</sup>"; } else { variableTextChain = variableTextChain + $"\na = ?"; }
        
        // Set variables text
        VariableText.text = variableTextChain;
        /*
        VariableText.text = $@"t = {ToDisplayString(problem.t, "0.00")}s
d<sub>i</sub> = {ToDisplayString(problem.di, "0.00")}m
d<sub>f</sub> = {ToDisplayString(problem.df, "0.00")}m
v<sub>i</sub> = {ToDisplayString(problem.vi, "0.00")}m/s
v<sub>f</sub> = {ToDisplayString(problem.vf, "0.00")}m/s
a = {ToDisplayString(problem.a, "0.00")}m/s<sup>2</sup>";
        //*/

        // Reset answer box text
        //Answer1Box.text = "";
        //Answer2Box.text = "";
    }

    // Specific function for answer kinematics problems
    public void AnswerKinematicsProblem()
    {
        BasicKinematicsSolutionContainer.SetActive(true);

        GameObject holder = BasicKinematicsSolutionContainer.transform.Find("Image").gameObject;
        TMP_Text solutionText = holder.transform.Find("SolutionText (TMP)").GetComponent<TMP_Text>();
        TMP_Text statusText = holder.transform.Find("StatusText (TMP)").GetComponent<TMP_Text>();
        TMP_Text plrAnswerText = holder.transform.Find("PlayerAnswerText (TMP)").GetComponent<TMP_Text>();
        //TMP_InputField Answer1Box = BasicKinematicsContainer.transform.Find("AnswerContainer").Find("Image").Find("Container").Find("InputField1").gameObject.GetComponent<TMP_InputField>();
        //TMP_InputField Answer2Box = BasicKinematicsContainer.transform.Find("AnswerContainer").Find("Image").Find("Container").Find("InputField2").gameObject.GetComponent<TMP_InputField>();

        string GetUnknown(string unknownVar)
        {
            if (unknownVar == "t") { return $"{ToDisplayString(currentBasicKinematicsProblem.t)}s"; };
            if (unknownVar == "di") { return $"{ToDisplayString(currentBasicKinematicsProblem.di)}m"; };
            if (unknownVar == "df") { return $"{ToDisplayString(currentBasicKinematicsProblem.df)}m"; };
            if (unknownVar == "vi") { return $"{ToDisplayString(currentBasicKinematicsProblem.vi)}m/s"; };
            if (unknownVar == "vf") { return $"{ToDisplayString(currentBasicKinematicsProblem.vf)}m/s"; };
            if (unknownVar == "a")  { return $"{ToDisplayString(currentBasicKinematicsProblem.a)}m/s<sup>2</sup>"; };
            return "";
        }

        currentBasicKinematicsProblem.CalculateUnknown();

        //float? FirstAnswer = FilterAnswer(Answer1Box.text);
        
        if (unknowns.Count == 1)
        {
            //plrAnswerText.text = $"{unknowns[0]} = {ToDisplayString(FirstAnswer)}";
            solutionText.text = $"{unknowns[0]} = {GetUnknown(unknowns[0])}";

            /*
            if (FloatsEqual(FirstAnswer, FirstSolution))
            {
                statusText.text = "Correct!";
            }
            else
            {
                statusText.text = "Incorrect.";    
            }
            //*/
        }
        else
        {
            //float? SecondAnswer = FilterAnswer(Answer2Box.text);

            //plrAnswerText.text = $"{unknowns[0]} = {ToDisplayString(FirstAnswer)}\n{unknowns[1]} = {ToDisplayString(SecondAnswer)}";
            solutionText.text = $"{unknowns[0]} = {GetUnknown(unknowns[0])}\n{unknowns[1]} = {GetUnknown(unknowns[1])}";

            /*
            if (FloatsEqual(FirstAnswer, FirstSolution) && FloatsEqual(SecondAnswer, SecondSolution))
            {
                statusText.text = "Correct!";
            }
            else
            {
                statusText.text = "Incorrect.";    
            }
            //*/
        }
    }

    // ############################################# Motion Graphs Functions #############################################
    // Inits for MotionGraphs problems
    List<int> currentVTGraph;

    private List<int> CalculateDT(List<int> vtValues)
    {
        List<int> dtValues = new List<int>();

        float runningTotal = 0f;
        dtValues.Add(0); // displacement at t = 0 as placeholder

        for (int i = 1; i < vtValues.Count; i++)
        {
            // Trapezoidal area between two velocity points formula that i found on the internet
            float area = (vtValues[i - 1] + vtValues[i]) * 0.5f;

            runningTotal += area;

            // Convert to int and round to nearest ones value
            dtValues.Add(Mathf.RoundToInt(runningTotal));
        }

        return dtValues;
    }

    private List<int> CalculateAT(List<int> vtValues)
    {
        List<int> atValues = new List<int>();

        // Initialize the first point in graph
        atValues.Add(0);

        for (int t = 1; t < vtValues.Count; t++)
        {
            // Run will always be 1, so just set rise as current y minus previous y
            int acceleration = vtValues[t] - vtValues[t - 1];
            atValues.Add(acceleration);

            // Set the first point to the next so it has the acceleration going to the starting point
            if (t == 1)
            {
                atValues[0] = acceleration;
            }
        }

        return atValues;
    }

    private List<int> CreateMotionGraphsProblem()
    {
        System.Random rand = new System.Random();
        int previousRandomPoint = -1000;
        int graphPoints = 10;
        List<int> newList = new List<int>() {};

        int maxY = 10;
        int minY = -10;

        for (int i = 0; i < graphPoints; i++)
        {
            int currentPoint;

            if (previousRandomPoint == -1000) { currentPoint = rand.Next(minY, maxY + 1); }
            else { currentPoint = (rand.Next(0, maxY + 1) - (maxY / 2)) + previousRandomPoint; currentPoint = Math.Clamp(currentPoint, minY, maxY); }

            newList.Add(currentPoint);
            previousRandomPoint = currentPoint;
        }

        return newList;
    }

    public void SetMotionGraphsProblem(List <int> inputList)
    {
        WindowGraph GraphScript = MotionGraphsContainer.transform.Find("ComputerGraph").GetComponent<WindowGraph>();
        
        GraphScript.ShowGraph(inputList);
        currentVTGraph = inputList;
    }

    public void AnswerMotionGraphsProblem(string graphInfo) 
    {
        MotionGraphsSolutionContainer.SetActive(true);
        WindowGraph GraphScript = MotionGraphsSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();

        if ( graphInfo == "dt" )
        {
            GraphScript.ShowGraph(CalculateDT(currentVTGraph), "line", true);
        }
        else if ( graphInfo == "vt" )
        { 
            GraphScript.ShowGraph(currentVTGraph);
        }
        else
        {
            GraphScript.ShowGraph(CalculateAT(currentVTGraph), "bar");
        }
    }

    // ############################################# Component Kinematics Functions #############################################
    // Inits for ComponentKinematics problems
    Vector2 pointA = new Vector2(10, 0);
    Vector2 pointB;
    Vector2 pointC;
    VectorComponent currentVectorA;
    VectorComponent currentVectorB;

    private VectorComponent CreateComponentKinematicsProblem()
    {
        VectorComponent vectorComponent = new VectorComponent();

        int dir = rand.Next(1, 3);
        vectorComponent.hypotenuse = rand.Next(30, 101);
        vectorComponent.angle = rand.Next(10, 81);

        vectorComponent.adjacent = vectorComponent.CalculateAdjacent();
        vectorComponent.opposite = vectorComponent.CalculateOpposite();

        if (dir != 1)
            vectorComponent.opposite = vectorComponent.opposite.Value * -1f;

        return vectorComponent;
    }

    public void SetComponentKinematicsProblem(VectorComponent vectorA, VectorComponent vectorB)
    {
        TMP_Text QuestionText = ComponentKinematicsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();

        pointB = new Vector2(pointA.x + vectorA.adjacent.Value, pointA.y + vectorA.opposite.Value);
        pointC = new Vector2(pointB.x + vectorB.adjacent.Value, pointB.y + vectorB.opposite.Value);

        currentVectorA = vectorA;
        currentVectorB = vectorB;

        string directionStringA;
        string directionStringB;

        Debug.Log(vectorA.opposite.Value);
        Debug.Log(vectorB.opposite.Value);

        if (vectorA.opposite.Value > 0)
            directionStringA = "N of E";
        else
            directionStringA = "S of E";
        if (vectorB.opposite.Value > 0)
            directionStringB = "N of E";
        else
            directionStringB = "S of E";

        QuestionText.text = $"You travel {ToDisplayString(vectorA.CalculateHypotenuse().Value)}m, {ToDisplayString(Mathf.Abs(vectorA.CalculateAngle().Value))}° {directionStringA}, then you turn and travel {ToDisplayString(vectorB.CalculateHypotenuse().Value)}m, {ToDisplayString(Mathf.Abs(vectorB.CalculateAngle().Value))}° {directionStringB}. What is your total displacement?";
    }

    public void AnswerComponentKinematicsProblem() 
    {
        ComponentKinematicsSolutionContainer.SetActive(true);
        TMP_Text AnswerText = ComponentKinematicsSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = ComponentKinematicsSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();

        VectorComponent totalDistance = new VectorComponent();
        totalDistance.opposite = currentVectorA.opposite.Value + currentVectorB.opposite.Value;
        totalDistance.adjacent = currentVectorA.adjacent.Value + currentVectorB.adjacent.Value;

        totalDistance.hypotenuse = totalDistance.CalculateHypotenuse();
        totalDistance.angle = totalDistance.CalculateAngle();

        string directionTotal;

        if (totalDistance.opposite.Value > 0)
            directionTotal = "N of E";
        else
            directionTotal = "S of E";

        AnswerText.text = $"{ToDisplayString(totalDistance.hypotenuse.Value)}m, {ToDisplayString(Mathf.Abs(totalDistance.angle.Value))}° {directionTotal}.";

        GraphScript.ClearGraph();

        GraphScript.CreateVectorComponent(pointA, pointB);
        GraphScript.CreateVectorComponent(pointC);
    }

    // ############################################# Basic Dynamics Functions #############################################
    // Inits for BasicDynamics problems
    DynamicsObject currentBasicDynamicsProblem;
    private int problemSelect;

    private DynamicsObject CreateBasicDynamicsProblem()
    {
        DynamicsObject problem = new DynamicsObject();
        problem.CalculateAllBasic();

        return problem;
    }

    public void SetBasicDynamicsProblem(DynamicsObject problem)
    {
        TMP_Text QuestionText = BasicDynamicsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string problemText = "";
        problemSelect = rand.Next(0, 2);
        currentBasicDynamicsProblem = problem;

        // Variables that need to be stated are left force, right force, acceleration, coeff of friction, friction, and mass
        if (problemSelect == 0) // Problem 1: Find netForce and acceleration
        {
            problemText = $"An object is being pulled to the left with {ToDisplayString(Mathf.Abs(problem.leftForce.hypotenuse.Value))}N and pulled to the right with {ToDisplayString(Mathf.Abs(problem.rightForce.hypotenuse.Value))}N. The mass of the object is {ToDisplayString(problem.mass)}kg and the coefficient of friction is {ToDisplayString(problem.frictionCoefficient)}. Find F<sub>net</sub> and acceleration.";
        }
        else if (problemSelect == 1) // Problem 2: Find coefficient friction (μ)
        {
            problemText = $"An object is being pulled to the left with {ToDisplayString(Mathf.Abs(problem.leftForce.hypotenuse.Value))}N and pulled to the right with {ToDisplayString(Mathf.Abs(problem.rightForce.hypotenuse.Value))}N. The mass of the object is {ToDisplayString(problem.mass)}kg and the acceleration is {ToDisplayString(problem.acceleration.hypotenuse.Value)}. Find μ.";
        }
        else if (problemSelect == 2) // Problem 3: Find mass, not actual built yet.
        {
            problemText = $"An object is experiencing a force of friction of {ToDisplayString(Mathf.Abs(problem.friction.hypotenuse.Value))}N. Find the mass of the object.";
        }

        QuestionText.text = problemText;
    }

    public void AnswerBasicDynamicsProblem()
    {
        BasicDynamicsSolutionContainer.SetActive(true);
        TMP_Text AnswerText = BasicDynamicsSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = BasicDynamicsSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
        DynamicsObject problem = currentBasicDynamicsProblem;

        string answerText = "";
        
        if (problemSelect == 0) // Problem 1: Find netForce and acceleration
        {
            answerText = $"F<sub>net</sub> = {ToDisplayString(problem.netForce.adjacent.Value)}N\nAcceleration = {ToDisplayString(problem.acceleration.hypotenuse.Value)}m/s<sup>2</sup>";
        }
        else if (problemSelect == 1) // Problem 2: Find coefficient friction (μ)
        {
            answerText = $"μ = {ToDisplayString(problem.frictionCoefficient)}";
        }

        AnswerText.text = answerText;

        GraphScript.CreateFreeBodyDiagramSimple(problem.mass, Mathf.Abs(problem.upForce.hypotenuse.Value), Mathf.Abs(problem.downForce.hypotenuse.Value), Mathf.Abs(problem.rightForce.hypotenuse.Value), Mathf.Abs(problem.leftForce.hypotenuse.Value), Mathf.Abs(problem.friction.hypotenuse.Value));
    }

    // ############################################# Component Dynamics Functions #############################################
    // Inits for ComponentDynamics problems
    DynamicsObject currentComponentDynamicsProblem;

    private DynamicsObject CreateComponentDynamicsProblem()
    {
        problemSelect = rand.Next(0, 3);
        DynamicsObject problem = new DynamicsObject(true);
        problem.CalculateAllVector();

        return problem;
    }

    public void SetComponentDynamicsProblem(DynamicsObject problem)
    {
        TMP_Text QuestionText = ComponentDynamicsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string problemText = "";
        currentComponentDynamicsProblem = problem;

        if (problemSelect == 0) // Problem 1: find acceleration when pushing object down slope to right
        {
            problem.leftForce.opposite = 0f;
            problem.leftForce.adjacent = 0f;
            problem.leftForce.hypotenuse = 0f;
            problem.CalculateAllVector();
            problemText = $"A {ToDisplayString(problem.mass)}kg object is on a slope of {ToDisplayString(Mathf.Abs(problem.inclineAngle))}° and is being pushed down it with {ToDisplayString(Mathf.Abs(problem.rightForce.adjacent.Value))}N of force. If the coefficient of friction is {ToDisplayString(problem.frictionCoefficient)}, find the acceleration of the object.";
        }
        else if (problemSelect == 1) // Problem 2: find acceleration when pushing object up a slope
        {
            problem.rightForce.opposite = 0f;
            problem.rightForce.adjacent = 0f;
            problem.rightForce.hypotenuse = 0f;
            problem.CalculateAllVector();
            problemText = $"A {ToDisplayString(problem.mass)}kg object is on a slope of {ToDisplayString(Mathf.Abs(problem.inclineAngle))}° and is being pushed up it with {ToDisplayString(Mathf.Abs(problem.leftForce.adjacent.Value))}N of force. If the coefficient of friction is {ToDisplayString(problem.frictionCoefficient)}, find the acceleration of the object.";
        }
        else if (problemSelect == 2) // Problem 3: find acceleration when pulling an object with a rope
        {
            problem.rightForce.angle = problem.inclineAngle;
            problem.rightForce.hypotenuse = rand.Next(2000, 5001) / 100f;
            problem.rightForce.adjacent = null;
            problem.rightForce.opposite = null;
            problem.rightForce.adjacent = problem.rightForce.CalculateAdjacent();
            problem.rightForce.opposite = problem.rightForce.CalculateOpposite();
            problem.inclineAngle = 0f;
            problem.leftForce.opposite = 0f;
            problem.leftForce.adjacent = 0f;
            problem.leftForce.hypotenuse = 0f;
            problem.downForce.hypotenuse = problem.mass * -9.81f;
            problem.downForce.adjacent = problem.mass * -9.81f;
            problem.downForce.angle = 0f;
            problem.downForce.opposite = 0f;
            problem.upForce.adjacent = Mathf.Max(0, Mathf.Abs(problem.downForce.adjacent.Value) - Mathf.Abs(problem.rightForce.opposite.Value));
            problem.CalculateAllVector();
            problemText = $"A {ToDisplayString(problem.mass)}kg object is being pulled to the right at {ToDisplayString(Mathf.Abs(problem.rightForce.angle.Value))}° above the horizontal with {ToDisplayString(Mathf.Abs(problem.rightForce.hypotenuse.Value))}N of force. If the coefficient of friction is {ToDisplayString(problem.frictionCoefficient)}, find the acceleration of the object.";
        }

        QuestionText.text = problemText;
    }

    public void AnswerComponentDynamicsProblem()
    {
        ComponentDynamicsSolutionContainer.SetActive(true);
        TMP_Text AnswerText = ComponentDynamicsSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = ComponentDynamicsSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
        DynamicsObject problem = currentComponentDynamicsProblem;

        string answerText = "";
        
        if (problemSelect == 0) // Problem 1: find acceleration when pushing object down slope to right
        {
            answerText = $"a<sub>x</sub> = {ToDisplayString(problem.acceleration.adjacent.Value)}m/s<sup>2</sup>";
        }
        else if (problemSelect == 1) // Problem 2: find acceleration when pushing object up a slope
        {
            answerText = $"a<sub>x</sub> = {ToDisplayString(problem.acceleration.adjacent.Value)}m/s<sup>2</sup>";
        }
        else if (problemSelect == 2) // Problem 2: find acceleration when pushing object up a slope
        {
            answerText = $"a<sub>x</sub> = {ToDisplayString(problem.acceleration.adjacent.Value)}m/s<sup>2</sup>\na<sub>y</sub> = {ToDisplayString(problem.acceleration.opposite.Value)}m/s<sup>2</sup>";
        }

        GraphScript.CreateFreeBodyDiagramVector(problem, problemSelect);
        AnswerText.text = answerText;
    }

    // ############################################# Tension Functions #############################################
    // Inits for Tension problems
    private int mass1;
    private int mass2;
    private float tensionAngle;
    private float tensionFriction;

    private void CreateTensionProblem()
    {
        mass1 = rand.Next(5, 31); 
        mass2 = rand.Next(5, 31);
        tensionAngle = rand.Next(300, 601) / 10f;
        tensionFriction = rand.Next(10, 30)/100f;
    }

    public void SetTensionProblem()
    {
        TMP_Text QuestionText = TensionContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        Transform DiagramContainer = TensionContainer.transform.Find("DiagramContainer");
        Transform FBD = null;

        problemSelect = rand.Next(0, 4);

        DiagramContainer.Find("SolutionGraphVertical").gameObject.SetActive(false);
        DiagramContainer.Find("SolutionGraphHorizontal").gameObject.SetActive(false);
        DiagramContainer.Find("SolutionGraphTwoRopes").gameObject.SetActive(false);
        DiagramContainer.Find("SolutionGraphAngled").gameObject.SetActive(false);

        if (problemSelect == 0) // Problem 1: find both tension when 2 objects are hanging
        {
            FBD = DiagramContainer.Find("SolutionGraphVertical").Find("GraphContainer").Find("FreeBodyDiagram");
            DiagramContainer.Find("SolutionGraphVertical").gameObject.SetActive(true);
            QuestionText.text = "Find F<sub>T1</sub> and F<sub>T2</sub>.";
        }
        else if (problemSelect == 1) // Problem 2: find acceleration when one object is hanging and another is on a surface
        {
            FBD = DiagramContainer.Find("SolutionGraphHorizontal").Find("GraphContainer").Find("FreeBodyDiagram");
            DiagramContainer.Find("SolutionGraphHorizontal").gameObject.SetActive(true);
            QuestionText.text = $"If the coefficient of friction is {ToDisplayString(tensionFriction)}, find acceleration.";
        }
        else if (problemSelect == 2) // Problem 3: find both tension when an object is hanging by 2 ropes
        {
            FBD = DiagramContainer.Find("SolutionGraphTwoRopes").Find("GraphContainer").Find("FreeBodyDiagram");
            DiagramContainer.Find("SolutionGraphTwoRopes").gameObject.SetActive(true);
            QuestionText.text = $"If two ropes are tied to the mass at {tensionAngle}° above the horizontal and each are the same length, find F<sub>T1</sub> and F<sub>T2</sub>.";
        }
        else if (problemSelect == 3) // Problem 4: find both tension when an object is at an angle
        {
            FBD = DiagramContainer.Find("SolutionGraphAngled").Find("GraphContainer").Find("FreeBodyDiagram");
            DiagramContainer.Find("SolutionGraphAngled").gameObject.SetActive(true);
            QuestionText.text = $"If the rope is tied to the mass at {tensionAngle}° above the horizontal, find F<sub>T</sub>.";
        }

        FBD.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass1)}kg";
        if (problemSelect < 2)
            FBD.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass2)}kg";
    }

    public void AnswerTensionProblem()
    {
        TensionSolutionContainer.SetActive(true);
        TMP_Text AnswerText = TensionSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = null;

        TensionSolutionContainer.transform.Find("SolutionGraphVertical").gameObject.SetActive(false);
        TensionSolutionContainer.transform.Find("SolutionGraphHorizontal").gameObject.SetActive(false);
        TensionSolutionContainer.transform.Find("SolutionGraphTwoRopes").gameObject.SetActive(false);
        TensionSolutionContainer.transform.Find("SolutionGraphAngled").gameObject.SetActive(false);

        if (problemSelect == 0) // Problem 1: find both tension when 2 objects are hanging
        {
            TensionSolutionContainer.transform.Find("SolutionGraphVertical").gameObject.SetActive(true);
            GraphScript = TensionSolutionContainer.transform.Find("SolutionGraphVertical").GetComponent<WindowGraph>();

            float Ft1 = (mass1 + mass2) * 9.81f;
            float Ft2 = mass2 * 9.81f;

            AnswerText.text = $"F<sub>T1</sub> = {ToDisplayString(Mathf.Abs(Ft1))}N\nF<sub>T2</sub> = {ToDisplayString(Mathf.Abs(Ft2))}N";
        }
        else if (problemSelect == 1) // Problem 2: find acceleration when one object is hanging and another is on a surface
        {
            TensionSolutionContainer.transform.Find("SolutionGraphHorizontal").gameObject.SetActive(true);
            GraphScript = TensionSolutionContainer.transform.Find("SolutionGraphHorizontal").GetComponent<WindowGraph>();

            float acceleration = ((mass2 * 9.81f) - (mass1 * 9.81f * tensionFriction)) / (mass1 + mass2); // (Fg - Ff) / m = a

            AnswerText.text = $"a = {ToDisplayString(Mathf.Abs(acceleration))}m/s<sup>2</sup>";
        }
        else if (problemSelect == 2) // Problem 3: find both tension when an object is hanging by 2 ropes
        {
            TensionSolutionContainer.transform.Find("SolutionGraphTwoRopes").gameObject.SetActive(true);
            GraphScript = TensionSolutionContainer.transform.Find("SolutionGraphTwoRopes").GetComponent<WindowGraph>();

            float Fty = mass1 * 9.81f / 2f; // Ft = mg / 2
            float Ft = Fty / Mathf.Sin(FloatToRadians(tensionAngle));

            AnswerText.text = $"F<sub>T1</sub> = {ToDisplayString(Mathf.Abs(Ft))}N\nF<sub>T2</sub> = {ToDisplayString(Mathf.Abs(Ft))}N";
        }
        else if (problemSelect == 3) // Problem 4: find both tension when an object is at an angle
        {
            TensionSolutionContainer.transform.Find("SolutionGraphAngled").gameObject.SetActive(true);
            GraphScript = TensionSolutionContainer.transform.Find("SolutionGraphAngled").GetComponent<WindowGraph>();

            float Fty = mass1 * 9.81f; // Ft = mg / 2
            float Ft = Fty / Mathf.Sin(FloatToRadians(tensionAngle));

            AnswerText.text = $"F<sub>T</sub> = {ToDisplayString(Mathf.Abs(Ft))}N";
        }

        GraphScript.CreateTensionDiagram(mass1, mass2, problemSelect, FloatToRadians(tensionAngle));
    }

    // ############################################# Centre of Mass Functions #############################################
    // Inits for Centre of Mass problems
    private float distance;

    private void CreateCentreOfMassProblem()
    {
        mass1 = rand.Next(5, 31); 
        mass2 = rand.Next(5, 31);
        distance = rand.Next(300, 1600)/100f;
    }

    public void SetCentreOfMassProblem()
    {
        TMP_Text QuestionText = CentreOfMassContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        Transform DiagramContainer = CentreOfMassContainer.transform.Find("DiagramContainer");
        Transform FBD = DiagramContainer.Find("SolutionGraph").Find("GraphContainer").Find("FreeBodyDiagram");
        QuestionText.text = "Find x<sub>cm</sub>.";
        FBD.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass1)}kg";
        FBD.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass2)}kg";
        FBD.Find("ArrowsFolder").Find("Distance").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"← {ToDisplayString(distance)}m →";

        RectTransform XcmContainer = FBD.Find("XcmContainer").GetComponent<RectTransform>();
        RectTransform DottedLine = XcmContainer.transform.Find("Dash").GetComponent<RectTransform>();
        RectTransform XcmText = XcmContainer.transform.Find("Text (TMP)").GetComponent<RectTransform>();

        float xcm = ((mass1 * 0f) + (mass2 * distance)) / (mass1 + mass2); // xcm = (m1 * x1 + m2 * x2) / (m1 + m2)
        float relativePosition = xcm / distance;

        DottedLine.anchoredPosition = new Vector2(XcmContainer.rect.size.x * relativePosition, 0);
        XcmText.anchoredPosition = new Vector2(XcmContainer.rect.size.x * relativePosition / 2, 0);
    }

    public void AnswerCentreOfMassProblem()
    {
        CentreOfMassSolutionContainer.SetActive(true);
        TMP_Text AnswerText = CentreOfMassSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = CentreOfMassSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();

        float xcm = ((mass1 * 0) + (mass2 * distance)) / (mass1 + mass2); // xcm = (m1 * x1 + m2 * x2) / (m1 + m2)

        AnswerText.text = $"x<sub>cm</sub> = {ToDisplayString(Mathf.Abs(xcm))}m";
        GraphScript.CreateCentreOfMassDiagram(mass1, mass2, distance);
    }

    // ############################################# Work Functions #############################################
    // Inits for Work problems
    KinematicsObject currentWorkProblem;

    private void CreateWorkProblem()
    {
        currentWorkProblem = new KinematicsObject(false);
        problemSelect = rand.Next(0, 4);
    }

    public void SetWorkProblem()
    {
        TMP_Text QuestionText = WorkContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";

        if (problemSelect == 0) // Problem 1: calculate work
        {
            question = $"How much work is done when moving a mass {ToDisplayString(currentWorkProblem.df.Value)}m with {ToDisplayString(currentWorkProblem.f)}N of force?";

        }
        else if (problemSelect == 1) // Problem 2: lifting a mass
        {
            currentWorkProblem.df = rand.Next(200, 751)/100f;
            currentWorkProblem.w = currentWorkProblem.df.Value * currentWorkProblem.f;
            question = $"How much work is done when a {ToDisplayString(currentWorkProblem.m)}kg box is lifted {ToDisplayString(currentWorkProblem.df.Value)}m off of the ground?";
        }
        else if (problemSelect == 2) // Problem 3: find force used
        {
            question = $"{ToDisplayString(currentWorkProblem.w)}Nm is used to move an object {ToDisplayString(currentWorkProblem.df.Value)}m. How much force was used?";
        }
        else if (problemSelect == 3) // Problem 4: find work when given time, velocity, mass, and distance
        {
            question = $"A {ToDisplayString(currentWorkProblem.m)}kg mass accelerates from {ToDisplayString(currentWorkProblem.vi.Value)}m/s to {ToDisplayString(currentWorkProblem.vf.Value)}m/s. How much work was done if it was pushed over {ToDisplayString(currentWorkProblem.t.Value)}s?";
        }

        QuestionText.text = question;
    }

    public void AnswerWorkProblem()
    {
        WorkSolutionContainer.SetActive(true);
        TMP_Text AnswerText = WorkSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        if (problemSelect == 0) // Problem 1: calculate work
        {
            answer = $"w = {ToDisplayString(currentWorkProblem.w)}Nm";
        }
        else if (problemSelect == 1) // Problem 2: lifting a mass
        {
            answer = $"w > {currentWorkProblem.m * 9.81f * currentWorkProblem.df}Nm"; // w = m*g*d
        }
        else if (problemSelect == 2) // Problem 3: find force used
        {
            answer = $"F = {ToDisplayString(currentWorkProblem.f)}N";
        }
        else if (problemSelect == 3) // Problem 4: find work when given time, velocity, mass, and distance
        {
            answer = $"w = {ToDisplayString(currentWorkProblem.w)}Nm";
        }

        AnswerText.text = answer;
    }

    // ############################################# Collisions Functions #############################################
    // Inits for Collisions problems
    CollisionsObject currentCollisionsObject;
    private string unknown;

    private void CreateCollisionsProblem()
    {
        problemSelect = rand.Next(0, 2);
        currentCollisionsObject = new CollisionsObject();
    }

    public void SetCollisionsProblem()
    {
        TMP_Text QuestionText = CollisionsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";
        Transform DiagramContainer = CollisionsContainer.transform.Find("DiagramContainer");
        WindowGraph GraphScript = null;
        DiagramContainer.Find("SolutionGraph").gameObject.SetActive(false);
        DiagramContainer.Find("SolutionGraphExplosion").gameObject.SetActive(false);

        if (problemSelect == 0) // Simple collisions problem (elastic/inelastic)
        {
            DiagramContainer.Find("SolutionGraph").gameObject.SetActive(true);
            GraphScript = DiagramContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
            int choice = rand.Next(0, 6);

            switch (choice)
            {
                case 0: {unknown = "m<sub>1</sub>";} break;
                case 1: {unknown = "m<sub>2</sub>";} break;
                case 2: {unknown = "v<sub>1i</sub>";} break;
                case 3: {unknown = "v<sub>2i</sub>";} break;
                case 4: {unknown = "v<sub>1f</sub>";} break;
                case 5: {unknown = "v<sub>2f</sub>";} break;
            }

            question = $"Find {unknown}.";
            GraphScript.CreateCollisionsDiagram(currentCollisionsObject, unknown);
        }
        else if (problemSelect == 1) // Problem 2: explosion collision problem
        {
            currentCollisionsObject.CalculateExplosion();
            DiagramContainer.Find("SolutionGraphExplosion").gameObject.SetActive(true);
            GraphScript = DiagramContainer.transform.Find("SolutionGraphExplosion").GetComponent<WindowGraph>();
            int choice = rand.Next(0, 2);

            switch (choice)
            {
                case 0: {unknown = "v<sub>2f</sub>";} break;
                case 1: {unknown = "v<sub>1f</sub>";} break;
            }

            question = $"Find {unknown}.";
        }

        QuestionText.text = question;
        GraphScript.CreateCollisionsDiagram(currentCollisionsObject, unknown);
    }

    public void AnswerCollisionsProblem()
    {
        CollisionsSolutionContainer.SetActive(true);
        TMP_Text AnswerText = CollisionsSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        if (unknown == "m<sub>1</sub>")
            answer = $"m<sub>1</sub> = {ToDisplayString(currentCollisionsObject.m1)}kg";
        else if (unknown == "m<sub>2</sub>")
            answer = $"m<sub>2</sub> = {ToDisplayString(currentCollisionsObject.m2)}kg";
        else if (unknown == "v<sub>1i</sub>")
            answer = $"v<sub>1i</sub> = {ToDisplayString(currentCollisionsObject.v1i)}m/s";
        else if (unknown == "v<sub>2i</sub>")
            answer = $"v<sub>2i</sub> = {ToDisplayString(currentCollisionsObject.v2i)}m/s";
        else if (unknown == "v<sub>1f</sub>")
            answer = $"v<sub>1f</sub> = {ToDisplayString(currentCollisionsObject.v1f)}m/s";
        else if (unknown == "v<sub>2f</sub>")
            answer = $"v<sub>2f</sub> = {ToDisplayString(currentCollisionsObject.v2f)}m/s";

        AnswerText.text = answer;
    }

    // ############################################# Electric Force Functions #############################################
    // Inits for Electric Force problems
    ElectricForceObject currentElectricForceObject;
    string selectedCharge = "";

    private void CreateElectricForceProblem()
    {
        bool extraCharge = rand.Next(0, 3) < 2;
        int randomCharge = rand.Next(0, 2);

        if (extraCharge)
            randomCharge = rand.Next(0, 3);
        if (randomCharge == 0)
            selectedCharge = "A";
        else if (randomCharge == 1)
            selectedCharge = "B";
        else
            selectedCharge = "C";

        currentElectricForceObject = new ElectricForceObject(extraCharge);
    }

    public void SetElectricForceProblem()
    {
        TMP_Text QuestionText = ElectricForceContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";
        Transform DiagramContainer = ElectricForceContainer.transform.Find("DiagramContainer");
        WindowGraph GraphScript = DiagramContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();

        question = $"What is the net force of {selectedCharge}?";
        
        GraphScript.CreateElectricForceDiagram(currentElectricForceObject);
        QuestionText.text = question;
    }

    public void AnswerElectricForceProblem()
    {
        ElectricForceSolutionContainer.SetActive(true);
        TMP_Text AnswerText = ElectricForceSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        answer = $"F<sub>net{selectedCharge}</sub> = {ToDisplayString(currentElectricForceObject.CalculateNetForce(selectedCharge))}N.";

        AnswerText.text = answer;
    }

    // ############################################# Electric Fields Functions #############################################
    // Inits for Electric Fields problems
    ElectricFieldObject currentElectricFieldsObject;

    private void CreateElectricFieldsProblem()
    {
        currentElectricFieldsObject = new ElectricFieldObject();
        problemSelect = rand.Next(0, 3);
    }

    public void SetElectricFieldsProblem()
    {
        TMP_Text QuestionText = ElectricFieldsContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";

        if (problemSelect == 0) // Problem 1: find electric field strength
        {
            question = $"A {ToDisplayString(currentElectricFieldsObject.charge)}C charge experiences a force of {currentElectricFieldsObject.force}N {currentElectricFieldsObject.GetDirection(true)}. What is the magnitude and direction of the electric field?";
        }
        else if (problemSelect == 1) // Problem 2: find electric force
        {
            question = $"Find the magnitude and direction of the force exerted on a {ToDisplayString(currentElectricFieldsObject.charge)}C charge in an electric field of {ToDisplayString(currentElectricFieldsObject.strength)}N/C {currentElectricFieldsObject.GetDirection(false)}.";
        }
        else if (problemSelect == 2) // Problem 3: find the charge
        {
            question = $"Find the strength of a charge if an electric field of {ToDisplayString(currentElectricFieldsObject.strength)}N/C {currentElectricFieldsObject.GetDirection(false)} exerts a force of {currentElectricFieldsObject.force}N {currentElectricFieldsObject.GetDirection(true)}.";
        }

        QuestionText.text = question;
    }

    public void AnswerElectricFieldsProblem()
    {
        ElectricFieldsSolutionContainer.SetActive(true);
        TMP_Text AnswerText = ElectricFieldsSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        WindowGraph GraphScript = ElectricFieldsSolutionContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
        string answer = "";

        if (problemSelect == 0) // Problem 1: find electric field strength
        {
            answer = $"E = {ToDisplayString(currentElectricFieldsObject.strength)}N/C, {currentElectricFieldsObject.GetDirection(false)}";
        }
        else if (problemSelect == 1) // Problem 2: find electric force
        {
            answer = $"F<sub>e</sub> = {ToDisplayString(currentElectricFieldsObject.force)}N, {currentElectricFieldsObject.GetDirection(true)}";
        }
        else if (problemSelect == 2) // Problem 3: find the charge
        {
            answer = $"Q = {ToDisplayString(currentElectricFieldsObject.charge)}C";
        }

        GraphScript.CreateElectricFieldDiagram(currentElectricFieldsObject);
        AnswerText.text = answer;
    }

    // ############################################# Current Electricity Functions #############################################
    // Inits for Current Electricity problems
    CurrentElectricityObject currentCurrentElectricityObject;

    private void CreateCurrentElectricityProblem()
    {
        currentCurrentElectricityObject = new CurrentElectricityObject();
        problemSelect = rand.Next(0, 3);
    }

    public void SetCurrentElectricityProblem()
    {
        TMP_Text QuestionText = CurrentElectricityContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";

        if (problemSelect == 0) // Problem 1: find current
        {
            question = $"{ToDisplayString(currentCurrentElectricityObject.charge)}C of charge flows through a circuit in {ToDisplayString(currentCurrentElectricityObject.time)}s. What is the current in the circuit?";
        }
        else if (problemSelect == 1) // Problem 2: find time
        {
            question = $"If a circuit has {ToDisplayString(currentCurrentElectricityObject.current)}A of current, how long does it take {ToDisplayString(currentCurrentElectricityObject.charge)}C to flow through it?";
        }
        else if (problemSelect == 2) // Problem 3: find charge (electrons)
        {
            question = $"If an appliance has {ToDisplayString(currentCurrentElectricityObject.current)}A of current, how many electrons flow through it in {ToDisplayString(currentCurrentElectricityObject.time)}s?";
        }

        QuestionText.text = question;
    }

    public void AnswerCurrentElectricityProblem()
    {
        CurrentElectricitySolutionContainer.SetActive(true);
        TMP_Text AnswerText = CurrentElectricitySolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        if (problemSelect == 0) // Problem 1: find current
        {
            answer = $"I = {ToDisplayString(currentCurrentElectricityObject.current)}A";
        }
        else if (problemSelect == 1) // Problem 2: find time
        {
            answer = $"t = {ToDisplayString(currentCurrentElectricityObject.time)}s";
        }
        else if (problemSelect == 2) // Problem 3: find charge (electrons)
        {
            answer = $"Q = {ScientificNotation(currentCurrentElectricityObject.charge * 6250000000000000000f)} electrons";
        }

        AnswerText.text = answer;
    }
    
    // ############################################# Magnetic Field Strength Functions #############################################
    // Inits for Magnetic Field Strength problems
    MagneticFieldObject currentMagneticFieldObject;
    private string currentDirection = "";

    private void CreateMagneticFieldStrengthProblem()
    {
        problemSelect = rand.Next(1, 3);
        currentMagneticFieldObject = new MagneticFieldObject(problemSelect);
    }

    public void SetMagneticFieldStrengthProblem()
    {
        TMP_Text QuestionText = MagneticFieldStrengthContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        Transform DiagramContainer = MagneticFieldStrengthContainer.transform.Find("DiagramContainer");
        Transform AnswerContainer = MagneticFieldStrengthContainer.transform.Find("AnswerContainer");
        Transform QuestionContainer = MagneticFieldStrengthContainer.transform.Find("QuestionContainer");
        string question = "";

        if (problemSelect == 1) // Problem 1: Find strength with flux lines and area
        {
            DiagramContainer.gameObject.SetActive(false);
            QuestionContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 120);
            AnswerContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -110);
            question = $"Find the magnetic field strength if {ToDisplayString(currentMagneticFieldObject.fluxLineCount)} flux lines pass through a {ToDisplayString(currentMagneticFieldObject.areaX)}m by {ToDisplayString(currentMagneticFieldObject.areaY)}m area.";    
        }
        else if (problemSelect == 2) // Problem 2: Find strength around a wire
        {
            DiagramContainer.gameObject.SetActive(true);
            QuestionContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-235, 120);
            AnswerContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-235, -110);

            int tempCurrentDirection = rand.Next(0, 2);
            if (tempCurrentDirection == 0)
            {
                currentDirection = "clockwise";
                DiagramContainer.Find("SolutionGraph").Find("GraphContainer").Find("FreeBodyDiagram").Find("Wire").Find("Dot").gameObject.SetActive(true);
                DiagramContainer.Find("SolutionGraph").Find("GraphContainer").Find("FreeBodyDiagram").Find("Wire").Find("Text (TMP)").gameObject.SetActive(false);
            }
            else
            {
                currentDirection = "counter-clockwise";
                DiagramContainer.Find("SolutionGraph").Find("GraphContainer").Find("FreeBodyDiagram").Find("Wire").Find("Dot").gameObject.SetActive(false);
                DiagramContainer.Find("SolutionGraph").Find("GraphContainer").Find("FreeBodyDiagram").Find("Wire").Find("Text (TMP)").gameObject.SetActive(true);
            }

            question = $"Find the magnetic field strength and direction of a wire {ToDisplayString(currentMagneticFieldObject.areaX)}m away from you with {ToDisplayString(currentMagneticFieldObject.current)}A flowing through it.";
        }

        QuestionText.text = question;
    }

    public void AnswerMagneticFieldStrengthProblem()
    {
        MagneticFieldStrengthSolutionContainer.SetActive(true);
        TMP_Text AnswerText = MagneticFieldStrengthSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        if (problemSelect == 1) // Problem 1: Find strength with flux lines and area
        {
            answer = $"B = {ToDisplayString(currentMagneticFieldObject.strength)}T";
        }
        else
        {
            answer = $"B = {ScientificNotation(currentMagneticFieldObject.strength)}T, {currentDirection}";
        }

        AnswerText.text = answer;
    }

    // ############################################# Wire in a Magnetic Field Functions #############################################
    // Inits for Wire in a Magnetic Field problems

    private void CreateWireinaMagneticFieldProblem()
    {
        problemSelect = rand.Next(3, 5);
        currentMagneticFieldObject = new MagneticFieldObject(problemSelect);
    }

    public void SetWireinaMagneticFieldProblem()
    {
        if (problemSelect == 3)
        {
            WireinaMagneticFieldContainer.SetActive(true);
            ChargeinaMagneticFieldContainer.SetActive(false);
            TMP_Text QuestionText = WireinaMagneticFieldContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
            Transform DiagramContainer = WireinaMagneticFieldContainer.transform.Find("DiagramContainer");
            WindowGraph GraphScript = DiagramContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
            string question = "";

            question = $"A {ToDisplayString(currentMagneticFieldObject.length)}m long wire with a current of {ToDisplayString(currentMagneticFieldObject.current)}A running through it is placed into a magnetic field with a strength of {ToDisplayString(currentMagneticFieldObject.strength)}T. Find the magnitude and direction of the magnetic force on the wire.";

            GraphScript.CreateWireinaMagneticFieldDiagram(currentMagneticFieldObject);
            QuestionText.text = question;
        }
        else
        {
            WireinaMagneticFieldContainer.SetActive(false);
            ChargeinaMagneticFieldContainer.SetActive(true);
            SetChargeinaMagneticFieldProblem();
        }
    }

    public void AnswerWireinaMagneticFieldProblem()
    {
        if (problemSelect == 3)
        {
            WireinaMagneticFieldSolutionContainer.SetActive(true);
            TMP_Text AnswerText = WireinaMagneticFieldSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
            string answer = "";

            answer = $"F<sub>B</sub> = {ToDisplayString(currentMagneticFieldObject.force)}N, [{currentMagneticFieldObject.GetDirection()}]";

            AnswerText.text = answer;
        }
        else
        {
            AnswerChargeinaMagneticFieldProblem();
        }
    }

    // ############################################# Charge in a Magnetic Field Functions #############################################
    // Inits for Charge in a Magnetic Field problems

    private void CreateChargeinaMagneticFieldProblem()
    {
        currentMagneticFieldObject = new MagneticFieldObject(4);
    }

    public void SetChargeinaMagneticFieldProblem()
    {
        TMP_Text QuestionText = ChargeinaMagneticFieldContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        Transform DiagramContainer = ChargeinaMagneticFieldContainer.transform.Find("DiagramContainer");
        WindowGraph GraphScript = DiagramContainer.transform.Find("SolutionGraph").GetComponent<WindowGraph>();
        string question = "";

        question = $"A particle with a {ToDisplayString(currentMagneticFieldObject.charge)}C charge moves through a magnetic field at a speed of {ToDisplayString(currentMagneticFieldObject.velocity)}m/s. If the magnetic field has a strength of {ToDisplayString(currentMagneticFieldObject.strength)}T, what is the strength and direction of the magnetic force exerted on the particle?";

        GraphScript.CreateChargeinaMagneticFieldDiagram(currentMagneticFieldObject);
        QuestionText.text = question;
    }

    public void AnswerChargeinaMagneticFieldProblem()
    {
        ChargeinaMagneticFieldSolutionContainer.SetActive(true);
        TMP_Text AnswerText = ChargeinaMagneticFieldSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        answer = $"F<sub>B</sub> = {ToDisplayString(currentMagneticFieldObject.force)}N, [{currentMagneticFieldObject.GetDirection()}]";

        AnswerText.text = answer;
    }

    // ############################################# Momentum Impulse Functions #############################################
    // Inits for Momentum Impulse problems
    KinematicsObject currentMomentumObject1;

    private void CreateMomentumImpulseProblem()
    {
        currentMomentumObject1 = new KinematicsObject(false);
        problemSelect = rand.Next(0, 5);
    }

    public void SetMomentumImpulseProblem()
    {
        TMP_Text QuestionText = MomentumImpulseContainer.transform.Find("QuestionContainer").Find("Image").Find("Text (TMP)").GetComponent<TMP_Text>();
        string question = "";

        if (problemSelect == 0) // Problem 1: Find momentum with velocity and mass
        {
            question = $"If a ball with a mass of {ToDisplayString(currentMomentumObject1.m)}kg is moving at a speed of {ToDisplayString(currentMomentumObject1.vf.Value)}m/s, what is it's momentum?";
        }
        else if (problemSelect == 1) // Problem 2: Find impulse with force and time
        {
            currentMomentumObject1.vi = rand.Next(700, 2501) / 100f;
            currentMomentumObject1.vf = 0f;
            currentMomentumObject1.m = rand.Next(400, 701) / 10f;
            currentMomentumObject1.a = (currentMomentumObject1.vf.Value - currentMomentumObject1.vi.Value) / currentMomentumObject1.t;
            currentMomentumObject1.CalculateDependant();
            question = $"A water skier lets go of the rope and comes to a stop. What is the impulse exerted on them if the water exerts an average force of {ToDisplayString(currentMomentumObject1.f)}N on them and they stop in {ToDisplayString(currentMomentumObject1.t.Value)}s?";
        }
        else if (problemSelect == 2) // Problem 3: Find impulse with mass and velocity
        {
            currentMomentumObject1.vi = 0f;
            currentMomentumObject1.vf = rand.Next(300, 1001) / 100f;
            currentMomentumObject1.m = rand.Next(500, 701) / 10f;
            currentMomentumObject1.a = (currentMomentumObject1.vf.Value - currentMomentumObject1.vi.Value) / currentMomentumObject1.t;
            currentMomentumObject1.CalculateDependant();
            question = $"A man with a mass of {ToDisplayString(currentMomentumObject1.m)}kg starts running and reaches a speed of {ToDisplayString(currentMomentumObject1.vf.Value)}m/s. What was the impulse applied to him?";
        }
        else if (problemSelect == 3) // Problem 4: Find time with velocity, mass, and force
        {
            currentMomentumObject1.vi = rand.Next(250, 601) / 100f;
            currentMomentumObject1.vf = 0f;
            currentMomentumObject1.m = rand.Next(200, 401) / 1000f;
            currentMomentumObject1.t = rand.Next(150, 301) / 10000f;
            currentMomentumObject1.a = (currentMomentumObject1.vf.Value - currentMomentumObject1.vi.Value) / currentMomentumObject1.t;
            currentMomentumObject1.CalculateDependant();
            question = $"A {ScientificNotation(currentMomentumObject1.m)}kg volleyball with an initial velocity of {ToDisplayString(currentMomentumObject1.vi.Value)}m/s (horizontally) hits a net, stops, and then drops to the ground. The average force exerted on the volleyball by the net is {ToDisplayString(currentMomentumObject1.f)}N. How long is the ball in contact with the net?";
        }
        else if (problemSelect == 4) // Problem 5: Find impulse and average force with acceleration, mass, time, and final velocity
        {
            float dropTime = rand.Next(75, 150) / 100f;
            currentMomentumObject1.vi = dropTime * -9.81f;
            currentMomentumObject1.vf = rand.Next(200, 501) / 100f;
            currentMomentumObject1.m = rand.Next(300, 701) / 1000f;
            currentMomentumObject1.t = rand.Next(50, 151) / 10000f;
            currentMomentumObject1.a = (currentMomentumObject1.vf.Value - currentMomentumObject1.vi.Value) / currentMomentumObject1.t;
            currentMomentumObject1.CalculateDependant();
            question = $"A {ScientificNotation(currentMomentumObject1.m)}kg ball is dropped and it takes {ToDisplayString(dropTime)}s for it to hit the ground. If it is in contact with the ground for {ScientificNotation(currentMomentumObject1.t.Value)}s and has a velocity of {ToDisplayString(currentMomentumObject1.vf.Value)}m/s after bouncing, what is the average force exerted on it by the ground and it's change in momentum?";
        }

        QuestionText.text = question;
    }

    public void AnswerMomentumImpulseProblem()
    {
        MomentumImpulseSolutionContainer.SetActive(true);
        TMP_Text AnswerText = MomentumImpulseSolutionContainer.transform.Find("Image").Find("Answer").GetComponent<TMP_Text>();
        string answer = "";

        // Imp. = F△t = m△v = m(vf - vi) = mvf - mvi = Pf - Pi = △P, Unit = kgms OR Ns (kgm/s is usually momentum while Ns is usually impulse, but same unit)
        if (problemSelect == 0) // Problem 1: Find momentum with velocity and mass
        {
            answer = $"P = {ToDisplayString(currentMomentumObject1.pf)}kgm/s";
        }
        else if (problemSelect == 1) // Problem 2: Find impulse with force and time
        {
            answer = $"Imp. = {ToDisplayString(currentMomentumObject1.impulse)}Ns";
        }
        else if (problemSelect == 2) // Problem 3: Find impulse with mass and velocity
        {
            answer = $"Imp. = {ToDisplayString(currentMomentumObject1.impulse)}Ns";
        }
        else if (problemSelect == 3) // Problem 4: Find time with velocity, mass, and force
        {
            answer = $"t = {ScientificNotation(currentMomentumObject1.t.Value)}s";
        }
        else if (problemSelect == 4) // Problem 5: Find impulse with acceleration, mass, time, and final velocity
        {
            answer = $"Imp. = {ToDisplayString(currentMomentumObject1.impulse)}Ns\nF<sub>avg</sub> = {ToDisplayString(currentMomentumObject1.f)}N";
        }

        AnswerText.text = answer;
    }

    // ############################################# Misc. Functions #############################################
    // Function for when answer is submitted in practice questions
    public void SubmitAnswer(string optionalVar)
    {
        string practiceUnit = SceneDataTransfer.PhysicsUnit;
        string problemType = SceneDataTransfer.ProblemType;

        if (problemType == "BasicKinematics") { AnswerKinematicsProblem(); }
        else if (problemType == "MotionGraphs") { AnswerMotionGraphsProblem(optionalVar); }
        else if (problemType == "ComponentKinematics") { AnswerComponentKinematicsProblem(); }
        else if (problemType == "BasicDynamics") { AnswerBasicDynamicsProblem(); }
        else if (problemType == "ComponentDynamics") { AnswerComponentDynamicsProblem(); }
        else if (problemType == "Tension") { AnswerTensionProblem(); }
        else if (problemType == "CentreOfMass") { AnswerCentreOfMassProblem(); }
        else if (problemType == "Work") { AnswerWorkProblem(); }
        else if (problemType == "Collisions") { AnswerCollisionsProblem(); }
        else if (problemType == "ElectricForce") { AnswerElectricForceProblem(); }
        else if (problemType == "ElectricFields") { AnswerElectricFieldsProblem(); }
        else if (problemType == "CurrentElectricity") { AnswerCurrentElectricityProblem(); }
        else if (problemType == "MagneticFieldStrength") { AnswerMagneticFieldStrengthProblem(); }
        else if (problemType == "WireinaMagneticField") { AnswerWireinaMagneticFieldProblem(); }
        else if (problemType == "ChargeinaMagneticField") { AnswerChargeinaMagneticFieldProblem(); }
        else if (problemType == "MomentumImpulse") { AnswerMomentumImpulseProblem(); }
        else
        {
            Debug.Log("Unknown ProblemType.");
        }
    }

    // Code for closing solution windows without having to code each button individually
    public void CloseSolutionWindow()
    {
        string practiceUnit = SceneDataTransfer.PhysicsUnit;
        string problemType = SceneDataTransfer.ProblemType;

        if (problemType == "BasicKinematics") { BasicKinematicsSolutionContainer.SetActive(false); }
        else if (problemType == "MotionGraphs") { MotionGraphsSolutionContainer.SetActive(false); }
        else if (problemType == "ComponentKinematics") { ComponentKinematicsSolutionContainer.SetActive(false); }
        else if (problemType == "BasicDynamics") { BasicDynamicsSolutionContainer.SetActive(false); }
        else if (problemType == "ComponentDynamics") { ComponentDynamicsSolutionContainer.SetActive(false); }
        else if (problemType == "Tension") { TensionSolutionContainer.SetActive(false); }
        else if (problemType == "CentreOfMass") { CentreOfMassSolutionContainer.SetActive(false); }
        else if (problemType == "Work") { WorkSolutionContainer.SetActive(false); }
        else if (problemType == "Collisions") { CollisionsSolutionContainer.SetActive(false); }
        else if (problemType == "ElectricForce") { ElectricForceSolutionContainer.SetActive(false); }
        else if (problemType == "ElectricFields") { ElectricFieldsSolutionContainer.SetActive(false); }
        else if (problemType == "CurrentElectricity") { CurrentElectricitySolutionContainer.SetActive(false); }
        else if (problemType == "MagneticFieldStrength") { MagneticFieldStrengthSolutionContainer.SetActive(false); }
        else if (problemType == "WireinaMagneticField") { WireinaMagneticFieldSolutionContainer.SetActive(false); ChargeinaMagneticFieldSolutionContainer.SetActive(false); }
        else if (problemType == "ChargeinaMagneticField") { WireinaMagneticFieldSolutionContainer.SetActive(false); ChargeinaMagneticFieldSolutionContainer.SetActive(false); }
        else if (problemType == "MomentumImpulse") { MomentumImpulseSolutionContainer.SetActive(false); }
        else
        {
            Debug.Log("Unknown ProblemType.");
        }
    }

    // Code for starting the practice problem scene
    public void SetupPhysicsPracticeProblem()
    {
        string practiceUnit = SceneDataTransfer.PhysicsUnit;
        string problemType = SceneDataTransfer.ProblemType;

        // Set all of the unit containers to hidden here
        BasicKinematicsContainer.SetActive(false);
        MotionGraphsContainer.SetActive(false);
        ComponentKinematicsContainer.SetActive(false);
        BasicDynamicsContainer.SetActive(false);
        ComponentDynamicsContainer.SetActive(false);
        TensionContainer.SetActive(false);
        CentreOfMassContainer.SetActive(false);
        WorkContainer.SetActive(false);
        CollisionsContainer.SetActive(false);
        ElectricForceContainer.SetActive(false);
        ElectricFieldsContainer.SetActive(false);
        CurrentElectricityContainer.SetActive(false);
        MagneticFieldStrengthContainer.SetActive(false);
        WireinaMagneticFieldContainer.SetActive(false);
        ChargeinaMagneticFieldContainer.SetActive(false);
        MomentumImpulseContainer.SetActive(false);


        if (practiceUnit == "Kinematics") // This was originally a feature but i removed it because it wasn't really needed
        {
            if (problemType == "BasicKinematics") { BasicKinematicsContainer.SetActive(true); BasicKinematicsSolutionContainer.SetActive(false); SetKinematicsProblem(CreateBasicKinematicsProblem()); }
            else if (problemType == "MotionGraphs") {MotionGraphsContainer.SetActive(true); MotionGraphsSolutionContainer.SetActive(false); SetMotionGraphsProblem(CreateMotionGraphsProblem()); }
            else if (problemType == "ComponentKinematics") {ComponentKinematicsContainer.SetActive(true); ComponentKinematicsSolutionContainer.SetActive(false); SetComponentKinematicsProblem(CreateComponentKinematicsProblem(), CreateComponentKinematicsProblem()); }
            else if (problemType == "BasicDynamics") {BasicDynamicsContainer.SetActive(true); BasicDynamicsSolutionContainer.SetActive(false); SetBasicDynamicsProblem(CreateBasicDynamicsProblem()); }
            else if (problemType == "ComponentDynamics") {ComponentDynamicsContainer.SetActive(true); ComponentDynamicsSolutionContainer.SetActive(false); SetComponentDynamicsProblem(CreateComponentDynamicsProblem()); }
            else if (problemType == "Tension") {TensionContainer.SetActive(true); TensionSolutionContainer.SetActive(false); CreateTensionProblem(); SetTensionProblem(); }
            else if (problemType == "CentreOfMass") {CentreOfMassContainer.SetActive(true); CentreOfMassSolutionContainer.SetActive(false); CreateCentreOfMassProblem(); SetCentreOfMassProblem(); }
            else if (problemType == "Work") {WorkContainer.SetActive(true); WorkSolutionContainer.SetActive(false); CreateWorkProblem(); SetWorkProblem(); }
            else if (problemType == "Collisions") {CollisionsContainer.SetActive(true); CollisionsSolutionContainer.SetActive(false); CreateCollisionsProblem(); SetCollisionsProblem(); }
            else if (problemType == "ElectricForce") {ElectricForceContainer.SetActive(true); ElectricForceSolutionContainer.SetActive(false); CreateElectricForceProblem(); SetElectricForceProblem(); }
            else if (problemType == "ElectricFields") {ElectricFieldsContainer.SetActive(true); ElectricFieldsSolutionContainer.SetActive(false); CreateElectricFieldsProblem(); SetElectricFieldsProblem(); }
            else if (problemType == "CurrentElectricity") {CurrentElectricityContainer.SetActive(true); CurrentElectricitySolutionContainer.SetActive(false); CreateCurrentElectricityProblem(); SetCurrentElectricityProblem(); }
            else if (problemType == "MagneticFieldStrength") {MagneticFieldStrengthContainer.SetActive(true); MagneticFieldStrengthSolutionContainer.SetActive(false); CreateMagneticFieldStrengthProblem(); SetMagneticFieldStrengthProblem(); }
            else if (problemType == "WireinaMagneticField") {WireinaMagneticFieldContainer.SetActive(true); WireinaMagneticFieldSolutionContainer.SetActive(false); ChargeinaMagneticFieldSolutionContainer.SetActive(false); CreateWireinaMagneticFieldProblem(); SetWireinaMagneticFieldProblem(); }
            else if (problemType == "ChargeinaMagneticField") {ChargeinaMagneticFieldContainer.SetActive(true); WireinaMagneticFieldSolutionContainer.SetActive(false); ChargeinaMagneticFieldSolutionContainer.SetActive(false); CreateChargeinaMagneticFieldProblem(); SetChargeinaMagneticFieldProblem(); }
            else if (problemType == "MomentumImpulse") {MomentumImpulseContainer.SetActive(true); MomentumImpulseSolutionContainer.SetActive(false); CreateMomentumImpulseProblem(); SetMomentumImpulseProblem(); }
            else
            {
                Debug.Log("Unknown ProblemType.");
            }
        }
        else
        {
            Debug.Log("Unknown PhysicsUnit.");
        }
    }

    // Use awake to assign variables
    void Awake()
    {
        Transform Background = transform.Find("Background");
        BasicKinematicsContainer = Background.Find("BasicKinematics").gameObject;
        BasicKinematicsSolutionContainer = BasicKinematicsContainer.transform.Find("SolutionContainer").gameObject;
        MotionGraphsContainer = Background.Find("MotionGraphs").gameObject;
        MotionGraphsSolutionContainer = MotionGraphsContainer.transform.Find("SolutionContainer").gameObject;
        ComponentKinematicsContainer = Background.Find("ComponentKinematics").gameObject;
        ComponentKinematicsSolutionContainer = ComponentKinematicsContainer.transform.Find("SolutionContainer").gameObject;
        BasicDynamicsContainer = Background.Find("BasicDynamics").gameObject;
        BasicDynamicsSolutionContainer = BasicDynamicsContainer.transform.Find("SolutionContainer").gameObject;
        ComponentDynamicsContainer = Background.Find("ComponentDynamics").gameObject;
        ComponentDynamicsSolutionContainer = ComponentDynamicsContainer.transform.Find("SolutionContainer").gameObject;
        TensionContainer = Background.Find("Tension").gameObject;
        TensionSolutionContainer = TensionContainer.transform.Find("SolutionContainer").gameObject;
        CentreOfMassContainer = Background.Find("CentreOfMass").gameObject;
        CentreOfMassSolutionContainer = CentreOfMassContainer.transform.Find("SolutionContainer").gameObject;
        WorkContainer = Background.Find("Work").gameObject;
        WorkSolutionContainer = WorkContainer.transform.Find("SolutionContainer").gameObject;
        CollisionsContainer = Background.Find("Collisions").gameObject;
        CollisionsSolutionContainer = CollisionsContainer.transform.Find("SolutionContainer").gameObject;
        ElectricForceContainer = Background.Find("ElectricForce").gameObject;
        ElectricForceSolutionContainer = ElectricForceContainer.transform.Find("SolutionContainer").gameObject;
        ElectricFieldsContainer = Background.Find("ElectricFields").gameObject;
        ElectricFieldsSolutionContainer = ElectricFieldsContainer.transform.Find("SolutionContainer").gameObject;
        CurrentElectricityContainer = Background.Find("CurrentElectricity").gameObject;
        CurrentElectricitySolutionContainer = CurrentElectricityContainer.transform.Find("SolutionContainer").gameObject;
        MagneticFieldStrengthContainer = Background.Find("MagneticFieldStrength").gameObject;
        MagneticFieldStrengthSolutionContainer = MagneticFieldStrengthContainer.transform.Find("SolutionContainer").gameObject;
        WireinaMagneticFieldContainer = Background.Find("WireinaMagneticField").gameObject;
        WireinaMagneticFieldSolutionContainer = WireinaMagneticFieldContainer.transform.Find("SolutionContainer").gameObject;
        ChargeinaMagneticFieldContainer = Background.Find("ChargeinaMagneticField").gameObject;
        ChargeinaMagneticFieldSolutionContainer = ChargeinaMagneticFieldContainer.transform.Find("SolutionContainer").gameObject;
        MomentumImpulseContainer = Background.Find("MomentumImpulse").gameObject;
        MomentumImpulseSolutionContainer = MomentumImpulseContainer.transform.Find("SolutionContainer").gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { SetupPhysicsPracticeProblem(); }
}
