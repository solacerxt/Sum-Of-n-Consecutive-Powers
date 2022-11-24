int p = int.Parse(Console.ReadLine()!);

var a = new Fraction64[p + 2];
var binom = new long[p + 2, p + 2];

for (var i = 0; i < p + 2; ++i)
{
    binom[i, 0] = binom[i, i] = 1;

    for (var j = 1; j < i; ++j)
        binom[i, j] = binom[i - 1, j] + binom[i - 1, j - 1];
}

a[p + 1] = (1, p + 1);

for (var i = p; i > 0; --i)
{
    a[i] = (0, 1);
    var sign = 1;
    for (var m = i + 1; m <= p + 1; ++m)
    {
        a[i] += sign * binom[m, i - 1] * a[m];
        sign *= -1;
    }

    a[i] /= i;
}

Console.Write($"{a[p + 1]}*n^{p + 1}");
for (int i = p; i > 0; --i)
{
    if (a[i].Numerator != 0)
        Console.Write($" + {a[i]}*n{(i > 1 ? ("^" + i) : "")}");
}

Console.WriteLine();

struct Fraction64
{
    public long Numerator { get; private set; }
    public long Denominator { get; private set; }

    public Fraction64(long numerator, long denominator)
    {
        Numerator = Math.Sign(denominator) * numerator;
        Denominator = Math.Abs(denominator);

        Reduce();
    }

    private void Reduce()
    {
        var gcd = GCD(Math.Abs(Numerator), Denominator);

        Numerator /= gcd;
        Denominator /= gcd;
    }

    public static Fraction64 operator *(long multiplier, Fraction64 fraction)
    {
        var gcd = GCD(Math.Abs(multiplier), fraction.Denominator);
        multiplier /= gcd;
        fraction.Numerator *= multiplier;
        fraction.Denominator /= gcd;

        return fraction;
    }

    public static Fraction64 operator /(Fraction64 fraction, long divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException();

        var gcd = GCD(Math.Abs(fraction.Numerator), Math.Abs(divisor));

        fraction.Numerator = Math.Sign(divisor) * fraction.Numerator / gcd;
        divisor = Math.Abs(divisor) / gcd;
        fraction.Denominator *= divisor;

        return fraction;
    }

    public static Fraction64 operator *(Fraction64 a, Fraction64 b)
    {
        return a.Numerator * b / a.Denominator;
    }

    public static Fraction64 operator +(Fraction64 a, Fraction64 b)
    {
        var d = a.Denominator * b.Denominator / GCD(a.Denominator, b.Denominator);
        var n = a.Numerator * (d / a.Denominator) + b.Numerator * (d / b.Denominator);
        var gcd = GCD(Math.Abs(n), d);

        return new Fraction64(n / gcd, d / gcd);
    }

    public static Fraction64 operator +(Fraction64 a, long b) => a + new Fraction64(b, 1);

    public static implicit operator Fraction64((long, long) fraction)
    {
        return new Fraction64(fraction.Item1, fraction.Item2);
    }

    public override string ToString()
    {
        var n = Numerator;
        var d = Denominator;
        return n % d == 0 ? $"{n / d}" : $"{Numerator}/{Denominator}";
    } 

    private static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}
