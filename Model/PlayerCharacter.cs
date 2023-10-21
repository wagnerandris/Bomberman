using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PlayerCharacter : Actor
    {
        public PlayerCharacter((int, int) player_start, Map map, List<Enemy> enemies) : base(player_start, map, enemies)
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
                // invoke game end event
            }
        }
    }
}
