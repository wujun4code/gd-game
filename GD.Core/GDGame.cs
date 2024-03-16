using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GD.Core
{
    public class GDGameResult
    {

    }

    public class GDGame
    {
        public List<GDTeam> Teams { get; set; }
        public List<GDPlayer> Players { get; set; }
        public GDDeck Deck { get; set; }

        public Action<string> GameLogger { get; set; }

        public GDGame(List<GDPlayer> players,
            List<GDTeam> teams,
            GDDeck deck)
        {
            Teams = teams;
            Players = players;
            Deck = deck;
            AssignSeats(players);
            DistributeCards(players);
        }

        public void AssignSeats(List<GDPlayer> players)
        {
            List<int> seatNumbers = Enumerable.Range(1, players.Count).ToList();

            Random random = new Random();
            seatNumbers = seatNumbers.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Seat = seatNumbers[i];
            }
        }

        public void DistributeCards(List<GDPlayer> players)
        {
            Shuffle(Deck.Cards);

            int numCardsPerPlayer = Deck.Cards.Count / players.Count;
            int remainingCards = Deck.Cards.Count % players.Count;

            int cardIndex = 0;
            foreach (var player in players)
            {
                for (int i = 0; i < numCardsPerPlayer; i++)
                {
                    player.Hand.Add(Deck.Cards[cardIndex]);
                    cardIndex++;
                }
            }

            for (int i = 0; i < remainingCards; i++)
            {
                players[i].Hand.Add(Deck.Cards[cardIndex]);
                cardIndex++;
            }
        }

        public GDTeam? Play()
        {
            RDRoundResult lastRoundResult = null;
            while (true)
            {
                var primaryPlayer = NextPrimary(lastRoundResult);
                var newRound = new GDRound(this, primaryPlayer);
                lastRoundResult = newRound.PlayRound(primaryPlayer);
                Log($"round result:${lastRoundResult}");

                foreach (var team in Teams)
                {
                    team.MergeResult(this, lastRoundResult);
                    if (team.Won)
                    {
                        Log($"game over:{team} won!!!");
                        return team;
                    }

                }

                if (IsOnlyLastOnePlayerRemaining()) break;
                if (IsOnlyOneTeamRemaining()) break;
            }

            return Teams.FirstOrDefault(t => t.Won);
        }

        public GDPlayer NextPrimary(RDRoundResult lastRoundResult = null)
        {
            if (lastRoundResult == null)
            {
                var primaryPlayer = Players.MinBy(p => p.Seat);
                if (primaryPlayer == null) throw new Exception("no player found.");

                return primaryPlayer;
            }
            else
            {
                return lastRoundResult.Winner;
            }
        }

        public void Log(string text)
        {
            if (GameLogger != null)
            {
                GameLogger(text);
            }
        }

        private bool IsOnlyLastOnePlayerRemaining()
        {
            return Players.Count(p => p.Hand.Count > 0) == 1;
        }

        private bool IsOnlyOneTeamRemaining()
        {
            var teamsWithActivePlayers = Teams.Count(team => team.Players.Any(player => !player.Done));

            return teamsWithActivePlayers < 2;
        }

        public void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public enum WinStyle
    {
        DoubleDown = 3,
        SingleDown = 2,
        NoDown = 1,
        None = 0,
    }

    public class RDRoundResult
    {
        public readonly List<GDPlayer> RecentlyPlayedPlayers;
        public GDPlayer? Winner { get; private set; }

        public RDRoundResult(List<GDPlayer> recentlyPlayedPlayers)
        {
            RecentlyPlayedPlayers = recentlyPlayedPlayers;
            if (recentlyPlayedPlayers.Count > 0)
                Winner = RecentlyPlayedPlayers[0];
        }

        public override string ToString()
        {
            string playersAsString = string.Join(", ", RecentlyPlayedPlayers);
            return playersAsString;
        }

        public WinStyle GetScores()
        {
            int scores = 0;
            int playerCount = RecentlyPlayedPlayers.Count;
            if (playerCount == 0) return WinStyle.None;
            if (playerCount == 1)
            {
                scores = 0;
            }
            else
            {
                var lastPlayer = RecentlyPlayedPlayers[0];
                var secondLastPlayer = RecentlyPlayedPlayers[1];
                if (playerCount == 2)
                {
                    scores = (lastPlayer.Team == secondLastPlayer.Team) ? 3 : 0;
                }
                else if (playerCount == 3)
                {
                    var thirdLastPlayer = RecentlyPlayedPlayers[2];
                    scores = lastPlayer.Team == secondLastPlayer.Team ? 3 :
                           (lastPlayer.Team == thirdLastPlayer.Team) ? 2 : 0;
                }
                else if (playerCount >= 4)
                {
                    var thirdLastPlayer = RecentlyPlayedPlayers[2];
                    var fourthLastPlayer = RecentlyPlayedPlayers[3];
                    scores = lastPlayer.Team == secondLastPlayer.Team ? 3 :
                           lastPlayer.Team == thirdLastPlayer.Team ? 2 :
                           (lastPlayer.Team == fourthLastPlayer.Team) ? 1 : 0;
                }
            }

            return (WinStyle)scores;
        }
    }
}
