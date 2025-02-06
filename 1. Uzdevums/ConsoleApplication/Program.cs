namespace ConsoleApplication
{
    public struct Coords
    {
        public Coords(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const int SIZE = 20;
            const int MIN_VALUE = 0;
            const int MAX_VALUE = 1000;
            const int MIN_OUTPUT_WIDHT = 4;

            Random random = new Random();

            // Izveido ciparu masīvu 20x20 aizpilda to ar gadījuma cipariem un izvada masīvu uz ekrāna.
            int[,] array = new int[SIZE, SIZE];

            Console.WriteLine("Masīvs:");
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    array[x, y] = random.Next(MIN_VALUE, MAX_VALUE + 1);
                    Console.Write($"{array[x, y], MIN_OUTPUT_WIDHT}");
                }

                Console.WriteLine();
            }

            // Atrod mazāko un lielāko ciparu ar to koordinātēm
            int min = array[0, 0];
            int max = array[0, 0];
            Coords minCoord = new Coords(0, 0);
            Coords maxCoord = new Coords(0, 0);

            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    if (array[x, y] < min)
                    {
                        min = array[x, y];
                        minCoord = new Coords(x, y);
                    }
                    if (array[x, y] > max)
                    {
                        max = array[x, y];
                        maxCoord = new Coords(x, y);
                    }
                }
            }

            Console.WriteLine($"\nMasīvā mazākais cipars: {min} (Koordinātes: {minCoord.X}, {minCoord.Y})");
            Console.WriteLine($"Masīvā lielākais cipars: {max} (Koordinātes: {maxCoord.X}, {maxCoord.Y})");

            // Atlasa masīvu augošā secībā un izvada uz ekrāna
            int[] flatArray = new int[SIZE * SIZE];

            int index = 0;

            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    flatArray[index++] = array[x, y];
                }
            }

            Array.Sort(flatArray);

            Console.WriteLine("\nMasīvs augošā secībā:");
            for (int i = 0; i < flatArray.Length; i++)
            {
                Console.Write($"{flatArray[i], MIN_OUTPUT_WIDHT}");

                if ((i + 1) % SIZE == 0)
                    Console.WriteLine();
            }
        }
    }
}
