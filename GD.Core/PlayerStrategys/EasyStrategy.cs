namespace GD.Core.PlayerStrategys
{
    public class EasyStrategy : GDPlayer
    {
        public EasyStrategy(string name) : base(name)
        {

        }

        public override (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
        {
            Random random = new Random();
            bool chooseToPlay = random.Next(2) == 0;
            if (playedCards == null || chooseToPlay)
            {
                var start = SingleRankHigher(playedCards);
                return (start != null, start, Hand.Count);
            }

            return (false, null, Hand.Count);
        }
    }
}
