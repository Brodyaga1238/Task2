
class  Program
{
    static void Main()
    {
       int m,n,new_try;
       double x, a, b;
       m = 21;
       a = 0;
       b = 1;
       n = 9;
       
       Console.WriteLine("Задача алгебраического интерполирования\nВариант 1");
       Console.WriteLine("Введите число значений в таблице m");
       // m =Convert.ToInt32(Console.ReadLine());
       Console.WriteLine("Введите [a,b]");
       //a =Convert.ToInt32(Console.ReadLine());
       //b =Convert.ToInt32(Console.ReadLine());
       Console.WriteLine("Введите точку интерполирования x");
       x =Convert.ToInt32(Console.ReadLine());
       /*Console.WriteLine("Введите n");
       while ( (n =Convert.ToInt32(Console.ReadLine()))>m)
       {
           Console.WriteLine("Введено недопустимое значение n");
       }
       */
       double[] nodes = new double[m];
       double[] values= new double[m];
       List<double> values2 = new List<double>();
       double h = (b - a) / (m-1);    // шаг, h=0.05
       Console.WriteLine($"{h}");
       for (int j = 0; j <= m-1; j++)
       {
           nodes[j] = a + j * h;
           values[j] =Function(nodes[j]);
       }
       Console.WriteLine("Узлы интерполяции\tЗначения функции");
       for (int i = 0; i <= m-1; i++)
       {
           Console.WriteLine($" {nodes[i]:F2} {values[i]:F2}" );
       }
       List<double> nodeList = new List<double>();
       nodeList.AddRange(nodes);
       List<double> sortList = new List<double>(SortNodes(nodeList, x, n));
       
       Console.WriteLine("Отсортированная таблица узлов ");
       for (int i = 0; i < sortList.Count; i++)
       {
           values2.Add(Function(sortList[i]));
           Console.WriteLine($"{sortList[i]:F2} {values2[i]:F2}");
       }
       double[] coefficients = DividedDifferences(sortList, values2,x,n);
       double y = InterpolateNewton(x, sortList, coefficients);
       Console.WriteLine($"Interpolated value at x = {x}: {y}");
       Console.WriteLine("Введите 1, если хотите изменить значения x,n\nВведите любое другое число, если хотите завершить");
       while ((new_try=Convert.ToInt32(Console.ReadLine()))==1)
       {
           Console.WriteLine("Введите точку интерполирования x");
           x =Convert.ToInt32(Console.ReadLine());
           Console.WriteLine("Введите n");
           while ( (n =Convert.ToInt32(Console.ReadLine()))>m)
           {
               Console.WriteLine("Введено недопустимое значение n");
           }
       }
    }
    static double[] DividedDifferences(List<double> nodes, List<double> values, double x, int n)
    {
        int m = nodes.Count;
        double[] f = new double[m];
        for (int i = 0; i < m; i++)
        {
            f[i] = values[i];
        }
        for (int j = 1; j < n; j++)
        {
            for (int i = m - 1; i >= j; i--)
            {
                f[i] = (f[i] - f[i - 1]) / (nodes[i] - nodes[i - j]);
            }
        }
        double[] coeffs = new double[n];
        coeffs[0] = f[0];
        for (int i = 1; i < n; i++)
        {
            double prod = 1.0;
            for (int j = 0; j < i; j++)
            {
                prod *= (x - nodes[j]);
            }
            coeffs[i] = f[i] * prod;
        }
        return coeffs;
    }
    static double DividedDifference(double x0, double x1, double f0, double f1)
    {
        return (f1 - f0) / (x1 - x0);
    }
     static List<double> SortNodes(List<double> nodes, double x, int n)
    {
        List<Tuple<double, double>> distances = nodes.Select(node => Tuple.Create(Math.Abs(node - x), node)).ToList(); 
        distances.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        List<double> selectedNodes = distances.Select(node => node.Item2).Take(n + 1).ToList();
        return selectedNodes;
    }
   static double InterpolateNewton(double x, List<double> nodes, double[] coefficients)
    {
        double result = coefficients[coefficients.Length - 1];
        for (int i = coefficients.Length - 2; i >= 0; i--)
        {
            result = result * (x - nodes[i]) + coefficients[i];
        }
        return result;
    }
    static double Function(double x)
    {
        return Math.Sin(x) - Math.Pow(x, 2) / 2;
    }
    

}