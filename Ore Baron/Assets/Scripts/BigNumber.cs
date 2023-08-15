using System;
using System.Collections;

[Serializable]
public struct BigNumber: IEquatable<BigNumber>, IComparable<BigNumber>
{
    public int[] N;
    public int this[int index]
    {
        get => N[index];
        set => N[index] = value;
    }

    public BigNumber(int[] ints)
    {
        N = new int[10];
        for (int i = 0; i < N.Length; i++)
        {
            N[i] = ints[N.Length - 1 - i];
        }
        Count();
    }

    public BigNumber(int size)
    {
        N = new int[size];
    }

    public BigNumber(int index, int amount)
    {
        N = new int[10];
        N[index] = amount;
        Count();
    }

    public override string ToString()
    {
        if (N[9] > 0) return $"{N[9]},{N[8]:d3}Oc";
        else if (N[8] > 0) return $"{N[8]},{N[7]:d3}Sp";
        else if (N[7] > 0) return $"{N[7]},{N[6]:d3}Sx";
        else if (N[6] > 0) return $"{N[6]},{N[5]:d3}Qi";
        else if (N[5] > 0) return $"{N[5]},{N[4]:d3}Qa";
        else if (N[4] > 0) return $"{N[4]},{N[3]:d3}T";
        else if (N[3] > 0) return $"{N[3]},{N[2]:d3}B";
        else if (N[2] > 0) return $"{N[2]},{N[1]:d3}M";
        else if (N[1] > 0) return $"{N[1]},{N[0]:d3}k";
        else return $"{N[0]}";
    }

    public void Count()
    {
        for (int i = 0; i < N.Length - 1; i++)
        {
            if (N[i] >= 1000)
            {
                N[i + 1] += N[i] / 1000;
                N[i] %= 1000;
            }
            else if (N[i] < 0) N[i] = 0;
        }
        if (N[9] < 0) N[9] = 0;
    }

    public BigNumber Increase(int power)
    {
        BigNumber temp = new BigNumber(10);
        for (int i = 0; i < N.Length; i++)
        {
            if (i + power < N.Length)
            {
                temp[i + power] = N[i];
            }
        }
        this = temp;
        return temp;
    }

    public BigNumber Power(int power)
    {
        BigNumber temp = new BigNumber(10);
        temp = this;
        if (power == 0)
        {
            temp[0] = 1;
        }
        else if (power == 1)
        {
            // Nothing
        }
        else
        {
            for (int i = 0; i < power - 1; i++)
            {
                temp *= this;
            }
        }
        
        this = temp;
        return temp;
    }

    public bool Equals(BigNumber other)
    {
        if (N.Length != other.N.Length) return false;
        for (int i = 0; i < N.Length; i++)
        {
            if (N[i] == N[i]) continue;
            else return false;
        }
        return true;
    }

    public int CompareTo(BigNumber other)
    {
        if (N.Length != other.N.Length) return 0;
        for (int i = N.Length - 1; i >= 0; i--)
        {
            if (N[i] > other.N[i]) return 1;
            else if (N[i] < other.N[i]) return -1;
        }
        return 0;
    }

    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        BigNumber n = new BigNumber(new int[10]);

        for (int i = 0; i < n.N.Length; i++)
        {
            n[i] = a[i] + b[i];
        }

        n.Count();

        return n;
    }
    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        BigNumber temp = new BigNumber(new int[10]);

        for (int i = 0; i < temp.N.Length; i++)
        {
            if (a[i] >= b[i]) temp[i] = a[i] - b[i];
            else
            {
                if (i != temp.N.Length - 1)
                {
                    temp[i] = 1000 + a[i] - b[i];
                    a[i + 1]--;
                }
                else a[i] = 0;
            }
        }

        temp.Count();

        return temp;
    }
    public static BigNumber operator *(int a, BigNumber b)
    {
        BigNumber n = new BigNumber(new int[10]);

        for (int i = 0; i < n.N.Length; i++)
        {
            n[i] = a * b[i];
        }

        n.Count();

        return n;
    }
    public static BigNumber operator *(BigNumber b, int a)
    {
        BigNumber n = new BigNumber(new int[10]);

        for (int i = 0; i < n.N.Length; i++)
        {
            n[i] = a * b[i];
        }

        n.Count();

        return n;
    }

    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        BigNumber n = new BigNumber(new int[10]);
        BigNumber result = new BigNumber(new int[10]);

        for (int i = 0; i < b.N.Length; i++)
        {
            result += (a * b[i]).Increase(i);
        }

        result.Count();

        return result;
    }
}
