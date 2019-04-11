using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace SharedScripts
{
    class Shared : BaseScript
    {

       

        public static void DrawTextSimple(string text)
        {
            Shared.DrawTextHandler(text, 1, true, 0.5f, 0.85f, 0.8f, 255, 255, 255, 255);
        }

        public static void DrawTextHandler(string text, int font, bool center, float x, float y, float scale, int r, int g, int b, int a)
        {
            API.SetTextFont(font);
            API.SetTextProportional(false);
            API.SetTextScale(scale, scale);
            API.SetTextColour(r, g, b, a);
            API.SetTextDropShadow();
            API.SetTextEdge(1, 0, 0, 0, 255);
            API.SetTextDropShadow();
            API.SetTextOutline();
            API.SetTextCentre(true);
            API.SetTextEntry("STRING");
            API.AddTextComponentString(text);
            API.DrawText(x, y);
        }

        

        public static void CreateBlip(double x, double y, double z)
        {
            Vector3 blipCoords = new Vector3(DoubleToFloat(x), DoubleToFloat(y), DoubleToFloat(z));
            Blip myBlip = World.CreateBlip(blipCoords);
            myBlip.Sprite = BlipSprite.AmmuNation;
        }

        public static float DoubleToFloat(double value)
        {
            return (float)value;
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
