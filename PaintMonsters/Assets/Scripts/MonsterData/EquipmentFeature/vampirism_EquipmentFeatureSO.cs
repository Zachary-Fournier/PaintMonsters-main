using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/vampirism")]
public class vampirism_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    private float percentToSteal = 0.2f;

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
        me.modHPNoHit((int)(attackPower * percentToSteal), me);
    }

}
