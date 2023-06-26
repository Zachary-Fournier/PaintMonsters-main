using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EquipmentFeature/delayed stat change")]
public class delayedStatChange_EquipmentFeatureSO : EquipmentFeatureSO
{
    [SerializeField]
    [Min(0)]
    private float delayUntilStatChange = 30.0f;

    //does not makes sense for hp mod to be here
    [SerializeField]
    [Range(-10, 10)]
    private int powerMod = 0, speedMod = 0, luckMod = 0;

    internal override void addToMonsterFightDirector(MonsterFightDirector mfd)
    {

        TimerCoroutine = mfd.StartCoroutine(waitToBoostCoroutine(mfd));
    }

    internal override void removeFromMonsterFightDirector(MonsterFightDirector mfd)
    {
        if (TimerCoroutine != null)
        {
            mfd.StopCoroutine(TimerCoroutine);
            TimerCoroutine = null;
        }
    }

    private Coroutine TimerCoroutine = null;

    private IEnumerator waitToBoostCoroutine(MonsterFightDirector mfd)
    {
        yield return new WaitForSeconds(delayUntilStatChange);

        mfd.Power += powerMod;
        mfd.Speed += speedMod;
        mfd.Luck += luckMod;

        TimerCoroutine = null;

    }
}
