using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    private float maxDisplacement = 2f;
    private float minTime = 0.1f, maxTime = 0.5f;
    private float rate = 20f;

    private float randomSeed = 0;//so no shakers have the same shake

    private Coroutine ShakeCoroutine = null;

    private void Awake()
    {
        randomSeed = Random.value;
    }

    public void shakeMe(float percentStrength)
    {
        if (ShakeCoroutine != null)
            StopCoroutine(ShakeCoroutine);

        ShakeCoroutine = StartCoroutine(shakeCoroutine(Mathf.Lerp(minTime, maxTime, percentStrength)));
    }

    private IEnumerator shakeCoroutine(float time)
    {
        while (time > 0)
        {
            float displacement = maxDisplacement * (time / maxTime);

            this.transform.localPosition = new Vector3(
                    Mathf.Lerp(-displacement, displacement, Mathf.PerlinNoise(randomSeed + Time.realtimeSinceStartup * rate, 0.5f)),
                    Mathf.Lerp(-displacement, displacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * rate)),
                    Mathf.Lerp(-displacement, displacement, Mathf.PerlinNoise(randomSeed + Time.realtimeSinceStartup * rate, -0.5f))
                );

            yield return null;//wait a frame

            time -= Time.deltaTime;
        }
    }
}
