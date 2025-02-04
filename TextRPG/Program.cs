namespace TextRPG
{
    public class Program
    {
        static void Main(string[] args)
        {
            string name;
            Character user = new Character();
            Game game = new Game(user);

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("1. 새로하기");
            Console.WriteLine("2. 이어하기");
            Console.WriteLine("0. 종료하기");

            Game.CheckWrongInput(out int select, 0, 2);
            switch (select)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    game.DeleteCharacter();
                    Console.Write("당신의 이름을 알려 주세요: ");
                    name = Console.ReadLine();
                    user = new Character(name);
                    game = new Game(user);
                    break;
                case 2:
                    game.LoadCharacter();
                    break;
            }

            Console.WriteLine($"{game.character.name}님 환영합니다!");
            Thread.Sleep(500);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기\n");
                Console.WriteLine("0. 종료하기");

                game.Select();

                game.SaveCharacter();
            }
        }
    }
}
