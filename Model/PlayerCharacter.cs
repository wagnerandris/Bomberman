﻿namespace Model
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

            base.Move(((int, int))new_position);
            
            // If the player steps on an enemy, it's game over
            if (!_enemies.All(e => e.Position != new_position))
            {
                InvokeDestroyed(this);
            }
        }

        public void PlaceBomb()
        {
            lock (_bombs)
            {
                _bombs.Add(new Bomb(Position));
            }
        }
    }
}
