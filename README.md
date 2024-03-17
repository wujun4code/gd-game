# GuanDan 


[GuanDan](https://zh.wikipedia.org/zh-hans/%E6%8E%BC%E8%9B%8B) is one of most popular poke games spreading fast in South of China.

This project is a simple implement of GuanDan, not all the rules are applied.


## Run locally

open the `./GDGame/GDGame.sln` with Visual Studio and press `F5`, you will run a mock game.


### Change strategy to play cards

you can create a new class extends from `GDPlayer`, here is a built in sample

```cs
public class EasyStrategy : GDPlayer
{
    public EasyStrategy(string name) : base(name)
    {

    }

    public override (bool, List<GDCard>?, int) Play(GDRound round, List<GDCard>? playedCards)
    {
        Random random = new Random();
        bool chooseToPlay = random.Next(2) == 0;
        if (playedCards == null || chooseToPlay)
        {
            var start = SingleRankHigher(playedCards);
            return (start != null, start, Hand.Count);
        }

        return (false, null, Hand.Count);
    }
}
```


## Design Concept

here is a State Transition workflow

```mermaid
stateDiagram-v2
    [*] --> Game
    Game --> [*]
    Game --> Round
    Round --> RoundResult
    RoundResult --> Round
    RoundResult --> GameResult
    GameResult --> [*]
```

## How these objects work together?

```mermaid
classDiagram
    Game <|-- Team
    Game <|-- Player
    Game <|-- Deck
    Game <|-- Round
    class Game {
        +[Team]Teams
        +[Player] Players
        +Deck Deck
        +AssignSeats([Player] players)
        +DistributeCards([Player] players)
        +Play() Team
    }

    Team <|-- Level
    Team <|-- Player
    class Team{
      +int Id
      +string Name
      +[Player] Players
      +Level Level
      +MergeResult(RoundResult result)
    }
    Player <|-- Card
    class Player{
      +[Card] Hand
    }
    class Deck{
    }
    Deck <|-- Card
    Deck: +Cards
    class Card{
      +enum Suit 
      +enum Rank  
    }
    Level <|-- Card
    class Level{
      +Card CurrentCard
    }
    Round <|-- RoundResult
    class Round {
       +Game Game
       +Dictionary[Player, int] _played;
       +PlayRound(Player onCallingPlayer) RoundResult
    }
    Player <|-- RoundResult
    class RoundResult {
       +Game Game
       +Player Winner
       +GetScores() int
    }
```