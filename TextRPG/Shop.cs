namespace TextRPG
{
    public class Shop
    {
        public Character character;
        public List<Item> items = new List<Item>
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

        public Shop()
        {

        }

        public Shop(Character c)
        {
            character = c;
        }

        public void PrintItemList(bool adjustFlag) // 아이템 목록 출력
        {
            Console.WriteLine("[보유 골드]");
            Console.WriteLine(character.gold + " G\n");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                Console.Write("- ");
                if (adjustFlag) Console.Write($"{i + 1} ");
                Console.Write($"{item.name,-10} | ");
                Console.Write(item is Weapon ? $"공격력 +{item.attack} | " : $"방어력 +{item.defense} | ");
                bool isSelled = character.items.Exists(eq => eq.name == item.name);
                Console.Write($"{item.desciption,-15} | {(isSelled ? "구매완료" : $"{item.price} G")}\n");
            }
        }

        public void PrintShop()//상점
        {
            Console.Clear();
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");

            PrintItemList(false);

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");

            Game.CheckWrongInput(out int select, 0, 2);
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

            PrintItemList(true);

            Console.WriteLine("\n0. 나가기");

            Game.CheckWrongInput(out int select, 0, items.Count);
            if (select == 0) PrintShop();
            else BuyItem(select);
        }

        public void BuyItem(int select)//아이템 구매
        {
            bool isSelled = false;
            foreach (Item eq in character.items)
            {
                if (eq.name == items[select - 1].name)
                {
                    isSelled = true;
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Thread.Sleep(500);
                    break;
                }
            }
            if (!isSelled && character.UseGold(items[select - 1].price))
            {
                character.items.Add(items[select - 1]);
                character.items.Last().price = (int)(character.items.Last().price * 0.85f);
            }

            SelectBuyItems();
        }

        public void SelectSellItems()//판매할 아이템 선택
        {
            Console.Clear();
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("해당 숫자 아이템을 선택해 판매할 수 있습니다.\n");

            for (int i = 0; i < character.items.Count; i++)
            {
                Item item = character.items[i];
                Console.Write($"- {i + 1} ");
                if (character.CheckEquip(item)) Console.Write("[E]");
                Console.Write($"{item.name,-10} | ");
                if (item is Weapon) Console.Write($"공격력 +{item.attack} | ");
                else Console.Write($"방어력 +{item.defense} | ");
                Console.Write(item.desciption + " | ");
                Console.WriteLine(item.price + " G");
            }

            Console.WriteLine("\n0. 나가기");

            Game.CheckWrongInput(out int select, 0, character.items.Count);
            if (select == 0) PrintShop();
            else SellItem(select);
        }

        public void SellItem(int select)//아이템 판매
        {

            Item item = character.items[select - 1];
            if (character.CheckEquip(item))
            {
                if (item is Weapon) character.weapon = null;
                if (item is Armor) character.armor = null;
            }
            //판매
            character.gold += item.price;
            character.items.Remove(item);

            SelectSellItems();
        }
    }
}
