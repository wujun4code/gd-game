namespace GD.Core.PlayerStrategys
{
    public class PassiveStrategry: GDPlayer
    {
        public PassiveStrategry(string name) : base(name)
        {

        }

        public override (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
        {
            if (playedCards == null)
            {
                var start = SingleRankHigher(playedCards);
                return (start != null, start, Hand.Count);
            }
           
            return (false, null, Hand.Count);
        }
    }
}
