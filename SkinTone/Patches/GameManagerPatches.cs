using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace SkinTone.Patches
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public class StartGame
    {
        static void Postfix(GameManager __instance)
        {
            foreach (AgentModel agentModel in AgentManager.instance.GetAgentList())
            {
                common.setSkinTone(agentModel);
            }
        }
    }
}
