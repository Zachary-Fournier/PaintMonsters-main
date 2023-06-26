using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class boingPlayableAsset : monsterVisualClip
{
    [SerializeField]
    private float boingScaleAmountY = 1.20f, boingScaleAmountX = 0.9f;

    [SerializeField]
    private AnimationCurve boingCurve = null;

    public float BoingScaleAmountY => boingScaleAmountY;
    public float BoingScaleAmountX => boingScaleAmountX;
    public AnimationCurve BoingCurve => boingCurve;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        boingPlayableBehaviour pb = new boingPlayableBehaviour();
        pb.boingPlayableAsset = this;

        return ScriptPlayable<boingPlayableBehaviour>.Create(graph, pb);
    }
}
