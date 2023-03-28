using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public class EnemyAttackInfo
    {
        public int attackPower;
    }
    public static Dictionary<int, EnemyAttackInfo> RANK_TO_ENEMYINFO;

    private void Awake()
    {
        RANK_TO_ENEMYINFO = new Dictionary<int, EnemyAttackInfo>
    {
    {1, new EnemyAttackInfo { attackPower = 10 }},
    {2, new EnemyAttackInfo { attackPower = 20 }},
    {3, new EnemyAttackInfo { attackPower = 30 }},
    {4, new EnemyAttackInfo { attackPower = 40 }},
    {5, new EnemyAttackInfo { attackPower = 50 }},
    {6, new EnemyAttackInfo { attackPower = 60 }},
    {7, new EnemyAttackInfo { attackPower = 70 }},
    {8, new EnemyAttackInfo { attackPower = 80 }},
    {9, new EnemyAttackInfo { attackPower = 90 }},
    };
    }


}
