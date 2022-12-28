using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager skillManager;
    public static SkillManager SkillManagement
    {
        get
        {
            if (null == skillManager)
            {
                return null;
            }
            return skillManager;
        }
    }

    private void Awake()
    {
        if (skillManager == null)
        {
            skillManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
