public static class LinearSolver
{
	public static float[] Solve(float[,] g, float[] i)
	{
		float? determinant = CalculateDeterminant(g);
		if (determinant == null || determinant == 0)
			return null;

		int l = i.Length;
		float[] v = new float[l];
		for (int x = 0; x < l; x++)
		{
			float[,] w = (float[,])g.Clone();
			for (int y = 0; y < l; y++)
				w[x, y] = i[y];
			float? determinantV = CalculateDeterminant(w);
			if (determinantV == null)
				return null;

			v[x] = (float)(determinantV / determinant);
		}
		return v;
	}

	private static float? CalculateDeterminant(float[,] matrix)
	{
		int length = matrix.GetLength(0);
		if (length < 3)
			return null;

		float determinant = 0;
		for (int x = 0; x < length; x++)
		{
			float local = 1f;
			for (int y = 0; y < length; y++)
			{
				local *= matrix[Repeat(x + y, length), y];
			}
			determinant += local;
		}

		for (int x = 0; x < length; x++)
		{
			float local = 1f;
			for (int y = 0; y < length; y++)
			{
				local *= matrix[Repeat(x - y, length), y];
			}
			determinant -= local;
		}

		return determinant;
	}

	private static int Repeat(int value, int mod)
	{
		return (value % mod + mod) % mod;
	}
}
