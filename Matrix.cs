using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphen_gui
{
    class Matrix
    {

        private int[,] adjazenzmatrix;
        private int[,] potenzmatrix; // eigentlich unnötig, aber wurde anfänglich programmiert.
        private int[,] distanzmatrix;
        private int[,] wegmatrix;


        public Matrix(int a)
        {
            erstellenAdjazenzMatrix(a);
        }

        public int[,] Adjazenzmatrix { get => adjazenzmatrix; set => adjazenzmatrix = value; }
        public int[,] Potenzmatrix { get => potenzmatrix; set => potenzmatrix = value; }
        public int[,] Distanzmatrix { get => distanzmatrix; set => distanzmatrix = value; }
        public int[,] Wegmatrix { get => wegmatrix; set => wegmatrix = value; }


        // basis adjazenz Matrix muss erstellt werden
        public void erstellenAdjazenzMatrix(int knotenanzahl)
        {
            if (knotenanzahl < 1)
            {
                Console.WriteLine("Im Graphen wurde noch nichts eingetragen.");
            }
            Adjazenzmatrix = new int[knotenanzahl, knotenanzahl];
            for (int i = 0; i < knotenanzahl; i++)
            {
                for (int i2 = 0; i2 < knotenanzahl; i2++)
                {
                    Adjazenzmatrix[i, i2] = 0;
                }
            }
        }
        // ist jetzt obsolet weil das projekt in ein Gui Programm umgewandelt wurde.
        public String printMatrix(int[,] matrix)
        {
            String z = "";

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int i2 = 0; i2 < matrix.GetLength(1); i2++)
                {
                    z += " " + matrix[i, i2] + " ";
                }
                z += "\n";
            }
            return z;
        }



        public int[,] potenziereMatrixen(int[,] matrix1, int[,] matrixadja)
        {
            if (matrix1 != null)
            {
                for (int feld = 0; feld < matrix1.GetLength(1); feld++)
                {
                    for (int unten = 0; unten < matrix1.GetLength(1); unten++)
                    {
                        int felderg = 0;
                        for (int seite = 0; seite < matrix1.GetLength(1); seite++)
                        {
                            felderg += (matrixadja[feld, seite] * matrixadja[seite, unten]);
                        }
                        matrix1[feld, unten] = felderg;
                    }
                }
                return matrix1;
            }
            else
            {
                Console.WriteLine("Matrix null kann nicht potenzieren!");
                return null;
            }
        }
        public int[,] MatrixMultiplikation(int[,] matrix1, int[,] matrix2)
        {
            int[,] ergebnis;
            if (matrix1 != null && matrix2 != null)
            {
                if (matrix1.GetLength(1) == matrix2.GetLength(1) && matrix1.GetLength(0) == matrix2.GetLength(0))
                {
                    ergebnis = new int[matrix1.GetLength(1), matrix2.GetLength(1)];
                    for (int i = 0; i < matrix1.GetLength(0); i++)
                    {
                        for (int i2 = 0; i2 < matrix1.GetLength(1); i2++)
                        {
                            int z = 0;
                            for (int i3 = 0; i3 < matrix2.GetLength(0); i3++)
                            {
                                z += matrix1[i, i3] * matrix2[i3, i2];
                            }
                            ergebnis[i, i2] = z;
                        }
                    }
                    return ergebnis;
                }
                else
                {
                    Console.WriteLine("dimensions of the matrix dont match");
                }
            }
            else
            {
                Console.WriteLine("first or second matrix is null");
            }
            return null;

        }

        public int[,] Distanzmatrixzeichnung(int[,] matrix1)
        {

            if (matrix1.GetLength(0) < 1)
            {
                Console.WriteLine("Im Graphen wurde noch nichts eingetragen.");
            }
            else
            {
                Distanzmatrix = new int[matrix1.GetLength(0), matrix1.GetLength(0)];
                Potenzmatrix = Adjazenzmatrix;

                for (int potenz = 1; potenz < Distanzmatrix.GetLength(0); potenz++)
                {
                    for (int x = 0; x < Distanzmatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < Distanzmatrix.GetLength(0); y++)
                        {
                            if (x == y)
                            {
                                Distanzmatrix[x, y] = 0;
                            }
                            else
                            {
                                if (potenz == 1)
                                {
                                    if (Potenzmatrix[x, y] == 1)
                                    {
                                        Distanzmatrix[x, y] = potenz;
                                    }
                                    else
                                    {
                                        Distanzmatrix[x, y] = -1;
                                    }
                                }
                                else
                                {
                                    if (Potenzmatrix[x, y] > 0 && Distanzmatrix[x, y] == -1)
                                    {
                                        Distanzmatrix[x, y] = potenz;
                                    }
                                }
                            }
                        }
                    }
                    int check = 0;
                    for (int x = 0; x < Distanzmatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < Distanzmatrix.GetLength(0); y++)
                        {
                            if (Distanzmatrix[x, y] == -1)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 0)
                    {
                        return Distanzmatrix;
                    }
                    Potenzmatrix = MatrixMultiplikation(Potenzmatrix, Adjazenzmatrix);
                }
            }
            return Distanzmatrix;
        }

        public int[,] Wegmatrixzeichnung(int[,] matrix)
        {
            if (matrix.GetLength(1) < 1)
            {
                Console.WriteLine("Im Graphen wurde noch nichts eingetragen.");
            }
            else
            {
                Wegmatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
                Potenzmatrix = matrix;

                for (int potenz = 1; potenz < Wegmatrix.GetLength(0); potenz++)
                {
                    for (int x = 0; x < Wegmatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < Wegmatrix.GetLength(0); y++)
                        {
                            if (x == y)
                            {
                                Wegmatrix[x, y] = 1;
                            }
                            else
                            {
                                if (potenz == 1)
                                {
                                    if (Potenzmatrix[x, y] > 0)
                                    {
                                        Wegmatrix[x, y] = 1;
                                    }
                                    else
                                    {
                                        Wegmatrix[x, y] = 0;
                                    }
                                }
                                else
                                {
                                    if (Potenzmatrix[x, y] > 0)
                                    {
                                        Wegmatrix[x, y] = 1;
                                    }
                                }
                            }
                        }
                    }
                    int check = 0;
                    for (int x = 0; x < Wegmatrix.GetLength(0); x++)
                    {
                        for (int y = 0; y < Wegmatrix.GetLength(0); y++)
                        {
                            if (Wegmatrix[x, y] == 0)
                            {
                                check++;
                            }
                        }
                    }
                    if (check == 0)
                    {
                        return Wegmatrix;
                    }
                    Potenzmatrix = MatrixMultiplikation(Potenzmatrix, matrix);
                }
            }
            return Wegmatrix;
        }
    }
}
