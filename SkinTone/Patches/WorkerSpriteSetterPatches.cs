using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using WorkerSprite;

namespace SkinTone.Patches
{
    // this function would set head and hands back to white, don't let it
    [HarmonyPatch(typeof(WorkerSpriteSetter), nameof(WorkerSpriteSetter.Apply))]
    public class Apply
    {
        static void Prefix(WorkerSpriteSetter __instance, ref List<SpineChangeData> data)
        {
            AgentModel agentModel = __instance.Model as AgentModel;
            if (agentModel == null || agentModel._agentName == null)
            {
                return;
            }
            foreach (SpineChangeData item in data)
            {
                if (item.slot == "Head" || item.slot == WorkerBodyRegionKey.L_Hand || item.slot == WorkerBodyRegionKey.R_Hand)
                {
                    bool switch_color = (item.regionColor == UnityEngine.Color.white || item.regionColor == null);
                    if (Harmony_Patch.charaSkinTones.ContainsKey(agentModel._agentName.id) && switch_color)
                    {
                        item.isSettedColor = true;
                        item.regionColor = Harmony_Patch.charaSkinTones[agentModel._agentName.id];
                    }
                }
            }
        }
    }
}
