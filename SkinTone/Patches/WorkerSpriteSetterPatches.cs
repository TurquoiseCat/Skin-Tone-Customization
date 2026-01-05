using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using WorkerSprite;

namespace SkinTone.Patches
{
    // for if other mods set head color to stuff then try to set it back to white later
    [HarmonyPatch(typeof(WorkerSpriteSetter), nameof(WorkerSpriteSetter.SetHeadColor))]
    public class SetHeadColor
    {
        static void Prefix(WorkerSpriteSetter __instance, ref UnityEngine.Color c)
        {
            if (c == UnityEngine.Color.white)
            {
                AgentModel agentModel = __instance.Model as AgentModel;
                if (agentModel == null || agentModel._agentName == null)
                {
                    return;
                }
                if (Harmony_Patch.charaSkinTones.ContainsKey(agentModel._agentName.id))
                {
                    c = Harmony_Patch.charaSkinTones[agentModel._agentName.id];
                }
            }
        }
    }

    [HarmonyPatch(typeof(WorkerSpriteSetter), nameof(WorkerSpriteSetter.BasicApply))]
    public class BasicApply
    {
        static void Postfix(WorkerSpriteSetter __instance)
        {
            AgentModel agentModel = __instance.Model as AgentModel;
            if (agentModel == null || agentModel._agentName == null)
            {
                return;
            }
            common.setSkinTone(agentModel);
        }
    }

    [HarmonyPatch(typeof(WorkerSpriteSetter), nameof(WorkerSpriteSetter.BaiscUniqueApply))]
    public class BasicUniqueApply
    {
        static void Postfix(WorkerSpriteSetter __instance)
        {
            AgentModel agentModel = __instance.Model as AgentModel;
            if (agentModel == null || agentModel._agentName == null)
            {
                return;
            }
            common.setSkinTone(agentModel);
        }
    }

    [HarmonyPatch(typeof(WorkerSpriteSetter), nameof(WorkerSpriteSetter.ArmorApply))]
    public class ArmorApply
    {
        static void Postfix(WorkerSpriteSetter __instance)
        {
            AgentModel agentModel = __instance.Model as AgentModel;
            if (agentModel == null || agentModel._agentName == null)
            {
                return;
            }
            common.setSkinTone(agentModel);
        }
    }
}
