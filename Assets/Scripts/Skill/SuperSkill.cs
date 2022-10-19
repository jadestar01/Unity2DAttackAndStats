using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuperSkill : MonoBehaviour
{
    public static GameObject player;
    public static WeaponController weaponController;

    private void Start()
    {
        player= GameObject.FindWithTag("Player");
        weaponController = GameObject.FindWithTag("WeaponController").GetComponent<WeaponController>();
    }

    [System.Serializable]
    public class Skill
    {
        public bool isCooltime = false;
        public float speedCoefficient = 1.0f;

        public string skillName;
        public string skillDescription;
        public KeyCode skillKey;
        public int costFP;
        public int costMP;
        public float coolTime;
        public float readyTime;
        public float attackTime;
        public int physicalDmg;
        public int physicalDmgCoefficient;
        public int magicalDmg;
        public int magicalDmgCoefficient;

        public virtual void UseSkill()
        {
        }

        public void SpeedSetter(float speed)
        {
            player.GetComponent<Move>().speedCoefficint = speed;
        }
        
        /*
        IEnumerator CoolDownCor()
        {
            
        }
        */

        public bool CheckMP()
        { return player.GetComponent<Resource>().curMP >= costMP && player.GetComponent<Resource>().curMP - costMP >= 0; }
        public void CostMP()
        { player.GetComponent<Resource>().curMP -= costMP; }

        public bool CheckFP()
        { return player.GetComponent<Resource>().curFP >= costFP && player.GetComponent<Resource>().curFP - costFP >= 0; }
        public void CostFP()
        { player.GetComponent<Resource>().curFP -= costFP; }

        public void SetisAttack(bool attack)
        {
            weaponController.isAttack = attack;
        }

        public bool GetisAttack()
        {
            return weaponController.isAttack;
        }
    }
}
