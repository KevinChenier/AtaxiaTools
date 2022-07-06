using Assets.Scripts;
using System.Diagnostics;
using UnityEngine;

public abstract class Tool<TConfig> : BaseTool where TConfig : IToolConfig
{
    public TConfig configs { get; set; }
    public bool toolBegan { get; set; }
    public bool toolEnded { get; set; }
    protected EventBus bus;
    protected Stopwatch sw;
    private string toolName;
    private ConfigManager ConfigManager;
    private UserInterfaceManager UserInterfaceManager;

    public Tool(string name)
    {
        this.toolName = name;
    }

    void Awake()
    {
        bus = EventBus.Instance;
        UserInterfaceManager = UserInterfaceManager.Instance;
        ConfigManager = ConfigManager.Instance;
        configs = ConfigManager.GetToolConfig<TConfig>(toolName);
        baseConfigs = configs;

        if(!ConfigManager.Config.ActivateTutorial)
            Invoke("InitTool", 0.1f);
    }

    /// <summary>
    /// This is used to alter the tool with the configs that can be found inside this.configs
    /// </summary>
    public override void InitTool()
    {
        toolBegan = true;
        UnityEngine.Debug.Log("Activity just began!");
        sw = new Stopwatch();
        sw.Start();
        Show();
    }

    public override void EndTool(int timer)
    {
        if (!IsInvoking())
        {
            toolEnded = true;
            Pause();
            configsSave();
            UnityEngine.Debug.Log("Activity just ended!");

            UserInterfaceManager.tips.giveTip("Activity Ended");
            UserInterfaceManager.GoButton.gameObject.SetActive(true);
            UserInterfaceManager.GoButton.onClick.AddListener(() => ConfigManager.ScenarioManager.LoadNextScene());

            /* Still a viable option that we can use. Need to know if we want it as an option
            tips.giveTip("Activity just ended! The activity will change in " + timer + " seconds.");
            Invoke("delayedEndTool", timer);
            */
        }
    }

    private void delayedEndTool()
    {
        if (ConfigManager.Config.ScenarioActive)
        {
            ConfigManager.ScenarioManager.LoadNextScene();
        }
    }

    public abstract void score();

    public abstract void configsSave();
}
