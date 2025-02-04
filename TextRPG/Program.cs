namespace TextRPG
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.Write("당신의 이름을 알려 주세요: ");
            string name = Console.ReadLine();
            Character user = new Character(name);
            Game game = new Game(user);
            game.LoadCharacter();

            Console.WriteLine("환영합니다!");
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

                game.Select();

                game.SaveCharacter();
            }
        }
    }
}
