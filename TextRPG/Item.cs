namespace TextRPG
{
    public class Item
    {
        public string name;
        public int attack;
        public int defense;
        public string desciption;
        public int price = 0;

        public Item()
        {

        }

        public Item(string name, int attack, int defense, string description, int price)
        {
            this.name = name;
            this.attack = attack;
            this.defense = defense;
            this.desciption = description;
            this.price = price;
        }
    }
}
