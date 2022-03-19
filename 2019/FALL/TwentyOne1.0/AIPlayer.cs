using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
using static TwentyOne.TwentyOne;

namespace TwentyOne
{
    public class AIPlayer: AbstractPlayer
    {
        public override void Act()
        {
            EvaluateHand();
            if (minHandValue >= 20)
            {
                playing = false;
                return;
            }
            if (maxHandValue <= 11)
                hand.Add(GenerateCard());
            else
            {
                var random = new Random();
                var n = random.NextDouble();
                var AIRiskChance = (21 - minHandValue) * AIRiskDelta;
                if (n < AIRiskChance)
                {
                    hand.Add(GenerateCard());
                }
                else
                    playing = false;
            }
        }
    }
}