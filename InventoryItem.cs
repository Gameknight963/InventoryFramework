using MelonLoader;

namespace InventoryFramework
{
    [RegisterTypeInIl2Cpp]
    public class InventoryItem : Il2CppSystem.Object
    {
        public ItemDefinition Definition { get; init; }
        public int Quantity { get; set; }
        
        public InventoryItem(ItemDefinition definition, int quantity)
        {
            Definition = definition;
            Quantity = quantity;
        }

        public InventoryItem(IntPtr ptr) : base(ptr) { }
    }
}
