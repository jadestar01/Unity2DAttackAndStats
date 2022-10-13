using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuperSkill : MonoBehaviour
{
    public class Skill
    {
        public bool isAttack = false;
        public bool canMove = true;

        public string skillName;
        public string skillDescription;
        public KeyCode skillKey;
        public int costFP;
        public int costMP;
        public float coolTime;
        public float readyTime;
        public float attackTime;

        public virtual void UseSkill()
        {
        }
    }
}
