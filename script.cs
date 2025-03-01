/*

██╗░░░██╗██████╗░███████╗██████╗░░██████╗███████╗████████╗███████╗████████╗  ███╗░░░███╗██╗████████╗
██║░░░██║██╔══██╗██╔════╝██╔══██╗██╔════╝██╔════╝╚══██╔══╝╚════██║╚══██╔══╝  ████╗░████║██║╚══██╔══╝
██║░░░██║██████╦╝█████╗░░██████╔╝╚█████╗░█████╗░░░░░██║░░░░░███╔═╝░░░██║░░░  ██╔████╔██║██║░░░██║░░░
██║░░░██║██╔══██╗██╔══╝░░██╔══██╗░╚═══██╗██╔══╝░░░░░██║░░░██╔══╝░░░░░██║░░░  ██║╚██╔╝██║██║░░░██║░░░
╚██████╔╝██████╦╝███████╗██║░░██║██████╔╝███████╗░░░██║░░░███████╗░░░██║░░░  ██║░╚═╝░██║██║░░░██║░░░
░╚═════╝░╚═════╝░╚══════╝╚═╝░░╚═╝╚═════╝░╚══════╝░░░╚═╝░░░╚══════╝░░░╚═╝░░░  ╚═╝░░░░░╚═╝╚═╝░░░╚═╝░░░

░█████╗░██╗░░░░░░█████╗░██╗░░░██╗██████╗░███████╗  ██████╗░░░░███████╗
██╔══██╗██║░░░░░██╔══██╗██║░░░██║██╔══██╗██╔════╝  ╚════██╗░░░╚════██║
██║░░╚═╝██║░░░░░███████║██║░░░██║██║░░██║█████╗░░  ░█████╔╝░░░░░░░██╔╝
██║░░██╗██║░░░░░██╔══██║██║░░░██║██║░░██║██╔══╝░░  ░╚═══██╗░░░░░░██╔╝░
╚█████╔╝███████╗██║░░██║╚██████╔╝██████╔╝███████╗  ██████╔╝██╗░░██╔╝░░
░╚════╝░╚══════╝╚═╝░░╚═╝░╚═════╝░╚═════╝░╚══════╝  ╚═════╝░╚═╝░░╚═╝░░░

░░██╗██████╗░███████╗░█████╗░░██████╗░█████╗░███╗░░██╗██╗███╗░░██╗░██████╗░██╗░░
░██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝██╔══██╗████╗░██║██║████╗░██║██╔════╝░╚██╗░
██╔╝░██████╔╝█████╗░░███████║╚█████╗░██║░░██║██╔██╗██║██║██╔██╗██║██║░░██╗░░╚██╗
╚██╗░██╔══██╗██╔══╝░░██╔══██║░╚═══██╗██║░░██║██║╚████║██║██║╚████║██║░░╚██╗░██╔╝
░╚██╗██║░░██║███████╗██║░░██║██████╔╝╚█████╔╝██║░╚███║██║██║░╚███║╚██████╔╝██╔╝░
░░╚═╝╚═╝░░╚═╝╚══════╝╚═╝░░╚═╝╚═════╝░░╚════╝░╚═╝░░╚══╝╚═╝╚═╝░░╚══╝░╚═════╝░╚═╝░░
Übersetzt mit Claude 3.7 (reasoning) - [https://claude.ai / https://t3.chat]
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BlackjackSimulation
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    
    public enum CardValue 
    { 
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, 
        Seven = 7, Eight = 8, Nine = 9, Ten = 10, 
        Jack = 10, Queen = 10, King = 10, Ace = 11 
    }
    
    public enum Action { Hit, Stand, Double, Split }
    
    public class Card
    {
        public Suit Suit { get; private set; }
        public CardValue Value { get; private set; }
        
        public Card(Suit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }
        
        public int GetValue()
        {
            return (int)Value;
        }
        
        public override string ToString()
        {
            return Value.ToString() + " of " + Suit.ToString();
        }
    }
    
    public class Deck
    {
        private List<Card> cards;
        private readonly int numDecks;
        private readonly Random random = new Random();
        
        public Deck(int numDecks)
        {
            this.numDecks = numDecks;
            Reset();
        }
        
        public void Reset()
        {
            cards = new List<Card>();
            
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    for (int i = 0; i < numDecks; i++)
                    {
                        cards.Add(new Card(suit, value));
                    }
                }
            }
            
            Shuffle();
        }
        
        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card temp = cards[k];
                cards[k] = cards[n];
                cards[n] = temp;
            }
        }
        
        public Card Deal()
        {
            if (cards.Count == 0)
            {
                Reset();
            }
            
            Card card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
        
        public int RemainingCards()
        {
            return cards.Count;
        }
    }
    
    public class Hand
    {
        public List<Card> Cards { get; private set; }
        public bool IsSplit { get; set; }
        public bool IsDoubled { get; set; }
        
        public Hand()
        {
            Cards = new List<Card>();
            IsSplit = false;
            IsDoubled = false;
        }
        
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }
        
        public int GetValue()
        {
            int value = 0;
            int aceCount = 0;
            
            foreach (Card card in Cards)
            {
                if (card.Value == CardValue.Ace)
                {
                    aceCount++;
                }
                value += card.GetValue();
            }
            
            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }
            
            return value;
        }
        
        public bool IsBlackjack()
        {
            return Cards.Count == 2 && GetValue() == 21;
        }
        
        public bool IsPair()
        {
            return Cards.Count == 2 && Cards[0].GetValue() == Cards[1].GetValue();
        }
        
        public bool IsSoft()
        {
            return Cards.Any(c => c.Value == CardValue.Ace) && 
                   GetValue() - 10 < 21;
        }
        
        public void Clear()
        {
            Cards.Clear();
            IsSplit = false;
            IsDoubled = false;
        }
    }
    
    public class BlackjackSimulator
    {
        private readonly Deck deck;
        private readonly List<Hand> playerHands = new List<Hand>();
        private readonly Hand dealerHand = new Hand();
        private readonly IBlackjackStrategy strategy;
        
        public int PlayerWins { get; private set; }
        public int DealerWins { get; private set; }
        public int Pushes { get; private set; }
        public int Blackjacks { get; private set; }
        public int TotalHands { get; private set; }
        
        private readonly Dictionary<int, Dictionary<int, Dictionary<Action, int>>> decisionStats;
        
        public BlackjackSimulator(IBlackjackStrategy strategy, int numDecks)
        {
            this.deck = new Deck(numDecks);
            this.strategy = strategy;
            playerHands.Add(new Hand());
            
            PlayerWins = 0;
            DealerWins = 0;
            Pushes = 0;
            Blackjacks = 0;
            TotalHands = 0;
            
            decisionStats = new Dictionary<int, Dictionary<int, Dictionary<Action, int>>>();
            for (int playerValue = 4; playerValue <= 21; playerValue++)
            {
                decisionStats[playerValue] = new Dictionary<int, Dictionary<Action, int>>();
                for (int dealerValue = 2; dealerValue <= 11; dealerValue++)
                {
                    decisionStats[playerValue][dealerValue] = new Dictionary<Action, int>
                    {
                        { Action.Hit, 0 },
                        { Action.Stand, 0 },
                        { Action.Double, 0 },
                        { Action.Split, 0 }
                    };
                }
            }
        }
        
        public void RunSimulation(int numRounds)
        {
            for (int i = 0; i < numRounds; i++)
            {
                PlayRound();
                
                if (deck.RemainingCards() < 52)
                {
                    deck.Reset();
                }
            }
        }
        
        private void PlayRound()
        {
            dealerHand.Clear();
            playerHands.Clear();
            playerHands.Add(new Hand());
            
            playerHands[0].AddCard(deck.Deal());
            dealerHand.AddCard(deck.Deal());
            playerHands[0].AddCard(deck.Deal());
            dealerHand.AddCard(deck.Deal());
            
            if (dealerHand.IsBlackjack())
            {
                DetermineWinner();
                return;
            }
            
            PlayHands();
            PlayDealerHand();
            DetermineWinner();
            
            TotalHands++;
        }
        
        private void PlayHands()
        {
            for (int i = 0; i < playerHands.Count; i++)
            {
                Hand currentHand = playerHands[i];
                
                while (currentHand.GetValue() < 21)
                {
                    int dealerUpCard = dealerHand.Cards[1].GetValue();
                    int playerValue = currentHand.GetValue();
                    bool isSoft = currentHand.IsSoft();
                    bool isPair = currentHand.IsPair();
                    
                    Action action = strategy.GetAction(playerValue, dealerUpCard, isSoft, isPair);
                    
                    if (action == Action.Hit)
                    {
                        currentHand.AddCard(deck.Deal());
                    }
                    else if (action == Action.Stand)
                    {
                        break;
                    }
                    else if (action == Action.Double && currentHand.Cards.Count == 2)
                    {
                        currentHand.IsDoubled = true;
                        currentHand.AddCard(deck.Deal());
                        break;
                    }
                    else if (action == Action.Split && isPair)
                    {
                        Hand newHand = new Hand();
                        newHand.IsSplit = true;
                        newHand.AddCard(currentHand.Cards[1]);
                        currentHand.Cards.RemoveAt(1);
                        currentHand.IsSplit = true;
                        
                        currentHand.AddCard(deck.Deal());
                        newHand.AddCard(deck.Deal());
                        
                        playerHands.Add(newHand);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        private void PlayDealerHand()
        {
            bool allBusted = true;
            foreach (Hand hand in playerHands)
            {
                if (hand.GetValue() <= 21)
                {
                    allBusted = false;
                    break;
                }
            }
            
            if (allBusted)
            {
                return;
            }
            
            while (dealerHand.GetValue() < 17)
            {
                dealerHand.AddCard(deck.Deal());
            }
        }
        
        private void DetermineWinner()
        {
            int dealerValue = dealerHand.GetValue();
            bool dealerBlackjack = dealerHand.IsBlackjack();
            
            foreach (Hand playerHand in playerHands)
            {
                int playerValue = playerHand.GetValue();
                bool playerBlackjack = playerHand.IsBlackjack();
                
                double payoutMultiplier = playerBlackjack ? 1.5 : 1.0;
                if (playerHand.IsDoubled)
                {
                    payoutMultiplier *= 2;
                }
                
                if (playerValue > 21)
                {
                    DealerWins++;
                }
                else if (dealerValue > 21)
                {
                    PlayerWins++;
                }
                else if (playerBlackjack && !dealerBlackjack)
                {
                    PlayerWins++;
                    Blackjacks++;
                }
                else if (dealerBlackjack && !playerBlackjack)
                {
                    DealerWins++;
                }
                else if (playerValue > dealerValue)
                {
                    PlayerWins++;
                }
                else if (dealerValue > playerValue)
                {
                    DealerWins++;
                }
                else
                {
                    Pushes++;
                }
            }
        }
        
        public void ExportStatistics(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Resultat");
                writer.WriteLine("============================");
                writer.WriteLine("Anzahl Hände: " + TotalHands);
                writer.WriteLine("Gewinne Spieler: " + PlayerWins + " (" + ((double)PlayerWins / TotalHands).ToString("P2") + ")");
                writer.WriteLine("Gewinne Dealer: " + DealerWins + " (" + ((double)DealerWins / TotalHands).ToString("P2") + ")");
                writer.WriteLine("Gleichstand: " + Pushes + " (" + ((double)Pushes / TotalHands).ToString("P2") + ")");
                writer.WriteLine("Blackjacks: " + Blackjacks + " (" + ((double)Blackjacks / TotalHands).ToString("P2") + ")");                
            }
        }
    }
    
    public interface IBlackjackStrategy
    {
        Action GetAction(int playerValue, int dealerUpCard, bool isSoft, bool isPair);
    }
    
    public class BasicStrategy : IBlackjackStrategy
    {
        public Action GetAction(int playerValue, int dealerUpCard, bool isSoft, bool isPair)
        {
            if (isPair)
            {
                if (playerValue == 16 || playerValue == 18)
                    return Action.Split;
                if (playerValue == 12 && dealerUpCard <= 6)
                    return Action.Split;
            }
            
            if (isSoft)
            {
                if (playerValue >= 19)
                    return Action.Stand;
                if (playerValue == 18 && dealerUpCard >= 9)
                    return Action.Hit;
                if (playerValue == 18)
                    return Action.Stand;
                if (playerValue <= 17 && dealerUpCard >= 7)
                    return Action.Hit;
                if (playerValue >= 15 && playerValue <= 17 && dealerUpCard <= 6)
                    return Action.Double;
                return Action.Hit;
            }
            
            if (playerValue >= 17)
                return Action.Stand;
            if (playerValue >= 13 && dealerUpCard <= 6)
                return Action.Stand;
            if (playerValue == 12 && dealerUpCard >= 4 && dealerUpCard <= 6)
                return Action.Stand;
            if (playerValue == 11 && dealerUpCard <= 10)
                return Action.Double;
            if (playerValue == 10 && dealerUpCard <= 9)
                return Action.Double;
            if (playerValue == 9 && dealerUpCard >= 3 && dealerUpCard <= 6)
                return Action.Double;
            
            return Action.Hit;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Blackjack Simulation Starting...");
            
            IBlackjackStrategy strategy = new BasicStrategy();
            BlackjackSimulator simulator = new BlackjackSimulator(strategy, 6);
            
            int numRounds = 1000000000;
            Console.WriteLine("Running " + numRounds.ToString("N0") + " simulations...");
            
            DateTime startTime = DateTime.Now;
            simulator.RunSimulation(numRounds);
            TimeSpan duration = DateTime.Now - startTime;
            
            Console.WriteLine("Simulation completed in " + duration.TotalSeconds.ToString("N2") + " seconds");
            Console.WriteLine("Player Win Rate: " + ((double)simulator.PlayerWins / simulator.TotalHands).ToString("P2"));
            Console.WriteLine("Dealer Win Rate: " + ((double)simulator.DealerWins / simulator.TotalHands).ToString("P2"));
            Console.WriteLine("Push Rate: " + ((double)simulator.Pushes / simulator.TotalHands).ToString("P2"));
            
            simulator.ExportStatistics("blackjack_stats.csv");
            Console.WriteLine("Statistics exported to blackjack_stats.csv");
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
