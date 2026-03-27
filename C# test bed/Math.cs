using System;
using System.Numerics;
public class Math
{
	public class TestClass
	{
		public static void Test()
		{
            double[] nums = [ -1273, 2, -143, 0, 1213.78, 1, 534];
            foreach (double num in nums)
            {
                Console.WriteLine($"{num}: {IsEven(num)}");
                
            }
        }
    }

    public static bool IsEven_Funny(int num)
    {
        if (num < 0)
        {
            num *= -1;
        }
        for (int i = 0; i <= num; i += 2)
        {
            if (i == num)
            {
                return true;
            }
        }
        return false;

    }

    public static bool IsEven<T>(T num) where T : INumber<T>
    {
        string asString = num!.ToString();
        if (asString!.Contains('.')) return false;
        return int.Parse(asString[^1].ToString(), null) % 2 == 0;
    }

    public static T ToNumber<T>(object? value) where T : INumber<T>
    {
        if (value is null || value is false) return T.Parse("0", null);
        if (value is true) return T.Parse("1", null);
        try
        {
            return T.Parse(value.ToString(), null);
        } catch
        {
            throw new FormatException($"Cannot convert '{value}' to '{typeof(T)}'");
        }
    }
}
