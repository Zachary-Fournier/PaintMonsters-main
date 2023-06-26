using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/crit builder")]
public class critBuilder_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    private int luckBoostPerNonCrit = 1;

    private int totalChange = 0;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.afterDealHit += afterDealHit;
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.afterDealHit -= afterDealHit;
    }

    private void afterDealHit(MonsterFightDirector me, MonsterFightDirector them, int attackPower, bool isCrit)
    {
        if (isCrit)
        {
            me.Luck -= totalChange;
            totalChange = 0;
        }

        else if (me.Luck < MonsterFightDirector.STAT_MAX)
        {
            if (me.Luck + luckBoostPerNonCrit > MonsterFightDirector.STAT_MAX)
                totalChange += MonsterFightDirector.STAT_MAX - me.Luck;
            else
                totalChange += luckBoostPerNonCrit;

            me.Luck += luckBoostPerNonCrit;
        }
    }

}
