using TetrisGame;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Tetris";
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Exit");
            Console.Write("Select option: ");

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.D1)
            {
                var game = new Game(10, 20);
                game.Run();
            }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }
}
