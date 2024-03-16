using Autofac.Extras.Moq;
using GD.Core;
using GD.Core.PlayerStrategys;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GD.UnitTests
{
    public class RoundTests
    {
        AutoMock _mock;
        private GDRound sut;
        public RoundTests()
        {
            _mock = AutoMock.GetLoose(cfg =>
            {

            });
        }

        class StaicMaxSixStrategyPlayer : GDPlayer
        {
            public StaicMaxSixStrategyPlayer(string name) : base(name)
            {

            }

            public override (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
            {
                int count = 6;
                if (playedCards != null) count = playedCards.Count;
                var cardsToPlay = RandomPlay(count);
                return (true, cardsToPlay, Hand.Count);
            }
        }

        List<GDPlayer> CreatePlayers(int count)
        {
            var players = new List<StaicMaxSixStrategyPlayer>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new StaicMaxSixStrategyPlayer("Player " + i));
            }

            return players.Cast<GDPlayer>().ToList();
        }

        List<GDPlayer> CreateSingleStrategyPlayers(int count)
        {
            var players = new List<SingleStrategy>();
            for (int i = 1; i <= count; i++)
            {
                players.Add(new SingleStrategy("SingleStrategy " + i));
            }

            return players.Cast<GDPlayer>().ToList();
        }

        [Fact]
        public void Round_End_Without_Winner()
        {
            var mocker = new MockData();
            var players = CreatePlayers(4);
            var teams = mocker.CreateTeams(players, 2);
            var deck = new GDDeck(2);

            var game = new GDGame(players, teams, deck);
            var team = game.Play();
            Assert.Null(team);
        }

        [Fact]
        public void RoundResult_Should_Get_3_Scores()
        {
            var mocker = new MockData();
            var players = CreatePlayers(4);
            List<GDTeam> teams = new List<GDTeam>()
            {
                { new GDTeam(1,"test team1"){ Players = players.Take(2).ToList()} },
                { new GDTeam(2,"test team1") { Players = players.Skip(2).Take(2).ToList()} },
            };

            foreach (var team in teams)
            {
                foreach (var player in team.Players)
                {
                    player.Team = team;
                }
            }

            var sutResult = new RDRoundResult(players);

            var actual = sutResult.GetScores();
            Assert.Equal(3, (int)actual);
        }


        [Fact]
        public void GetNext_Test()
        {
            var mocker = new MockData();
            var players = CreatePlayers(4);
            var teams = mocker.CreateTeams(players, 2);
            var deck = new GDDeck(2);
            var game = new GDGame(players, teams, deck);
            var primaryPlayer = game.NextPrimary();
            var round = new GDRound(game, primaryPlayer);

            var test1 = round.GetNextPlayer(primaryPlayer);
            Assert.Equal(2, test1?.Seat);

            var test2 = round.GetNextPlayer(test1);
            var test3 = round.GetNextPlayer(test2);
            var test4 = round.GetNextPlayer(test3);
            Assert.Equal(1, test4?.Seat);

            test1.Hand = new List<GDCard>();
            test4.Hand = new List<GDCard>();

            var test5 = round.GetNextPlayer(test4);
            Assert.Equal(3, test5?.Seat);
        }
    }
}
