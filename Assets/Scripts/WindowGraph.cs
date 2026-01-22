using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WindowGraph : MonoBehaviour
{
    public Sprite CircleSprite;
    public Sprite ArrowSprite;
    private RectTransform GraphContainer;
    private RectTransform LabelTemplateX;
    private RectTransform LabelTemplateY;
    private RectTransform DashTemplateX;
    private RectTransform DashTemplateY;
    private List<GameObject> gameObjectList;
    private TMP_Text Title;
    private TMP_Text LabelY;
    private Transform FBD;

    private void Awake()
    {
        GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        Title = transform.Find("Background").Find("Title").GetComponent<TMP_Text>();
        LabelY = GraphContainer.Find("LabelYAxis").GetComponent<TMP_Text>();
        LabelTemplateX = GraphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        LabelTemplateY = GraphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        DashTemplateX = GraphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        DashTemplateY = GraphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        gameObjectList = new List<GameObject>();
    }

    // Converts a float to a string
    public string ToDisplayString(float? value, string format = "0.##")
    {
        return value?.ToString(format) ?? "?";
    }

    public void ClearGraph()
    {
        foreach (GameObject obj in gameObjectList) {
            Destroy(obj);
        }
        gameObjectList.Clear();
    }

    public void ShowGraph(List<int> valueList, string graphType = "line", bool skipFirstValue = false)
    {
        float graphWidth = GraphContainer.sizeDelta.x;
        float graphHeight = GraphContainer.sizeDelta.y;
        float xSize = graphWidth / valueList.Count;
        float yMax = 0f;
        float yMin = 0f;

        if (skipFirstValue) { Title.text = "Computer's Distance-Time Graph"; LabelY.text = "-> displacement (m)"; }
        else if (graphType == "line") { Title.text = "Computer's Velocity-Time Graph"; LabelY.text = "-> velocity (m/s)"; }
        else { Title.text = "Computer's Acceleration-Time Graph"; LabelY.text = "-> acceleration (m/s<sup>2</sup>)"; }

        ClearGraph();

        foreach (int value in valueList)
        {
            if (value > yMax)
            {
                yMax = value;
            }
            if (value < yMin)
            {
                yMin = value;
            }
        }

        // Added this to make sure values go up by consistent amount, and to make sure there are no repeats in numbers
        // There is probably a more clear way to do this with modulo, but I cannot think of it atm
        float yTotalDifference = Mathf.Abs(yMax - yMin);
        if (yTotalDifference < 10f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 10f;
        }
        //*
        else if (yTotalDifference >= 10f && yTotalDifference < 20f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 20f;
        }
        else if (yTotalDifference >= 20f && yTotalDifference < 30f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 30f;
        }
        else if (yTotalDifference >= 30f && yTotalDifference < 40f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 40f;
        }
        else if (yTotalDifference >= 40f && yTotalDifference < 50f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 50f;
        }
        else if (yTotalDifference >= 50f && yTotalDifference < 60f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 60f;
        }
        else if (yTotalDifference >= 60f && yTotalDifference < 70f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 70f;
        }
        else if (yTotalDifference >= 70f && yTotalDifference < 80f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 80f;
        }
        else if (yTotalDifference >= 80f && yTotalDifference < 90f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 90f;
        }
        else if (yTotalDifference >= 90f && yTotalDifference < 100f)
        {
            yMax = yMax + 1f;
            yMin = yMax - 100f;
        }
        //*/

        GameObject previousCircle = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = i * xSize + 20;
            float yPos = (valueList[i] - yMin) / (yMax - yMin) * graphHeight; 
            
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));

            if (previousCircle != null && !(skipFirstValue && i < 2)) 
                CreateDotConnection(previousCircle.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            if (skipFirstValue && i == 0)
                Destroy(circleGameObject);
            previousCircle = circleGameObject;

            // Extra stuff if the graph is an at graph
            if (graphType == "bar" && i != 9)
                circleGameObject = CreateCircle(new Vector2(xPos, (valueList[i + 1] - yMin) / (yMax - yMin) * graphHeight));

                CreateDotConnection(previousCircle.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);

                previousCircle = circleGameObject;

            RectTransform labelX = Instantiate(LabelTemplateX);
            labelX.SetParent(GraphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPos, -8f);
            labelX.GetComponent<TMP_Text>().text = (i+1).ToString();
            labelX.localScale = new Vector3(1,1,1);
            gameObjectList.Add(labelX.gameObject);

            RectTransform dashY = Instantiate(DashTemplateY);
            dashY.SetParent(GraphContainer);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(xPos, 200f);
            dashY.localScale = new Vector3(1,1,1);
            gameObjectList.Add(dashY.gameObject);
        }

        int seperatorCount = 10;
        for (int i = 1; i < (seperatorCount+1); i++)
        {
            RectTransform labelY = Instantiate(LabelTemplateY);
            labelY.SetParent(GraphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / seperatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<TMP_Text>().text = Mathf.RoundToInt(yMin + (normalizedValue * (yMax - yMin))).ToString();
            labelY.localScale = new Vector3(1,1,1);
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashX = Instantiate(DashTemplateX);
            dashX.SetParent(GraphContainer);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(250, normalizedValue * graphHeight);
            dashX.localScale = new Vector3(1,1,1);
            gameObjectList.Add(dashX.gameObject);
        }

        // Extra line for bottom of graph
        RectTransform bottomLabel = Instantiate(LabelTemplateY);
        bottomLabel.SetParent(GraphContainer, false);
        bottomLabel.gameObject.SetActive(true);
        bottomLabel.anchoredPosition = new Vector2(-7f, 0 * graphHeight);
        bottomLabel.GetComponent<TMP_Text>().text = Mathf.RoundToInt(yMin + (0 * (yMax - yMin))).ToString();
        bottomLabel.localScale = new Vector3(1,1,1);
        gameObjectList.Add(bottomLabel.gameObject);

        RectTransform bottomDashX = Instantiate(DashTemplateX);
        bottomDashX.SetParent(GraphContainer);
        bottomDashX.gameObject.SetActive(true);
        bottomDashX.anchoredPosition = new Vector2(250, 0 * graphHeight);
        bottomDashX.localScale = new Vector3(1,1,1);
        gameObjectList.Add(bottomDashX.gameObject);
    }
    
    Vector2 increment = new Vector2(0, 10);

    public void CreateVectorComponent(Vector2 dotPosA, Vector2? dotPosB = null)
    {
        List<Vector2> circlePosList = new List<Vector2>();

        if (dotPosB == null)
        {
            foreach (GameObject item in gameObjectList)
            {
                if (item.name == "circle")
                    circlePosList.Add((item.GetComponent<RectTransform>().anchoredPosition - increment) / 2);
            }

            circlePosList.Add(dotPosA);
        }
        else
        {
            circlePosList.Add(dotPosA);
            circlePosList.Add(dotPosB.Value);   
        }

        ClearGraph();

        float lowestY = 0;

        for (int i = 0; i < circlePosList.Count; i++)
        {
            circlePosList[i] = circlePosList[i] * 2;

            if (circlePosList[i].y < lowestY)
                lowestY = circlePosList[i].y; 
        }

        for (int i = 0; i < circlePosList.Count; i++)
        {
            increment = new Vector2(0, Mathf.Abs(lowestY) + 10);
            Vector2 newPos = circlePosList[i] + increment;
            Debug.Log(newPos);
            CreateCircle(newPos);

            if (i != 0)
                CreateDotConnection(newPos, circlePosList[i - 1] + increment);
        }
    }

    public void CreateFreeBodyDiagramSimple(float mass, float upForce, float downForce, float rightForce, float leftForce, float frictionForce)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");
        FBD.Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass)}kg";

        TMP_Text upArrow = FBD.Find("ArrowUp").Find("Text (TMP)").GetComponent<TMP_Text>();
        TMP_Text downArrow = FBD.Find("ArrowDown").Find("Text (TMP)").GetComponent<TMP_Text>();
        TMP_Text rightArrow = FBD.Find("ArrowRight").Find("Text (TMP)").GetComponent<TMP_Text>();
        TMP_Text leftArrow = FBD.Find("ArrowLeft").Find("Text (TMP)").GetComponent<TMP_Text>();

        upArrow.text = $"F<sub>N</sub> = {ToDisplayString(upForce)}N";
        downArrow.text = $"F<sub>g</sub> = {ToDisplayString(downForce)}N";
        rightArrow.text = $"F<sub>A2</sub> = {ToDisplayString(rightForce)}N";
        leftArrow.text = $"F<sub>A1</sub> = {ToDisplayString(leftForce)}N";

        Transform frictionArrow;

        if (rightForce > leftForce)
        {
            frictionArrow = FBD.Find("ArrowFrictionLeft");
            FBD.Find("ArrowFrictionRight").gameObject.SetActive(false);
        }
        else
        {
            frictionArrow = FBD.Find("ArrowFrictionRight");
            FBD.Find("ArrowFrictionLeft").gameObject.SetActive(false);
        }

        frictionArrow.gameObject.SetActive(true);
        frictionArrow.Find("Text (TMP)").GetComponent<TMP_Text>().text = $"F<sub>f</sub> = {ToDisplayString(frictionForce)}";
    }

    // Arrows are sorted differently in this function || theta and degree symbols for copy+paste: θ °
    public void CreateFreeBodyDiagramVector(DynamicsObject problem, int questionNumber)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");
        FBD.Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(problem.mass)}kg";
        if (questionNumber < 2)
            GraphContainer.Find("Surface").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"θ = {ToDisplayString(problem.inclineAngle)}°";
        else
            GraphContainer.Find("Surface").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"θ = {ToDisplayString(problem.rightForce.angle.Value)}°";
        Transform ArrowsFolder = FBD.Find("ArrowsFolder");
        HideAllArrows(ArrowsFolder);

        // We need the arrows for forces Fgy and Fn by default
        ShowArrowOnFBD("Fgy", $"F<sub>gy</sub> = {ToDisplayString(Mathf.Abs(problem.downForce.adjacent.Value))}N");
        if (problem.upForce.adjacent > 0)
            ShowArrowOnFBD("Fn", $"F<sub>N</sub> = {ToDisplayString(Mathf.Abs(problem.upForce.adjacent.Value))}N");

        // Set default position for FBD
        RectTransform diagramRectTransform = FBD.GetComponent<RectTransform>();
        diagramRectTransform.pivot = new Vector2(0, 0);
        diagramRectTransform.localEulerAngles = new Vector3(0, 0, 0);
        diagramRectTransform.anchoredPosition = new Vector2(-48, -82);

        if (questionNumber == 0) // Problem 1: find acceleration when pushing object down slope to right
        {
            // We need to arrows for Ffleft, Fgxright, and Faright
            ShowArrowOnFBD("Ffleft", $"F<sub>f</sub> = {ToDisplayString(problem.friction.hypotenuse.Value)}N");
            ShowArrowOnFBD("Fgxright", $"F<sub>gx</sub> = {ToDisplayString(Mathf.Abs(problem.downForce.opposite.Value))}N");
            ShowArrowOnFBD("Faright", $"F<sub>A</sub> = {ToDisplayString(Mathf.Abs(problem.rightForce.adjacent.Value))}N");
            ArrowsFolder.Find("Faright").GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (questionNumber == 1) // Problem 2
        {
            // We need to arrows for Ffleft/Ffright, Fgxright, and Faleft
            if (problem.netForce.adjacent.Value > 0f)
                ShowArrowOnFBD("Ffleft", $"F<sub>f</sub> = {ToDisplayString(problem.friction.hypotenuse.Value)}N");
            else if (problem.netForce.adjacent.Value < 0f)
                ShowArrowOnFBD("Ffright", $"F<sub>f</sub> = {ToDisplayString(problem.friction.hypotenuse.Value)}N");

            ShowArrowOnFBD("Fgxright", $"F<sub>gx</sub> = {ToDisplayString(Mathf.Abs(problem.downForce.opposite.Value))}N");
            ShowArrowOnFBD("Faleft", $"F<sub>A</sub> = {ToDisplayString(Mathf.Abs(problem.leftForce.adjacent.Value))}N");
            ArrowsFolder.Find("Faright").GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (questionNumber == 2) // Problem 3
        {
            // We need to arrows for Ffleft, Fg, and Faright
            if (problem.friction.hypotenuse > 0)
                ShowArrowOnFBD("Ffleft", $"F<sub>f</sub> = {ToDisplayString(problem.friction.hypotenuse.Value)}N");
            ShowArrowOnFBD("Fgy", $"F<sub>gx</sub> = {ToDisplayString(Mathf.Abs(problem.downForce.adjacent.Value))}N");
            ShowArrowOnFBD("Faright", $"F<sub>A</sub> = {ToDisplayString(Mathf.Abs(problem.rightForce.hypotenuse.Value))}N");

            ArrowsFolder.Find("Faright").GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, problem.rightForce.angle.Value);
        }

        diagramRectTransform.pivot = new Vector2(1.65f, 0);
        diagramRectTransform.localEulerAngles = new Vector3(0, 0, -problem.inclineAngle);
        diagramRectTransform.anchoredPosition = new Vector2(115, -82);
    }

    public void CreateTensionDiagram(int mass1, int mass2, int problemNumber, float? angle)
    {
        //GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        FBD = GraphContainer.Find("FreeBodyDiagram");
        FBD.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass1)}kg";
        if (problemNumber < 2)
            FBD.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(mass2)}kg";

        if (problemNumber == 0)
        {
            float Ft1 = (mass1 + mass2) * 9.81f;
            float Ft2 = mass2 * 9.81f;

            ShowArrowOnFBD("Ft1", $"F<sub>T1</sub> = {ToDisplayString(Mathf.Abs(Ft1))}N");
            ShowArrowOnFBD("Ft2", $"F<sub>T2</sub> = {ToDisplayString(Mathf.Abs(Ft2))}N");   
        }
        else if (problemNumber == 1)
        {
            // Nothing to do here yet
        }
        else if (problemNumber == 2)
        {
            float Fty = mass1 * 9.81f / 2f; // Ft = mg / 2
            float Ft = Fty / Mathf.Sin(angle.Value);

            ShowArrowOnFBD("Ft1", $"F<sub>T1</sub> = {ToDisplayString(Ft)}N");
            ShowArrowOnFBD("Ft2", $"F<sub>T2</sub> = {ToDisplayString(Ft)}N");   
        }
        else if (problemNumber == 3)
        {
            float Fty = mass1 * 9.81f; // Ft = mg
            float Ft = Fty / Mathf.Sin(angle.Value);

            ShowArrowOnFBD("Ft1", $"F<sub>T</sub> = {ToDisplayString(Ft)}N");
        }
    }

    public void CreateCentreOfMassDiagram(int mass1, int mass2, float distance)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");
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

    public void CreateCollisionsDiagram(CollisionsObject currentCollisionsObject, string unknown)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");

        Transform Before = FBD.Find("Before");
        Transform After = FBD.Find("After");
        Transform BeforeArrows = Before.Find("ArrowsFolder");
        Transform AfterArrows = After.Find("ArrowsFolder");
        After.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentCollisionsObject.m1)}kg";
        After.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentCollisionsObject.m2)}kg";

        HideAllArrows(BeforeArrows);
        HideAllArrows(AfterArrows);

        if (currentCollisionsObject.explosionVi == 0f)
        {
            Before.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentCollisionsObject.m1)}kg";
            Before.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentCollisionsObject.m2)}kg";

            // Set arrows of before folder
            if (currentCollisionsObject.v1i < 0f)
            {
                BeforeArrows.Find("Left1").gameObject.SetActive(true);
                BeforeArrows.Find("Left1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>1i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v1i))}m/s";
            }
            else if (currentCollisionsObject.v1i > 0f)
            {
                BeforeArrows.Find("Right1").gameObject.SetActive(true);
                BeforeArrows.Find("Right1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>1i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v1i))}m/s";
            }
            if (currentCollisionsObject.v2i < 0f)
            {
                BeforeArrows.Find("Left2").gameObject.SetActive(true);
                BeforeArrows.Find("Left2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>2i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v2i))}m/s";
            }
            else if (currentCollisionsObject.v2i > 0f)
            {
                BeforeArrows.Find("Right2").gameObject.SetActive(true);
                BeforeArrows.Find("Right2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>2i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v2i))}m/s";
            }
        }
        else
        {
            Before.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentCollisionsObject.m1 + currentCollisionsObject.m2)}kg";

            // Set arrows of before folder
            if (currentCollisionsObject.explosionVi < 0f)
            {
                BeforeArrows.Find("Left1").gameObject.SetActive(true);
                BeforeArrows.Find("Left1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.explosionVi))}m/s";
            }
            else if (currentCollisionsObject.explosionVi > 0f)
            {
                BeforeArrows.Find("Right1").gameObject.SetActive(true);
                BeforeArrows.Find("Right1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>i</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.explosionVi))}m/s";
            }
        }

        // Set arrows of after folder
        if (currentCollisionsObject.v1f < 0f)
        {
            AfterArrows.Find("Left1").gameObject.SetActive(true);
            AfterArrows.Find("Left1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>1f</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v1f))}m/s";
        }
        else if (currentCollisionsObject.v1f > 0f)
        {
            AfterArrows.Find("Right1").gameObject.SetActive(true);
            AfterArrows.Find("Right1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>1f</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v1f))}m/s";
        }
        if (currentCollisionsObject.v2f < 0f)
        {
            AfterArrows.Find("Left2").gameObject.SetActive(true);
            AfterArrows.Find("Left2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>2f</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v2f))}m/s";
        }
        else if (currentCollisionsObject.v2f > 0f)
        {
            AfterArrows.Find("Right2").gameObject.SetActive(true);
            AfterArrows.Find("Right2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"V<sub>2f</sub> = {ToDisplayString(Mathf.Abs(currentCollisionsObject.v2f))}m/s";
        }

        // Setting the unknown value to ? on diagram
        if (unknown == "m<sub>1</sub>")
        {
            Before.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            After.Find("Mass1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }
        else if (unknown == "m<sub>2</sub>")
        {
            Before.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            After.Find("Mass2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }
        else if (unknown == "v<sub>1i</sub>")
        {
            if (BeforeArrows.Find("Left1").gameObject.activeInHierarchy)
                BeforeArrows.Find("Left1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            else
                BeforeArrows.Find("Right1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }
        else if (unknown == "v<sub>2i</sub>")
        {
            if (BeforeArrows.Find("Left2").gameObject.activeInHierarchy)
                BeforeArrows.Find("Left2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            else
                BeforeArrows.Find("Right2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }
        else if (unknown == "v<sub>1f</sub>")
        {
            if (AfterArrows.Find("Left1").gameObject.activeInHierarchy)
                AfterArrows.Find("Left1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            else
                AfterArrows.Find("Right1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }
        else if (unknown == "v<sub>2f</sub>")
        {
            if (AfterArrows.Find("Left2").gameObject.activeInHierarchy)
                AfterArrows.Find("Left2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
            else
                AfterArrows.Find("Right2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{unknown} = ?";
        }    
    }

    public void CreateElectricForceDiagram(ElectricForceObject currentElectricForceObject)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");

        Transform ArrowsFolder = FBD.Find("ArrowsFolder");
        FBD.Find("ChargeA").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricForceObject.chargeA * 1000000f)}µC";
        FBD.Find("ChargeB").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricForceObject.chargeB * 1000000f)}µC";
        ArrowsFolder.Find("Distance1").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricForceObject.dist1 * 100f)}cm";

        FBD.Find("ChargeC").gameObject.SetActive(false);
        ArrowsFolder.Find("Distance2").gameObject.SetActive(false);

        if (currentElectricForceObject.moreThanTwo)
        {
            FBD.Find("ChargeC").gameObject.SetActive(true);
            ArrowsFolder.Find("Distance2").gameObject.SetActive(true);

            FBD.Find("ChargeC").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricForceObject.chargeC * 1000000f)}µC";
            ArrowsFolder.Find("Distance2").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricForceObject.dist2 * 100f)}cm";
        } 
    }

    public void CreateElectricFieldDiagram(ElectricFieldObject currentElectricFieldObject)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");

        Transform ArrowsFolder = FBD.Find("ArrowsFolder");
        RectTransform ChargeDirection = ArrowsFolder.Find("ChargeDirection").GetComponent<RectTransform>();
        FBD.Find("Charge").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentElectricFieldObject.charge)}C";

        ArrowsFolder.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, currentElectricFieldObject.direction);

        if (currentElectricFieldObject.charge < 0)
            ChargeDirection.localEulerAngles = new Vector3(0, 0, 180);
        else
            ChargeDirection.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void CreateWireinaMagneticFieldDiagram(MagneticFieldObject currentMagneticFieldObject)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");

        float graphWidth = GraphContainer.sizeDelta.x;
        float graphHeight = GraphContainer.sizeDelta.y;
        float xCount = 7;
        float yCount = 7;
        float xIncrement = graphWidth * 0.9f / xCount;
        float yIncrement = graphHeight * 0.9f / yCount;

        Transform MagneticFieldFolder = FBD.Find("MagneticFieldFolder");
        RectTransform copyObject;

        Transform Wire = FBD.Find("Wire");
        Wire.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, currentMagneticFieldObject.wireDirection);
        Wire.Find("NegativeWireEnd").Find("MinusSymbol").GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, currentMagneticFieldObject.wireDirection);

        if (currentMagneticFieldObject.fieldFacingPlayer)
            copyObject = FBD.Find("MagneticFieldFolder").Find("DotTemplate").GetComponent<RectTransform>();
        else
            copyObject = FBD.Find("MagneticFieldFolder").Find("XTemplate").GetComponent<RectTransform>();

        ClearGraph();

        for (int ix = 0; ix < xCount; ix++)
        {
            for (int iy = 0; iy < yCount; iy++)
            {
                RectTransform label = Instantiate(copyObject);
                label.SetParent(MagneticFieldFolder, false);
                label.gameObject.SetActive(true);
                label.anchoredPosition = new Vector2((ix * xIncrement) + (graphWidth * 0.1f), (iy * yIncrement) + (graphHeight * 0.1f));
                label.localScale = new Vector3(1,1,1);
                gameObjectList.Add(label.gameObject);
            }
        }
    }

    public void CreateChargeinaMagneticFieldDiagram(MagneticFieldObject currentMagneticFieldObject)
    {
        FBD = GraphContainer.Find("FreeBodyDiagram");

        float graphWidth = GraphContainer.sizeDelta.x;
        float graphHeight = GraphContainer.sizeDelta.y;
        float xCount = 7;
        float yCount = 7;
        float xIncrement = graphWidth * 0.9f / xCount;
        float yIncrement = graphHeight * 0.9f / yCount;

        Transform MagneticFieldFolder = FBD.Find("MagneticFieldFolder");
        RectTransform copyObject;

        FBD.Find("Charge").Find("Text (TMP)").GetComponent<TMP_Text>().text = $"{ToDisplayString(currentMagneticFieldObject.charge)}C";
        FBD.Find("ArrowsFolder").Find("ChargeDirection").GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, currentMagneticFieldObject.wireDirection);

        if (currentMagneticFieldObject.fieldFacingPlayer)
            copyObject = FBD.Find("MagneticFieldFolder").Find("DotTemplate").GetComponent<RectTransform>();
        else
            copyObject = FBD.Find("MagneticFieldFolder").Find("XTemplate").GetComponent<RectTransform>();

        ClearGraph();

        for (int ix = 0; ix < xCount; ix++)
        {
            for (int iy = 0; iy < yCount; iy++)
            {
                RectTransform label = Instantiate(copyObject);
                label.SetParent(MagneticFieldFolder, false);
                label.gameObject.SetActive(true);
                label.anchoredPosition = new Vector2((ix * xIncrement) + (graphWidth * 0.1f), (iy * yIncrement) + (graphHeight * 0.1f));
                label.localScale = new Vector3(1,1,1);
                gameObjectList.Add(label.gameObject);
            }
        }
    }

    private void ShowArrowOnFBD(string arrowName, string textValue)
    {
        if (FBD == null)
            FBD = GraphContainer.Find("FreeBodyDiagram");
        Transform objectTransform = FBD.Find("ArrowsFolder").Find(arrowName);
        objectTransform.gameObject.SetActive(true);
        objectTransform.Find("Text (TMP)").GetComponent<TMP_Text>().text = textValue;
    }

    private void HideAllArrows(Transform folder)
    {
        foreach (Transform arrow in folder)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject newCircle = new GameObject("circle", typeof(Image));
        newCircle.transform.SetParent(GraphContainer, false);
        newCircle.GetComponent<Image>().sprite = CircleSprite;

        RectTransform circleTransform = newCircle.GetComponent<RectTransform>();
        circleTransform.sizeDelta = new Vector2(12, 12);
        circleTransform.anchoredPosition = anchoredPosition;
        circleTransform.anchorMin = new Vector2(0, 0);
        circleTransform.anchorMax = new Vector2(0, 0);

        gameObjectList.Add(newCircle);

        return newCircle;
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        Vector2 dir = dotPosB - dotPosA;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float dist = Vector2.Distance(dotPosA, dotPosB);

        GameObject newObject = new GameObject("dotConnection", typeof(Image));
        newObject.transform.SetParent(GraphContainer, false);
        newObject.GetComponent<Image>().color = new Color(1,1,1, .5f);

        RectTransform rectTransform = newObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(dist, 3f);
        rectTransform.anchoredPosition = (dotPosA + dotPosB) / 2;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        gameObjectList.Add(newObject);
    }

    public void GraphVisible()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
