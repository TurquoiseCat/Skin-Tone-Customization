using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customizing;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkinTone.Patches
{
    [HarmonyPatch(typeof(AppearanceUI), nameof(AppearanceUI.OpenWindow))]
    public class OpenWindow
    {
        public static void setToHair(AppearanceUI instance)
        {
            common.skin_selected = false;
            instance.HairTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Hair");
            instance.palette.OnSetColor(instance.hairColor.CurrentColor);
        }
        static void Prefix(AppearanceUI __instance)
        {
            setToHair(__instance);
        }
    }

    [HarmonyPatch(typeof(AppearanceUI), nameof(AppearanceUI.InitialDataLoad))]
    public class InitialDataLoad
    {
        // add the button to switch to skin tone

        private static Text skin_selector = null;
        private static AppearanceUI instance = null;

        public static AppearanceUI get_instance()
        {
            return instance;
        }

        public static void setTo(bool skin)
        {
            common.skin_selected = skin;
            if (common.skin_selected)
            {
                instance.HairTitle.text = "Complexion";
                if (instance.original.agentName.id == common.temp_chosen_id)
                {
                    instance.palette.OnSetColor(common.color);
                }
                else if (Harmony_Patch.charaSkinTones.ContainsKey(instance.original.agentName.id))
                {
                    UnityEngine.Color color = Harmony_Patch.charaSkinTones[instance.original.agentName.id];
                    instance.palette.OnSetColor(color);
                }
                else
                {
                    UnityEngine.Color color = UnityEngine.Color.white;
                    instance.palette.OnSetColor(color);
                }
            }
            else
            {
                instance.HairTitle.text = LocalizeTextDataModel.instance.GetText("Customizing_Hair");
                instance.palette.OnSetColor(instance.hairColor.CurrentColor);
            }
        }

        static void onChange()
        {
            setTo(!common.skin_selected);
        }

        static void Prefix(AppearanceUI __instance)
        {
            instance = __instance;

            skin_selector = UnityEngine.Object.Instantiate<Text>(__instance.currentFaceTypeText);
            skin_selector.transform.SetParent(__instance.HairTitle.transform.parent);
            skin_selector.text = "";

            // scale
            Vector3 scale = __instance.currentFaceTypeText.transform.localScale;
            skin_selector.transform.localScale = scale;

            // position
            Vector3 pos = __instance.currentFaceTypeText.transform.localPosition;
            skin_selector.transform.localPosition = pos;

            // set buttons
            for (int i = 0; i < skin_selector.transform.childCount; i++)
            {
                Transform child = skin_selector.transform.GetChild(i);
                if (child.name == "PrevButton" || child.name == "NextButton")
                {
                    Button button = child.gameObject.GetComponent<Button>();
                    button.onClick.SetPersistentListenerState(0, UnityEventCallState.Off); // persistent are different and are not removed by next line
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(onChange);
                }
            }
        }
    }
}
