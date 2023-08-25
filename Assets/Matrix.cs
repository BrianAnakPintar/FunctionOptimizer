using System.Collections.Generic;
using System.Linq;

public class Matrix
{
    public int rows;
    public int cols;

    public double[,] values;

    // This constructor creates an r x c dimension matrix. Where r = rows, and c = cols.
    public Matrix(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        values = new double[rows, cols];
    }

    // MODIFIES: this
    // EFFECTS: Sets the rowNum row as vals.
    public void SetRow(int rowNum, List<double> vals)
    {
        for (int i = 0; i < cols; i++)
        {
            values[rowNum, i] = vals[i];
        }
    }

    public List<double> GetRow(int rowNum)
    {
        List<double> result = new List<double>();

        for (int i = 0; i < cols; i++)
        {
            result.Add(values[rowNum, i]);
        }
        
        return result;
    }
    
    // EFFECTS: returns the transposed version of the current matrix.
    public Matrix Transpose()
    {
        Matrix resultMatrix = new Matrix(cols, rows);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                resultMatrix.values[j, i] = values[i, j];
            }
        }
        return resultMatrix;
    }

    // EFFECTS: If this matrix is a square then it returns true.
    private bool IsSquareMatrix()
    {
        return rows == cols;
    }
}
