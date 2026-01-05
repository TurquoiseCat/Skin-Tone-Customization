using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using WorkerSprite;

namespace SkinTone.Patches
{
    public class common
    {
        public static int temp_chosen_id = -1;
        public static string temp_chosen_name = "";
        public static UnityEngine.Color color = UnityEngine.Color.white; // new UnityEngine.Color(181f / 255, 135f / 255, 103f / 255);
        public static bool skin_selected = false;

        public static void SetSpriteColor(WorkerSpriteSetter setter, UnityEngine.Color c, Sprite sprite, string region)
        {
            // similar to SetHeadColor()
            List<SpineChangeData> list = new List<SpineChangeData>();
            SpineChangeData spineChangeData = new SpineChangeData
            {
                sprite = sprite
            };
            spineChangeData.slot = (spineChangeData.attachmentName = region);
            spineChangeData.isSettedColor = true;
            spineChangeData.regionColor = c;
            list.Add(spineChangeData);
            setter.Apply(list);
        }

        public static void setSkinTone(AgentModel agentModel)
        {
            if (agentModel._agentName == null)
            {
                return;
            }
            if (Harmony_Patch.charaSkinTones.ContainsKey(agentModel._agentName.id))
            {
                UnityEngine.Color color = Harmony_Patch.charaSkinTones[agentModel._agentName.id];
                common.SetSpriteColor(agentModel.GetWorkerUnit().spriteSetter, color, agentModel.GetWorkerUnit().spriteSetter.currentSpriteSet.Left_Hand, WorkerBodyRegionKey.L_Hand);
                common.SetSpriteColor(agentModel.GetWorkerUnit().spriteSetter, color, agentModel.GetWorkerUnit().spriteSetter.currentSpriteSet.Right_Hand, WorkerBodyRegionKey.R_Hand);
                agentModel.GetWorkerUnit().spriteSetter.SetHeadColor(color);
            }
        }
    }
}
