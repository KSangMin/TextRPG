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
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 휴식하기\n");

                game.Select();
            }
        }

        public static bool CheckWrongInput(ref int select, int minN, int maxN)//입력 예외처리
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
                new Item("천 옷", false, 0, 2, "활동성이 좋은 옷입니다.")
                , new Item("나무 몽둥이", true, 1, 0, "두꺼운 나무 몽둥이입니다.")
            };
            public int level;
            public string name;
            public string classType;
            public int attack;
            public int additionalAttack;//아이템
            public int defense;
            public int additionalDefense;//아이템
            public int health;
            public int gold;

            public Character(string name)
            {
                level = 1;
                this.name = name;
                classType = "전사";
                attack = 10;
                additionalAttack = 0;
                defense = 5;
                additionalDefense = 0;
                health = 100;
                gold = 1500;
            }
            
            public void PrintState()//상태 보기
            {
                Console.Clear();
                Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {level:D2}");
                Console.WriteLine($"{name} ( {classType} )");
                Console.WriteLine($"공격력: {attack}{(additionalAttack > 0 ? $" (+{additionalAttack})" : "")}");
                Console.WriteLine($"방어력: {defense}{(additionalDefense > 0 ? $" (+{additionalDefense})" : "")}");
                Console.WriteLine($"체 력: {health}");
                Console.WriteLine($"Gold: {gold}G");

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, 0)) PrintState();
                else return;
            }

            public void PrintInventory()//인벤토리
            {
                Console.Clear();
                Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                for(int i = 0; i < items.Count; i++)
                {
                    Item item = items[i];
                    Console.Write($"- ");
                    if (item.isEquipped) Console.Write("[E]");
                    Console.Write($"{item.name, -10} | ");
                    if (item.isWeapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    Console.WriteLine(item.desciption);
                }

                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, 1)) PrintInventory();
                if (select == 0) return;
                else AdjustInventory();
            }

            public void AdjustInventory()//인벤토리 - 장착 관리
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("해당 숫자 아이템을 선택해 착용/해제할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < items.Count; i++)
                {
                    Item item = items[i];
                    Console.Write($"- {i + 1} ");
                    if (item.isEquipped) Console.Write("[E]");
                    Console.Write($"{item.name,-10} | ");
                    if (item.isWeapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    Console.WriteLine(item.desciption);
                }

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, items.Count)) AdjustInventory();
                if (select == 0) PrintInventory();
                else
                {
                    Item item = items[select - 1];
                    item.isEquipped = !item.isEquipped;
                    if (item.isEquipped)
                    {
                        additionalAttack += item.attack;
                        additionalDefense += item.defense;
                    }
                    else
                    {
                        additionalAttack -= item.attack;
                        additionalDefense -= item.defense;
                    }
                    AdjustInventory();
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
                    Console.WriteLine($"{price} Gold를 지불했습니다.");
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
                new Item("천 옷", false, 0, 2, "활동성이 좋은 옷입니다.", 500)
                , new Item("수련자 갑옷", false, 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000)
                , new Item("무쇠갑옷", false, 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500)
                , new Item("스파르타의 갑옷", false, 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500)
                , new Item("나무 몽둥이", true, 1, 0, "두꺼운 나무 몽둥이입니다.", 300)
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
                int select = 0;
                if (CheckWrongInput(ref select, 1, 4)) return;

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
                    case 4:
                        Rest();
                        break;
                }
            }

            public void PrintShop()//상점
            {
                Console.Clear();
                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine(_character.gold + " G\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < _shop.Count; i++)
                {
                    Item item = _shop[i];
                    Console.Write($"- {item.name,-10} | ");
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

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, 1)) PrintShop();
                switch (select)
                {
                    case 0:
                        return;
                    case 1:
                        SelectItems();
                        break;
                }
            }

            public void SelectItems()//구매할 아이템 선택
            {
                Console.Clear();
                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.");
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
                    Console.Write($"{item.desciption,-15} | {(isSelled ? "구매완료" : $"{item.price} G")}\n");
                }

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, _shop.Count)) SelectItems();
                if (select == 0) PrintShop();
                else BuyItem(select);
            }

            public void BuyItem(int select)//아이템 구매
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
                if (!isSelled && _character.UseGold(_shop[select - 1].price)) _character.items.Add(_shop[select - 1]);
                SelectItems();
            }

            public void Rest()//휴식하기
            {
                Console.Clear();
                Console.Write("휴식하기\n500 G 를 내면 체력을 회복할 수 있습니다.");
                Console.WriteLine($" (보유 골드 : {_character.gold} G)\n");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0, 나가기\n");

                int select = 0;
                if (CheckWrongInput(ref select, 0, 1)) Rest();
                switch (select)
                {
                    case 0:
                        Rest();
                        break;
                    case 1:
                        if(_character.UseGold(500))
                        {
                            _character.health = Math.Max(_character.health + 50, 100);
                            Console.WriteLine("체력이 회복되었습니다.");
                            Thread.Sleep(500);
                        }
                        break;
                }
            }
        }
    }
}
