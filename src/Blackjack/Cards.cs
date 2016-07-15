using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Card
    {
		public int ID { get; }
		public string Suit { get; }
		public string Symbol { get; }
		public char SuitSymbol { get; }
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

		public Card(int id, string suit)
		{
			ID = id;
			Suit = suit;
			Symbol = CardSymbols[ID];
			SuitSymbol = SuitSymbols[suit];
			IsVisible = true;
		}

		public override string ToString()
		{
			if (IsVisible)
				return Symbol + SuitSymbol;
			else
				return "??";
		}

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

		public static Dictionary<string, char> SuitSymbols = new Dictionary<string, char>()
		{
			{"spade",'\u2660'},
			{"heart",'\u2665'},
			{"club",'\u2663'},
			{"diamond",'\u2666'}
		};
	}

	public class Deck
	{
		public ArrayList Cards;

		public Deck()
		{
			string[] suits = { "spade","heart","club","diamond" };

			Cards = new ArrayList(52);

			foreach (var suit in suits)
				for (int val = 0; val < 13; val++)
					Cards.Add(new Card(val + 1, suit));

			Shuffle();
		}

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

		public void DealHand(Hand h, int num, int hideNum=0)
		{
			for (var i = 0; i < num; i++)
			{
				DealCard(h,i < hideNum);
			}
		}

		public void DealCard(Hand hand, bool hide)
		{
			int idx = new Random().Next(Cards.Count);

			((Card)Cards[idx]).IsVisible = !hide;

			hand.Add((Card)Cards[idx]);

			Cards.Remove(Cards[idx]);
		}


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach(var c in Cards)
				sb.Append(c.ToString()).Append("\n");

			return sb.ToString();
		}
	}

	public class Hand
	{
		public ArrayList Cards;

		public Hand()
		{
			Cards = new ArrayList();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach( Card c in Cards)
				sb.Append(c).Append(" ");

			return sb.ToString();
		}

		public void Add(Card c)
		{
			Cards.Add(c);
		}
	}
}
