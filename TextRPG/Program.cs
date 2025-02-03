namespace TextRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.Write("당신의 이름을 알려 주세요: ");
            
            string name = Console.ReadLine();
            Character user = new Character(name);
            Game game = new Game(user);

            Console.WriteLine("환영합니다!");
            while (true)
            {
                Console.Clear();
                game.Select();
            }
        }

        public static bool CheckWrongInput(ref int select, int minN, int maxN)
        {
            Console.Write("\n원하시는 행동을 입력해주세요.(숫자로 입력): ");
            bool rightInput = int.TryParse(Console.ReadLine(), out select);

            if (!rightInput)
            {
                Console.WriteLine("입력이 잘못되었습니다. 다시 입력해주세요.");
                return true;
            }
            if (select < minN || select > maxN)
            {
                Console.WriteLine($"{minN}~{maxN}의 숫자를 입력해주세요.");
                return true;
            }
            return false;
        }

        class Character
        {
            private int _level;
            private string _name;
            private string _classType;
            private int _attack;
            private int _defense;
            private int _health;
            private int _gold;

            public Character(string name)
            {
                _level = 1;
                _name = name;
                _classType = "전사";
                _attack = 10;
                _defense = 5;
                _health = 100;
                _gold = 1500;
            }

            public void PrintState()
            {
                Console.Clear();
                Console.WriteLine($"Lv. {_level:D2}");
                Console.WriteLine($"{_name} ({_classType})");
                Console.WriteLine($"공격력: {_attack}");
                Console.WriteLine($"방어력: {_defense}");
                Console.WriteLine($"체 력: {_health}");
                Console.WriteLine($"Gold: {_gold}G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int select = -1;
                if (CheckWrongInput(ref select, 0, 0)) PrintState();
                else return;
            }

        }

        class Game
        {
            private Character _character;

            public Game(Character character)
            {
                _character = character;
            }

            public void Select()
            {
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점\n");

                int select = -1;
                if (CheckWrongInput(ref select, 1, 3)) return;

                switch (select)
                {
                    case 1:
                        _character.PrintState();
                        return;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
        }
    }
}
