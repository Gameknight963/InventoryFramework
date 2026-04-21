using MelonLoader;

namespace InventoryFramework
{
    public class Core : MelonMod
    {
        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (sceneName != "Version 1.9 POST")
            {
                InventoryManager.Instance.PlayerInventory.Clear();
                return;
            }
        }
    }
}
