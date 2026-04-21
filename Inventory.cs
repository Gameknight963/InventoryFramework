namespace InventoryFramework
{
    public class Inventory
    {
        private readonly List<InventoryItem> _items = new();
        public IReadOnlyList<InventoryItem> Items => _items;

        public event Action? OnChanged;

        public void AddItem(string id, int quantity = 1)
        {
            ItemDefinition def = InventoryManager.Instance.GetItem(id);
            InventoryItem? existing = _items.FirstOrDefault(i => i.Definition.Id == id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new InventoryItem(def, quantity));
            }
            OnChanged?.Invoke();
        }
        public void RemoveItem(string id, int quantity = 1)
        {
            InventoryItem? item = Items.FirstOrDefault(i => i.Definition.Id == id) 
                ?? throw new InvalidOperationException($"Item '{id}' not found in inventory.");
            item.Quantity -= quantity;

            if (item.Quantity <= 0)
                _items.Remove(item);
            OnChanged?.Invoke();
        }
        public InventoryItem GetItem(string id)
        {
            return Items.FirstOrDefault(i => i.Definition.Id == id)
                ?? throw new InvalidOperationException($"Item '{id}' not found in inventory.");
        }

        public bool HasItem(string id, int quantity = 1)
        {
            InventoryItem? item = Items.FirstOrDefault(i => i.Definition.Id == id);
            return item != null && item.Quantity >= quantity;
        }
        public void Clear()
        {
            _items.Clear();
            OnChanged?.Invoke();
        }
    }
}
