using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public Button GoButton;
    public BaseTool tool;
    public GameObject Video;
    public TextMeshPro tipsText;

    private static TutorialManager _instance;

    public static TutorialManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ConfigManager.Instance.Config.ActivateTutorial)
        {
            Debug.Log("Tutorial!");
            tool.Pause();
            GoButton.onClick.AddListener(() => BeginActivity());
            Video.GetComponent<RawImage>().enabled = true;
            Video.GetComponentInChildren<VideoPlayer>().enabled = true;
            tool.tips.giveTip(tool.baseConfigs.TutorialTip);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void BeginActivity()
    {
        Debug.Log("Activity beginning!");
        tool.Show();
        gameObject.SetActive(false);
        tipsText.text = "";
    }
}
