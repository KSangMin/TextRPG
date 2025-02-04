namespace TextRPG
{
    public class Dungeon
    {
        public Character character;
        public string name;
        public int recommend;
        public int minDamage = 20;
        public int maxDamage = 35 + 1;
        public int reward;

        public Dungeon()
        {

        }

        public Dungeon(Character character, string name, int recommend, int reward)
        {
            this.character = character;
            this.name = name;
            this.recommend = recommend;
            this.reward = reward;
        }

        public void ChallangeDungeon()//던전 도전
        {
            if (recommend > character.GetArmor() && new Random().Next(0, 10) < 4) DungeonFail();
            else DungeonSuccess();
        }

        public void DungeonSuccess()//던전 도전 성공 시, 체력을 깎고 보상 증정
        {
            Console.Clear();
            Console.WriteLine($"던전 클리어\n축하드립니다!\n{name}을 클리어했습니다.\n");
            Console.WriteLine("[탐험 결과]");

            Console.Write($"체력 {character.health} -> ");
            int adjust = recommend - character.GetArmor();
            character.health -= new Random().Next(minDamage + adjust, maxDamage + adjust);
            Console.WriteLine(character.health);

            Console.Write($"Gold {character.gold} -> ");
            int additionalRatio = new Random().Next(character.GetAttack(), character.GetAttack() * 2 + 1);
            float ratio = 1f + additionalRatio / 100f;
            character.gold += (int)(reward * ratio);
            Console.WriteLine(character.gold);

            character.clear++;
            if (character.IsLevelUp())
            {
                Console.WriteLine($"레벨 {character.level - 1} -> {character.level}");
                Console.WriteLine($"공격력 {character.attack - 0.5f} -> {character.attack}");
                Console.WriteLine($"방어력 {character.defense - 1} -> {character.defense}");
            }

            return;
        }

        public void DungeonFail()//던전 도전 실패 시, 체력을 깎음
        {
            Console.Clear();
            Console.WriteLine($"던전 도전 실패\n안타깝네요..\n{name} 클리어에 실패했습니다.\n");
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {character.health} -> {character.health / 2}");
            character.health /= 2;

            return;
        }
    }
}
