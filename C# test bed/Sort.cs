using System;
using System.Numerics;
using Catalog;

namespace Sort
{
	public class TestClass
	{
		public static void Test()
		{
			int[] nums = [14, 53, 1, 9, 20];
            Sorter.BubbleSort<int>(nums);

			Console.WriteLine(new Catalog<int>(nums));


        }
	}

	public class Sorter
	{
		public static void BubbleSort<T>(T[] values) where T : INumber<T>
		{
			while (true)
			{
                bool changed = false;
                for (int ii = 0; ii < values.Length - 1; ii++)
                {
                    if (values[ii] > values[ii + 1])
                    {
                        (values[ii + 1], values[ii]) = (values[ii], values[ii + 1]);
                        changed = true;
                    }
                }
                if (!changed) return;
            }
		}
	}
}
