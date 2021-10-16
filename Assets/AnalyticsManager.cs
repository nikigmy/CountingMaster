using GameAnalyticsSDK;

public class AnalyticsManager
{
    private const string LEVEL_FORMAT = "level_{0}";

    public AnalyticsManager()
    {
#if ANALYTICS
        GameAnalytics.Initialize();
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }
    
    public void ReportResource(GAResourceFlowType flowType, ResourceTypes resource) 
    {
#if ANALYTICS
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, resource.ToString(), 0, resource.ToString(), resource.ToString());
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }
    
    public void ReportLevelProgression(GAProgressionStatus status, int level, int score = 0)
    {
#if ANALYTICS
        if (status == GAProgressionStatus.Complete || status == GAProgressionStatus.Fail)
        {
            GameAnalytics.NewProgressionEvent(status, string.Format(LEVEL_FORMAT, level), score);
        }
        else
        {
            GameAnalytics.NewProgressionEvent(status, string.Format(LEVEL_FORMAT, level));
        }
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }

    public void ReportColorChanged()
    {
#if ANALYTICS
        GameAnalytics.NewDesignEvent("ColorChanged");
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }

    public void ReportError(GAErrorSeverity severity, string message)
    {
#if ANALYTICS
        GameAnalytics.NewErrorEvent (severity, message);
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }

    public void SetDimension1(string dimensionName)
    {
        
#if ANALYTICS
        GameAnalytics.SetCustomDimension01(dimensionName);
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }
    
    public void SetDimension2(string dimensionName)
    {
#if ANALYTICS
        GameAnalytics.SetCustomDimension01(dimensionName);
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }
    
    public void SetDimension3(string dimensionName)
    {
#if ANALYTICS
        GameAnalytics.SetCustomDimension01(dimensionName);
#else
        Debug.LogWarning("Analytics are not enabled");
#endif
    }
}