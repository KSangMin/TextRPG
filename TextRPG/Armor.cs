namespace TextRPG
{
    public class Armor : Item
    {
        public Armor() 
        { 

        }
        public Armor(string name, int attack, int defense, string description, int price)
                : base(name, attack, defense, description, price)
        {

        }
    }
}
