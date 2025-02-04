namespace TextRPG
{
    public class Character
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

        public Character()
        {

        }

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
            Console.WriteLine($"공격력: {(int)attack}{(weapon is not null ? $" (+{weapon.attack})" : "")}");
            Console.WriteLine($"방어력: {defense}{(armor is not null ? $" (+{armor.defense})" : "")}");
            Console.WriteLine($"체 력: {health}");
            Console.WriteLine($"Gold: {gold}G");

            Console.WriteLine("\n0. 나가기");

            Game.CheckWrongInput(out int select, 0, 0);
            if (select == 0) return;
        }

        public void PrintItemList(bool adjustFlag)
        {
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                Console.Write("- ");
                if (adjustFlag) Console.Write($"{i + 1} ");
                if (CheckEquip(item)) Console.Write("[E]");
                Console.Write($"{item.name,-10} | ");
                if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
                else Console.Write($"방어력 +{item.defense} | ");
                Console.WriteLine(item.desciption);
            }
        }

        public void PrintInventory()//인벤토리
        {
            Console.Clear();
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");

            PrintItemList(false);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");

            Game.CheckWrongInput(out int select, 0, 1);
            if (select == 0) return;
            else AdjustInventory();
        }

        public void AdjustInventory()//인벤토리 - 장착 관리
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("해당 숫자 아이템을 선택해 착용/해제할 수 있습니다.\n");

            PrintItemList(true);

            Console.WriteLine("\n0. 나가기");

            Game.CheckWrongInput(out int select, 0, items.Count);
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
            if (level <= clear)
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
}
