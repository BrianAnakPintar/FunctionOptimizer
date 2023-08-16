using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GraphingController : MonoBehaviour
{
    [SerializeField] private Sprite pointSprite;
    private RectTransform graph;

    public DoubleVector[] pointsArr;

    [SerializeField] private InputField inputField;
    
    void Awake()
    {
        graph = transform.Find("Graph").GetComponent<RectTransform>();
        
        CreatePoint(new Vector2(5,5));
        CreatePoint(new Vector2(50, 20));
    }


    private void CreatePoint(Vector2 anchoredPos)
    {
        GameObject go = new GameObject("point", typeof(Image));
        go.transform.SetParent(graph, false);
        go.GetComponent<Image>().sprite = pointSprite;
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(10, 10);
    }

    // Scans the text input for the point and ensuring it is correctly formatted.
    public void GetTextInput()
    {
        string text = inputField.text;
        // #1 remove any additional whitespaces and check proper formatting.
        text = text.Trim();

        Vector2 point;
        if (checkFormat(text))
        {
            // Here we remove the brackets and find index of the comma.
            text = text.Substring(1, text.Length - 2);
            int split = text.IndexOf(",");

            string numberString1 = text.Substring(0, split);
            string numberString2 = text.Substring(split + 1, text.Length - split- 1);
            try
            {
                double d1 = Convert.ToDouble(numberString1.Trim());
                double d2 = Convert.ToDouble(numberString2.Trim());
                // Here we want to store d1, d2 as a list of doubles.
                DoubleVector dv = new DoubleVector(d1, d2);
                pointsArr.Append(dv);
            }
            catch (FormatException e)
            {
                SendWarning();
                return;
            }

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