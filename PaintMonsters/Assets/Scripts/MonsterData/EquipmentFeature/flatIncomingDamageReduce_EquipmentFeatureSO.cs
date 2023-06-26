using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/flat incoming damage reduce")]
public class flatIncomingDamageReduce_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    private int reduceIncomingDamageBy = -2;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.beforeTakeHit += beforeTakeHit;
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.beforeTakeHit += beforeTakeHit;
    }

    private void beforeTakeHit(MonsterFightDirector me, MonsterFightDirector them, ref int attackPower, bool isCrit)
    {
        attackPower -= reduceIncomingDamageBy;

        if (attackPower < 0) attackPower = 0;
    }

}
