using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorPallete;

namespace ColorPallete
{
    public enum ItemQuality
    {
        None,
        Normal,
        Rare,
        Epic,
        Unique,
        Legendary
    };
}

namespace Inventory.UI
{
    public class QualityColorPallete : MonoBehaviour
    {
        [SerializeField] public Color None;
        [SerializeField] public Color Normal;
        [SerializeField] public Color Rare;
        [SerializeField] public Color Epic;
        [SerializeField] public Color Unique;
        [SerializeField] public Color Legendary;
        public Color ColorPallete(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.None:
                    {
                        return None;
                    }
                case ItemQuality.Normal:
                    {
                        return Normal;
                    }
                case ItemQuality.Rare:
                    {
                        return Rare;
                    }
                case ItemQuality.Epic:
                    {
                        return Epic;
                    }
                case ItemQuality.Unique:
                    {
                        return Unique;
                    }
                case ItemQuality.Legendary:
                    {
                        return Legendary;
                    }
            }
            return Color.white;
        }

        public string QualityString(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.None:
                    {
                        return " ";
                    }
                case ItemQuality.Normal:
                    {
                        return "ÀÏ¹Ý";
                    }
                case ItemQuality.Rare:
                    {
                        return "Èñ±Í";
                    }
                case ItemQuality.Epic:
                    {
                        return "¼­»ç";
                    }
                case ItemQuality.Unique:
                    {
                        return "Æ¯º°";
                    }
                case ItemQuality.Legendary:
                    {
                        return "Àü¼³";
                    }
            }
            return "";
        }


    }
}