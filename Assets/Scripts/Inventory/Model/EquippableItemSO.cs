using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public enum UpgradeResult
        {
            None,
            Normal,
            Special,
            Fail
        };
        public string ActionName => "Equip";

        [field: SerializeField] public AudioClip actionSFX { get; set; }
        [field: SerializeField] public GameObject weapon;
        public List<UpgradeResult> upgradeResults = new List<UpgradeResult>();

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if (weaponSystem != null)
            {
                weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            return false;
        }

        public EquippableItemSO DeepCopy()
        {
            EquippableItemSO item = new EquippableItemSO();
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
            item.weapon = weapon;

            return item;
        }
    }
}