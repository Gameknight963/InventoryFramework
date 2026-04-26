using MelonLoader;

namespace InventoryFramework
{
    public class InventoryItem
    {
        public ItemDefinition Definition { get; init; }
        public int Quantity { get; set; }

        public InventoryItem(ItemDefinition definition, int quantity)
        {
            Definition = definition;
            Quantity = quantity;
        }
    }
}
