using MelonLoader;

namespace InventoryFramework
{
    internal class InventoryLogger
    {
        internal static void Msg(string msg) => Core.Logger.Msg(msg);
    }
}
