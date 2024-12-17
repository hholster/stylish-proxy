using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace StylishProxy
{
    internal class PluginInfo
    {
        internal const string GUID = "hol.vaproxy.stylishproxy";
        internal const string Name = "StylishProxy";
        internal const string Version = "1.0.1";
    }

    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> CycleForward;
        private ConfigEntry<KeyCode> CycleBack;
        private ConfigEntry<KeyCode> Weapon1Bind;
        private ConfigEntry<KeyCode> Weapon2Bind;
        private ConfigEntry<KeyCode> Weapon3Bind;
        private ConfigEntry<KeyCode> Weapon4Bind;
        private ConfigEntry<KeyCode> CycleSwords;
        private ConfigEntry<KeyCode> CycleLongarms;

        internal static new ManualLogSource Logger;

        internal GameObject Sen;
        internal DATA data;
        internal int curWeapon;
        internal Inventory inv;
        internal ConfigFile cfg;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"{PluginInfo.Name} is loaded!");
            SceneManager.sceneLoaded += OnSceneLoaded;
            cfg = Config;
            CycleBack = cfg.Bind("Keybinds", "Cycle Weapon Back Bind", KeyCode.Q, "Keyboard bind to cycle weapons back, in the order Usurper, Judgement, Loyalty, Duty, Repeat.");
            CycleForward = cfg.Bind("Keybinds", "Cycle Weapon Forward Bind", KeyCode.R, "Keyboard bind to cycle weapons forward, in the order Duty, Loyalty, Judgement, Usurper, Repeat.");
            Weapon1Bind = cfg.Bind("Keybinds", "Duty Bind", KeyCode.Alpha1, "Keyboard bind to switch to Duty");
            Weapon2Bind = cfg.Bind("Keybinds", "Loyalty Bind", KeyCode.Alpha2, "Keyboard bind to switch to Loyalty");
            Weapon3Bind = cfg.Bind("Keybinds", "Judgement Bind", KeyCode.Alpha3, "Keyboard bind to switch to Judgement");
            Weapon4Bind = cfg.Bind("Keybinds", "Usurper Bind", KeyCode.Alpha4, "Keyboard bind to switch to Usurper");
            CycleSwords = cfg.Bind("Keybinds", "Cycle Swords", KeyCode.None, "Keyboard bind to cycle between Duty and Judgement");
            CycleLongarms = cfg.Bind("Keybinds", "Cycle Longarms", KeyCode.None, "Keyboard bind to cycle between Loyalty and Usurper");
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "BirdCage")
            {
                Logger.LogInfo($"{scene.name} is loaded");
                data = GameObject.Find("MAINMENU").GetComponent("DATA") as DATA;
                Sen = GameObject.Find("S-105.1");
            }
        }

        private void LateUpdate()
        {
            if (SceneManager.GetActiveScene().name == "BirdCage") 
            {
                inv = Sen.GetComponent<Inventory>();
                data = FindObjectOfType<DATA>();
                if (inv)
                {
                    curWeapon = inv.weaponIDS1 + 9;
                    if (Input.GetKeyDown(Weapon1Bind.Value))
                    {
                        curWeapon = 9;
                        Logger.LogDebug("Weapon Switched to Duty");
                    }
                    else if (Input.GetKeyDown(Weapon2Bind.Value))
                    {
                        curWeapon = 10;
                        Logger.LogDebug("Weapon Switched to Loyalty");
                    }
                    else if (Input.GetKeyDown(Weapon3Bind.Value))
                    {
                        curWeapon = 11;
                        Logger.LogDebug("Weapon Switched to Judgment");
                    }
                    else if (Input.GetKeyDown(Weapon4Bind.Value))
                    {
                        curWeapon = 12;
                        Logger.LogDebug("Weapon Switched to Usurper");
                    }
                    else if (Input.GetKeyDown(CycleBack.Value))
                    {
                        Logger.LogDebug("CurWeapon is" + curWeapon);
                        if (curWeapon != 9) {
                            curWeapon--;
                        }
                        else
                        {
                            curWeapon = 12;
                        }
                        Logger.LogDebug("Cycling backwards to" + curWeapon);
                    }
                    else if (Input.GetKeyDown(CycleForward.Value) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                    {
                        Logger.LogDebug("CurWeapon is" + curWeapon);
                        if (curWeapon != 12)
                        {
                            curWeapon++;
                        }
                        else
                        {
                            curWeapon = 9;
                        }
                        Logger.LogDebug("Cycling forward to" + curWeapon);
                    }
                    else if (Input.GetKeyDown(CycleSwords.Value))
                    {
                        if (curWeapon == 9)
                        {
                            curWeapon = 11;
                        }
                        else
                        {
                            curWeapon = 9;
                        }
                    }

                    else if (Input.GetKeyDown(CycleLongarms.Value))
                    {
                        if (curWeapon == 10)
                        {
                            curWeapon = 12;
                        }
                        else
                        {
                            curWeapon = 10;
                        }
                    }
                    data.SetWeapon1(curWeapon);
                    data.SetWeapon2(curWeapon);
                }
            }
        }
        
    }
}
