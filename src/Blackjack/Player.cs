using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
	/// <summary>
	/// A generic class representing a player.
	/// </summary>
	/// <typeparam name="H">The kind of hand (e.g. Blackjack, Poker, etc.).</typeparam>
    public class Player<H> where H: Hand
    {
		/// <summary>
		/// The player's name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The player's hand.
		/// </summary>
		public virtual H Hand { get; set; }

		/// <summary>
		/// Initialize the player.
		/// </summary>
		/// <param name="name">The player's name.</param>
		/// <param name="cards">The player's hand.</param>
		public Player(string name, H cards)
		{
			Name = name;
			Hand = cards;
		}

		/// <summary>
		/// Allows the player to select a move.
		/// </summary>
		/// <param name="possibleMoves">All of the possible moves.</param>
		/// <returns>Returns a string representing the selected move.</returns>
		public virtual string SelectMove(List<string> possibleMoves)
		{
			string input = Console.ReadLine();

			while (!possibleMoves.Contains(input))
			{
				Console.WriteLine("\nInvalid Move!\n");
				input = Console.ReadLine();
			}
			return input;
		}

		/// <summary>
		/// Converts the player to a string.
		/// </summary>
		/// <returns>Returns the player's name and his/her hand.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			return sb.Append(Name).Append(":\n").Append(Hand).ToString();
		}
	}

	/// <summary>
	/// A blackjack player.
	/// </summary>
	public class BlackjackPlayer : Player<BlackjackHand>
	{
		/// <summary>
		/// True if the player is the dealer.
		/// </summary>
		public bool IsDealer { get; set; }

		/// <summary>
		/// True if the player has busted, chosen to stay, or is at 21.
		/// </summary>
		public bool IsFinished { get; set; }

		/// <summary>
		/// Initializes the player.
		/// </summary>
		/// <param name="name">The player's name.</param>
		/// <param name="cards">The player's hand.</param>
		public BlackjackPlayer(string name, BlackjackHand cards, bool isDealer=false) : base(name, cards) {
			IsFinished = false;
			IsDealer = isDealer;
		}

		/// <summary>
		/// Chooses a move. If the player is a dealer, it operates according to rules.
		/// </summary>
		/// <param name="possibleMoves">The possible moves for the player.</param>
		/// <returns>A string representing the player's move.</returns>
		public override string SelectMove(List<string> possibleMoves)
		{
			if (IsDealer)
			{
				if (Hand.HandValue < 17)
					return "hit";
				else
					return "stay";
			}

			return base.SelectMove(possibleMoves);
		}

	}
}
