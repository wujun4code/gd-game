namespace GD.Core
{
    public class GDRound
    {
        public GDGame Game { get; set; }
        public GDPlayer PrimaryPlayer { get; set; }

        List<GDPlayer> _passedPlayers;
        Dictionary<GDPlayer, int> _played;
        public GDRound(GDGame game, GDPlayer primary)
        {
            Game = game;
            PrimaryPlayer = primary;
            _passedPlayers = new List<GDPlayer> { };
            _played = new Dictionary<GDPlayer, int> { };
        }

        public RDRoundResult PlayRound(GDPlayer onCallingPlayer)
        {
            var currentPlayer = onCallingPlayer;
            List<GDCard>? current = null;
            while (true)
            {
                var (played, payload, remaining) = currentPlayer.Play(this, current);
                if (played)
                {
                    Game.Log($"{currentPlayer} played cards:{GDCard.FormatListAsString(payload)}");
                    foreach (var key in _played.Keys.ToList())
                    {
                        _played[key]++;
                    }
                    _played[currentPlayer] = 0;
                    _passedPlayers.Clear();
                }
                else
                {
                    Game.Log($"{currentPlayer} passed.");
                    _passedPlayers.Add(currentPlayer);
                }

                currentPlayer = GetNextPlayer(currentPlayer);
                if (currentPlayer == null)
                {
                    Game.Log($"no next player, round end.");
                    break;
                }
                if (IsOnlyLastOnePlayerRemaining())
                {
                    Game.Log($"only last one player remaining, round end.");
                    break;
                }
                if (payload != null)
                    current = payload;
                if (AllPlayersPassed())
                {
                    Game.Log($"all players passed, round end.");
                    break;
                }
            }

            return Snapshot();
        }

        public RDRoundResult Snapshot()
        {
            var sortedPlayers = _played.OrderBy(kv => kv.Value)
                                      .Select(kv => kv.Key)
                                      .ToList();

            return new RDRoundResult(sortedPlayers);
        }

        public GDPlayer? GetNextPlayer(GDPlayer currentPlayer)
        {
            int nextSeat = (currentPlayer.Seat % Game.Players.Count) + 1;
            var nextPlayer = Game.Players.OrderBy(p => p.Seat)
                                   .SkipWhile(p => p.Seat != nextSeat)
                                   .SkipWhile(p => p.Seat == currentPlayer.Seat)
                                   .FirstOrDefault(p => !p.Done);
            return nextPlayer;
        }

        private bool IsOnlyLastOnePlayerRemaining()
        {
            return Game.Players.Count(p => p.Hand.Count > 0) == 1;
        }

        private bool AllPlayersPassed()
        {
            return _passedPlayers.Count == Game.Players.Count - 1;
        }
    }
}
