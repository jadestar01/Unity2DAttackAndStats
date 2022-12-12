using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.EquippableItemSO;

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

        public UpgradeItemSO DeepCopy()
        {
            UpgradeItemSO item = new UpgradeItemSO();
            item.InStackable = InStackable;
            item.MaxStackSize = MaxStackSize;
            item.ItemImage = ItemImage;
            item.ID = ID;
            item.Name = Name;
            item.Type = Type;
            item.Quality = Quality;
            item.Description = Description;
            item.DefaultParametersList = new List<ItemParameter>();
            for (int i = 0; i < DefaultParametersList.Count; i++)
            {
                item.DefaultParametersList.Add(DefaultParametersList[i]);
            }
            item.DefaultUpgradeResults = new List<UpgradeResult>();
            for (int i = 0; i < DefaultUpgradeResults.Count; i++)
            {
                item.DefaultUpgradeResults.Add(DefaultUpgradeResults[i]);
            }
            item.actionSFX = actionSFX;
            item.upgradeType = upgradeType;
            item.upgradeRate = upgradeRate;

            return item;
        }
    }
}
