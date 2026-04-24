namespace InventoryFramework
{
    public class InventoryManager
    {
        public static InventoryManager Instance { get; private set; } = new();
        public Inventory PlayerInventory { get; } = new();

        public InventoryItem? SelectedItem { get; private set; }

        public event Action<InventoryItem?>? OnItemSelected;
        public event Action<InventoryItem?>? OnItemUsed;
        public event Action<InventoryItem?>? OnItemAltUsed;
        public event Action<InventoryItem?>? OnKeyDown;

        public void SelectItem(InventoryItem? item)
        {
            SelectedItem = item;
            OnItemSelected?.Invoke(item);
        }

        public void UseItem() => OnItemUsed?.Invoke(SelectedItem);
        public void AltUseItem() => OnItemAltUsed?.Invoke(SelectedItem);
        public void KeyDown() => OnKeyDown?.Invoke(SelectedItem);

        private Dictionary<string, ItemDefinition> _registry = new();
        public IReadOnlyDictionary<string, ItemDefinition> Registry => _registry;
        public event Action<ItemDefinition>? OnItemRegistered;

        public void RegisterItem(ItemDefinition def)
        {
            if (_registry.ContainsKey(def.Id))
                throw new InvalidOperationException($"Item '{def.Id}' already registered.");
            _registry[def.Id] = def;
            InventoryLogger.Msg($"Item registered: {def.Id}");
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
