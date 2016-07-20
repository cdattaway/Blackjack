using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack {

	/// <summary>
	/// An abstract class containing a list of possible moves in a game.
	/// </summary>
	public abstract class Game {
		public static List<string> Moves;
	}

	/// <summary>
	/// The Blackjack game. Creating a new instance of the game will run the game in the console.
	/// </summary>
	public class Blackjack : Game {
		/// <summary>
		/// The current player.
		/// </summary>
		private BlackjackPlayer CurrentPlayer;

		/// <summary>
		/// The list of all players.
		/// </summary>
		public List<BlackjackPlayer> Players;

		/// <summary>
		/// The deck of cards for the game.
		/// </summary>
		public Deck Deck { get; set; }


		/// <summary>
		/// Determines if all players are finished.
		/// </summary>
		/// <returns>Returns true if all players are finished. Otherwise, returns false.</returns>
		public bool AllFinished {
			get {
				foreach (BlackjackPlayer p in Players)
					if (p.IsFinished == false)
						return false;
				return true;
			}
		}

		/// <summary>
		/// A list of the possible moves.
		/// </summary>
		new public static List<string> Moves = new List<string>{
			"hit",
			"stay",
			"quit"
		};

		/// <summary>
		/// Creates a game of Blackjack with players whose names are those names provided in the string array.
		/// </summary>
		/// <param name="names">The names of the players.</param>
		public Blackjack(string[] names) {
			Players = new List<BlackjackPlayer>();

			Deck = new Deck();


			// Initialize the players.
			foreach (string n in names) {
				BlackjackPlayer p; 
				if (n == "Dealer")
					 p = new BlackjackPlayer(n, new BlackjackHand(), true);
				else
					 p = new BlackjackPlayer(n, new BlackjackHand());
				Players.Add(p);
			}

			// Deal the cards to each player.
			foreach (BlackjackPlayer p in Players) {
				if (p.IsDealer)
					Deck.DealHand(p.Hand, 2, 1);
				else
					Deck.DealHand(p.Hand, 2);
			}
		}

		/// <summary>
		/// This is the primary loop containing the logic of the game.
		/// </summary>
		public void GameLoop() {

			// Necessary to run the game.
			Console.OutputEncoding = System.Text.Encoding.Unicode;

			// Check if there are players.
			if (Players.Count > 0)
				CurrentPlayer = NextPlayer();
			else {
				Console.WriteLine("Error! No players");
				return;
			}

			string input = "";									// For selecting moves from user input
			bool gameOver = false;								// This is a flag for when the game is over
			StringBuilder moveBuilder = new StringBuilder();	// This builds a string to show the available moves

			// Builds a string for displaying possible moves
			foreach (string s in Moves) {
				moveBuilder.Append(s);
				if (s != Moves[Moves.Count - 1])
					moveBuilder.Append(", ");
			}


			// Continues until the player requests to quit or the game is over
			while (input != "quit" && !gameOver) {

				// Doesn't repeat the turn for players who are finished.
				if (!CurrentPlayer.IsFinished) {
					Console.WriteLine("{0}\nSelect a move: ", CurrentPlayer);

					Console.WriteLine(moveBuilder);

					// Allows the current player to select a move.
					// If that player is the dealer, choose the move.

					input = CurrentPlayer.SelectMove(Moves);

					// Behave according to the move chosen
					switch (input) {
						case "hit":
							Console.WriteLine("Hit me.");
							Deck.DealCard(CurrentPlayer.Hand);
							Console.WriteLine(CurrentPlayer.Hand);
							break;
						case "stay":
							Console.WriteLine("I'll stay.");
							CurrentPlayer.IsFinished = true;
							break;
						case "quit":
							return;
					}
				}

				// Check if the player has busted
				if (CurrentPlayer.Hand.HandValue > 21) {
					Console.WriteLine("{0} has busted!", CurrentPlayer.Name);
					CurrentPlayer.IsFinished = true;
				}
				// Don't let a player bust when at 21.
				else if (CurrentPlayer.Hand.HandValue == 21) {
					CurrentPlayer.IsFinished = true;
				}

				// Continues cycling until all players are finished.
				if (!AllFinished)
					CurrentPlayer = NextPlayer();
				// When all players have finished, it calculates a winner.
				else {
					BlackjackPlayer winner = CalculateWinner();
					
					if (winner != null)
						Console.WriteLine("Game Over! The winner is {0} with a hand of {1}", winner.Name,winner.Hand);
					// If there is no winner, show that everyone busts.
					else
						Console.WriteLine("Everyone busts.");

					// This will exit the game
					gameOver = true;
				}
			}
		}


		/// <summary>
		/// Calculates a winner.
		/// </summary>
		/// <returns>Returns the winner. If everyone busts, returns null.</returns>
		private BlackjackPlayer CalculateWinner() {

			// This is the minimum difference between the 
			// 
			var minDiff = 21;
			BlackjackPlayer currentWinner = null;

			foreach (BlackjackPlayer p in Players) {
				var pHandDiff = 21 - p.Hand.HandValue;
				if (pHandDiff >= 0 && pHandDiff < minDiff) {
					currentWinner = p;
					minDiff = pHandDiff;
				}
			}
			return currentWinner;
		}

		public void AddPlayer(BlackjackPlayer p) {
			Players.Add(p);
		}

		/// <summary>
		/// Gives the next player.
		/// </summary>
		/// <returns>Returns the next (or first) player.</returns>
		public BlackjackPlayer NextPlayer() {
			return Players[(Players.IndexOf(CurrentPlayer) + 1) % Players.Count];
		}
	}
}
