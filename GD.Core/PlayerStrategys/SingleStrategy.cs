using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.Core.PlayerStrategys
{
    public class SingleStrategy : GDPlayer
    {
        public SingleStrategy(string name) : base(name)
        {

        }

        public override (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
        {
            if (playedCards == null)
            {
                var start = SingleRankHigher(playedCards);
                return (start != null, start, Hand.Count);
            }
            if (playedCards != null && playedCards.Count > 1)
                return (true, null, Hand.Count);
            var judge = JudgeSnapshot(round);
            if (judge)
            {
                var cardsToPlay = SingleRankHigher(playedCards);
                return (cardsToPlay != null, cardsToPlay, Hand.Count);
            }
            return (false, null, Hand.Count);
        }

        private bool JudgeSnapshot(GDRound round)
        {
            var snapshot = round.Snapshot();
            if (snapshot.Winner == null) return true;
            if (snapshot.Winner.Team.Id == Team.Id)
            {
                var scores = snapshot.GetScores();
                if (scores >= WinStyle.SingleDown)
                    return false;
            }
            return true;
        }
    }
}
