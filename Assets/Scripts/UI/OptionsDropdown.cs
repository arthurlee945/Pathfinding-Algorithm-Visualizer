using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class OptionsDropdown : MonoBehaviour
{
    [SerializeField] RectTransform optDropdown;
    [SerializeField] float dropdownSpeed = 700f;
    RectTransform rectTransform;
    float maxHeight = 220f;
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
    void Update()
    {
        if ((PathFinder.Instance.IsRunning || PathFinder.Instance.IsPreview) && isOpen) HandleOptionsDropdownToggle();
    }
    public void HandleOptionsDropdownToggle()
    {
        if (PathFinder.Instance.IsRunning || PathFinder.Instance.IsPreview) return;
        if (dropdownAnimationCoroutine != null)
            StopCoroutine(dropdownAnimationCoroutine);
        isOpen = !isOpen;
        dropdownAnimationCoroutine = StartCoroutine(DropdownAnimation(isOpen));
    }
    IEnumerator DropdownAnimation(bool open)
    {
        bool criteria = open ? rectTransform.sizeDelta.y < maxHeight : rectTransform.sizeDelta.y > optDropdown.sizeDelta.y;
        while (criteria)
        {
            rectTransform.sizeDelta = new Vector2(dropdownWidth, rectTransform.sizeDelta.y + ((open ? 1 : -1) * dropdownSpeed * Time.deltaTime));
            criteria = open ? rectTransform.sizeDelta.y <= maxHeight : rectTransform.sizeDelta.y >= optDropdown.sizeDelta.y;
            yield return new WaitForEndOfFrame();
        }
        rectTransform.sizeDelta = new Vector2(dropdownWidth, open ? maxHeight : optDropdown.sizeDelta.y);

    }
}
