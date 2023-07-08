using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class OptionsDropdown : MonoBehaviour
{
    [SerializeField] RectTransform optDropdown;
    [SerializeField] float dropdownSpeed = 650f;
    RectTransform rectTransform;
    float maxHeight = 125f;
    float dropdownWidth;
    bool isOpen;
    Coroutine dropdownAnimationCoroutine;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxHeight = rectTransform.sizeDelta.y;
        dropdownWidth = rectTransform.sizeDelta.x;
        rectTransform.sizeDelta = new Vector2(dropdownWidth, optDropdown.sizeDelta.y);
    }
    public void HandleOptionsDropdownToggle()
    {
        if (dropdownAnimationCoroutine != null)
            StopCoroutine(dropdownAnimationCoroutine);
        isOpen = !isOpen;
        dropdownAnimationCoroutine = StartCoroutine(DropdownAnimation(isOpen));
    }
    IEnumerator DropdownAnimation(bool open)
    {
        bool criteria = open ? rectTransform.sizeDelta.y <= maxHeight : rectTransform.sizeDelta.y >= optDropdown.sizeDelta.y;
        while (criteria)
        {
            rectTransform.sizeDelta = new Vector2(dropdownWidth, rectTransform.sizeDelta.y + ((open ? 1 : -1) * dropdownSpeed * Time.deltaTime));
            criteria = open ? rectTransform.sizeDelta.y <= maxHeight : rectTransform.sizeDelta.y >= optDropdown.sizeDelta.y;
            yield return new WaitForEndOfFrame();
        }
    }
}
