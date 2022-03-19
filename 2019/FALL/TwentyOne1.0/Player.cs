using System;
using static TwentyOne.TwentyOne;

namespace TwentyOne
{
    public class Player: AbstractPlayer
    {
        public override void Act()
        {
            Console.WriteLine("Cards in your hand:");
            PrintHand();
            Console.WriteLine("Do you want to take a card? (1 - yes, 2 - no)");
            string a;
            do
            {
                a = Console.ReadLine();
            } while (a != "1" && a != "2");
            if (a == "1")
                hand.Add(GenerateCard());
            if (a == "2")
            {
                playing = false;
                return;
            }
            EvaluateHand();
            if (minHandValue > 21)
            {
                playing = false;
                Console.WriteLine("You have more than 21 points.");
                PrintHand();
            }
        }

    }
}