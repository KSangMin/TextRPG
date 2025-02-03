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
                Thread.Sleep(500);
                return true;
            }
            if (select < minN || select > maxN)
            {
                Console.WriteLine($"{minN}~{maxN}의 숫자를 입력해주세요.");
                Thread.Sleep(500);
                return true;
            }
            return false;
        }

        class Character
        {
            public List<Item> items = new List<Item>
            {
                new Item("천 옷", false, 0, 5, "활동성이 좋은 옷입니다.")
                , new Item("나무 몽둥이", true, 2, 0, "두꺼운 나무 몽둥이입니다.")
                , new Item("낡은 검", true, 2, 0, "쉽게 볼 수 있는 낡은 검입니다.")
            };
            public int level;
            public string name;
            public string classType;
            public int attack;
            public int defense;
            public int health;
            public int gold;

            public Character(string name)
            {
                level = 1;
                this.name = name;
                classType = "전사";
                attack = 10;
                defense = 5;
                health = 100;
                gold = 1500;
            }

            public void PrintState()
            {
                int itemAttack = 0, itemDefense = 0;
                foreach (Item item in items)
                {
                    if (item.isEquipped)
                    {
                        itemAttack += item.attack;
                        itemDefense += item.defense;
                    }
                }

                Console.Clear();
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {level:D2}");
                Console.WriteLine($"{name} ( {classType} )");
                Console.WriteLine($"공격력: {attack}{(itemAttack > 0 ? $" (+{itemAttack})" : "")}");
                Console.WriteLine($"방어력: {defense}{(itemDefense > 0 ? $" (+{itemDefense})" : "")}");
                Console.WriteLine($"체 력: {health}");
                Console.WriteLine($"Gold: {gold}G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int select = -1;
                if (CheckWrongInput(ref select, 0, 0)) PrintState();
                else return;
            }

            public void PrintInventory()
            {
                Console.Clear();
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("해당 숫자 아이템을 선택해 착용/해제할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                for(int i = 0; i < items.Count; i++)
                {
                    Item item = items[i];
                    Console.Write($"- {i + 1} ");
                    if (item.isEquipped) Console.Write("[E]");
                    Console.Write($"{item.name, -10} | ");
                    if (item.isWeapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    Console.WriteLine(item.desciption);
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int select = -1;
                if (CheckWrongInput(ref select, 0, items.Count)) PrintInventory();
                if (select == 0) return;
                else
                {
                    items[select - 1].isEquipped = !items[select - 1].isEquipped;
                    PrintInventory();
                }
            }

            public bool UseGold(int price)
            {
                if (price > gold)
                { 
                    Console.WriteLine("Gold가 부족합니다.");
                    Thread.Sleep(500);
                    return false;
                }
                else
                {
                    gold -= price;
                    Console.WriteLine("구매를 완료했습니다.");
                    Thread.Sleep(500);
                    return true;
                } 
            }
        }

        class Item
        {
            public bool isEquipped = false;
            public string name;
            public bool isWeapon;
            public int attack;
            public int defense;
            public string desciption;
            public int price = 0;

            public Item(string name, bool isWeapon, int attack, int defense, string description)
            {
                this.name = name;
                this.isWeapon = isWeapon;
                this.attack = attack;
                this.defense = defense;
                this.desciption = description;
            }

            public Item(string name, bool isWeapon, int attack, int defense, string description, int price)
            {
                this.name = name;
                this.isWeapon = isWeapon;
                this.attack = attack;
                this.defense = defense;
                this.desciption = description;
                this.price = price;
            }
        }

        class Game
        {
            private List<Item> _shop = new List<Item>
            {
                new Item("천 옷", false, 0, 5, "활동성이 좋은 옷입니다.", 500)
                , new Item("수련자 갑옷", false, 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000)
                , new Item("무쇠갑옷", false, 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500)
                , new Item("스파르타의 갑옷", false, 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500)
                , new Item("나무 몽둥이", true, 2, 0, "두꺼운 나무 몽둥이입니다.", 300)
                , new Item("낡은 검", true, 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 600)
                , new Item("청동 도끼", true, 5, 0, "어디선가 사용됐던 것 같은 도끼입니다.", 1500)
                , new Item("스파르타의 창", true, 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2100)
            };
            private Character _character;

            public Game(Character character)
            {
                _character = character;
            }

            public void Select()
            {
                Console.Clear();
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
                        _character.PrintInventory();
                        break;
                    case 3:
                        PrintShop();
                        break;
                }
            }

            public void PrintShop()
            {
                Console.Clear();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("해당 숫자 아이템을 선택해 구매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine(_character.gold + " G\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < _shop.Count; i++)
                {
                    Item item = _shop[i];
                    Console.Write($"- {i + 1} {item.name,-10} | ");
                    if (item.isWeapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    bool isSelled = false;
                    foreach (Item eq in _character.items)
                    {
                        if (eq.name == item.name)
                        {
                            isSelled = true;
                            break;
                        }
                    }
                    Console.Write($"{item.desciption, -15} | {(isSelled ? "구매완료" : $"{item.price} G")}\n");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int select = -1;
                if (CheckWrongInput(ref select, 0, _shop.Count)) PrintShop();
                if (select == 0) return;
                else
                {
                    bool isSelled = false;
                    foreach (Item eq in _character.items)
                    {
                        if (eq.name == _shop[select - 1].name)
                        {
                            isSelled = true;
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            Thread.Sleep(500);
                            break;
                        }
                    }
                    if(!isSelled && _character.UseGold(_shop[select- 1].price)) _character.items.Add(_shop[select - 1]);
                    PrintShop();
                }
            }
        }
    }
}
