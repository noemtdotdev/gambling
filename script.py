import random
import matplotlib.pyplot as plt

class Card:
    def __init__(self, suit, value):
        self.suit = suit
        self.value = value
    
    def __str__(self):
        return f"{self.value} of {self.suit}"
    
    def get_value(self):
        if self.value in ["J", "Q", "K"]:
            return 10
        elif self.value == "A":
            return 11
        else:
            return int(self.value)

class Deck:
    def __init__(self, num_decks=6):
        self.cards = []
        self.num_decks = num_decks
        self.reset()
    
    def reset(self):
        suits = ["Hearts", "Diamonds", "Clubs", "Spades"]
        values = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"]
        self.cards = [Card(suit, value) for _ in range(self.num_decks) 
                      for suit in suits for value in values]
        self.shuffle()
    
    def shuffle(self):
        random.shuffle(self.cards)
    
    def deal(self):
        return self.cards.pop()

class Hand:
    def __init__(self):
        self.cards = []
    
    def add_card(self, card):
        self.cards.append(card)
    
    def get_value(self):
        value = sum(card.get_value() for card in self.cards)
        num_aces = sum(1 for card in self.cards if card.value == "A")
        while value > 21 and num_aces > 0:
            value -= 10
            num_aces -= 1
        return value
    
    def is_blackjack(self):
        return len(self.cards) == 2 and self.get_value() == 21

class BlackjackGame:
    def __init__(self, num_decks=6):
        self.deck = Deck(num_decks)
        self.player_hands = [Hand()]
        self.dealer_hand = Hand()
        self.game_stats = {
            'player_wins': 0,
            'dealer_wins': 0,
            'pushes': 0,
            'blackjacks': 0
        }
        self.decisions_data = []
    
    def deal_initial_cards(self):
        self.player_hands = [Hand()]
        self.dealer_hand = Hand()
        
        self.player_hands[0].add_card(self.deck.deal())
        self.dealer_hand.add_card(self.deck.deal())
        self.player_hands[0].add_card(self.deck.deal())
        self.dealer_hand.add_card(self.deck.deal())
    
    def player_turn(self, strategy_func=None):
        for hand_idx, hand in enumerate(self.player_hands):
            while hand.get_value() < 21:
                dealer_up_card = self.dealer_hand.cards[1].get_value()
                player_value = hand.get_value()
                
                if strategy_func:
                    action = strategy_func(player_value, dealer_up_card, hand.cards)
                else:
                    if player_value < 17:
                        action = "hit"
                    else:
                        action = "stand"
                
                self.decisions_data.append({
                    'player_value': player_value,
                    'dealer_up_card': dealer_up_card,
                    'action': action
                })
                
                if action == "hit":
                    hand.add_card(self.deck.deal())
                elif action == "stand":
                    break
                elif action == "double" and len(hand.cards) == 2:
                    hand.add_card(self.deck.deal())
                    break
                elif action == "split" and len(hand.cards) == 2 and hand.cards[0].get_value() == hand.cards[1].get_value():
                    # Implementation des splits soll hier hin
                    # Der einfachheit halber wird passiert hier nichts
                    pass
    
    def dealer_turn(self):
        while self.dealer_hand.get_value() < 17:
            self.dealer_hand.add_card(self.deck.deal())
    
    def determine_winner(self):
        dealer_value = self.dealer_hand.get_value()
        dealer_blackjack = self.dealer_hand.is_blackjack()
        
        for hand in self.player_hands:
            player_value = hand.get_value()
            player_blackjack = hand.is_blackjack()
            
            if player_value > 21:
                self.game_stats['dealer_wins'] += 1
            elif dealer_value > 21:
                self.game_stats['player_wins'] += 1
            elif player_blackjack and not dealer_blackjack:
                self.game_stats['player_wins'] += 1
                self.game_stats['blackjacks'] += 1
            elif dealer_blackjack and not player_blackjack:
                self.game_stats['dealer_wins'] += 1
            elif player_value > dealer_value:
                self.game_stats['player_wins'] += 1
            elif dealer_value > player_value:
                self.game_stats['dealer_wins'] += 1
            else:
                self.game_stats['pushes'] += 1
    
    def play_round(self, strategy_func=None):
        self.deal_initial_cards()
        
        if self.dealer_hand.is_blackjack():
            self.determine_winner()
            return
        
        self.player_turn(strategy_func)
        self.dealer_turn()
        self.determine_winner()
    
    def run_simulation(self, num_rounds=1000, strategy_func=None):
        for _ in range(num_rounds):
            self.play_round(strategy_func)
            if len(self.deck.cards) < 52:
                self.deck.reset()
        
        self.visualize_results()
    
    def visualize_results(self):
        labels = ['Spieler gewinnt', 'Dealer gewinnt', 'Gleichstand', 'Blackjacks']
        values = [
            self.game_stats['player_wins'], 
            self.game_stats['dealer_wins'],
            self.game_stats['pushes'],
            self.game_stats['blackjacks']
        ]
        
        plt.figure(figsize=(10, 6))
        plt.bar(labels, values, color=['green', 'red', 'blue', 'gold'])
        plt.title('Resultate')
        plt.ylabel('Gespielte Spiele')
        plt.grid(axis='y', linestyle='--', alpha=0.7)
        
        total_games = sum(values[:-1])
        win_rate = self.game_stats['player_wins'] / total_games * 100
        plt.annotate(f'Gewinnrate: {win_rate:.2f}%', 
                     xy=(0.5, 0.9), 
                     xycoords='axes fraction', 
                     ha='center',
                     bbox=dict(boxstyle="round,pad=0.3", fc="w", ec="k"))
        
        plt.tight_layout()
        plt.show()

def basic_strategy(player_value, dealer_upcard, player_cards):
    if player_value >= 17:
        return "stand"
    elif player_value <= 8:
        return "hit"
    elif player_value >= 13 and dealer_upcard < 7:
        return "stand"
    else:
        return "hit"

game = BlackjackGame()
game.run_simulation(10000, basic_strategy)
