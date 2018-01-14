using System;

public static class LinearEquationSolver
{
	public static float MatrixDeterminant(float[,] matrix)
	{
		int[] perm;
		int toggle;
		float[,] lum = MatrixDecompose(matrix, out perm, out toggle);
		if (lum == null)
			throw new Exception("Unable to compute MatrixDeterminant");
		float result = toggle;
		for (int i = 0; i < lum.GetLength(0); ++i)
			result *= lum[i, i];

		return result;
	}

	private static float[,] MatrixCreate(int rows, int cols)
	{
		float[,] result = new float[rows, cols];
		return result;
	}

	private static float[,] MatrixDecompose(float[,] matrix, out int[] perm, out int toggle)
	{
		int rows = matrix.GetLength(0);
		int cols = matrix.GetLength(1);

		if (rows != cols)
			throw new Exception("Attempt to MatrixDecompose a non-square mattrix");

		float[,] result = MatrixDuplicate(matrix);

		perm = new int[rows];
		for (int i = 0; i < rows; ++i) { perm[i] = i; }

		toggle = 1;

		for (int j = 0; j < rows - 1; ++j)
		{
			float colMax = Math.Abs(result[j, j]);
			int pRow = j;
			for (int i = j + 1; i < rows; ++i)
			{
				if (result[i, j] > colMax)
				{
					colMax = result[i, j];
					pRow = i;
				}
			}

			if (pRow != j)
			{
				float[] rowPtr = new float[result.GetLength(1)];
				for (int k = 0; k < result.GetLength(1); k++)
				{
					rowPtr[k] = result[pRow, k];
				}

				for (int k = 0; k < result.GetLength(1); k++)
				{
					result[pRow, k] = result[j, k];
				}

				for (int k = 0; k < result.GetLength(1); k++)
				{
					result[j, k] = rowPtr[k];
				}

				int tmp = perm[pRow];
				perm[pRow] = perm[j];
				perm[j] = tmp;

				toggle = -toggle;
			}

			if (Math.Abs(result[j, j]) < 1.0E-20)
				return null;

			for (int i = j + 1; i < rows; ++i)
			{
				result[i, j] /= result[j, j];
				for (int k = j + 1; k < rows; ++k)
				{
					result[i, k] -= result[i, j] * result[j, k];
				}
			}
		}
		return result;
	}

	private static float[,] MatrixDuplicate(float[,] matrix)
	{
		float[,] result = MatrixCreate(matrix.GetLength(0), matrix.GetLength(1));
		for (int i = 0; i < matrix.GetLength(0); ++i)
			for (int j = 0; j < matrix.GetLength(1); ++j)
				result[i, j] = matrix[i, j];
		return result;
	}
}