using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/double edged crit")]
public class doubleEdgedCrit_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    float critDamageIncreaseFactor = 1.5f, selfDamageOnCritFactor = 0.25f;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.beforeDealHit += beforeDealHit;
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.beforeDealHit -= beforeDealHit;
    }

    private void beforeDealHit(MonsterFightDirector me, MonsterFightDirector them, ref int attackPower, bool isCrit)
    {
        if (isCrit)
        {
            attackPower = (int)(attackPower*critDamageIncreaseFactor);

            me.modHPNoHit((int)(-attackPower * selfDamageOnCritFactor), me);
        }
    }
}
