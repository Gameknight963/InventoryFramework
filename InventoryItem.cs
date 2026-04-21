using MelonLoader;

namespace InventoryFramework
{
    [RegisterTypeInIl2Cpp]
    public class InventoryItem : Il2CppSystem.Object
    {
        public ItemDefinition Definition;
        public int Quantity;
        
        public InventoryItem(ItemDefinition definition, int quantity)
        {
            Definition = definition;
            Quantity = quantity;
        }

        public InventoryItem(IntPtr ptr) : base(ptr) { }
    }
}
