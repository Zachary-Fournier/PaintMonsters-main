using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/trickleHeal")]
public class trickleHeal_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    private int healthGain = 1;
    [SerializeField]
    private float interval = 2.0f;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {

        HealCoroutine = mfd.StartCoroutine(WaitHealCoroutine(mfd));
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        mfd.StopCoroutine(HealCoroutine);
        HealCoroutine = null;
    }

    private Coroutine HealCoroutine = null;

    private IEnumerator WaitHealCoroutine(MonsterFightDirector mfd)
    {
        do
        {
            yield return new WaitForSeconds(interval);

            mfd.modHPNoHit(healthGain, mfd);

        } while (true);

    }
}
