using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/revive on death")]
public class reviveOnDeath_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    [Range(-10, 10)]
    private int healthModOnRevive = 0, powerModOnRevive = 0, speedModOnRevive = 0, luckModOnRevive = 0;

    private bool hasRevivedAlready = false;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.onDeathBlow += onDeathBlow;
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.onDeathBlow -= onDeathBlow;
    }

    private void onDeathBlow(MonsterFightDirector me, MonsterFightDirector them)
    {
        if (me.CurrentHP == 0 && !hasRevivedAlready)
        {
            hasRevivedAlready = true;

            me.Health += healthModOnRevive;
            me.Power += powerModOnRevive;
            me.Speed += speedModOnRevive;
            me.Luck += luckModOnRevive;

            me.modHPNoHit(MonsterFightDirector.MIN_BASE_HP + me.Health * MonsterFightDirector.HP_PER_STATPOINT, me);
        }
    }

}
