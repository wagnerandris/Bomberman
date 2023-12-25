using Model;

namespace Persistence
{

    public class StringMapReader : IMapReader
    {

        public Map Map { get; private set; }
        public (int, int) Player_start { get; private set; }
        public List<(int, int)> Enemies_start { get; private set; } = new List<(int, int)>();

        private static readonly char[] separator = new char[] { '\r', '\n' };

        public StringMapReader(string text)
        {
            string[] lines = text.Split(separator, options: StringSplitOptions.RemoveEmptyEntries).ToArray();
            int height = lines.Length;
            if (height == 0) throw new MapFileException("Empty mapfile");
            int width = lines[0].Length;

            bool player_found = false;
            bool [,] walls = new bool[height, width];
            for (int j = 0; j < height; j++)
            {
                if (lines[j].Length != width) throw new MapFileException("Uneven dimensions found in mapfile.");
                for (int i = 0; i < width; i++)
                {
                    switch (lines[j][i])
                    {
                        case '#':
                            walls[j, i] = true; break;
                        case '*':
                            Enemies_start.Add((j, i)); break;
                        case '@':
                            if (player_found) throw new MapFileException("Multiple player characters found in mapfile.");
                            player_found = true;
                            Player_start = (j, i); break;
                        case '.':
                            break;
                        default: throw new MapFileException("Invalid character found in mapfile.");
                    }
                }
            }

            if (Enemies_start.Count == 0) throw new MapFileException("No enemies found in mapfile.");

            Map = new Map(walls, width, height);
        }
    }
}