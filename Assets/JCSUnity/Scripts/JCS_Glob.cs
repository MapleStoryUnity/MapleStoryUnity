namespace JCSUnity
{
    /// <summary>
    /// The global utility module.
    /// </summary>
    public static class JCS_Glob
    {
        /* Variables */

        public static JCS_AppSettings apps => JCS_AppSettings.FirstInstance();
        public static JCS_GameSettings games => JCS_GameSettings.FirstInstance();
        public static JCS_InputSettings inputs => JCS_InputSettings.FirstInstance();
        public static JCS_NetworkSettings networks => JCS_NetworkSettings.FirstInstance();
        public static JCS_PackageDataSettings pds => JCS_PackageDataSettings.FirstInstance();
        public static JCS_PauseSettings pauses => JCS_PauseSettings.FirstInstance();
        public static JCS_SceneSettings scenes => JCS_SceneSettings.FirstInstance();
        public static JCS_ScreenSettings screens => JCS_ScreenSettings.FirstInstance();
        public static JCS_SoundSettings sounds => JCS_SoundSettings.FirstInstance();
        public static JCS_StreamingAssets streams => JCS_StreamingAssets.FirstInstance();
        public static JCS_UISettings uis => JCS_UISettings.FirstInstance();
        public static JCS_PlatformSettings platforms => JCS_PlatformSettings.FirstInstance();
        public static JCS_PortalSettings portals => JCS_PortalSettings.FirstInstance();

#if (UNITY_ANDRIOD || UNITY_IOS || UNITY_EDITOR) && UNITY_ADS
        public static JCS_AdsManager adsm => JCS_AdsManager.FirstInstance();
#endif
        public static JCS_AppManager appm => JCS_AppManager.FirstInstance();
        public static JCS_ClientManager clientm => JCS_ClientManager.FirstInstance();
        public static JCS_GameManager gamem => JCS_GameManager.FirstInstance();
#if IAP_ENABLED
        public static JCS_IAPManager iapm => JCS_IAPManager.FirstInstance();
#endif
        public static JCS_InputManager inputm => JCS_InputManager.FirstInstance();
        public static JCS_NetworkManager networkm => JCS_NetworkManager.FirstInstance();
        public static JCS_PatchManager patchm => JCS_PatchManager.FirstInstance();
        public static JCS_PauseManager pausem => JCS_PauseManager.FirstInstance();
        public static JCS_PlayerManager playerm => JCS_PlayerManager.FirstInstance();
        public static JCS_SceneManager scenem => JCS_SceneManager.FirstInstance();
        public static JCS_ScreenManager screenm => JCS_ScreenManager.FirstInstance();
        public static JCS_SoundManager soundm => JCS_SoundManager.FirstInstance();
        public static JCS_TimeManager timem => JCS_TimeManager.FirstInstance();
        public static JCS_UIManager uim => JCS_UIManager.FirstInstance();
        public static JCS_UtilManager utilm => JCS_UtilManager.FirstInstance();
        public static JCS_2DDynamicSceneManager dsm2d => JCS_2DDynamicSceneManager.FirstInstance();
        public static JCS_2DGameManager gm2d => JCS_2DGameManager.FirstInstance();
        public static JCS_3DWalkActionManager wam3d => JCS_3DWalkActionManager.FirstInstance();
        public static JCS_ClimbableManager climbm => JCS_ClimbableManager.FirstInstance();
        public static JCS_PortalManager portalm => JCS_PortalManager.FirstInstance();

        public static JCS_TouchInput ti => JCS_TouchInput.FirstInstance();
        public static JCS_SoundPlayer soundPlayer => soundm.GlobalSoundPlayer();
        public static JCS_IGLogSystem igls => JCS_IGLogSystem.instance;
        public static JCS_DialogueSystem dialogueSystem => JCS_DialogueSystem.FirstInstance();

        public static JCS_PacketLostPreventer plp => JCS_PacketLostPreventer.FirstInstance();
        public static JCS_ServerRequestProcessor srp => JCS_ServerRequestProcessor.FirstInstance();

        /* Setter & Getter */

        /* Functions */

    }
}
