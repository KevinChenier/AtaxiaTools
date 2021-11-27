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


    public Tool(string name)
    {
        this.toolName = name;
    }

    void Awake()
    {
        bus = EventBus.Instance;
        configs = ConfigManager.Instance.GetToolConfig<TConfig>(toolName);
        baseConfigs = configs;

        // Some tools need a lateStart
        Invoke("InitTool", 1);
    }

    /// <summary>
    /// This is used to alter the tool with the configs that can be found inside this.configs
    /// </summary>
    protected virtual void InitTool()
    {
        toolBegan = true;
        UnityEngine.Debug.Log("Activity just began!");
        sw = new Stopwatch();
        sw.Start();
    }

    public virtual void EndTool(int timer)
    {
        if (!IsInvoking())
        {
            toolEnded = true;
            configsSave();
            UnityEngine.Debug.Log("Activity just ended!");
            tips.giveTip("Activity just ended! The activity will change in " + timer + " seconds.");
            Invoke("delayedEndTool", timer);
        }
    }

    private void delayedEndTool()
    {
        if (ConfigManager.Instance.Config.ScenarioActive)
        {
            ConfigManager.Instance.ScenarioManager.LoadNextScene();
        }
    }

    public abstract void score();

    public abstract void configsSave();
}
