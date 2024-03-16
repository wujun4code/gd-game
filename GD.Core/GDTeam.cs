namespace GD.Core
{
    public class GDTeam
    {
        public string Name { get; set; }
        public List<GDPlayer> Players { get; set; }
        public GDLevel Level { get; set; }
        public int Id { get; }

        public bool Won { get; set; }
        public GDTeam(int id, string name)
        {
            Name = name;
            Players = new List<GDPlayer>();
            Level = new GDLevel();
            Id = id;
        }

        public override string ToString()
        {
            return $"team-{Name}-{Id}";
        }

        public void MergeResult(GDGame game, RDRoundResult result)
        {
            var winStyle = result.GetScores();
            int scores = (int)winStyle;
            if (result.Winner.Team.Id != Id)
            {
                if (Level.CurrentCard.Rank == Rank.Ace)
                {
                    Level.AceTries++;
                    game.Log($"{this} round failed and AceTries becomes {Level.AceTries}");
                    if (Level.AceTries == 3)
                    {
                        game.Log($"{this} round failed and AceTries upto 3, will reset level.");
                        Level.CurrentCard.Rank = Level.Reset();
                    }
                }
                Won = false;
                return;
            }
            else
            {
                var won = Level.CurrentCard.Rank == Rank.Ace && scores > 1;
                if (won)
                {
                    game.Log($"{this} round won and pass Ace level, WON!!!");
                    Won = true;
                    return;
                }

                if (Level.CurrentCard.Rank == Rank.Ace)
                {
                    Level.AceTries++;
                    game.Log($"{this} round won but scores is {scores}, AceTries becomes {Level.AceTries}");
                    if (Level.AceTries == 3)
                    {
                        game.Log($"{this} round won but scores is {scores}, and AceTries upto 3, will reset level.");
                        Level.CurrentCard.Rank = Level.Reset();
                    }
                }
                else
                {
                    Level.CurrentCard.Rank = Level.LevelUp(scores);
                    game.Log($"{this} round won and scores is {scores}, will level up to {Level.CurrentCard.Rank}");
                }

                Won = false;
                return;
            }
        }
    }
}
