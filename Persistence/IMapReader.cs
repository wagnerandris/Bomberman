using Model;

namespace Persistence
{
    public interface IMapReader
    {
        public Map Map { get; }
        public (int, int) Player_start { get; }
        public List<(int, int)> Enemies_start { get; }
    }
}