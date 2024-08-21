using System;
using System.Collections.Generic;

namespace Cards
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Play();
        }
    }

    class Game
    {
        private Dealer _dealer = new Dealer();

        public void Play()
        {
            string commandNextMove = "1";
            string commandShowPlayerCards = "2";
            string commandChangeDeck = "3";
            string commandExit = "4";

            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                string userInput;
                Console.WriteLine($"{commandNextMove}) Следующий ход" +
                                  $"\n{commandShowPlayerCards}) Показать карты игрока" +
                                  $"\n{commandChangeDeck}) Сменить колоду" +
                                  $"\n{commandExit}) Выход");
                Console.Write("Введите команду: ");
                userInput = Console.ReadLine();

                if (userInput == commandNextMove)
                {
                    MakeNextMove();
                }
                else if (userInput == commandShowPlayerCards)
                {
                    _dealer.ShowPlayerCards();
                }
                else if (userInput == commandChangeDeck)
                {
                    ChangeDeck();
                }
                else if (userInput == commandExit)
                {
                    isRunning = false;
                }
                else
                {
                    Console.WriteLine("Неверная команда");
                }

                Console.ReadKey();
            }
        }

        private void MakeNextMove()
        {
            string userInput;
            Console.Write("Введите количество карт: ");
            userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int countOfCards))
            {
                if (_dealer.TryDealCards(countOfCards))
                {
                    Console.WriteLine("Карты успешно выданы");
                }
                else
                {
                    Console.WriteLine("Карты закончились");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод");
            }

            Console.ReadKey();
        }

        private void ChangeDeck()
        {
            string commandSmallDeck = "1";
            string commandFullDeck = "2";

            string userInput;

            Console.Clear();
            Console.WriteLine($"{commandSmallDeck}) Колода из 36 карт" +
                                  $"\n{commandFullDeck}) Колода из 54 карт");

            Console.Write("Введите команду: ");
            userInput = Console.ReadLine();

            if (userInput == commandSmallDeck)
            {
                _dealer.FillSmallDeck();
            }
            else if (userInput == commandFullDeck)
            {
                _dealer.FillFullDeck();
            }
            else
            {
                Console.WriteLine("Неверная команда");
            }
        }
    }

    class Dealer
    {
        private DeckFactory _deckFactory = new DeckFactory();
        private Player _player = new Player();
        private Deck _deck;

        public Dealer()
        {
            _deck = _deckFactory.CreateSmallDeck();
        }

        public bool TryDealCards(int countOfCards)
        {
            Card card;
            bool isSuccessDeal = true;

            for (int i = 0; i < countOfCards; i++)
            {
                card = _deck.GiveCard();

                if (card == null)
                {
                    isSuccessDeal = false;
                    break;
                }
                else
                {
                    _player.TakeCard(card);
                }
            }

            return isSuccessDeal;
        }

        public void ShowPlayerCards()
        {
            Console.Write("Карты Игрока: ");
            _player.ShowCards();
        }

        public void FillSmallDeck()
        {
            _deck = _deckFactory.CreateSmallDeck();
        }

        public void FillFullDeck()
        {    
            _deck = _deckFactory.CreateFullDeck();
        }
    }

    class Player
    {
        private List<Card> _cards = new List<Card>();

        public void TakeCard(Card card)
        {
            if (card != null)
                _cards.Add(card);
        }

        public void ShowCards()
        {
            foreach (var card in _cards)
            {
                Console.Write($"{card.Value} - {card.Suit}; ");
            }
        }
    }

    class Deck
    {
        private List<Card> _cards;
        private Random _random = new Random();

        public Deck(List<Card> cards)
        {
            _cards = cards;
        }

        public Card GiveCard()
        {
            Card card = GetCardByIndex(_random.Next(0, _cards.Count));
            _cards.Remove(card);
            return card;
        }

        private Card GetCardByIndex(int index)
        {
            if (_cards.Count == 0)
            {
                return null;
            }
            else
            {
                return _cards[index];
            }
        }
    }

    class DeckFactory
    {
        private string[] _smallDeckNames = new[] { "6", "7", "8", "9", "10", "Валет", "Дама", "Король", "Туз" };
        private string[] _fullDeckNames = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Валет", "Дама", "Король", "Туз" };
        private char[] _suits = new[] { '♣', '♥', '♦', '♠' };

        public Deck CreateSmallDeck()
        {
            return CreateDeck(_smallDeckNames);
        }

        public Deck CreateFullDeck()
        {
            return CreateDeck(_fullDeckNames);
        }

        private Deck CreateDeck(string[] cardNames)
        {
            List<Card> cards = new List<Card>();

            foreach (string name in cardNames)
            {
                foreach (char suit in _suits)
                {
                    cards.Add(new Card(name, suit));
                }
            }

            return new Deck(cards);
        }
    }

    class Card
    {
        public Card(string value, char suit)
        {
            Value = value;
            Suit = suit;
        }

        public string Value { get; private set; }
        public char Suit { get; private set; }
    }
}
