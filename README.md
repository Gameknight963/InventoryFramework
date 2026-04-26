# InventoryFramework

A MelonLoader mod that provides a data and event layer for inventory management. Designed to be consumed by a separate UI assembly as it has no UI of its own.

---

## Getting Started

Add a reference to `InventoryFramework.dll` in your project. The framework initializes itself via MelonLoader. You do not need to instantiate anything.

Items must be registered before they can be added to any inventory:

```csharp
InventoryManager.Instance.RegisterItem(new ItemDefinition("wood", "Wood"));
InventoryManager.Instance.PlayerInventory.AddItem("wood", 5);
```

That first argument is the ID, and the second is the display name.

---

## InventoryManager

The main entry point. Access via `InventoryManager.Instance`.

### Properties

| Property          | Type                                          | Description                           |
| ----------------- | --------------------------------------------- | ------------------------------------- |
| `Instance`        | `InventoryManager`                            | Singleton.                            |
| `PlayerInventory` | `Inventory`                                   | The player's inventory.               |
| `SelectedItem`    | `InventoryItem?`                              | The currently selected item, or null. |
| `Registry`        | `IReadOnlyDictionary<string, ItemDefinition>` | All registered item definitions.      |

### Methods

| Method                             | Description                                                           |
| ---------------------------------- | --------------------------------------------------------------------- |
| `RegisterItem(ItemDefinition def)` | Registers an item definition. Throws if the ID is already registered. |
| `GetItem(string id)`               | Returns the definition for a registered item ID. Throws if not found. |
| `SelectItem(InventoryItem? item)`  | Sets the selected item and fires `OnItemSelected`.                    |

### Events

| Event                | Signature                | Description                           |
| -------------------- | ------------------------ | ------------------------------------- |
| `OnItemSelected`     | `Action<InventoryItem?>` | Fired when the selected item changes. |
| `OnItemUsed`         | `Action<InventoryItem?>` | Fired on left mouse button down.      |
| `OnItemAltUsed`      | `Action<InventoryItem?>` | Fired on right mouse button down.     |
| `OnInventoryKeyDown` | `Action<InventoryItem?>` | Fired on any key down.                |
| `OnItemRegistered`   | `Action<ItemDefinition>` | Fired when a new item is registered.  |

---

## Inventory

Manages a collection of `InventoryItem`s. Access the player inventory via `InventoryManager.Instance.PlayerInventory`.

### Properties

| Property | Type                           | Description                         |
| -------- | ------------------------------ | ----------------------------------- |
| `Items`  | `IReadOnlyList<InventoryItem>` | The current items in the inventory. |

### Methods

| Method                                    | Description                                                                                                               |
| ----------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| `AddItem(string id, int quantity = 1)`    | Adds quantity to an existing stack, or creates a new one.                                                                 |
| `RemoveItem(string id, int quantity = 1)` | Removes quantity from a stack. Removes the stack entirely if it reaches zero. Throws if the item is not in the inventory. |
| `GetItem(string id)`                      | Returns the `InventoryItem` for the given ID. Throws if not found.                                                        |
| `HasItem(string id, int quantity = 1)`    | Returns true if the inventory contains at least the given quantity.                                                       |
| `Clear()`                                 | Fires `OnCleared`, then removes all items and fires `OnChanged`.                                                          |

### Events

| Event                   | Signature               | Description                                                                      |
| ----------------------- | ----------------------- | -------------------------------------------------------------------------------- |
| `OnChanged`             | `Action`                | Fired after any change to the inventory.                                         |
| `OnCleared`             | `Action`                | Fired before the inventory is cleared. Items are still readable at this point.   |
| `OnItemAdded`           | `Action<InventoryItem>` | Fired when a new item stack is created.                                          |
| `OnItemRemoved`         | `Action<InventoryItem>` | Fired when an item stack is fully removed. Does not fire when Clear() is called. |
| `OnItemQuantityChanged` | `Action<InventoryItem>` | Fired when an existing stack's quantity changes.                                 |

---

## ItemDefinition

Defines a type of item. Create one per item type and register it with `InventoryManager.RegisterItem`.

### Constructor

```csharp
new ItemDefinition(string id, string name, Sprite? image = null)
```

### Properties

| Property | Type      | Description                                         |
| -------- | --------- | --------------------------------------------------- |
| `Id`     | `string`  | Unique identifier used in all inventory operations. |
| `Name`   | `string`  | Display name.                                       |
| `Image`  | `Sprite?` | Optional icon sprite.                               |

---

## InventoryItem

Represents a stack of a particular item in an inventory.

### Properties

| Property     | Type             | Description                                               |
| ------------ | ---------------- | --------------------------------------------------------- |
| `Definition` | `ItemDefinition` | The item type. Set at construction, cannot be reassigned. |
| `Quantity`   | `int`            | The current stack size.                                   |

> Do not modify `Quantity` directly. Use `Inventory.AddItem` and `Inventory.RemoveItem` so events fire correctly.

---

## Input

The framework listens for input in its own `OnUpdate` and fires the corresponding events on `InventoryManager`. You should not need to poll input yourself in most cases, subscribe to the events instead.

| Input              | Event fired          |
| ------------------ | -------------------- |
| Left mouse button  | `OnItemUsed`         |
| Right mouse button | `OnItemAltUsed`      |
| Any key            | `OnInventoryKeyDown` |

---

## Writing a UI Assembly

`InventoryFramework` is designed so any assembly can provide a UI by referencing it. The intended pattern:

- Subscribe to `InventoryManager` and `Inventory` events to react to state changes.
- Call `InventoryManager.Instance.SelectItem(item)` to update the selected item.
- Store subscribed lambdas in fields so they can be unsubscribed on scene unload.

```csharp
// Subscribe
_onChanged = () => Refresh(InventoryManager.Instance.PlayerInventory.Items);
InventoryManager.Instance.PlayerInventory.OnChanged += _onChanged;

// Unsubscribe (on dispose / scene unload)
InventoryManager.Instance.PlayerInventory.OnChanged -= _onChanged;
```

> Items registered before your UI is constructed will not trigger `OnItemRegistered`. Call your populate logic once at construction for existing items, then use `OnItemRegistered` for items registered afterward.