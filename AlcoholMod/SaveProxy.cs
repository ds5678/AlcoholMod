namespace AlcoholMod
{
	internal class SaveProxy
	{
		public string data;

		public SaveProxy()
		{
			this.data = "";
		}

		public static SaveProxy ParseJson(string jsonText)
		{
			var result = new SaveProxy();
			var dict = MelonLoader.TinyJSON.JSON.Load(jsonText) as MelonLoader.TinyJSON.ProxyObject;
			result.data = dict["data"];
			return result;
		}

		public string DumpJson()
		{
			return MelonLoader.TinyJSON.JSON.Dump(this, MelonLoader.TinyJSON.EncodeOptions.NoTypeHints);
		}
	}
}
