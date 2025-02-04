using System.Xml.Serialization;

namespace TextRPG
{
    public class Game
    {
        public List<Dungeon> dungeons;
        public Character character;
        public Shop shop;

        public Game(Character character)
        {
            this.character = character;
            shop = new Shop(character);
            dungeons = new List<Dungeon>
            {
                new Dungeon(character, "쉬운 던전", 5, 1000)
                , new Dungeon(character, "일반 던전", 11, 1700)
                , new Dungeon(character, "어려운 던전", 17, 2500)
            };
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

        public void Select()
        {
            int select = 0;
            if (CheckWrongInput(ref select, 1, 5)) return;

            switch (select)
            {
                case 1:
                    character.PrintState();
                    return;
                case 2:
                    character.PrintInventory();
                    break;
                case 3:
                    shop.PrintShop();
                    break;
                case 4:
                    SelectDungeon();
                    break;
                case 5:
                    Rest();
                    break;
            }
        }

        public void SelectDungeon()//던전 선택
        {
            Console.Clear();
            Console.WriteLine("던전입장\n이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            for (int i = 0; i < dungeons.Count; i++)
            {
                Dungeon d = dungeons[i];
                Console.WriteLine($"{i + 1}. {d.name, -10} | 방어력 {d.recommend} 이상 권장");
            }
            Console.WriteLine("0. 나가기\n");

            int select = 0;
            if (CheckWrongInput(ref select, 0, dungeons.Count)) SelectDungeon();
            if (select == 0) return;
            else 
            { 
                dungeons[select - 1].ChallangeDungeon();

                Console.WriteLine("\n0. 나가기");

                select = 0;
                if (CheckWrongInput(ref select, 0, dungeons.Count)) SelectDungeon();
                if (select == 0) return;
            }
        }

        public void Rest()//휴식하기
        {
            Console.Clear();
            Console.Write("휴식하기\n500 G 를 내면 체력을 회복할 수 있습니다.");
            Console.WriteLine($" (보유 골드 : {character.gold} G)\n");
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
                    if (character.UseGold(500))
                    {
                        character.health = Math.Max(character.health + 50, 100);
                        Console.WriteLine("체력이 회복되었습니다.");
                        Thread.Sleep(500);
                    }
                    break;
            }
        }

        public void SaveCharacter()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Character));
            using (StreamWriter writer = new StreamWriter("character.xml"))
            {
                serializer.Serialize(writer, character);
            }
        }

        public void LoadCharacter()
        {
            if (File.Exists("character.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Character));
                using (StreamReader reader = new StreamReader("character.xml"))
                {
                    character = (Character)serializer.Deserialize(reader);
                    Console.WriteLine("저장된 캐릭터를 불러왔습니다.");
                }
            }
            else
            {
                Console.WriteLine("저장된 캐릭터가 없습니다. 캐릭터를 새로 생성했습니다.");
            }
        }
    }
}
