using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class MonsterFightDirector : MonoBehaviour
{
    /*[SerializeField]
    [Tooltip("This determines what a monster is.")]*/
    //set this in Fight Manager
    private MonsterDataSO monsterData = null;
    public MonsterDataSO MonsterData => monsterData;
    
    [SerializeField]
    [Tooltip("This is used to give the monster some animation.")]
    private MonsterAnimationDirector monsterAnimationDirector = null;

    [SerializeField]
    [Tooltip("Controls displays like health and name.")]
    private MonsterUIDisplayManager monsterUIDisplayManager = null;
    public MonsterUIDisplayManager MonsterUIDisplayManager => monsterUIDisplayManager;

    [SerializeField]
    private SpriteRenderer MonsterSpriteRenderer = null;

    [Header("Temp/debug")]
    public MonsterFightDirector attackTarget = null;


    //triggers and events
    [HideInInspector]
    public UnityEvent onDeath;//do not use on death for equipment, on death is only called when monster for sure dies, use onDeathBlow instead

    public delegate void Action_PreAttackInteraction(MonsterFightDirector me, MonsterFightDirector them, ref int attackPower, bool isCrit);//declaration here is nessasary to have referanced values
    public delegate void Action_PostAttackInteraction(MonsterFightDirector me, MonsterFightDirector them, int attackPower, bool isCrit);
    public delegate void Action_DeathInteraction(MonsterFightDirector me, MonsterFightDirector them);

    [HideInInspector]
    public Action_DeathInteraction onDeathBlow = delegate { };//called before death occures, if health is not above 0 after this goes through the monster dies

    [HideInInspector]
    public Action_PreAttackInteraction beforeDealHit = delegate { };
    [HideInInspector]
    public Action_PostAttackInteraction afterDealHit = delegate { };
    [HideInInspector]
    public Action_PreAttackInteraction beforeTakeHit = delegate { };
    [HideInInspector]
    public Action_PostAttackInteraction afterTakeHit = delegate { };

    //add more of these triggers for equipment if needed

    /*
     * private stat information
     */
    public static readonly int MIN_BASE_HP = 10, HP_PER_STATPOINT = 5;
    public int CurrentHP { get; private set; } = 0;

    public static readonly float ATTACK_DELLAY_MIN = 0.5f, ATTACK_DELLAY_MAX = 10f;
    public float baseAttackDelay = 0;

    public static readonly int MIN_BASE_ATTACK_POWER = 0, ATTACK_POWER_PER_STATPOINT = 1;
    public static readonly int MIN_POWER_ROLL = -3, MAX_POWER_ROLL = 3;
    public int baseAttackPower = 0;

    /*
     * simplified stat information
     */
    private int health = 10, power = 10, speed = 10, luck = 10;

    public static readonly int STAT_MIN = 0, STAT_MAX = 20, STAT_BASE = 10;

    public int Health { get { return health; } set { health = (int)Mathf.Clamp(value, STAT_MIN, STAT_MAX); } }
    public int Power { get { return power; } set {

            power = (int)Mathf.Clamp(value, STAT_MIN, STAT_MAX);
            baseAttackPower = MIN_BASE_ATTACK_POWER + Power * ATTACK_POWER_PER_STATPOINT;

        } }
    public int Speed { get { return speed; } set {
            
            speed = (int)Mathf.Clamp(value, STAT_MIN, STAT_MAX);
            baseAttackDelay = Mathf.Lerp(ATTACK_DELLAY_MAX, ATTACK_DELLAY_MIN, ((float)speed) / STAT_MAX);

        }
    }
    public int Luck { get { return luck; } set { luck = (int)Mathf.Clamp(value, STAT_MIN, STAT_MAX); } }

    private List<EquipmentFeatureSO> InstantiatedEquipmentFeatures = new List<EquipmentFeatureSO>();//need this in case equipment has local data it tracks

    internal void InitializeMonsterData(MonsterDataSO md)
    {
        monsterData = md;

        monsterUIDisplayManager.nameText = monsterData.name;

        foreach (EquipmentSO e in monsterData.Equipment)
        {
            e.applyBaseStatModifications(this);

            foreach (EquipmentFeatureSO ef in e.Features)
            {
                EquipmentFeatureSO newCopy = Instantiate(ef);

                InstantiatedEquipmentFeatures.Add(newCopy);

                newCopy.addToMonsterFightDirector(this);
            }
        }

        CurrentHP = MIN_BASE_HP + health * HP_PER_STATPOINT;

        /*baseAttackDelay = Mathf.Lerp(ATTACK_DELLAY_MAX, ATTACK_DELLAY_MIN, ((float)speed) / STAT_MAX);

        baseAttackPower = MIN_BASE_ATTACK_POWER + Power * ATTACK_POWER_PER_STATPOINT;*/

        monsterUIDisplayManager.setHealthInstant(CurrentHP);
        MonsterSpriteRenderer.sprite = monsterData.StandingSprite;

        monsterUIDisplayManager.setEquipmentVisuals(monsterData.Equipment);
    }

    void Start()
    {
        monsterAnimationDirector.initialize(this);
    }

    private void resetStatsAndAbilities()
    {
        health = power = speed = luck = STAT_BASE;

        foreach (EquipmentFeatureSO ef in InstantiatedEquipmentFeatures)
            ef.removeFromMonsterFightDirector(this);

        InstantiatedEquipmentFeatures.Clear();
    }

    /*
     * in moment calculations with stats
     */
    private int calculateAttackPower()
    {
        return (int)
            (
                baseAttackPower
                + UnityEngine.Random.Range(
                        MIN_POWER_ROLL + UnityEngine.Random.Range(0.0f, Luck * 0.25f),
                        MAX_POWER_ROLL + UnityEngine.Random.Range(0.0f, Luck * 0.25f)
                    )
            );
    }

    private bool rollForCrit()
    {
        return
            7f <
                (
                    UnityEngine.Random.Range(0, Luck - (attackTarget.Luck * 0.25f))
                );
    }

    private int calculateCritAttackPower()
    {
        return (int)((baseAttackPower + MAX_POWER_ROLL) * 1.5f);
    }

    private float calculateAttackDelay()
    {
        return (
                    (
                        UnityEngine.Random.Range(ATTACK_DELLAY_MIN, ATTACK_DELLAY_MAX)
                        + UnityEngine.Random.Range(ATTACK_DELLAY_MIN, ATTACK_DELLAY_MAX)
                    ) * 0.6f
                    + baseAttackDelay * 0.4f
                );
    }

    public void modHPNoHit(int healthChange, MonsterFightDirector cause)
    {
        CurrentHP += healthChange;
        monsterUIDisplayManager.setHealthAnimated(CurrentHP);

        if (CurrentHP <= 0)//if the monster dies
        {
            monsterUIDisplayManager.setHealthInstant(CurrentHP);

            onDeathBlow.Invoke(this, cause);

            if (CurrentHP <= 0)//in case equipment prevents death or something
            {
                onDeath.Invoke();

                this.resetStatsAndAbilities();

            }
        }

    }

    /*
     * interactions, attacks
     */
    public int takeHit(MonsterFightDirector aggressor, int attackPower, bool isCrit = false)
    {

        beforeTakeHit.Invoke(this, aggressor, ref attackPower, isCrit);

        CurrentHP -= attackPower;

        if(CurrentHP <= 0)//if the monster dies
        {
            monsterUIDisplayManager.setHealthInstant(CurrentHP);

            onDeathBlow.Invoke(this, aggressor);

            if (CurrentHP <= 0)//in case equipment prevents death or something
            {
                onDeath.Invoke();

                this.resetStatsAndAbilities();
                
            }
        }
        else
        {
            afterTakeHit.Invoke(this, aggressor, attackPower, isCrit);

            monsterUIDisplayManager.setHealthAnimated(CurrentHP);

            monsterAnimationDirector.takeHit(attackPower);
        }

        return attackPower;

    }

    private Coroutine AttackCoroutine = null;
    public bool Attacking { get; private set; } = false;

    public void startAttack()
    {
        Attacking = true;

        if (AttackCoroutine == null)
            AttackCoroutine = StartCoroutine(attackWaitCoroutine());
    }

    public void stopAttack()
    {
        Attacking = false;

        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = null;
        }
    }

    private IEnumerator attackWaitCoroutine()
    {
        yield return new WaitForSeconds(calculateAttackDelay());

        monsterAnimationDirector.playAttackAnimation();

        monsterAnimationDirector.onAnimationCue.AddListener(animationAttackCallback);

        AttackCoroutine = null;
    }

    private void animationAttackCallback()//called when the animator claims an attack strike landed in the animation and damage needs to be delt
    {
        monsterAnimationDirector.onAnimationCue.RemoveListener(animationAttackCallback);

        if (Attacking)
        {
            if (AttackCoroutine == null)
                AttackCoroutine = StartCoroutine(attackWaitCoroutine());

            if (rollForCrit())//if attack is a crit
            {
                power = calculateCritAttackPower();

                beforeDealHit.Invoke(this, attackTarget, ref power, true);

                power = attackTarget.takeHit(this, power, true);

                afterDealHit.Invoke(this, attackTarget, power, true);
            }
            else//if attack is not a crit
            {
                power = calculateAttackPower();

                beforeDealHit.Invoke(this, attackTarget, ref power, false);

                power = attackTarget.takeHit(this, power);

                afterDealHit.Invoke(this, attackTarget, power, false);
            }

        }
    }

}
