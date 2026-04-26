namespace InventoryFramework
{
    public class Inventory
    {
        private readonly List<InventoryItem> _items = new();
        public IReadOnlyList<InventoryItem> Items => _items;

        public event Action? OnChanged;
        public event Action? OnCleared;
        public event Action<InventoryItem>? OnItemAdded;
        public event Action<InventoryItem>? OnItemRemoved;
        public event Action<InventoryItem>? OnItemQuantityChanged;

        public void AddItem(string id, int quantity = 1)
        {
            ItemDefinition def = InventoryManager.Instance.GetItem(id);
            InventoryItem? existing = _items.FirstOrDefault(i => i.Definition.Id == id);
            if (existing != null)
            {
                existing.Quantity += quantity;
                OnItemQuantityChanged?.Invoke(existing);
            }
            else
            {
                InventoryItem item = new(def, quantity);
                _items.Add(item);
                OnItemAdded?.Invoke(item);
            }
            OnChanged?.Invoke();
        }

        public void RemoveItem(string id, int quantity = 1)
        {
            InventoryItem? item = _items.FirstOrDefault(i => i.Definition.Id == id)
                ?? throw new InvalidOperationException($"Item '{id}' not found in inventory.");
            item.Quantity = Math.Max(0, item.Quantity - quantity);
            if (item.Quantity <= 0)
            {
                _items.Remove(item);
                OnItemRemoved?.Invoke(item);
            }
            else
            {
                OnItemQuantityChanged?.Invoke(item);
            }
            OnChanged?.Invoke();
        }

        public InventoryItem GetItem(string id)
        {
            return _items.FirstOrDefault(i => i.Definition.Id == id)
                ?? throw new InvalidOperationException($"Item '{id}' not found in inventory.");
        }

        public bool HasItem(string id, int quantity = 1)
        {
            InventoryItem? item = _items.FirstOrDefault(i => i.Definition.Id == id);
            return item != null && item.Quantity >= quantity;
        }

        public void Clear()
        {
            OnCleared?.Invoke();
            _items.Clear();
            OnChanged?.Invoke();
        }
    }
}
