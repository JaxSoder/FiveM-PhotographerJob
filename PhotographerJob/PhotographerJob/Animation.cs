using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace AnimationScripts
{
    class Animation : BaseScript
    {

        public static AnimationFlags anim_flags_with_movement    = AnimationFlags.AllowRotation  | AnimationFlags.UpperBodyOnly | AnimationFlags.Loop;
        public static AnimationFlags anim_flags_without_movement = AnimationFlags.StayInEndFrame | AnimationFlags.Loop          | AnimationFlags.UpperBodyOnly;

       
        public void StopAnimating()
        {
            Game.PlayerPed.Task.ClearAll();
        }


        public static async Task RequestAnimationDictionary(string dict)
        {
            if (!API.DoesAnimDictExist(dict))
            {
                Debug.WriteLine(string.Format("Animation {0} does not exist\n", dict));
                return;
            }

            if (API.HasAnimDictLoaded(dict))
            {
                return;
            }

            API.RequestAnimDict(dict);
            while (API.HasAnimDictLoaded(dict))
            {
                await Delay(1);
            }
        }

        public static void PlayAnimation(string dict, string animation, AnimationFlags anim_flags)
        {
            Game.PlayerPed.Task.ClearAll();
            Game.PlayerPed.Task.PlayAnimation(dict, animation, -1, -1, anim_flags);
        }

        public static async Task AnimatePlayer(string dict, string animation, AnimationFlags anim_flags)
        {
            await RequestAnimationDictionary(dict);
            PlayAnimation(dict, animation, anim_flags);
        }

        public static int CreateObjectGen(string prop, float zRot = 0.0f)
        {
            int bone = API.GetPedBoneIndex(Game.PlayerPed.Handle, 28422);
            int entity = API.CreateObject(API.GetHashKey(prop), 0f, 0f, 0f, true, true, true);
            API.AttachEntityToEntity(entity, Game.PlayerPed.Handle, bone, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, zRot, true, true, false, false, 2, true);
            return entity;
        }

        public static async Task LoadParticleFx(string fx)
        {
            if (API.HasNamedPtfxAssetLoaded(fx))
            {
                return;
            }
            API.RequestNamedPtfxAsset(fx);
            while (!API.HasNamedPtfxAssetLoaded(fx))
            {
                await Delay(1);
            }
        }
    }
}
