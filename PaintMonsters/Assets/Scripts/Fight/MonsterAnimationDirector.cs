using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class MonsterAnimationDirector : MonoBehaviour
{
    [SerializeField]
    private Shaker shaker = null;

    [SerializeField]
    private IdleWaggler IdleWaggler = null;

    private MonsterFightDirector monsterFightDirector = null;
    private MonsterUIDisplayManager monsterUIDisplayManager = null;

    [SerializeField]
    private TimelineAsset attackAnimation = null;

    [Tooltip("Used for playables visual animations, will be reset to local scale 1,1,1 and transform 0,0,0 regularly")]
    [SerializeField]
    private Transform animationAnchor = null;
    internal Transform AnimationAnchor => animationAnchor;

    [SerializeField]
    private Animator animator = null;

    private PlayableDirector playableDirector = null;

    [HideInInspector]
    public UnityEvent onAnimationCue;

    private static readonly int HP_CHANGE_FOR_MAX_SHAKE = 15;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void initialize(MonsterFightDirector mfd)
    {
        monsterFightDirector = mfd;
        monsterUIDisplayManager = mfd.MonsterUIDisplayManager;

        monsterFightDirector.onDeath.AddListener(playDeathAnimation);
    } 

    private void playDeathAnimation()
    {
        IdleWaggler.stopWaggle();

        //play death animation
    }

    internal void takeHit(int attackPower)
    {
        shaker.shakeMe(Mathf.Clamp01(Mathf.Abs(attackPower) / HP_CHANGE_FOR_MAX_SHAKE));
    }

    internal void playAttackAnimation()
    {
        IdleWaggler.stopWaggle();

        playableDirector.playableAsset = Instantiate(attackAnimation);

        foreach (PlayableBinding pb in playableDirector.playableAsset.outputs)
        {
            if (pb.outputTargetType == typeof(Animator))
                playableDirector.SetGenericBinding(pb.sourceObject, animator);
            else if (pb.outputTargetType == typeof(MonsterAnimationDirector))
                playableDirector.SetGenericBinding(pb.sourceObject, this);
        }

        playableDirector.Play();
        playableDirector.stopped += finishedAnimation;
    }

    public void animationCue()
    {
        onAnimationCue.Invoke();
    }

    private void finishedAnimation(PlayableDirector pd)
    {
        playableDirector.Stop();

        IdleWaggler.startWaggle();

        playableDirector.stopped -= finishedAnimation;

        animationAnchor.localPosition = Vector3.zero;
        animationAnchor.localRotation = Quaternion.identity;
        animationAnchor.localScale = Vector3.one;

    }

}
