using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphingController : MonoBehaviour
{
    [SerializeField] private Sprite pointSprite;
    private RectTransform graph;

    private List<DoubleVector> pointsArr = new List<DoubleVector>();

    public LineRenderer lr;
    
    [SerializeField] private GameObject pointInfoGUI;
    [SerializeField] private GameObject pointsContainer;

    [SerializeField] private TMP_InputField inputField;
    
    void Awake()
    {
        graph = transform.Find("Graph").GetComponent<RectTransform>();
        
        CreatePoint(new Vector2(5,5));
        CreatePoint(new Vector2(50, 20));
    }


    // This draws the point onto the scene.
    private void CreatePoint(Vector2 anchoredPos)
    {
        GameObject go = new GameObject("point", typeof(Image));
        go.transform.SetParent(graph, false);
        go.GetComponent<Image>().sprite = pointSprite;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(10, 10);
    }
    
    private void DrawLine(Vector2 pointA, Vector2 pointB)
    {
        lr.SetPosition(0, pointA);
        lr.SetPosition(1, pointB);
    }

    // Scans the text input for the point and ensuring it is correctly formatted.
    public void GetTextInput()
    {
        string text = inputField.text;
        double d1;
        double d2;
        // #1 remove any additional whitespaces and check proper formatting.
        text = text.Trim();
        
        if (checkFormat(text))
        {
            // Here we remove the brackets and find index of the comma.
            text = text.Substring(1, text.Length - 2);
            int split = text.IndexOf(",");

            string numberString1 = text.Substring(0, split);
            string numberString2 = text.Substring(split + 1, text.Length - split- 1);
            try
            {
                d1 = Convert.ToDouble(numberString1.Trim());
                d2 = Convert.ToDouble(numberString2.Trim());
                // Here we want to store d1, d2 as a list of doubles.
                DoubleVector dv = new DoubleVector(d1, d2);
                pointsArr.Add(dv);
                Debug.Log("Appended");
            }
            catch (FormatException e)
            {
                SendWarning();
                return;
            }
            // Here we want to create a point "Graphically".
            CreatePoint(new Vector2((float) d1, (float) d2));
            // Now we want to add that into the GUI.
            GameObject point = Instantiate(pointInfoGUI, pointsContainer.transform);
            point.GetComponentInChildren<TextMeshProUGUI>().text = "(" + d1 + "," + d2 + ").";
        }
        else
        {
            SendWarning();
        }
    }
    
    

    private bool checkFormat(string str)
    {
        return str.StartsWith("(") && str.EndsWith(")") && str.Contains(",");
    }
    
    // TODO: Sends a warning GUI message saying invalid format.
    private void SendWarning()
    {
        Debug.Log("invalid format, please fix it.");
    }

    public void generateModel()
    {
        Debug.Log("GENERATING");
        // Here we use the example of solving a linear model, y = mx + b.
        
        // Ensure that there is at least 3 items in our list
        if (pointsArr.Count < 3)
        {
            Debug.Log(pointsArr.Count());
            Debug.Log("LOL");
            return;
        }

        
        Matrix xMatrix = new Matrix(pointsArr.Count,2);
        Matrix yMatrix = new Matrix(pointsArr.Count, 1);

        for (int i = 0; i < pointsArr.Count; i++)
        {
            List<double> xList = new List<double>();
            xList.Add(pointsArr[i].getXPoint());
            xList.Add(1);
            xMatrix.SetRow(i, xList);
            
            List<double> yAns = new List<double>();
            yAns.Add(pointsArr[i].getYPoint());
            yMatrix.SetRow(i, yAns);
        }
        
        // Now we "solve"
        Matrix xTrans = xMatrix.Transpose();
        Matrix resultMatrix = MatrixOperations.MatrixMultiply(xTrans, xMatrix);
        Matrix yResult = MatrixOperations.MatrixMultiply(xTrans, yMatrix);
        // It's best to combine these two matrices and do RREF
        Matrix final = MatrixOperations.appendMatrix(resultMatrix, yResult);
        final = MatrixOperations.RREF(final);
        Vector2 pointA = new Vector2(-500f, linearFunction(-500f, final.values[0, 2], final.values[1, 2]));
        Vector2 pointB = new Vector2(500f, linearFunction(500f, final.values[0, 2], final.values[1, 2]));
        
        DrawLine(pointA, pointB);
    }

    private float linearFunction(float x, double m, double b)
    {
        return (float)m * x + (float)b;
    }
}

public class DoubleVector
{
    private double xPoint;
    private double yPoint;
    
    public DoubleVector(double d1, double d2)
    {
        xPoint = d1;
        yPoint = d2;
    }

    public double getXPoint()
    {
        return xPoint;
    }

    public double getYPoint()
    {
        return yPoint;
    }
}