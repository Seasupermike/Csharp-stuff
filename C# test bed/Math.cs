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
                Console.WriteLine($"{num}: {IsEven<double>(num)}");
                
            }

            ToNumber<float>(1);
        }
    }

    /// <summary>Checks if a number is even</summary>
    /// <param name="num">The number being checked</param>
    /// <returns>If num is true or false</returns>
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

    /// <summary>Checks if a number is even</summary>
    /// <typeparam name="T">Type of the numeber being checked</typeparam>
    /// <param name="num">The number being checked</param>
    /// <returns>If num is true or false</returns>
    public static bool IsEven<T>(T num) where T : INumber<T>
    {
        string asString = num.ToString()!;
        if (asString.Contains('.')) return false;
        return int.Parse(asString[^1].ToString(), null) % 2 == 0;
    }

    /// <summary>Coverts a value to a numeric type</summary>
    /// <typeparam name="T">Type to convert value to</typeparam>
    /// <param name="value">The value to be converted</param>
    /// <returns>The converted value</returns>
    /// <exception cref="FormatException"/>
    /// <exception cref="OverflowException"/>
    public static T ToNumber<T>(object? value) where T : INumber<T>
    {
        if (value is null || value is false) return T.Parse("0", null);
        if (value is true) return T.Parse("1", null);
        try
        {
            return T.Parse(value.ToString(), null);
        } catch (FormatException e)
        {
            throw new FormatException($"Cannot convert '{value}' to '{typeof(T)}'");
        } catch (OverflowException e)
        {
            throw new OverflowException($"Converting '{value}' to '{typeof(T)}' causes overflow");
        }
    }
}
