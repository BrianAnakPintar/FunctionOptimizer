using System;
using System.Collections.Generic;
using UnityEngine;

public class FunctionOptimizer : MonoBehaviour
{
    private Matrix matrixA = new Matrix(2, 3);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            test();
        }
    }

    void test()
    {
        List<double> r1 = new List<double>()
        {
            1f, 2f, 3f
        };
        List<double> r2 = new List<double>()
        {
            3f, 4f, 5f
        };
        List<double> r3 = new List<double>()
        {
            5f, 6f
        };
        matrixA.SetRow(0, r1);
        matrixA.SetRow(1, r2);
        // matrixA.SetRow(2, r3);
        
        // Matrix ans = MatrixOperations.RREF(matrixA);

        Matrix ans = MatrixOperations.RREF(matrixA);
        for (int i = 0; i < ans.rows; i++)
        {
            for (int j = 0; j < ans.cols; j++)
            {
                Debug.Log(ans.values[i,j]);
            }
        }
    }
}
