using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(InventoryFramework.Core), "InventoryFramework", "1.0.1", "gameknight963")]

namespace InventoryFramework
{
    public class Core : MelonMod
    {
        internal static MelonLogger.Instance Logger;

        public override void OnInitializeMelon()
        {
            Logger = LoggerInstance;
        }

        public override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
                InventoryManager.Instance.UseItem();
            if (Input.GetMouseButtonDown(1))
                InventoryManager.Instance.AltUseItem();
            if (Input.anyKeyDown)
                InventoryManager.Instance.KeyDown();
        }
    }
}
