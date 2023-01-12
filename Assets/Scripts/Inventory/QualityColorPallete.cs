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

public class QualityColorPallete : MonoBehaviour
{

    private static QualityColorPallete instance;
    public static QualityColorPallete Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Color ColorPallete(ItemQuality quality)
    {
        switch (quality)
        {
            case ItemQuality.None:
                {
                    return new Color(147f / 255f, 147f / 255f, 147f / 255f);
                }
            case ItemQuality.Normal:
                {
                    return new Color(93f / 255f, 255f / 255f, 0f);
                }
            case ItemQuality.Rare:
                {
                    return new Color(0f, 165f / 255f, 255f / 255f);
                }
            case ItemQuality.Epic:
                {
                    return new Color(209f / 255f, 93f / 255f, 255f / 255f);
                }
            case ItemQuality.Unique:
                {
                    return new Color(255f / 255f, 252f / 255f, 0f);
                }
            case ItemQuality.Legendary:
                {
                    return new Color(255f / 255f, 142f / 255f, 0f);
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