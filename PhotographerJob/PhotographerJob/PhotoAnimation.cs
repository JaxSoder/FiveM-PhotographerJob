using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using AnimationScripts;
using PhotographerJob;

namespace PhotoAnimationScripts
{
    class PhotoAnimation : BaseScript
    {
        public static int camera_prop;
        public static int particle_fx;

        public static bool RunningFullAnimation = false;

        public static void DespawnCamera()
        {
            API.DeleteObject(ref camera_prop);
        }

        public static void StopAnimating()
        {
            Game.PlayerPed.Task.ClearAll();
        }

        public static void StartPaparazziAnimation()
        {
            SpawnCameraProp();
            AnimateCamera();
            StartParticleLoop();
        }

        public static void StopPaparazziAnimation()
        {
            DespawnCamera();
            StopAnimating();
            StopParticleLoop();

        }

        public static async void AnimateCamera()
        {
            Debug.WriteLine("Running Animation");
            await Animation.AnimatePlayer("amb@world_human_paparazzi@male@base", "base", Animation.anim_flags_with_movement);
        }

        public static void SpawnCameraProp()
        {
            camera_prop = Animation.CreateObjectGen("prop_pap_camera_01", 0f);
        }

        public static async void StartParticleLoop()
        {
            await Animation.LoadParticleFx("core");
            API.SetPtfxAssetNextCall("core");
            particle_fx = API.StartParticleFxLoopedOnEntity("ent_anim_paparazzi_flash", camera_prop, 0f, 0f, 0f, 0f, 0f, 0f, 1f, false, false, false);
        }

        public static void StopParticleLoop()
        {
            API.StopParticleFxLooped(particle_fx, false);
        }

        public static async void RunFullAnimation()
        {
            while (RunningFullAnimation)
            {
                Job.EndJobSuccessfully();

                SpawnCameraProp();
                Debug.WriteLine();
                AnimateCamera();
                await Delay(2000);
                StartParticleLoop();
                await Delay(1000);
                StartParticleLoop();
                await Delay(3000);
                StopPaparazziAnimation();
                RunningFullAnimation = false;
                await Delay(5);
            }
        }
    }
}
