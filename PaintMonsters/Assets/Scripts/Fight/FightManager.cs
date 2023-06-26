using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This determines what a monster is.")]
    private MonsterDataSO leftMonsterData = null, rightMonsterData = null;

    [SerializeField]
    [Tooltip("Directors that will control monsters in battle.")]
    private MonsterFightDirector leftMonster = null, rightMonster = null;

    void Awake()
    {
        leftMonster.InitializeMonsterData(leftMonsterData);
        rightMonster.InitializeMonsterData(rightMonsterData);

        //This will be hardcoded like this for now as there are only two monsters
        leftMonster.attackTarget = rightMonster;
        rightMonster.attackTarget = leftMonster;

        leftMonster.onDeath.AddListener(rightWins);
        rightMonster.onDeath.AddListener(leftWins);

    }

    private void Start()
    {
        leftMonster.startAttack();
        rightMonster.startAttack();
    }

    private void leftWins()
    {
        leftMonster.stopAttack();
        rightMonster.stopAttack();

        // do later :)
    }

    private void rightWins()
    {
        leftMonster.stopAttack();
        rightMonster.stopAttack();

        // do later :)
    }

}
