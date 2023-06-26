using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class boingPlayableBehaviour : PlayableBehaviour
{
    public boingPlayableAsset boingPlayableAsset = null;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        float percentBoing = boingPlayableAsset.BoingCurve.Evaluate((float)(playable.GetTime() / playable.GetDuration()));

        (playerData as MonsterAnimationDirector).AnimationAnchor.localScale = new Vector3(Mathf.LerpUnclamped(1, boingPlayableAsset.BoingScaleAmountX, percentBoing), Mathf.LerpUnclamped(1, boingPlayableAsset.BoingScaleAmountY, percentBoing), 1);

    }
}
