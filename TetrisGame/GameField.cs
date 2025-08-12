using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    public class GameField
    {
        public int Width { get; }
        public int Height { get; }
        public int[,] Field { get; private set; }

        public GameField(int width, int height)
        {
            Width = width;
            Height = height;
            Field = new int[height, width];
        }

        // Проверка столкновений
        public bool CheckCollision(Figure figure)
        {
            for (int i = 0; i < figure.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < figure.Shape.GetLength(1); j++)
                {
                    if (figure.Shape[i, j] == 0) continue;

                    int newX = figure.X + j;
                    int newY = figure.Y + i;

                    // Если часть фигуры уже на игровом поле (newY >= 0)
                    if (newY >= 0)
                    {
                        // Выход за границы по X или низ поля
                        if (newX < 0 || newX >= Width || newY >= Height)
                            return true;

                        // Столкновение с существующим блоком
                        if (Field[newY, newX] != 0)
                            return true;
                    }
                }
            }
            return false;
        }

        // Добавление фигуры на поле
        public void MergeFigure(Figure figure)
        {
            for (int i = 0; i < figure.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < figure.Shape.GetLength(1); j++)
                {
                    if (figure.Shape[i, j] != 0)
                    {
                        int y = figure.Y + i;
                        int x = figure.X + j;
                        if (y >= 0) Field[y, x] = figure.Shape[i, j];
                    }
                }
            }
        }

        // Проверка и удаление заполненных линий
        public int CheckLines()
        {
            int linesCleared = 0;

            for (int y = Height - 1; y >= 0; y--)
            {
                bool lineComplete = true;

                // Проверяем заполненность строки
                for (int x = 0; x < Width; x++)
                {
                    if (Field[y, x] == 0)
                    {
                        lineComplete = false;
                        break;
                    }
                }

                if (lineComplete)
                {
                    linesCleared++;
                    // Сдвигаем все строки выше вниз
                    for (int y2 = y; y2 > 0; y2--)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            Field[y2, x] = Field[y2 - 1, x];
                        }
                    }
                    // Очищаем верхнюю строку
                    for (int x = 0; x < Width; x++)
                    {
                        Field[0, x] = 0;
                    }
                    // Проверяем текущую строку снова
                    y++;
                }
            }

            return linesCleared;
        }

        public bool IsGameOver()
        {
            // Проверяем верхнюю строку (индекс 0) на наличие блоков
            for (int x = 0; x < Width; x++)
            {
                if (Field[0, x] != 0)
                    return true;
            }
            return false;
        }
    }
}
