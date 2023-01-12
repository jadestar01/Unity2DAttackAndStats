using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
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
    //사용할 스킬들을 담아두는 통
    [ShowInInspector] public Dictionary<SkillKey, SkillSO> meleeSkill = new Dictionary<SkillKey, SkillSO>
    { { SkillKey.LB, null}, { SkillKey.RB, null}, { SkillKey.Q, null}, { SkillKey.E, null} };
    [ShowInInspector] public Dictionary<SkillKey, SkillSO> magicSkill = new Dictionary<SkillKey, SkillSO>
    { { SkillKey.LB, null}, { SkillKey.RB, null}, { SkillKey.Q, null}, { SkillKey.E, null} };
    [ShowInInspector] public Dictionary<SkillKey, SkillSO> rangeSkill = new Dictionary<SkillKey, SkillSO>
    { { SkillKey.LB, null}, { SkillKey.RB, null}, { SkillKey.Q, null}, { SkillKey.E, null} };

    private void Update()
    {
        UseSkill();
    }

    void UseSkill()
    {
        //전투/기절/스킬중 판단

        Dictionary<SkillKey, SkillSO> skillSet = new Dictionary<SkillKey, SkillSO>();

        int weaponAcitive = WeaponController.Instance.weaponActive;

        if (weaponAcitive == 0)
            skillSet = meleeSkill;
        else if (weaponAcitive == 1)
            skillSet = magicSkill;
        else if (weaponAcitive == 2)
            skillSet = rangeSkill;

        if (WeaponController.Instance.weapon[WeaponController.Instance.weaponActive] != null)
        {
            if (Input.GetMouseButtonDown(0) && skillSet[SkillKey.LB] != null)        //LB
            {
                skillSet[SkillKey.LB].SkillAction(WeaponController.Instance.anchor[WeaponController.Instance.weaponActive]);
            }
            else if (Input.GetMouseButtonDown(1) && skillSet[SkillKey.RB] != null)   //RB
            {
                skillSet[SkillKey.RB].SkillAction(WeaponController.Instance.anchor[WeaponController.Instance.weaponActive]);
            }
            else if (Input.GetKeyDown(KeyCode.Q) && skillSet[SkillKey.Q] != null)   //Q
            {
                skillSet[SkillKey.Q].SkillAction(WeaponController.Instance.anchor[WeaponController.Instance.weaponActive]);
            }
            else if (Input.GetKeyDown(KeyCode.E) && skillSet[SkillKey.E] != null)   //E
            {
                skillSet[SkillKey.E].SkillAction(WeaponController.Instance.anchor[WeaponController.Instance.weaponActive]);
            }
        }
    }
}
