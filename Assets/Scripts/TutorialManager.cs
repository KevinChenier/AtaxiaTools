using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    private UserInterfaceManager UserInterfaceManager;

    public BaseTool tool;
    public GameObject Video;

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
        UserInterfaceManager = UserInterfaceManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ConfigManager.Instance.Config.ActivateTutorial)
        {
            Debug.Log("Tutorial!");
            tool.Pause();
            UserInterfaceManager.GoButton.onClick.AddListener(() => BeginActivity());
            Video.GetComponent<RawImage>().enabled = true;
            Video.GetComponentInChildren<VideoPlayer>().enabled = true;
            Video.GetComponentInChildren<VideoPlayer>().loopPointReached += OnVideoEnded;
            UserInterfaceManager.tips.giveTip(tool.baseConfigs.Name);

            // If the current tool was already done in the scenario, we show him the go button
            if (ConfigManager.Instance.ScenarioManager.toolsOrder.Count != 0)
                if (ConfigManager.Instance.ScenarioManager.toolsDone.Contains(SceneManager.GetActiveScene().name))
                    UserInterfaceManager.GoButton.gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void BeginActivity()
    {
        tool.InitTool();
        gameObject.SetActive(false);
        UserInterfaceManager.GoButton.gameObject.SetActive(false);
        UserInterfaceManager.tips.deactivateTip();
    }

    void OnVideoEnded(VideoPlayer video)
    {
        UserInterfaceManager.GoButton.gameObject.SetActive(true);
    }
}
