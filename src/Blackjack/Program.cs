using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Program
    {
        public static void Main(string[] args)
        {
			// Notice how little occurs here. Everything is encapsulated in the behaviors of the classes.

			// Initialize the game.
			Blackjack game = new Blackjack(new string[]{ "Dealer", "Player1" });

			// Run the game.
			game.GameLoop();

			// Wait before closing the console at the end of the game.
			Console.ReadLine();
        }
    }
}
