namespace GD.Core
{
    public class GDLevel
    {
        public GDCard CurrentCard { get; set; }
        public int AceTries { get; set; } = 0;
        public GDLevel() :
            this(new GDCard(Suit.Hearts, Rank.Two))
        {

        }

        public GDLevel(GDCard current)
        {
            CurrentCard = current;
        }

        public Rank LevelUp(int scores)
        {
            if (CurrentCard.Rank == Rank.Ace) return CurrentCard.Rank;

            int nextRankValue = (int)CurrentCard.Rank + scores;
            if (nextRankValue >= (int)Rank.Ace)
            {
                CurrentCard.Rank = Rank.Ace;
            }
            else
            {
                CurrentCard.Rank = (Rank)nextRankValue;
            }
            return CurrentCard.Rank;
        }

        public Rank Reset()
        {
            return Rank.Two;
        }
    }
}
