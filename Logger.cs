using MelonLoader;

namespace InventoryFramework
{
    internal static class InventoryLogger
    {
        internal static void Msg(string msg) => MelonLogger.Msg($"[InventoryFramework] {msg}");
    }
}
