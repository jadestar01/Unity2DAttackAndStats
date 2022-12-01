using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class UpgradeItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public enum UpgradeType
        {
            None,
            Normal,
            Special
        }
        public string ActionName => "Upgrade";
        public UpgradeType upgradeType;
        [Range(0, 100)]public int upgradeRate;
        [field: SerializeField] public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            Debug.Log("°­È­");
            return true;
        }
    }
}
