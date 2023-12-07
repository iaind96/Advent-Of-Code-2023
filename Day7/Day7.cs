namespace Day7
{
    internal class Day7
    {
        private static readonly string inputFilepathExample = "input_example.txt";
        private static readonly string inputFilepath = "input.txt";

        private enum Hands
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        private class Hand
        {
            public string Cards;
            public int Bid;

            public int HandScore;
            public int HandScore2;

            private static readonly Dictionary<char, int> cardMapping = new Dictionary<char, int>()
            {
                { 'A', 14 },
                { 'K', 13 },
                { 'Q', 12 },
                { 'J', 11 },
                { 'T', 10 }
            };

            private static readonly Dictionary<char, int> cardMapping2 = new Dictionary<char, int>()
            {
                { 'A', 14 },
                { 'K', 13 },
                { 'Q', 12 },
                { 'J', 0 },
                { 'T', 10 }
            };

            public Hand(string cards, int bid)
            {
                Cards = cards;
                Bid = bid;

                HandScore = ScoreHand(cards);
                HandScore2 = ScoreHand2(cards);
            }

            private static int ScoreHand(string cards)
            {

                // first get the type of hand
                string cardsToCount = string.Copy(cards);

                List<int> cardCounts = new List<int>();

                while (cardsToCount.Length > 0)
                {
                    int count = 0;
                    for (int i = 0; i < cardsToCount.Length; i++)
                    {
                        if (cardsToCount[0] == cardsToCount[i])
                        {
                            count++;
                        }
                    }
                    cardCounts.Add(count);

                    cardsToCount = cardsToCount.Replace(cardsToCount[0].ToString(), string.Empty);
                }

                cardCounts = cardCounts.OrderDescending().ToList();

                Hands hand;
                if (cardCounts[0] == 5)
                {
                    hand = Hands.FiveOfAKind;
                }
                else if (cardCounts[0] == 4)
                {
                    hand = Hands.FourOfAKind;
                }
                else if (cardCounts[0] == 3 & cardCounts[1] == 2)
                {
                    hand = Hands.FullHouse;
                }
                else if (cardCounts[0] == 3)
                {
                    hand = Hands.ThreeOfAKind;
                }
                else if (cardCounts[0] == 2 & cardCounts[1] == 2)
                {
                    hand = Hands.TwoPair;
                }
                else if (cardCounts[0] == 2)
                {
                    hand = Hands.OnePair;
                }
                else
                {
                    hand = Hands.HighCard;
                }

                // create combined score by bit shifting
                int handScore = (int)hand << 20;

                for (int i = 0; i < cards.Length; i++)
                {
                    int cardScore;
                    if (char.IsDigit(cards[i]))
                    {
                        cardScore = int.Parse(cards[i].ToString());
                    }
                    else
                    {
                        cardScore = cardMapping[cards[i]];
                    }


                    handScore |= cardScore << (16 - 4 * i);
                }

                return handScore;
            }

            private static int ScoreHand2(string cards) 
            {
                // first get the type of hand
                string cardsToCount = string.Copy(cards);

                Dictionary<char, int> cardCountsByChar = new Dictionary<char, int>();

                while (cardsToCount.Length > 0)
                {
                    int count = 0;
                    for (int i = 0; i < cardsToCount.Length; i++)
                    {
                        if (cardsToCount[0] == cardsToCount[i])
                        {
                            count++;
                        }
                    }
                    cardCountsByChar.Add(cardsToCount[0], count);

                    cardsToCount = cardsToCount.Replace(cardsToCount[0].ToString(), string.Empty);
                }

                // joker total gets added to the highest total of other cards
                if (cardCountsByChar.Remove('J', out int jokerCount))
                {
                    if (jokerCount < 5)
                    {
                        char maxCard = cardCountsByChar.MaxBy(kvp => kvp.Value).Key;
                        cardCountsByChar[maxCard] += jokerCount;
                    } 
                    else
                    {
                        cardCountsByChar['J'] = jokerCount;
                    }
                    
                }

                List<int> cardCounts = cardCountsByChar.Values.OrderDescending().ToList();

                Hands hand;
                if (cardCounts[0] == 5)
                {
                    hand = Hands.FiveOfAKind;
                }
                else if (cardCounts[0] == 4)
                {
                    hand = Hands.FourOfAKind;
                }
                else if (cardCounts[0] == 3 & cardCounts[1] == 2)
                {
                    hand = Hands.FullHouse;
                }
                else if (cardCounts[0] == 3)
                {
                    hand = Hands.ThreeOfAKind;
                }
                else if (cardCounts[0] == 2 & cardCounts[1] == 2)
                {
                    hand = Hands.TwoPair;
                }
                else if (cardCounts[0] == 2)
                {
                    hand = Hands.OnePair;
                }
                else
                {
                    hand = Hands.HighCard;
                }

                // create combined score by bit shifting
                int handScore = (int)hand << 20;

                for (int i = 0; i < cards.Length; i++)
                {
                    int cardScore;
                    if (char.IsDigit(cards[i]))
                    {
                        cardScore = int.Parse(cards[i].ToString());
                    }
                    else
                    {
                        cardScore = cardMapping2[cards[i]];
                    }


                    handScore |= cardScore << (16 - 4 * i);
                }

                return handScore;
            }
        }

        static void Main(string[] args)
        {
            List<Hand> inputs = ReadInput(inputFilepath);

            PartOne(inputs);
            PartTwo(inputs);
        }

        private static List<Hand> ReadInput(string inputFilepath)
        {
            List<string> lines = File.ReadLines(inputFilepath).ToList();

            List<Hand> hands = new List<Hand>();
            foreach (string line in lines)
            {
                string[] handComponents = line.Split(' ');
                string cards = handComponents[0];
                int bid = int.Parse(handComponents[1]);

                hands.Add(new Hand(cards, bid));
            }

            return hands;
        }

        private static void PartOne(List<Hand> hands)
        {
            hands = hands.OrderBy(hand => hand.HandScore).ToList();

            int totalWinnings = 0;
            for (int handIndex = 0; handIndex < hands.Count; handIndex++)
            {
                totalWinnings += (handIndex + 1) * hands[handIndex].Bid;
            }

            Console.WriteLine($"Part one: Total winnings = {totalWinnings}");
        }

        private static void PartTwo(List<Hand> hands)
        {
            hands = hands.OrderBy(hand => hand.HandScore2).ToList();

            int totalWinnings = 0;
            for (int handIndex = 0; handIndex < hands.Count; handIndex++)
            {
                totalWinnings += (handIndex + 1) * hands[handIndex].Bid;
            }

            Console.WriteLine($"Part two: Total winnings = {totalWinnings}");
        }
    }
}
