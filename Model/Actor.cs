using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public abstract class Actor : IGameObject
    {
        private (int, int) position;
        public (int, int) Position { get => position; protected set => position = value; }

        public static event EventHandler<ActorMovedEventArgs>? ActorMoved;

        public Actor((int, int) start_position)
        {
            position = start_position;
        }

        public void Move((int, int) new_position)
        {
            (int, int) old_position = position;
            position = new_position;
            ActorMoved?.Invoke(this, new ActorMovedEventArgs(old_position, new_position));
        }
    }
}
