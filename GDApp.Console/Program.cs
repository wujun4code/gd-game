using GD.Core.PlayerStrategys;
using GD.Core;
using System.Net.Sockets;

namespace GDApp.Gamer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, GD!");

            MockEasyBattleEasy();

            MockSingleAIBattelEasy();
            Console.ReadLine();
        }

        static void MockEasyBattleEasy()
        {
            var easyPlayers = CreateEasyPlayers(4);
            var teams = CreateTeams(easyPlayers, 2);
            var deck = new GDDeck(2);
            var game = new GDGame(easyPlayers, teams, deck);
            game.GameLogger = Console.WriteLine;
            var wonTeam = game.Play();
        }

        static void MockSingleAIBattelEasy() 
        {
            var aiPlayers = CreateSingleStrategyPlayers(2);
            var easyPlayers = CreateEasyPlayers(2);
            List<GDTeam> teams = new List<GDTeam>()
            {
                { new GDTeam(1,"AI"){ Players = aiPlayers} },
                { new GDTeam(2,"Easy") { Players = easyPlayers} },
            };
            var mergedTeams = aiPlayers.Concat(easyPlayers).ToList();

            foreach (var team in teams)
            {
                foreach (var player in team.Players)
                {
                    player.Team = team;
                }
            }

            var deck = new GDDeck(2);
            var game = new GDGame(mergedTeams, teams, deck);
            game.GameLogger = Console.WriteLine;
            var wonTeam = game.Play();
        }

        static List<GDTeam> CreateTeams(List<GDPlayer> players, int count)
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
        static List<GDPlayer> CreateSingleStrategyPlayers(int count)
        {
            var players = new List<SingleStrategy>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new SingleStrategy("Single " + i));
            }

            return players.Cast<GDPlayer>().ToList();
        }
        static List<GDPlayer> CreatePassivePlayers(int count)
        {
            var players = new List<PassiveStrategry>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new PassiveStrategry("Passive " + i));
            }

            return players.Cast<GDPlayer>().ToList();
        }

        static List<GDPlayer> CreateEasyPlayers(int count)
        {
            var players = new List<EasyStrategy>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new EasyStrategy("Easy " + i));
            }

            return players.Cast<GDPlayer>().ToList();
        }
    }
}
