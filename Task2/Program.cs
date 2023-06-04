
class  Program
{
    static void Main()
    {
        int m, n, new_try;
        double x, a, b;
        Console.WriteLine("Задача алгебраического интерполирования\nВариант 1");
        Console.WriteLine("Введите число значений в таблице m");
        m = Convert.ToInt32(Console.ReadLine());
        double[] nodes = new double[m];
        double[] values = new double[m];
        List<double> values2 = new List<double>();
        Console.WriteLine("Введите [a,b]");
        a = Convert.ToDouble(Console.ReadLine());
        b = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите точку интерполирования x=");
        x = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите n=");
        do
        {
            n = Convert.ToInt32(Console.ReadLine());
            if (n > m - 1) Console.WriteLine("Введено недопустимое значение n");
        } while (n > m - 1);
        double h = (b - a) / (m - 1);
        Console.WriteLine($"h={h}");
        
        for (int j = 0; j <= m - 1; j++)
        {
            nodes[j] = a + j * h;
            values[j] = Function(nodes[j]);
        }

        Console.WriteLine(" Узлы \t\t Значения ");
        for (int i = 0; i <= m - 1; i++)
        {
            Console.WriteLine($" {nodes[i]:F2}\t\t {values[i]:F2}");
        }

        List<double> nodeList = new List<double>(nodes);
        List<double> sortList = new List<double>(SortNodes(nodeList, x, n));

        Console.WriteLine(" Отсортированная таблица узлов ");
        for (int i = 0; i < sortList.Count; i++)
        {
            values2.Add(Function(sortList[i]));
            Console.WriteLine($" {sortList[i]:F2}\t\t {values2[i]:F2}");
        }

        LagrangePolynomial(sortList, values2, x);
        NewtonPolynomial(sortList, values2, x);

        Console.WriteLine("Введите 1, если хотите изменить значения x,n\nВведите любое другое число, если хотите завершить");
        while ((new_try = Convert.ToInt32(Console.ReadLine())) == 1)
        {
            Call( m, nodeList, sortList, values2);
            Console.WriteLine("Введите 1, если хотите изменить значения x,n\nВведите любое другое число, если хотите завершить");
        }

    }
    static void Call(int m,List<double> nodeList, List<double> sortList, List<double> values2)
    {
        Console.Write("Введите новое значение x=");
        double x = Convert.ToDouble(Console.ReadLine());
        int n;
        Console.Write("Введите новое значение n=");
        do
        {
            n = Convert.ToInt32(Console.ReadLine());
            if (n > m - 1) Console.WriteLine("Введено недопустимое значение n");
        } while (n > m - 1);

        sortList = new List<double>(SortNodes(nodeList, x, n));
        values2.Clear();
        Console.WriteLine(" Отсортированная таблица узлов ");
        for (int i = 0; i < sortList.Count; i++)
        {
            values2.Add(Function(sortList[i]));
            Console.WriteLine($" {sortList[i]:F2}\t\t {values2[i]:F2}");
        }

        LagrangePolynomial(sortList, values2, x);
        NewtonPolynomial(sortList, values2, x);
    }
    static List<double> SortNodes(List<double> nodes, double x, int n)
    {
        List<Tuple<double, double>> distances = nodes.Select(node => Tuple.Create(Math.Abs(node - x), node)).ToList(); 
        distances.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        List<double> selectedNodes = distances.Select(node => node.Item2).Take(n + 1).ToList();
        return selectedNodes;
    }
    static double Function(double x)
   {
       return Math.Sin(x) - Math.Pow(x, 2) / 2;
   }
    static void LagrangePolynomial(List<double> sortList, List<double> values2, double x)
    {
        double result = 0;
        for (int i = 0; i < sortList.Count; i++)
        {
            double term = 1;
            for (int j = 0; j < sortList.Count; j++)
            {
                if (i != j) term *= (x - sortList[j]) / (sortList[i] - sortList[j]);
            }
            result += values2[i] * term;
        }
        Console.WriteLine($"Значение интерполяционного многочлена в форме Лагранжа при x={x}: {result}");
        Console.WriteLine($"Абсолютная фактическая погрешность для формы Лагранжа при x={x}: {Math.Abs(Function(x)-result)}");
    }
    static List<double> DividedDifferences(List<double> X, List<double> Y, int n)
    {
        List<double> F = Y.ToList();
        for (int i = 0; i < X.Count; i++)
            F[i] = Y[i];

        for (int j = 1; j <= n; j++)
        {
            for (int i = X.Count - 1; i >= j; i--)
            {
                F[i] = (F[i] - F[i - 1]) / (X[i] - X[i - j]);
            }
        }

        return F;
    }
    static void NewtonPolynomial(List<double> sortList, List<double> values2, double x)
    {
        double result = values2[0];
        double F;
        List<double> dividedDifferences = DividedDifferences(sortList, values2, sortList.Count - 1);

        for (int i = 1; i < sortList.Count; i++)
        {
            F = 1;
            for (int j = 0; j < i; j++)
                F *= (x - sortList[j]);
            result += F * dividedDifferences[i];
        }
        Console.WriteLine($"Значение интерполяционного многочлена в форме Ньютона при x={x}: {result}");
        Console.WriteLine($"Абсолютная фактическая погрешность для формы Ньютона при x={x}: {Math.Abs(Function(x)-result)}");
    }
}