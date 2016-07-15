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
			Console.OutputEncoding = System.Text.Encoding.Unicode;

			Deck deck = new Deck();

			//Console.Write(deck);

			Hand hand = new Hand();
			Hand hand2 = new Hand();

			deck.DealHand(hand, 2, 1);
			deck.DealHand(hand2, 5, 5);

			Console.WriteLine(hand);
			Console.WriteLine(hand2);

			Console.ReadLine();
        }
    }
}
