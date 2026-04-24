using MelonLoader;

[assembly: MelonInfo(typeof(InventoryFramework.Core), "InventoryFramework", "1.0.0", "gameknight963")]

namespace InventoryFramework
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance Logger;

        public override void OnInitializeMelon()
        {
            Logger = LoggerInstance;
        }
    }
}
