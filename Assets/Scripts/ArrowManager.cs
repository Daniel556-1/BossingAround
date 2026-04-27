using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public Transform LeftArrow;
    public Transform RightArrow;
    public float xOffset = 0f;

    public void AddArrows(GameObject selected)
    {
        if (selected != null)
        {
            RectTransform selectedRect = selected.GetComponent<RectTransform>();

            if (selectedRect == null) return;

            float half = selectedRect.rect.width / 2;
            Vector3 selectedVector = selected.transform.position;

            LeftArrow.position = new Vector3(selectedVector.x - half - xOffset, selectedVector.y, selectedVector.z);
            RightArrow.position = new Vector3(selectedVector.x + half + xOffset, selectedVector.y, selectedVector.z);

            LeftArrow.gameObject.SetActive(true);
            RightArrow.gameObject.SetActive(true);
        }

        return;
    }

    public void RemoveArrows()
    {
        LeftArrow.gameObject.SetActive(false);
        RightArrow.gameObject.SetActive(false);
    }
}
