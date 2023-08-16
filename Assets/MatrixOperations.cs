using System;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixOperations
{
    // Multiplies matrix A and matrix B (AB). In that specified order.
    public static Matrix MatrixMultiply(Matrix A, Matrix B)
    {
        if (A.cols != B.rows)
        {
            throw new Exception("Invalid Matrix multiplication");
        }

        Matrix resultMatrix = new Matrix(A.rows, B.cols);
        
        // For every row in A
        for (int i = 0; i < A.rows; i++)
        {
            List<double> rowVals = new List<double>();
            // And for every row B
            for (int j = 0; j < B.cols; j++)
            {
                double result = 0;
                // We will compute the dot product here
                for (int k = 0; k < A.cols; k++)
                {
                    result += A.values[i, k] * B.values[k, j];
                }
                rowVals.Add(result);
            }
            resultMatrix.SetRow(i, rowVals);
        }
        return resultMatrix;
    }

    // MODIFIES: matrix
    private static Matrix SwapRows(Matrix matrix, int row1, int row2)
    {
        List<double> r1Vals = new List<double>();
        List<double> r2Vals = new List<double>();
        for (int i = 0; i < matrix.cols; i++)
        {
            r1Vals.Add(matrix.values[row1, i]);
            r2Vals.Add(matrix.values[row2, i]);
        }
        
        matrix.SetRow(row1, r2Vals);
        matrix.SetRow(row2, r1Vals);
        
        return matrix;
    }
    
    // MODIFIES: matrix
    private static Matrix ScaleRow(Matrix matrix, int row, double factor)
    {
        for (int i = 0; i < matrix.cols; i++)
        {
            matrix.values[row, i] *= factor;
        }
        
        return matrix;
    }

    // MODIFIES: this
    // Adds row 1 by factor * row 2
    private static Matrix RowAddition(Matrix matrix, int row1, int row2, double factor)
    {
        List<double> r1Vals = new List<double>();
        for (int i = 0; i < matrix.cols; i++)
        {
            r1Vals.Add(matrix.values[row1, i] + factor * matrix.values[row2, i]);
        }
        matrix.SetRow(row1, r1Vals);
        
        return matrix;   
    }
    
    // Turns matrix to it's RREF form
    public static Matrix RREF(Matrix A)
    {
        return RrefHelper(A, 0, 0);
    }

    // Recursive helper method to help find the RREF.
    private static Matrix RrefHelper(Matrix A, int row, int col)
    {
        Matrix temp = A;
        // BASE CASE
        if (col >= temp.cols || row >= temp.rows)
        {
            // STAGE 2 (REF -> RREF) | If we just simply return temp here it's in REF.
            
            for (int c = col - 1; c > 0; c--)
            {
                // As long as our factor is not 1/0
                double factorInv = temp.values[row - 1, col - 1];
                if (factorInv == 0)
                {
                    continue;
                }
                temp = ScaleRow(temp, row-1, 1/factorInv);
                for (int r = row - 1; r > 0; r--)
                {
                    double factor = temp.values[r-1, c] / temp.values[row - 1, col - 1];
                    temp = RowAddition(temp, r - 1, row - 1, -factor);
                }
                
                row--;
                col--;
            }

            return temp;
        }
        
        // #1 Ensure that (row,col) is not 0, OR ensure that the entirety of the row is 0.
        if (temp.values[row, col] == 0)
        {
            // If it is a zero, we should find a column that's not 0 and start from there.
            for (int i = row; i < A.rows - row; i++)
            {
                if (temp.values[i, col] != 0)
                {
                    temp = SwapRows(A, row, i);
                    break;
                }
            }
        }
        
        // #2 Row replacement
        for (int i = row + 1; i < temp.rows; i++)
        {
            // Special case where the entire row is 0.
            if (temp.values[row, col] == 0)
            {
                break;
            }
            double factor = temp.values[i, col] / temp.values[row, col];
            temp = RowAddition(A, i, row, -factor);
        }
        
        // #3 Repeat for smaller matrix
        int newRow = row + 1;
        int newCol = col + 1;
        
        return RrefHelper(temp, newRow, newCol);
    }
} 
