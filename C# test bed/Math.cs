using System;
using System.Numerics;
using Microsoft.VisualBasic;
public class Math
{
	public class TestClass
	{
		public static void Test()
		{
            object?[] nums = [ 'a', 2, "one", false, 1213.78f, true, null];
            foreach (object? num in nums)
            {
                try
                {
                    Console.WriteLine(ToNumber<float>(num));
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }
    }

    static bool IsEven_Funny(int num)
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

    static bool IsEven<T>(T num) where T : INumber<T>
    {
        string asString = num.ToString();
        if (asString.Contains('.')) return false;
        return T.Parse(asString[^1].ToString(), null) % T.Parse("2", null) == T.Parse("0", null);
    }

    static bool IsEven(object? value)
    {
        if (!Information.IsNumeric(value)) throw new FormatException($"'{value}' is not numeric.");

        string asString = value.ToString();
        if (asString.Contains('.')) return false;

        switch (asString[^1])
        {
            case '1':
            case '3':
            case '5':
            case '7':
            case '9':
                return false;
            default:
                return true;
        }
    }

    static T ToNumber<T>(object? value) where T : INumber<T>
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
