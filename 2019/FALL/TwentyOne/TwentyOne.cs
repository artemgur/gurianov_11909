using System;
using System.Security.Cryptography;

namespace TwentyOne
{
    public enum Cards
    {
        cA,
        c2,
        c3,
        c4,
        c5,
        c6,
        c7,
        c8,
        c9,
        c10,
        cJ,
        cQ,
        cK
    };
    
    public static class TwentyOne
    {
        private const int ColorsNumber = 4;
        private const int CardsNumber = 13;
        public const double AIRiskDelta = 0.1;
        public const int FirstPictureNumber = 10;
        private static AbstractPlayer[] players;
        private static Cards[] pack;
        private static int cardsGiven = 0;

        public static Cards[] Pack
        {
            get => pack;
        }


        public static Cards GenerateCard()
        {
            cardsGiven++;
            return pack[cardsGiven - 1];
        }

        public static void GeneratePack()
        {
            pack = new Cards[CardsNumber * 4];
            for (var i = 0; i < ColorsNumber; i++)
                for (var j = 0; j < CardsNumber; j++)
                    pack[CardsNumber * i + j] = (Cards) j;
            pack = Shuffle(pack);
            cardsGiven = 0;
        }

        private static Cards[] Shuffle(Cards[] a)
        {
            var random = new Random();
            for (var i = 0; i < a.Length; i++)
            {
                var x = random.Next(0, a.Length);
                var k = a[i];
                a[i] = a[x];
                a[x] = k;
            }

            return a;
        }

        private static int EvaluateScore(AbstractPlayer player)
        {
            var score = player.MinHandValue;
            if (score > 21)
                return -1;
            while (score < player.MaxHandValue || score < 11)
                score += 10;
            return score;
        }

        public static void Game()
        {
            players = new AbstractPlayer[2];
            players[0] = new Player();
            players[1] = new AIPlayer();
            while (players[0].Playing || !players[1].Playing)
            {
                foreach (var player in players)
                    if (player.Playing)
                        player.Act();
            }
            var scorePlayer = EvaluateScore(players[0]);
            var scoreAI = EvaluateScore(players[1]);
            if (scorePlayer < scoreAI)
                Console.WriteLine("You won");
            if (scorePlayer < scoreAI)
                Console.WriteLine("You lost");
            if (scorePlayer == scoreAI)
                Console.WriteLine("Tie");
        }
    }
}