using System;
using System.Diagnostics;
using System.Threading;

namespace Engine
{
    public class Engine
    {
        private const int MapWidth = 50;
        private const int MapHeight = 20;

        private const int PlayerBeginX = MapWidth - 2;
        private const int PlayerBeginY = MapHeight - 2;

        private int MainLoopIterations = 0;
        private int Score = 0;

        private const char SpaceFiller = ' ';
        private const char PlayerHeadChar = 'O';
        private const char Wall = '█';
        private const char Food = 'X';

        private protected ConsoleChar[,] MAP = new ConsoleChar[MapWidth, MapHeight];

        // Old implementation: SOMEHOW if we do this, then the player head (or whole idk) disappears beyond the second half of the map,
        // though it is still there, just not visible

        // Same is the case for 25 : 10, I guess it inlines the constants
        // No clue how or why, but then if we dont do this it works fine

        // As I dont wanna fuck with this part of the project any longer, the player will now start at the bottom right :)

        // Player player = new Player(new ConsoleChar(PlayerHeadChar, MapWidth / 2, MapHeight / 2));

        Player player = new Player(new ConsoleChar(PlayerHeadChar, PlayerBeginX, PlayerBeginY));

        private void FillMap()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    char val = SpaceFiller;

                    if (x == 0 || x == MapWidth - 1 || y == 0 || y == MapHeight - 1)
                    {
                        val = Wall;
                    }

                    AddToMap(new ConsoleChar(val, x, y));
                }
            }
        }

        private void PlaceFood()
        {
            Random random = new Random();

            int FoodX = random.Next(1, MapWidth - 2);
            int FoodY = random.Next(1, MapHeight - 2);

            ConsoleChar food = new ConsoleChar(Food, FoodX, FoodY);

            AddToMap(food);
        }

        private void OnFoodPickup()
        {
            Score++;
        }

        private void AddToMap(ConsoleChar c)
        {
            MAP[c.x, c.y] = c;
        }

        private void RemoveFromMap(ConsoleChar c)
        {
            int x = c.x;
            int y = c.y;
            MAP[x, y] = new ConsoleChar(SpaceFiller, x, y);
        }

        private void Replace(int x, int y, ConsoleChar New)
        {
            MAP[x, y] = New;
        }

        private void DisplayMap()
        {
            foreach (ConsoleChar c in MAP)
            {
                Console.SetCursorPosition(c.x, c.y);
                Console.WriteLine(c.value);
            }
        }

        private void MovePlayer()
        {
            ConsoleChar Head = player.Head;

            int x = Head.x;
            int y = Head.y;

            int NextX = 0;
            int NextY = 0;

            switch (player.Direction)
            {
                case Direction.Forward:
                    NextX = x;
                    NextY = y - 1;
                    break;
                case Direction.Backwards:
                    NextX = x;
                    NextY = y + 1;
                    break;
                case Direction.Left:
                    NextX = x - 1;
                    NextY = y;
                    break;
                case Direction.Right:
                    NextX = x + 1;
                    NextY = y;
                    break;
            }

            ConsoleChar NextChar = MAP[NextX, NextY];

            if (NextChar.value == Wall)
            {
                player.Kill();
            }
            else if(NextChar.value == Food)
            {
                OnFoodPickup();

                // Destroy Food
                AddToMap(new ConsoleChar(SpaceFiller, NextX, NextY));

                player.Head.SetCoordinates(NextX, NextY);
            }
            else
            {
                player.Head.SetCoordinates(NextX, NextY);
            }
        }

        private void CheckForDirectionInput()
        {
            ConsoleKey input = Console.ReadKey(true).Key;

            switch (input)
            {
                case ConsoleKey.UpArrow:
                    player.Direction = Direction.Forward;
                    break;
                case ConsoleKey.DownArrow:
                    player.Direction = Direction.Backwards;
                    break;
                case ConsoleKey.LeftArrow:
                    player.Direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    player.Direction = Direction.Right;
                    break;
            }
        }

        private void MainLoop()
        {
            if (MainLoopIterations % 5 == 0) PlaceFood();

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(200);
                MovePlayer();
                if (!player.IsAlive)
                {
                    Console.SetCursorPosition(MapHeight + 1, MapWidth + 1);
                    Console.WriteLine("Snake has died. Score: " + Score);
                    Console.Read();
                    return;
                }
                DisplayMap();
            }
            CheckForDirectionInput();

            MainLoopIterations++;
            
            MainLoop();
        }

        public void BeginGame()
        {
            FillMap();
            ConsoleChar PlayerHead = player.Head;
            Replace(PlayerBeginX, PlayerBeginY, PlayerHead);

            // Game loop
            MainLoop();
        }
    }
}
