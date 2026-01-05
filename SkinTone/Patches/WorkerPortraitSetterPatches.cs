using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Customizing;
using Harmony;
using WorkerSprite;

namespace SkinTone.Patches
{
    [HarmonyPatch(typeof(WorkerPortraitSetter), nameof(WorkerPortraitSetter.SetCustomizing))]
    public class SetCustomizing
    {
        static void Postfix(WorkerPortraitSetter __instance, AgentData data, WorkerFaceType face = WorkerFaceType.DEFAULT)
        {
            UnityEngine.Color color;
            if (data.agentName == null)
            {
                // happens when hiring a new agent
                color = UnityEngine.Color.white;
                common.color = color;
            }
            else if (data.agentName.id == common.temp_chosen_id)
            {
                color = common.color;
            }
            else if (Harmony_Patch.charaSkinTones.ContainsKey(data.agentName.id))
            {
                color = Harmony_Patch.charaSkinTones[data.agentName.id];
            }
            else
            {
                color = UnityEngine.Color.white;
            }
            __instance.Head.color = color;
            __instance.LeftHand.color = color;
            __instance.RightHand.color = color;
        }
    }

    [HarmonyPatch(typeof(WorkerPortraitSetter), nameof(WorkerPortraitSetter.SetWorker))]
    public class SetWorker
    {
        static void Postfix(WorkerPortraitSetter __instance, WorkerModel worker)
        {
            UnityEngine.Color color;
            AgentModel agent = worker as AgentModel;
            if (agent == null)
            {
                color = UnityEngine.Color.white;
            }
            else if (Harmony_Patch.charaSkinTones.ContainsKey(agent._agentName.id))
            {
                color = Harmony_Patch.charaSkinTones[agent._agentName.id];
            }
            else
            {
                color = UnityEngine.Color.white;
            }
            __instance.Head.color = color;
            __instance.LeftHand.color = color;
            __instance.RightHand.color = color;
        }
    }
}
