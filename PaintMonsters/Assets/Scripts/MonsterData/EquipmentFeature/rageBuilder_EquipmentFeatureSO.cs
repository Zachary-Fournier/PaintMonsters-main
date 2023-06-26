using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/rage builder")]
public class rageBuilder_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    private int powerBoostPerTakeHit = 1;

    private int totalChange = 0;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.afterTakeHit += afterTakeHit;
        mfd.afterDealHit += afterDealHit;
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.afterTakeHit -= afterTakeHit;
        mfd.afterDealHit -= afterDealHit;
    }

    private void afterTakeHit(MonsterFightDirector me, MonsterFightDirector them, int attackPower, bool isCrit)
    {
        if (me.Power < MonsterFightDirector.STAT_MAX)
        {
            if (me.Power + powerBoostPerTakeHit > MonsterFightDirector.STAT_MAX)
                totalChange += MonsterFightDirector.STAT_MAX - me.Power;
            else
                totalChange += powerBoostPerTakeHit;

            me.Power += powerBoostPerTakeHit;
        }
    }

    private void afterDealHit(MonsterFightDirector me, MonsterFightDirector them, int attackPower, bool isCrit)
    {
        me.Power -= totalChange;
        totalChange = 0;
    }

}
