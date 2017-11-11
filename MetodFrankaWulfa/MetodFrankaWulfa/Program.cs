using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetodFrankaWulfa
{
   
    class Program
    {
        public static void PrintTheTask(List<List<double>> A, List<List<double>> Q, List<double> B, List<double> W)
        {
            Console.WriteLine("===========================================================");
            string str = "F(X) = ";
            for (int i = 0; i < A.Count; i++)
                for (int j = i; j < A[i].Count; j++)
                    if (A[i][j] != 0)
                        if (i == j)
                        {
                            if (A[i][j] > 0)
                                str += " +" + A[i][j] + "x" + (i+1) + "^2";
                            else
                                str += " " + A[i][j] + "x" + (i+1) + "^2";
                        }
                        else
                        {
                            if(A[i][j]>0)
                            str += " +" + A[i][j] + "x" + (i+1) + "x" + (j+1);
                            else
                                str += " " + A[i][j] + "x" + (i+1) + "x" + (j+1);

                        }
            for(int i=0;i<B.Count;i++)
                if(B[i]!=0)
                {
                    if (B[i] > 0)
                        str += " + " + B[i] + "x" + (i+1);
                    else
                        str += " " + B[i] + "x" + (i+1);
                }
            Console.Write(str);
            str = "\n";
            for (int i = 0; i < Q.Count; i++)
            {
                str += "" + (i+1) + ") ";
                for (int j = 0; j < Q[i].Count; j++)
                    if (Q[i][j] != 0)
                    {
                        //str += Q[i][j] > 0 ? " + " + Q[i][j] + "x" +(j+1) : " " + Q[i][j] + "x" + (j+1);
                        if (Q[i][j] > 0)
                            str += " + " + Q[i][j] + "x" + (j + 1);
                        else
                            str += " " + Q[i][j] + "x" + (j + 1);
                    }
                str += " <= " + W[i]+"\n";
            }
            Console.WriteLine(str);

        }
        public static double func(List<double> x, List<List<double>> A, List<double> B)
        {
            double temp = 0;
            for (int i = 0; i < A.Count; i++)
                for (int j = i; j < A[i].Count; j++)
                    temp += A[i][j] * x[i] * x[j];
            for (int i = 0; i < B.Count; i++)
                temp += B[i] * x[i];
            return temp;
        }
        public static List<double> diff(List<double>x, List<List<double>> A, List<double> B)
        {
            List<double> temp = new List<double>();
            for (int i = 0; i < A.Count; i++)
                temp.Add(0);
            for (int i = 0; i < A.Count; i++)
                for (int j = 0; j < A[i].Count; j++)
                    if (i == j)
                        temp[i] += 2 * A[i][j] * x[j];
                    else
                        temp[i] += A[i][j] * x[j];
            for (int i = 0; i < B.Count; i++)
                temp[i] += B[i];
            return temp;
        }

        public  static List<double> SimplexMetod(List<List<double>> Q, List<double> W, List<double> F)
        {
            List<List<double>> SimplexTablica = new List<List<double>>();
            for (int i = 0; i < Q.Count; i++)
                SimplexTablica.Add(new List<double>());
            for (int i = 0; i < Q.Count; i++)
                for (int j = 0; j < Q[i].Count; j++)
                    SimplexTablica[i].Add(Q[i][j]);

            for(int i=0;i<Q.Count;i++)
            {
                for (int j = 0; j < Q.Count; j++)
                    if (i == j)
                        SimplexTablica[i].Add(1);
                    else
                        SimplexTablica[i].Add(0);
            }
            List<int> Basis = new List<int>();
            for (int i = 0; i < Q.Count; i++)
                Basis.Add(Q[i].Count + i);
            for (int i = 0; i < W.Count; i++)
                SimplexTablica[i].Add(W[i]);
            List<double> StrokaF = new List<double>();
            for (int i = 0; i < F.Count; i++)
                StrokaF.Add(-F[i]);
            for (int i = 0; i < Q.Count; i++)
                StrokaF.Add(0);
            StrokaF.Add(0);//znachenie F v (0,0);

            bool CanExit = true;
            for (int i = 0; i < StrokaF.Count - 1;i++)
                if (StrokaF[i] < 0)
                    CanExit = false;
            while (!CanExit)
            {
                int numofcol = 0, numofline = 0;
                double D = 0;
                for (int i = 0; i < StrokaF.Count - 1; i++)
                    if (StrokaF[i] < D)
                    {
                        numofcol = i;
                        D = StrokaF[i];
                    }
                D = double.MaxValue;

                for (int i = 0; i < SimplexTablica.Count; i++)
                    if(SimplexTablica[i][SimplexTablica[i].Count - 1] / SimplexTablica[i][numofcol] < D && SimplexTablica[i][numofcol]>=0)
                    {
                        numofline = i;
                        D = SimplexTablica[i][SimplexTablica[i].Count - 1] / SimplexTablica[i][numofcol];
                    }
                double delCoef = SimplexTablica[numofline][numofcol];
                for (int i = 0; i < SimplexTablica[numofline].Count;i++)
                    SimplexTablica[numofline][i] /= delCoef;

                for (int i = 0; i < SimplexTablica.Count; i++)
                {
                    D = SimplexTablica[i][numofcol];
                    for (int j = 0; j < SimplexTablica[i].Count; j++)
                        if (i != numofline)
                            SimplexTablica[i][j] -= SimplexTablica[numofline][j] * D;
                }
                Basis[numofline] = numofcol;
                D = StrokaF[numofcol];
                for (int i = 0; i < StrokaF.Count; i++)
                {
                    
                    StrokaF[i] -= SimplexTablica[numofline][i] * D;
                }
                CanExit = true;
                for (int i = 0; i < StrokaF.Count - 1; i++)
                    if (StrokaF[i] < 0)
                        CanExit = false;
            }
            List<double> exit = new List<double>();
            for (int i = 0; i < F.Count; i++)
                exit.Add(0);
            for (int i = 0; i < Basis.Count; i++)
                if (Basis[i] < F.Count)
                    exit[Basis[i]] = SimplexTablica[i][SimplexTablica[i].Count - 1];
            return exit;


            
        }
        static void Main(string[] args)
        {
            int n;
            int m;
            Console.WriteLine("Введите размерность матрицы");
            n=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите количество условий");
            m = Convert.ToInt32(Console.ReadLine());

            List<List<double>> A = new List<List<double>>();
            List<double> B = new List<double> ();
            List<List<double>> Q = new List<List<double>>();
            for (int i = 0; i < m; i++)
                Q.Add(new List<double>());
            List<double> W = new List<double>();
            for (int i = 0; i < n; i++)
                A.Add(new List<double>());
            Console.WriteLine("Введите функцию: ");
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (i == j)
                        Console.Write("\tx" + (j+1) + "^2 = ");
                    else
                        Console.Write("\tx" + (i+1) + "*x" + (j+1) + "/2 = ");
                    A[i].Add( Convert.ToInt32(Console.ReadLine()));
                    if (i != j)
                        A[j].Add(A[i][A[i].Count - 1]);
                }
                
            }
            for(int i=0;i<n;i++)
            {
                Console.Write("\tx" + (i+1) + " = ");
                B.Add(Convert.ToInt32(Console.ReadLine()));
            }
            Console.WriteLine("Введите условия: ");
            for (int i = 0; i < m; i++)
            {
                Console.Write((i+1) + ") ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write("\tx" + (j + 1) + "*");
                    
                    Q[i].Add(Convert.ToInt32(Console.ReadLine()));
                }
                Console.Write(" <= ");
                W.Add(Convert.ToInt32(Console.ReadLine()));
            }
            double eps = 10E-5;
            PrintTheTask(A, Q, B, W);
            List<double> Xpr = new List<double>();
            List<double> Xnxt = new List<double>();
            double pogr;
            for (int i = 0; i < n; i++)
            {
                Xpr.Add(0);
                Xnxt.Add(0);
            }
            do
            {
                var linkoef = diff(Xpr, A, B);
                var z = SimplexMetod(Q, W, linkoef);
                var y = new List<double>();
                for (int i = 0; i < Xpr.Count; i++)
                    y.Add(z[i] - Xpr[i]);
                List<double> ur = new List<double> { 0, 0 };
                for(int i=0;i<A.Count;i++)
                    for(int j=i;j<A[i].Count;j++)
                    {
                        ur[0] += 2 * A[i][j] * y[i] * y[j];
                        ur[1] += A[i][j] * Xpr[i] * y[j] + A[i][j] * Xpr[j] * y[i];
                    }
                for (int i = 0; i < B.Count; i++)
                    ur[1] += B[i]*y[i];
                double lResult = -ur[1] / ur[0];
                for (int i = 0; i < Xpr.Count; i++)
                    Xnxt[i] = Xpr[i] + lResult * y[i];
               pogr = Math.Abs(func(Xpr, A, B) - func(Xnxt, A, B));
                for (int i = 0; i < Xpr.Count; i++)
                    Xpr[i] = Xnxt[i];
            }
            while (pogr > eps);
            Console.WriteLine("======================================\nОтвет:\n");
            for (int i = 0; i < Xnxt.Count; i++)
                Console.WriteLine(Xnxt[i]);
           // var q = SimplexMetod(Q, W, new List<double> { 2, 4 });

            Console.ReadKey();
        }
    }
}
