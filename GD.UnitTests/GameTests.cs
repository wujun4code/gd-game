using GD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.UnitTests
{
    public class GameTests
    {
        [Fact]
        public void Game_Start_Should_From_Level_Two() 
        {
            var mocker = new MockData();
            var players = mocker.CreatePlayers(4);
            var teams = mocker.CreateTeams(players, 2);
            var deck = new GDDeck(2);

            var game = new GDGame(players, teams, deck);

            game.AssignSeats(players);
            game.DistributeCards(players);
        }
    }
}
