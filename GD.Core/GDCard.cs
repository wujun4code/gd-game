using System.Numerics;
using System.Text;

namespace GD.Core
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        SmallJoker,
        BigJoker,
    }

    public class GDCard
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public GDCard(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Rank}";
        }

        public static string FormatListAsString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToString());
                if (i < list.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
    }

    public class GDDeck
    {
        public List<GDCard> Cards;

        public GDDeck(int count = 1)
        {
            Cards = new List<GDCard>();
            for (int i = 0; i < count; i++)
            {
                InitializeDeck();
            }
        }

        private void InitializeDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    if (rank != Rank.BigJoker && rank != Rank.SmallJoker)
                    {
                        Cards.Add(new GDCard(suit, rank));
                    }
                }
            }

            Cards.Add(new GDCard(Suit.Hearts, Rank.BigJoker));
            Cards.Add(new GDCard(Suit.Hearts, Rank.SmallJoker));
        }
    }

}
