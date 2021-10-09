using MelonLoader;

namespace AlcoholMod
{
	internal class AlcoholMod : MelonMod
    {
		public override void OnApplicationStart()
		{
			ModHealthManager.Initialize();
		}
	}
}
