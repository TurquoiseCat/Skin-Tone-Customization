using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customizing;
using Harmony;

namespace SkinTone.Patches
{
    [HarmonyPatch(typeof(ColorSelector), nameof(ColorSelector.OnExitEdit))]
    public class OnExitEdit
    {
        static bool Prefix(ColorSelector __instance, UnityEngine.Color c)
        {
            if (common.skin_selected)
            {
                AppearanceUI instance = InitialDataLoad.get_instance();
                if (instance == null)
                {
                    return true;
                }
                common.color = c;
                instance.portrait.Head.color = c;
                instance.portrait.LeftHand.color = c;
                instance.portrait.RightHand.color = c;
                common.temp_chosen_id = instance.original.agentName.id;
                common.temp_chosen_name = instance.original.agentName.GetName();
                return false;
            }
            return true;
        }
    }
}
