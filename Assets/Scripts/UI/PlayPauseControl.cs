using UnityEngine;
using UnityEngine.UI;
public class PlayPauseControl : MonoBehaviour
{
    [SerializeField] Sprite playImage, pauseImage;
    bool isPlaying = false;
    Button playButton;
    Image displayImage;
    void Awake()
    {
        playButton = GetComponent<Button>();
        displayImage = GetComponent<Image>();
    }
    void Update()
    {
        if ((PathFinder.Instance.IsPreview || (!PathFinder.Instance.IsPreview && !PathFinder.Instance.IsRunning)) && isPlaying)
        {
            HandlePlayPauseClick();
        }
    }
    void OnEnable()
    {
        playButton.onClick.AddListener(HandlePlayPauseClick);
    }
    void OnDisable()
    {
        playButton.onClick.RemoveListener(HandlePlayPauseClick);
    }
    void HandlePlayPauseClick()
    {
        isPlaying = !isPlaying;
        displayImage.sprite = (isPlaying ? pauseImage : playImage);
        if (isPlaying) PathFinder.Instance.StartFindPath();
        else PathFinder.Instance.PausePath();
    }
}
