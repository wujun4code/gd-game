using GD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GD.UnitTests
{
    public class MockData
    {
        public List<GDPlayer> CreatePlayers(int count)
        {
            var players = new List<GDPlayer>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new GDPlayer("Player " + i));
            }
            return players;
        }

        public List<GDTeam> CreateTeams(List<GDPlayer> players, int count)
        {
            if (players.Count % count != 0)
            {
                throw new ArgumentException("Cannot evenly distribute players into teams.");
            }

            List<GDTeam> teams = new List<GDTeam>();
            for (int i = 1; i <= count; i++)
            {
                var team = new GDTeam(i, "Team " + i);
                teams.Add(team);
            }

            int playersPerTeam = players.Count / count;
            for (int i = 0; i < count; i++)
            {
                List<GDPlayer> teamPlayers = players.Skip(i * playersPerTeam).Take(playersPerTeam).ToList();
                teams[i].Players.AddRange(teamPlayers);
                foreach (var player in teamPlayers)
                {
                    player.Team = teams[i];
                }
            }

            return teams;
        }
    }
}
