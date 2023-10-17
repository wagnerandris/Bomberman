using Model;

namespace Persistence
{

    public class TXTMapReader : IMapReader
    {

        public Map Map { get; private set; }
        public (int, int) Player_start { get; private set; }
        public List<(int, int)> Enemies_start { get; private set; } = new List<(int, int)>();

        public TXTMapReader(string filepath)
        {
            string text;
            try
            {
                text = File.ReadAllText(filepath);
            }
            catch (Exception ex)
            {
                throw new MapFileException(ex.Message, ex);
            }

            string[] lines = text.Split(new char[] { '\r', '\n' }, options: StringSplitOptions.RemoveEmptyEntries).ToArray();
            int height = lines.Length;
            if (height == 0) throw new MapFileException("Empty mapfile");
            int width = lines[0].Length;

            bool [,] walls = new bool[width, height];
            for (int j = 0; j < height; j++)
            {
                if (lines[j].Length != width) throw new MapFileException("Uneven dimensions found in mapfile.");
                for (int i = 0; i < width; i++)
                {
                    switch (lines[j][i])
                    {
                        case '#':
                            walls[i, j] = true; break;
                        case '*':
                            Enemies_start.Add((i, j)); break;
                        case '@':
                            Player_start = (i, j); break;
                        case '.':
                            break;
                        default: throw new MapFileException("Invalid character found in mapfile.");
                    }
                }
            }

            Map = new Map(walls, width, height);
        }
    }
}