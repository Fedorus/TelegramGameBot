namespace GameLibrary
{
    public class Item
    {
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public bool CanBeSold { get; set; }
        public string ImageLink { get; set; }
        
        public int Damage { get; set; }
        public int ArmorValue { get; set; }

        private Item()
        {
        }

        public Item(ItemType type = ItemType.QuestItem)
        {
            Type = type;
        }
    }
}