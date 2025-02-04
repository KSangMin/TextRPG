namespace TextRPG
{
    public class Weapon : Item
    {
        public Weapon()
        {

        }

        public Weapon(string name, int attack, int defense, string description, int price)
                : base(name, attack, defense, description, price)
        {

        }
    }
}
