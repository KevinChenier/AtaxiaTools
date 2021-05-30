public abstract class Tool<TConfig> : BaseTool where TConfig : IToolConfig
{
    protected TConfig configs;
    private string toolName;

    public Tool(string name)
    {
        this.toolName = name;
    }

    private void Start()
    {
        configs = ConfigManager.Instance.GetToolConfig<TConfig>(toolName);
        InitTool();
    }

    /// <summary>
    /// This is used to alter the tool with the configs that can be found inside this.configs
    /// </summary>
    protected abstract void InitTool();
}
