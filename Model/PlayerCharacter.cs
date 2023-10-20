using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PlayerCharacter : Actor
    {
        public PlayerCharacter((int, int) player_start) : base(player_start)
        {
        }
    }
}
