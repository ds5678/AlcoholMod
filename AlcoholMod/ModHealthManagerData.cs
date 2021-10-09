using System.Collections.Generic;

namespace AlcoholMod
{
	internal class ModHealthManagerData
	{
		public float alcoholPermille;
		public AlcoholUptake[] uptakes;

		public static ModHealthManagerData ParseJson(string jsonText)
		{
			var result = new ModHealthManagerData();
			var dict = MelonLoader.TinyJSON.JSON.Load(jsonText) as MelonLoader.TinyJSON.ProxyObject;
			result.alcoholPermille = dict["alcoholPermille"];

			var array = dict["uptakes"] as MelonLoader.TinyJSON.ProxyArray;
			List<AlcoholUptake> uptakes = new List<AlcoholUptake>();
			foreach (MelonLoader.TinyJSON.ProxyObject item in array)
			{
				AlcoholUptake uptake = new AlcoholUptake();
				uptake.amountPerGameSecond = item["amountPerGameSecond"];
				uptake.remainingGameSeconds = item["remainingGameSeconds"];
				uptakes.Add(uptake);
			}
			result.uptakes = uptakes.ToArray();

			return result;
		}

		public string DumpJson()
		{
			return MelonLoader.TinyJSON.JSON.Dump(this, MelonLoader.TinyJSON.EncodeOptions.NoTypeHints);
		}
	}
}
