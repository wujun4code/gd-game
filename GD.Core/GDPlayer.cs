using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GD.Core
{
    public class GDPlayer
    {
        public string Name { get; set; }
        public List<GDCard> Hand { get; set; }
        public int Seat { get; set; }
        public bool Done => Hand.Count == 0;
        public GDTeam Team { get; set; }

        public GDPlayer(string name)
        {
            Name = name;
            Hand = new List<GDCard>();
        }

        public override string ToString()
        {
            return $"{Name}@team{Team.Id}-seat{Seat}";
        }

        public virtual (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
        {
            if (playedCards == null || !playedCards.Any())
            {
                int max = 6;
                var cardsToPlay = RandomPlay(max);
                return (true, cardsToPlay, Hand.Count);
            }
            if (!CanBeat(playedCards)) return (false, null, Hand.Count);

            Random random = new Random();
            bool chooseToPlay = random.Next(2) == 0;

            if (chooseToPlay)
            {
                var cardsToPlay = RandomPlay(playedCards.Count);
                return (true, cardsToPlay, Hand.Count);
            }

            return (false, null, Hand.Count);
        }

        protected List<GDCard>? RandomPlay(int count)
        {
            if (count < 1) return null;
            if (count > Hand.Count) return RandomPlay(Hand.Count);
            List<GDCard> cardsToPlay = new List<GDCard>();
            Random rng = new Random();
            int n = Hand.Count;
            for (int i = 0; i < count; i++)
            {
                int randomIndex = rng.Next(n);

                cardsToPlay.Add(Hand[randomIndex]);
                Hand.RemoveAt(randomIndex);
                n--;
            }
            return cardsToPlay;
        }

        protected List<GDCard>? PlayCards(List<GDCard> toGo)
        {
            Hand.RemoveAll(item => toGo.Contains(item));
            return toGo;
        }

        protected List<GDCard>? SingleRankHigher(List<GDCard>? current)
        {
            if (Done) return null;
            if (current == null)
            {
                var minOne = Hand.MinBy(item => item.Rank);
                return PlayCards(new List<GDCard> { minOne });
            }
            if (current.Count > 1) return null;
            var currentOne = current.First();
            var options = Hand.Where(x => x.Rank > currentOne.Rank);
            if (options == null || !options.Any()) return null;
            var picked = options.OrderBy(x => x.Rank).First();
            if (picked == null) return null;
            return PlayCards(new List<GDCard> { picked });
        }

        private bool CanBeat(List<GDCard> playedCards)
        {
            if (Hand.Count < playedCards.Count) return false;
            // more logic to check if current hand can beat current cards
            return true;
        }
    }
}
