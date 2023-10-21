using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PlayerCharacter : Actor
    {
        public PlayerCharacter((int, int) start_position, Map map, List<Enemy> enemies, List<Bomb> bombs) : base(start_position, map, enemies, bombs)
        {
        }

        public void Move(Direction direction)
        {
            (int, int)? new_position = NewPosition(direction);
            if (new_position == null) return;

            // If the player steps on an enemy, it's game over
            if (_enemies.All(e => e.Position != new_position))
            {
                base.Move(((int, int))new_position);
            } else
            {
                Actor.Destroyed?.Invoke(this, new ActorDestroyedEventArgs(Position));
            }
        }

        internal void PlaceBomb()
        {
            _bombs.Add(new Bomb(Position));
        }
    }
}
