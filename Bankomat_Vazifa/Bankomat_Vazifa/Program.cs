using System;
using System.Text;

namespace Lesson_07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Card card = new Card(1, 2000,
                "4000", "1234",
                "Miraziz", DateTime.Now.AddYears(4));
            Bankomat bankomat = new Bankomat(1, "address");
            bankomat.Start(card);
        }
    }

    class Bankomat
    {
        private int id;
        private string address;
        private Card card;
        private Language language;

        public Bankomat(int id, string address)
        {
            this.id = id;
            this.address = address;
        }

        public void Start(Card card)
        {
            this.card = card;
            if (card is null)
            {
                Console.WriteLine("Error!!! Card is null.");
                return;
            }

            Console.WriteLine("---- Welcome to Bankomat ATM -----");

            SelectLanguage();
        }

        private void SelectLanguage()
        {
            Console.WriteLine("1 - Eng");
            Console.WriteLine("2 - Rus");
            Console.WriteLine("3 - Uzb");

            SetLanguage(int.Parse(Console.ReadLine() ?? ""));

            if (!EnterPin())
                return;

            DisplayStartOptions();
        }

        private void DisplayStartOptions()
        {
            Console.WriteLine("1 - Display Balance");
            Console.WriteLine("2 - Widthdraw cash");
            Console.WriteLine("3 - Change pin");

            ChooseStartOption();
        }

        private void ChooseStartOption()
        {
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    DisplayBalance();
                    break;
                case 2:
                    Widthdraw();
                    break;
                case 3:
                    ChangePin();
                    break;
                default:
                    break;
            }
        }

        private void DisplayBalance()
        {
            Console.WriteLine($"Your current balance is: {card.Balance}");
            IsContinue();
        }

        private void Widthdraw()
        {
            DisplayWidthdrawOptions();

            IsContinue();
        }

        private void DisplayWidthdrawOptions()
        {
            Console.WriteLine("1 - 50.000                 2 - 100.000");
            Console.WriteLine("3 - 200.000                4 - 300.000");
            Console.WriteLine("5 - 400.000                6 - 500.000");
            Console.WriteLine("7 - Another amount          ");

            SelectWidthdrawOption();
        }

        private void SelectWidthdrawOption()
        {
            int option = GetInputOption();

            if (option == 1)
            {
                card.Widthdraw(50.000m);
            }
            else if (option == 2)
            {
                card.Widthdraw(100.000m);
            }
            else if (option == 3)
            {
                card.Widthdraw(200.000m);
            }
            else if (option == 4)
            {
                card.Widthdraw(300.000m);
            }
            else if (option == 5)
            {
                card.Widthdraw(400.000m);
            }
            else if (option == 6)
            {
                card.Widthdraw(500.000m);
            }
            else if (option == 7)
            {
                decimal amount = decimal.Parse(Console.ReadLine());
                card.Widthdraw(amount);
            }
            else
            {
                Console.WriteLine("Error!");
            }
        }

        private void ChangePin()
        {
            if (!EnterPin())
                return;

            Console.WriteLine("Enter new pin. It must be 4 digits");
            string newPin = Console.ReadLine() ?? string.Empty;
            card.ChangePin(newPin);

            IsContinue();
        }

        private bool EnterPin()
        {
            Console.WriteLine("Please, enter your pin");
            string pin = Console.ReadLine();
            int times = 3;
            while (times > 0)
            {
                if (CheckPin(pin))
                {
                    return true;  
                }
                Console.WriteLine("Wrong password, try again");
                    times--;
            }

            if (times == 0)
            {
                Console.WriteLine("Your card is blocked. Contact your bank.");
                return false;
            }
            return true;
        }

        private bool CheckPin(string input)
        {
            bool checkpin = int.TryParse(input, out int inputpin);
            if (checkpin == true)
            return this.card.CheckPin(input);
            else return false;
        }

        private void IsContinue()
        {
            Console.WriteLine("Do you want to do another operation or exit?");
            Console.WriteLine("1 - Yes");
            Console.WriteLine("2 - No");

            SelectContinueOption();
        }

        private void SelectContinueOption()
        {
            int option = int.Parse(Console.ReadLine());

            if (option == 1)
            {
                DisplayStartOptions();
            }
            else
            {
                Console.WriteLine("-- Thank you --");
                Console.Clear();
                Console.WriteLine("Enter your card to start.");
                card = null;
            }
        }

        private void SetLanguage(int language)
        {
            if (language == 1)
            {
                this.language = Language.Eng;
            }
            else if (language == 2)
            {
                this.language = Language.Rus;
            }
            else
            {
                this.language = Language.Uzb;
            }
        }

        private int GetInputOption() => int.Parse(Console.ReadLine());
    }

    enum Language
    {
        Uzb,
        Eng,
        Rus
    }

    class Card
    {
        private readonly int id;
        private decimal balance;
        private int[] pin1 = new int[4];

        public string CardNumber { get; private set; }
        public string CardOwnder { get; private set; }

        public DateTime ExpireDate { get; private set; }

        public Card(int id, decimal balance, string pin,
            string cardNumber, string cardOwnder, DateTime expireDate)
        {
            this.id = id;
            this.balance = balance;

            ChangePin(pin);

            CardNumber = cardNumber;
            CardOwnder = cardOwnder;
            ExpireDate = expireDate;
        }
        public decimal Balance
        {
            get => balance;
        }

        public string Pin
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (var number in pin1)
                {
                    result.Append(number.ToString());
                }

                return result.ToString();
            }
        }

        
        public void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                return;
            }

            balance += amount;
        }

        public void Widthdraw(decimal amount)
        {
            if (amount > balance)
            {
                return;
            }

            balance -= amount;
        }

        public void ChangePin(string newPin)
        {
            if (newPin.Length != 4)
            {
                Console.WriteLine("Pin 4 ta raqam bo'lishi shart!!!");
                return;
            }

            int[] newPinArray = new int[4];
            int index = 0;
            foreach (char ch in newPin)
            {
                if (!char.IsDigit(ch))
                {
                    Console.WriteLine("Pin faqat raqam bo'lishi shart!!!");
                    return;
                }
                else
                {
                    newPinArray[index++] = ch;
                }
            }

            pin1 = newPinArray;
        }

        public bool CheckPin(string pin)
        {
            if (pin.Length != 4)
            {
                Console.WriteLine("Pin 4 ta raqam bo'lishi shart!!!");
                return false;
            }
            string pinginput = "";
            for (int i = 0; i < pin.Length; i++)
            {   
                pinginput += pin[i];
            }

            for (int i = 0; i < this.pin1.Length; i++)
            {
                if (pinginput[i] != pin1[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}