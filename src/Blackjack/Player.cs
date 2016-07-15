using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Player
    {
		public string Name { get; set; }
		public Hand Cards { get; set; }

		public Player(string name, Hand cards)
		{
			Name = name;
			Cards = cards;
		}


    }
}
