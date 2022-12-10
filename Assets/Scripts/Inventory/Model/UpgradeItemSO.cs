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
        [Range(0, 100)]public float upgradeRate;
        [field: SerializeField] public AudioClip actionSFX { get; set; }

        public bool PerformAction(GameObject character, List<ItemParameter> ltemState = null)
        {
            return true;
        }
    }
}
