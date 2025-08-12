using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    public class Figure
    {
        // Матрица, представляющая фигуру (0 - пусто, 1 - блок)
        public int[,] Shape { get; private set; }
        public int X { get; set; } // Позиция по X
        public int Y { get; set; } // Позиция по Y

        // Конструктор для создания фигуры
        public Figure(int[,] shape)
        {
            Shape = shape;
            X = 0;
            Y = 0;
        }

        // Метод для вращения фигуры
        public void Rotate()
        {
            int[,] rotated = new int[Shape.GetLength(1), Shape.GetLength(0)];

            for (int i = 0; i < Shape.GetLength(0); i++)
            {
                for (int j = 0; j < Shape.GetLength(1); j++)
                {
                    rotated[j, Shape.GetLength(0) - 1 - i] = Shape[i, j];
                }
            }

            Shape = rotated;
        }
    }
}
