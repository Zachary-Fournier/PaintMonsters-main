using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleWaggler : MonoBehaviour
{
    private float maxHorizontalDisplacement = 0.8f, maxAngleDisplacementDeg = 8f;
    private float rotationRate = 0.35f, moveRate = 0.65f;

    private float randomSeed = 0;//so no waggles have the same waggle

    private static readonly float TRANSITION_TIME = 0.25f;

    private Coroutine WaggleCoroutine = null;

    private void Awake()
    {
        randomSeed = Random.value;
    }

    private void Start()
    {
        startWaggle();
    }

    public void startWaggle()
    {
        if (WaggleCoroutine != null)
            StopCoroutine(WaggleCoroutine);

        WaggleCoroutine = StartCoroutine(buildToWaggle());
    }

    public void stopWaggle()
    {
        if (WaggleCoroutine != null)
        {
            StopCoroutine(WaggleCoroutine);

            WaggleCoroutine = StartCoroutine(dropFromWaggle());
        }
    }

    private IEnumerator buildToWaggle()
    {
        float elapsedTime = 0;

        do
        {

            float percentDone = elapsedTime / TRANSITION_TIME;

            float zRotationDegree =
                Mathf.Lerp(-maxAngleDisplacementDeg, maxAngleDisplacementDeg, Mathf.PerlinNoise(randomSeed + Time.realtimeSinceStartup * rotationRate, 0.5f) * percentDone);

            this.transform.localPosition = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)) * percentDone,
                    Mathf.Sin(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)) * percentDone,
                    1
                );

            this.transform.localRotation = Quaternion.Euler(0, 0, zRotationDegree);

            elapsedTime += Time.deltaTime;

            yield return null;

        } while (elapsedTime < TRANSITION_TIME);

        WaggleCoroutine = StartCoroutine(Waggle());
    }

    private IEnumerator Waggle()
    {
        do
        {

            float zRotationDegree =
                Mathf.Lerp(-maxAngleDisplacementDeg, maxAngleDisplacementDeg, Mathf.PerlinNoise(randomSeed + Time.realtimeSinceStartup * rotationRate, 0.5f));

            this.transform.localPosition = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)),
                    Mathf.Sin(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)),
                    1
                );

            this.transform.localRotation = Quaternion.Euler(0, 0, zRotationDegree);

            yield return null;

        } while (true);
    }

    private IEnumerator dropFromWaggle()
    {
        float elapsedTime = 0;

        do
        {

            float revPercentDone = 1 - elapsedTime / TRANSITION_TIME;

            float zRotationDegree =
                Mathf.Lerp(-maxAngleDisplacementDeg, maxAngleDisplacementDeg, Mathf.PerlinNoise(randomSeed + Time.realtimeSinceStartup * rotationRate, 0.5f) * revPercentDone);

            this.transform.localPosition = new Vector3(
                    Mathf.Cos(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)) * revPercentDone,
                    Mathf.Sin(Mathf.Deg2Rad * zRotationDegree) * Mathf.Lerp(-maxHorizontalDisplacement, maxHorizontalDisplacement, Mathf.PerlinNoise(0.5f, randomSeed + Time.realtimeSinceStartup * moveRate)) * revPercentDone,
                    1
                );

            this.transform.localRotation = Quaternion.Euler(0, 0, zRotationDegree);

            elapsedTime += Time.deltaTime;

            yield return null;

        } while (elapsedTime < TRANSITION_TIME);

        WaggleCoroutine = null;
    }

}
