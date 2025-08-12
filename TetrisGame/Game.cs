using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    public class Game
    {
        private GameField field;
        private Figure currentFigure;
        private Figure nextFigure;
        private Random random;
        private bool isGameOver;

        // Все возможные фигуры тетриса
        private readonly List<int[,]> figures = new List<int[,]>
        {
            // I-фигура
            new int[4, 1] { {1}, {1}, {1}, {1} },
            // J-фигура
            new int[3, 2] { {1, 0}, {1, 0}, {1, 1} },
            // L-фигура
            new int[3, 2] { {0, 1}, {0, 1}, {1, 1} },
            // квадрат
            new int[2, 2] { {1, 1}, {1, 1} },
            // S-фигура
            new int[3, 2] { {0, 1}, {1, 1}, {1, 0} },
            // T-фигура
            new int[2, 3] { {1, 1, 1}, {0, 1, 0} },
            // Z-фигура
            new int[3, 2] { {1, 0}, {1, 1}, {0, 1} }
        };

        public Game(int width, int height)
        {
            field = new GameField(width, height);
            random = new Random();
            isGameOver = false;
            SpawnFigure();
        }

        // Создание новой фигуры
        private void SpawnFigure()
        {
            currentFigure = nextFigure ?? GetRandomFigure();
            nextFigure = GetRandomFigure();
            currentFigure.X = field.Width / 2 - currentFigure.Shape.GetLength(1) / 2;
            currentFigure.Y = -currentFigure.Shape.GetLength(0);

            // Новая проверка - если фигура сразу сталкивается с блоком
            if (field.CheckCollision(currentFigure))
            {
                // Пытаемся сдвинуть фигуру вниз на 1 клетку
                currentFigure.Y++;
                if (field.CheckCollision(currentFigure))
                {
                    isGameOver = true;
                    return;
                }
            }
        }

        private Figure GetRandomFigure()
        {
            return new Figure(figures[random.Next(figures.Count)]);
        }


        // Движение фигуры вниз
        public bool MoveDown()
        {
            currentFigure.Y++;

            if (field.CheckCollision(currentFigure))
            {
                currentFigure.Y--;
                field.MergeFigure(currentFigure);
                field.CheckLines();

                SpawnFigure();
                return false;
            }
            return true;
        }

        // Движение влево/вправо
        public void MoveHorizontal(int direction)
        {
            currentFigure.X += direction;
            if (field.CheckCollision(currentFigure))
            {
                currentFigure.X -= direction;
            }
        }

        // Вращение фигуры
        public void Rotate()
        {
            Figure rotated = new Figure((int[,])currentFigure.Shape.Clone())
            {
                X = currentFigure.X,
                Y = currentFigure.Y
            };
            rotated.Rotate();

            // Проверка, можно ли вращать
            if (!field.CheckCollision(rotated))
            {
                currentFigure = rotated;
            }
        }

        // Отрисовка игры
        public void Draw()
        {
            Console.Clear();

            // Рисуем границы
            Console.Write("+");
            for (int i = 0; i < field.Width; i++) Console.Write("-");
            Console.WriteLine("+");

            // Рисуем поле с текущей фигурой
            for (int i = 0; i < field.Height; i++)
            {
                Console.Write("|");
                for (int j = 0; j < field.Width; j++)
                {
                    bool isFigureBlock = false;

                    // Проверяем, принадлежит ли блок текущей фигуре
                    if (currentFigure != null && i >= currentFigure.Y &&
                        i < currentFigure.Y + currentFigure.Shape.GetLength(0) &&
                        j >= currentFigure.X &&
                        j < currentFigure.X + currentFigure.Shape.GetLength(1) &&
                        currentFigure.Shape[i - currentFigure.Y, j - currentFigure.X] != 0)
                    {
                        isFigureBlock = true;
                    }

                    if (isFigureBlock)
                    {
                        Console.Write("O");
                    }
                    else if (i < field.Height && j < field.Width && field.Field[i, j] != 0)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("|");
            }

            Console.Write("+");
            for (int i = 0; i < field.Width; i++) Console.Write("-");
            Console.WriteLine("+");

            // Рисуем следующую фигуру
            Console.WriteLine("\nNext figure:");
            if (nextFigure != null)
            {
                for (int i = 0; i < nextFigure.Shape.GetLength(0); i++)
                {
                    for (int j = 0; j < nextFigure.Shape.GetLength(1); j++)
                    {
                        Console.Write(nextFigure.Shape[i, j] != 0 ? "O" : " ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private void ResetGame()
        {
            field = new GameField(field.Width, field.Height);
            isGameOver = false;
        }

        // Основной игровой цикл
        public void Run()
        {
            SpawnFigure();
            DateTime lastUpdate = DateTime.Now;

            while (!isGameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.LeftArrow: MoveHorizontal(-1); break;
                        case ConsoleKey.RightArrow: MoveHorizontal(1); break;
                        case ConsoleKey.UpArrow: Rotate(); break;
                        case ConsoleKey.DownArrow: MoveDown(); break;
                        case ConsoleKey.Spacebar: HardDrop(); break;
                        case ConsoleKey.Escape: return;
                    }
                }

                if ((DateTime.Now - lastUpdate).TotalMilliseconds > 1000)
                {
                    if (!MoveDown() && field.IsGameOver())
                    {
                        isGameOver = true;
                    }
                    lastUpdate = DateTime.Now;
                }

                Draw();
                Thread.Sleep(30);
            }

            ShowGameOverScreen();
        }

        private void HardDrop()
        {
            while (MoveDown()) { }
        }

        private void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("====================");
            Console.WriteLine("     GAME OVER");
            Console.WriteLine("====================");
            Console.WriteLine("\nPress R to restart");
            Console.WriteLine("Press ESC to exit");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    ResetGame();
                    Run();
                    return;
                }
                else if (key == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }
    }
}
