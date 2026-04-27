using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public float intensity = 0.1f;
    public float duration = 1f;
    public GameObject charSelectUI;

    public void TransitionDownWithBob()
    {
        Vector3 goalPosition = new Vector3(transform.position.x, -198.5f, transform.position.z);
        StartCoroutine(TransitionRoutine(transform.position, goalPosition, true));
    }

    public void TransitionUpWithBob()
    {
        Vector3 goalPosition = new Vector3(transform.position.x, 1, transform.position.z);
        StartCoroutine(TransitionRoutine(transform.position, goalPosition, false));
    }

    private IEnumerator TransitionRoutine(Vector3 start, Vector3 end, bool isUp)
    {
        float totalTime = 0f;

        while (totalTime < duration)
        {
            totalTime += Time.deltaTime;

            //formula for lerping the position of the camera so we get that nice bob
            // time^3 * (time - intensity)
            float customFormula = totalTime * totalTime * totalTime * (totalTime - intensity);
            transform.position = Vector3.LerpUnclamped(start, end, Mathf.Min(customFormula, 1f));

            yield return null;
        }

        if (isUp)
        {
            charSelectUI.SetActive(true);
        }

            transform.position = end;
    }
}
