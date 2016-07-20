using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
	/// <summary>
	/// A single card.
	/// </summary>
    public class Card
    {
		/// <summary>
		/// The ID of the card represented as an int. 1: Ace, 11-13: Jack-King.
		/// </summary>
		public int ID { get; }

		/// <summary>
		/// Spade, Heart, Club, Diamond
		/// </summary>
		public string Suit { get; }

		/// <summary>
		/// The character representing the card face value.
		/// </summary>
		public string Symbol { get; }

		/// <summary>
		/// The character representing the card's suit.
		/// </summary>
		public char SuitSymbol { get; }

		/// <summary>
		/// Whether the card shows its values or appears as ??.
		/// </summary>
		public bool IsVisible { get; set; }
		public int Value
		{
			get
			{
				if (ID >= 10)
					return 10;


				return 0;
			}
		}

		/// <summary>
		/// Constructor. Creates a card of the given ID and suit.
		/// </summary>
		/// <param name="id">Value of the card from Ace to King, represented as 1-13.</param>
		/// <param name="suit">Suit of the card.</param>
		public Card(int id, string suit)
		{
			ID = id;
			Suit = suit;
			Symbol = CardSymbols[ID];
			SuitSymbol = SuitSymbols[suit];
			IsVisible = true;
		}

		/// <summary>
		/// Gives the card as a string of two characters.
		/// </summary>
		/// <returns>If the card is hidden, returns ??. Otherwise, returns two characters representing the card.</returns>
		public override string ToString()
		{
			if (IsVisible)
				return Symbol + SuitSymbol;
			else
				return "??";
		}

		/// <summary>
		/// A map from IDs to symbols of the cards.
		/// </summary>
		public static Dictionary<int, string> CardSymbols = new Dictionary<int, string>()
		{
			{1, "A" },
			{2, "2" },
			{3, "3" },
			{4, "4" },
			{5, "5" },
			{6, "6" },
			{7, "7" },
			{8, "8" },
			{9, "9" },
			{10, "10" },
			{11, "J" },
			{12, "Q" },
			{13, "K" }
		};

		/// <summary>
		/// A map from suits to symbols of the suits.
		/// </summary>
		public static Dictionary<string, char> SuitSymbols = new Dictionary<string, char>()
		{
			{"spade",'\u2660'},
			{"heart",'\u2665'},
			{"club",'\u2663'},
			{"diamond",'\u2666'}
		};
	}

	/// <summary>
	/// A deck of 52 cards.
	/// </summary>
	public class Deck
	{
		public List<Card> Cards;

		/// <summary>
		/// Initializes a standard 52 card deck.
		/// </summary>
		public Deck()
		{
			string[] suits = { "spade","heart","club","diamond" };

			Cards = new List<Card>(52);

			foreach (var suit in suits)
				for (int val = 0; val < 13; val++)
					Cards.Add(new Card(val + 1, suit));

			Shuffle();
		}

		/// <summary>
		/// Randomizes the order of the cards.
		/// </summary>
		public void Shuffle()
		{
			Random rand = new Random();

			for(int i = 0; i < 500; i++)
			{
				var idx1 = rand.Next(Cards.Count);
				var idx2 = rand.Next(Cards.Count);

				var cardTemp = Cards[idx1];
				Cards[idx1] = Cards[idx2];
				Cards[idx2] = cardTemp;
			}
		}

		/// <summary>
		/// Deals a given number of cards to a hand.
		/// </summary>
		/// <param name="h">The Hand object to which the deck will deal cards.</param>
		/// <param name="num">The number of cards to deal.</param>
		/// <param name="hideNum">The number of cards which will be hidden.</param>
		public void DealHand(Hand h, int num, int hideNum=0)
		{
			for (var i = 0; i < num; i++)
			{
				DealCard(h,i < hideNum);
			}
		}

		/// <summary>
		/// Deals a card to a given hand.
		/// </summary>
		/// <param name="hand">The hand which receives the card.</param>
		/// <param name="hide">Whether or not to hide the card (it will appear as ??).</param>
		public void DealCard(Hand hand, bool hide=false)
		{
			int idx = new Random().Next(Cards.Count);

			((Card)Cards[idx]).IsVisible = !hide;

			hand.Add((Card)Cards[idx]);

			Cards.Remove(Cards[idx]);
		}

		/// <summary>
		/// Outputs the contents of the deck.
		/// </summary>
		/// <returns>Returns every card on a separate line.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach(var c in Cards)
				sb.Append(c.ToString()).Append("\n");

			return sb.ToString();
		}
	}

	/// <summary>
	/// A single hand of several cards.
	/// </summary>
	public abstract class Hand
	{
		/// <summary>
		/// The list of cards which holds the hand.
		/// </summary>
		public List<Card> Cards { get; set; }

		/// <summary>
		/// Initializes the hand.
		/// </summary>
		public Hand()
		{
			Cards = new List<Card>();
		}
		
		/// <summary>
		/// Adds a card to the hand.
		/// </summary>
		/// <param name="c">The card to add.</param>
		public void Add(Card c) {
			Cards.Add(c);
		}

		/// <summary>
		/// Converts the hand to a string.
		/// </summary>
		/// <returns>Returns the ToString() for each card separated by spaces.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach( Card c in Cards)
				sb.Append(c).Append(" ");

			return sb.ToString();
		}
	}

	/// <summary>
	/// A hand which is able to calculate its value according to the rules of blackjack.
	/// </summary>
	public class BlackjackHand : Hand
	{
		/// <summary>
		/// A calculcated property. The value of the hand.
		/// </summary>
		public int HandValue
		{
			get
			{
				int handVal = 0;

				foreach(Card c in Cards)
				{
					int cardID = c.ID;

					if(cardID >= 10)
					{
						handVal += 10;
					}
					else if (cardID == 1)
					{
						handVal += 11;
					}
					else
					{
						handVal += cardID;
					}
				}

				int downgradedAces = 0;
				int numAces = Cards.Count(c => c.ID == 1);

				while (handVal > 21 && numAces > downgradedAces)
				{
					handVal -= 10;
					downgradedAces++;
				}

				return handVal;
			}
		}
	}
}
