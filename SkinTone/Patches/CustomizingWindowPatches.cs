using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Customizing;
using Harmony;

namespace SkinTone.Patches
{
    [HarmonyPatch(typeof(CustomizingWindow), nameof(CustomizingWindow.Confirm))]
    public class Confirm
    {
        private static void Prefix(CustomizingWindow __instance)
        {
            bool flag = __instance.CurrentData.agentName.id == common.temp_chosen_id;
            if (flag)
            {
                Harmony_Patch.addToDict(__instance.CurrentData.agentName.id, common.color);
            }
        }

        // changing line 29, to be this.CurrentData.agentName = global::AgentNameList.instance.GetCustomNameByInfo(this.CurrentData.CustomName, this.CurrentData.agentName.id);
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructions_list = instructions.ToList();
            int index = instructions_list.FindIndex(line => line.opcode == OpCodes.Ldc_I4_M1);
            if (index == -1)
            {
                FileLog.Log("SkinTone: Error in Confirm: Could not find Ldc_I4_M1");
            }
            instructions_list.RemoveAt(index);

            // Remember, they get added in reverse order
            instructions_list.Insert(index, new CodeInstruction(OpCodes.Ldfld, typeof(AgentName).GetField(nameof(AgentName.id))));
            instructions_list.Insert(index, new CodeInstruction(OpCodes.Ldfld, typeof(AgentData).GetField(nameof(AgentData.agentName))));
            instructions_list.Insert(index, new CodeInstruction(OpCodes.Ldfld, typeof(CustomizingWindow).GetField(nameof(CustomizingWindow.CurrentData))));
            instructions_list.Insert(index, new CodeInstruction(OpCodes.Ldarg_0));
            return instructions_list;
        }
    }

    [HarmonyPatch(typeof(CustomizingWindow), nameof(CustomizingWindow.CloseWindow))]
    public class CloseWindow
    {
        // clear chosen agent
        static void Postfix()
        {
            common.temp_chosen_id = -1;
            common.temp_chosen_name = "";
            common.color = UnityEngine.Color.white;
        }
    }
}
