using GD.Core;
using System.Numerics;

namespace GD.UnitTests
{
    public class CardTests
    {
        [Fact]
        public void Shuffle_Cards_Count_Should_Equals_108()
        {
            var deck = new GDDeck(2);

            Assert.Equal(108, deck.Cards.Count);
        }

        [Fact]
        public void Distribute_Cards_To_Players_Should_Be_Equally()
        {
            var mocker = new MockData();
            var players = mocker.CreatePlayers(4);
            var teams = mocker.CreateTeams(players, 2);
            var deck = new GDDeck(2);

            var game = new GDGame(players, teams, deck);

            var totalCardCount = deck.Cards.Count;

            Assert.Equal(totalCardCount / players.Count, players[0].Hand.Count);
            Assert.Equal(totalCardCount / players.Count, players[1].Hand.Count);
            Assert.Equal(totalCardCount / players.Count, players[2].Hand.Count);
            Assert.Equal(totalCardCount / players.Count, players[3].Hand.Count);

            HashSet<GDCard> allCards = new HashSet<GDCard>();
            foreach (var player in players)
            {
                foreach (var card in player.Hand)
                {
                    Assert.DoesNotContain(card, allCards);
                    allCards.Add(card);
                }
            }
        }
    }
}