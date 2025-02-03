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
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기\n");

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
                new Armor("천 옷", 0, 2, "활동성이 좋은 옷입니다.", 425)
                , new Weapon("나무 몽둥이", 1, 0, "두꺼운 나무 몽둥이입니다.", 255)
            };
            public Weapon weapon;
            public Armor armor;
            public int level;
            public int clear;
            public string name;
            public string classType;
            public float attack;
            public int defense;
            public int health;
            public int gold;

            public Character(string name)
            {
                level = 1;
                clear = 0;
                this.name = name;
                classType = "전사";
                attack = 10;
                defense = 5;
                health = 100;
                gold = 1500;
            }

            public void PrintState()//상태 보기
            {
                Console.Clear();
                Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다.\n");
                Console.WriteLine($"Lv. {level:D2}");
                Console.WriteLine($"{name} ( {classType} )");
                Console.WriteLine($"공격력: {attack}{(weapon is not null ? $" (+{weapon.attack})" : "")}");
                Console.WriteLine($"방어력: {defense}{(armor is not null ? $" (+{armor.defense})" : "")}");
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
                for (int i = 0; i < items.Count; i++)
                {
                    Item item = items[i];
                    Console.Write($"- ");
                    if (CheckEquip(item)) Console.Write("[E]");
                    Console.Write($"{item.name,-10} | ");
                    if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
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
                    if (CheckEquip(item)) Console.Write("[E]");
                    Console.Write($"{item.name,-10} | ");
                    if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    Console.WriteLine(item.desciption);
                }

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, items.Count)) AdjustInventory();
                if (select == 0) PrintInventory();
                else EquipItem(select);
            }

            public void EquipItem(int select)//무기, 방어구 장착 체크
            {
                Item item = items[select - 1];
                if (item is Weapon) weapon = (Weapon)item;
                else if (item is Armor) armor = (Armor)item;

                AdjustInventory();
            }

            public bool CheckEquip(Item item)
            {
                if (item == weapon || item == armor) return true;
                return false;
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

            public int GetAttack()
            {
                return weapon is null ? (int)attack : (int)(attack + weapon.attack);
            }

            public int GetArmor()
            {
                return armor is null ? defense : defense + armor.defense;
            }

            public bool IsLevelUp()
            {
                if(level <= clear)
                {
                    clear = 0;
                    level++;
                    attack += 0.5f;
                    defense += 1;
                    return true;
                }
                return false;
            }
        }

        class Item
        {
            public string name;
            public int attack;
            public int defense;
            public string desciption;
            public int price = 0;

            public Item(string name, int attack, int defense, string description, int price)
            {
                this.name = name;
                this.attack = attack;
                this.defense = defense;
                this.desciption = description;
                this.price = price;
            }
        }

        class Weapon : Item
        {
            public Weapon(string name, int attack, int defense, string description, int price)
                : base(name, attack, defense, description, price)
            {

            }
        }

        class Armor : Item
        {
            public Armor(string name, int attack, int defense, string description, int price)
                : base(name, attack, defense, description, price)
            {

            }
        }

        class Dungeon
        {
            public string name;
            public int recommend;
            public int minDamage = 20;
            public int maxDamage = 35 + 1;
            public int reward;

            public Dungeon(string name, int recommend, int reward)
            {
                this.name = name;
                this.recommend = recommend;
                this.reward = reward;
            }
        }

        class Game
        {
            private List<Item> _shop = new List<Item>
            {
                new Armor("천 옷", 0, 2, "활동성이 좋은 옷입니다.", 500)
                , new Armor("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000)
                , new Armor("무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500)
                , new Armor("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500)
                , new Weapon("나무 몽둥이", 1, 0, "두꺼운 나무 몽둥이입니다.", 300)
                , new Weapon("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 600)
                , new Weapon("청동 도끼", 5, 0, "어디선가 사용됐던 것 같은 도끼입니다.", 1500)
                , new Weapon("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2100)
            };
            private List<Dungeon> _dungeons = new List<Dungeon>
            {
                new Dungeon("쉬운 던전", 5, 1000)
                , new Dungeon("일반 던전", 11, 1700)
                , new Dungeon("어려운 던전", 17, 2500)
            };
            private Character _character;

            public Game(Character character)
            {
                _character = character;
            }

            public void Select()
            {
                int select = 0;
                if (CheckWrongInput(ref select, 1, 5)) return;

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
                        SelectDungeon();
                        break;
                    case 5:
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
                    if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
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

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, 2)) PrintShop();
                switch (select)
                {
                    case 0:
                        return;
                    case 1:
                        SelectBuyItems();
                        break;
                    case 2:
                        SelectSellItems();
                        break;
                }
            }

            public void SelectBuyItems()//구매할 아이템 선택
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
                    if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
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
                if (CheckWrongInput(ref select, 0, _shop.Count)) SelectBuyItems();
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
                if (!isSelled && _character.UseGold(_shop[select - 1].price))
                {
                    _character.items.Add(_shop[select - 1]);
                    _character.items.Last().price = (int)(_character.items.Last().price * 0.85f);
                }

                SelectBuyItems();
            }

            public void SelectSellItems()//판매할 아이템 선택
            {
                Console.Clear();
                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("해당 숫자 아이템을 선택해 판매할 수 있습니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine(_character.gold + " G\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < _character.items.Count; i++)
                {
                    Item item = _character.items[i];
                    Console.Write($"- {i + 1} ");
                    //if (item.isEquipped) Console.Write("[E]");
                    Console.Write($"{item.name,-10} | ");
                    if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
                    else Console.Write($"방어력 +{item.defense} | ");
                    Console.Write(item.desciption + " | ");
                    Console.WriteLine(item.price + " G");
                }

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, _character.items.Count)) SelectSellItems();
                if (select == 0) PrintShop();
                else SellItem(select);
            }

            public void SellItem(int select)//아이템 판매
            {

                Item item = _character.items[select - 1];
                if (_character.CheckEquip(item))
                {
                    if (item is Weapon) _character.weapon = null;
                    if (item is Armor) _character.armor = null;
                }
                //판매
                _character.gold += item.price;
                _character.items.Remove(item);

                SelectSellItems();
            }

            public void SelectDungeon()//던전 선택
            {
                Console.Clear();
                Console.WriteLine("던전입장\n이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                for (int i = 0; i < _dungeons.Count; i++)
                {
                    Dungeon d = _dungeons[i];
                    Console.WriteLine($"{i + 1}. {d.name, -10} | 방어력 {d.recommend} 이상 권장");
                }
                Console.WriteLine("0. 나가기\n");

                int select = 0;
                if (CheckWrongInput(ref select, 0, _dungeons.Count)) SelectDungeon();
                if (select == 0) return;
                else ChallangeDungeon(_dungeons[select - 1]);
            }

            public void ChallangeDungeon(Dungeon d)//던전 도전
            {
                if(d.recommend > _character.GetArmor() && new Random().Next(0, 10) < 4) DungeonFail(d);
                else DungeonSuccess(d);
            }

            public void DungeonSuccess(Dungeon d)
            {
                Console.Clear();
                Console.WriteLine($"던전 클리어\n축하드립니다!\n{d.name}을 클리어했습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.Write($"체력 {_character.health} -> ");
                int adjust = d.recommend - _character.GetArmor();
                _character.health -= new Random().Next(d.minDamage + adjust, d.maxDamage + adjust);
                Console.WriteLine(_character.health);
                Console.Write($"Gold {_character.gold} -> ");
                int additionalRatio = new Random().Next(_character.GetAttack(), _character.GetAttack() * 2 + 1);
                float ratio = 1f + additionalRatio / 100f;
                _character.gold += (int)(d.reward * ratio);
                Console.WriteLine(_character.gold);
                _character.clear++;
                if(_character.IsLevelUp())
                {
                    Console.WriteLine($"레벨 {_character.level - 1} -> {_character.level}");
                    Console.WriteLine($"공격력 {_character.attack - 0.5f} -> {_character.attack}");
                    Console.WriteLine($"방어력 {_character.defense - 1} -> {_character.defense}");
                }

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, _dungeons.Count)) SelectDungeon();
                if (select == 0) return;
            }

            public void DungeonFail(Dungeon d)
            {
                Console.Clear();
                Console.WriteLine($"던전 도전 실패\n안타깝네요..\n{d.name} 클리어에 실패했습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {_character.health} -> {_character.health / 2}");
                _character.health /= 2;

                Console.WriteLine("\n0. 나가기");

                int select = 0;
                if (CheckWrongInput(ref select, 0, _dungeons.Count)) SelectDungeon();
                if (select == 0) return;
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
                        if (_character.UseGold(500))
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
