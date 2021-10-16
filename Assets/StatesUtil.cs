using UnityEngine;

public partial class GameManager
{
    protected static class StatesUtil
    {
        public static void Pause(bool stopTime = true)
        {
            if (stopTime)
            {
                Time.timeScale = 0;
            }

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        public static void Unpause()
        {
            Time.timeScale = 1;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

    }
}