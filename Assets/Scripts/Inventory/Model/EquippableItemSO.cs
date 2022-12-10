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
    }
}