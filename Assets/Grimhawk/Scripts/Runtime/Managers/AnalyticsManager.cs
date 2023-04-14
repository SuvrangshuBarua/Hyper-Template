#if SUPERSONIC_WISDOM_SDK_INSTALLED
using SupersonicWisdomSDK;
#endif
namespace grimhawk.managers
{
    public class AnalyticsManager : GameBehavior
    {
#if SUPERSONIC_WISDOM_SDK_INSTALLED
        public void SupersonicLevelStartNotifier()
        {
            SupersonicWisdom.Api.NotifyLevelStarted(_gameManager._levelManager.GetLevel(), null);    
        }
        public void SupersonicLevelCompletedNotifier() 
        {
            SupersonicWisdom.Api.NotifyLevelCompleted(_gameManager._levelManager.GetLevel(), null);
        }
        public void SupersonicLevelFailedNotifier()
        {
            SupersonicWisdom.Api.NotifyLevelFailed(_gameManager._levelManager.GetLevel(), null);
        }
#endif
    }
}
