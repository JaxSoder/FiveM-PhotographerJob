using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using SharedScripts;
using AnimationScripts;
using PhotoAnimationScripts;

namespace PhotographerJob
{
    public class Main : BaseScript
    {
        private static readonly Vector3 start_marker_pos = new Vector3(-601.523f, -929.926f, 22.8646f);
        private static readonly Vector3 start_marker_dir = new Vector3(0, 0, 0);
        private static readonly Vector3 start_marker_rot = new Vector3(0, 0, 0);
        private static readonly Vector3 start_marker_scale = new Vector3(2f, 2f, 1f);
        private static readonly Vector3 start_mainpoint = new Vector3(-601.523f, -929.926f, 22.8646f);
        private static Color start_marker_color = Color.FromArgb(150, 150, 255, 0);

        public static bool IsJobDone = false;
        public static bool IsCurrentlyWorking = false;

        private Random rand = new Random();

        private float DistanceToMarker;


        private int job_idx = 0;
        
        private List<Job> jobsList = new List<Job>()
        {
            new Job("FirstMission",  1000, new Vector3(-607.009f, -936.583f, 22.8607f)),
            new Job("SecondMission", 1000, new Vector3(-602.106f, -949.168f, 21.2025f))
        };

        public Main()
        {
            CheckDistanceToPoint();
        }

        private async void CheckDistanceToPoint()
        {
            while (true)
            {
                DistanceToMarker = Vector3.Distance(Game.PlayerPed.Position, start_mainpoint);

                if (DistanceToMarker >= 50)
                {
                    await Delay(3000);
                }
                else
                {
                    HandleDrawingAndInput();
                    await Delay(5);
                }
            }
        }

        private void HandleDrawingAndInput()
        {
            DrawMarkerIfNecessary();
            DrawTextIfNecessary();
            CheckInput();
        }

        private void CheckInput()
        {
            if (DistanceToMarker <= 2 && API.IsControlJustPressed(1, 18) && !IsCurrentlyWorking)
            {
                StartJob();
            }
            else if (DistanceToMarker <= 2 && API.IsControlJustPressed(1, 18) && IsCurrentlyWorking)
            {
                Job.EndJobFailed();
 
            }
        }

        private void DrawMarkerIfNecessary()
        {
            if (DistanceToMarker <= 30)
            {
                World.DrawMarker(MarkerType.VerticalCylinder, start_marker_pos, start_marker_dir, start_marker_rot, start_marker_scale, start_marker_color);
            }
        }

        private void DrawTextIfNecessary()
        {
            if (DistanceToMarker <= 2 && !IsCurrentlyWorking)
            {
                Shared.DrawTextSimple("Press ~g~Enter~w~ To Start a Photgraphy Job");
            }
            else if (DistanceToMarker <= 2 && IsCurrentlyWorking)
            {
                Shared.DrawTextSimple("Press ~g~Enter~w~ To Quit Current Job");
            }
        }

        private void StartJob()
        {
            IsCurrentlyWorking = true;
            IsJobDone = false;
            job_idx = rand.Next(jobsList.Count);
            jobsList[job_idx].RunJob();
        }
    }



    public class Job : BaseScript
    {
        public readonly string mission_name;
        public readonly int base_pay;
        public readonly Vector3 end_point;

        private static Vector3 marker_dir = new Vector3(0, 0, 0);
        private static Vector3 marker_rot = new Vector3(0, 0, 0);
        private static Vector3 marker_scale = new Vector3(4f, 4f, 4f);
        private static Color marker_color = Color.FromArgb(150, 255, 255, 0);



        private static Random rand = new Random();



        private float distance_to_end;

        public Job(string mission_name, int base_pay, Vector3 end_point)
        {
            this.mission_name = mission_name;
            this.base_pay = base_pay;
            this.end_point = end_point;
        }
        public async void RunJob()
        {
            while (Main.IsJobDone == false)
            {
                SetNewWaypoint();
                CheckJobInput();
                DrawJobMarkerIfNearby();
                DrawJobTextIfNecessary();
                await Delay(5);
            }

        }

        private void CheckJobInput()
        {
            distance_to_end = Vector3.Distance(Game.PlayerPed.Position, end_point);

            if (distance_to_end <= 2 && API.IsControlJustPressed(1, 18))
            {
                PhotoAnimation.RunningFullAnimation = true;
                PhotoAnimation.RunFullAnimation();
            }
        }

        private void DrawJobMarkerIfNearby()
        {
            if (distance_to_end <= 30)
            {
                World.DrawMarker(MarkerType.VerticalCylinder, end_point, Job.marker_dir, Job.marker_rot, Job.marker_scale, Job.marker_color);
            }
        }

        private void DrawJobTextIfNecessary()
        {
            if (distance_to_end <= 2)
            {
                Shared.DrawTextSimple("Press ~g~Enter~w~ To Take a Picture");
            }
        }

        public static void EndJobSuccessfully()
        {
            Debug.WriteLine("500$ Added");
            Main.IsJobDone = true;
            Main.IsCurrentlyWorking = false;
        }

        public static void EndJobFailed()
        {
            Main.IsJobDone = true;
            Main.IsCurrentlyWorking = false;
        }

        private void SetNewWaypoint()
        {
            API.SetNewWaypoint(end_point[0], end_point[1]);
        }
    }
}
