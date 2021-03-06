using System.Collections.Generic;
using static TwentyOne.TwentyOne;

namespace TwentyOne
{
    public abstract class AbstractPlayer
    {
        protected List<Cards> hand;
        protected bool playing;
        protected int minHandValue;
        protected int maxHandValue;

        public bool Playing
        {
            get => playing;
        }

        public List<Cards> Hand
        {
            get => hand;
        }

        public int MaxHandValue
        {
            get => maxHandValue;
        }

        public int MinHandValue
        {
            get => minHandValue;
        }

        public abstract void Act();

        public void InitPlayer()
        {
            hand = new List<Cards> {GenerateCard()};
            playing = true;
        }
        
        public void EvaluateHand()
        {
            foreach (var card in hand)
            {
                var cardValue = (int) card;
                if (cardValue == 0)
                {
                    minHandValue += 1;
                    maxHandValue += 11;
                }
                else
                    if (cardValue >= TwentyOne.FirstPictureNumber)
                    {
                        minHandValue += 10;
                        maxHandValue += 10;
                    }
                    else
                    {
                        minHandValue += cardValue;
                        maxHandValue += cardValue;
                    }
            }
        }
    }
}