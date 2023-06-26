using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentFeatureSO : ScriptableObject
{
    internal abstract void addToMonsterFightDirector(MonsterFightDirector mfd);
    internal abstract void removeFromMonsterFightDirector(MonsterFightDirector mfd);

    //Children of this class should either register functions on unity events that are called when
    //MonsterFightDirector evokes certain things or add Monobehaviors to the object MonsterFightDirector
    //is attached to.
}
