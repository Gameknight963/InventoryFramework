namespace InventoryFramework
{
    public class InventoryManager
    {
        public static InventoryManager Instance { get; private set; } = new();
        public Inventory PlayerInventory { get; } = new();

        private Dictionary<string, ItemDefinition> _registry = new();
        public IReadOnlyDictionary<string, ItemDefinition> Registry => _registry;

        public event Action<ItemDefinition>? OnItemRegistered;

        public void RegisterItem(ItemDefinition def)
        {
            if (_registry.ContainsKey(def.Id))
                throw new InvalidOperationException($"Item '{def.Id}' already registered.");
            _registry[def.Id] = def;
            OnItemRegistered?.Invoke(def);
        }

        public ItemDefinition GetItem(string id)
        {
            return _registry.TryGetValue(id, out ItemDefinition? def)
                ? def
                : throw new InvalidOperationException($"Item '{id}' not registered.");
        }
    }
}
