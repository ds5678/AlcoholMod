namespace AlcoholMod
{
	internal class AlcoholUptake
	{
		public float amountPerGameSecond;

		public float remainingGameSeconds;

		public static AlcoholUptake Create(float amount, float gameSeconds)
		{
			AlcoholUptake result = new AlcoholUptake();

			result.amountPerGameSecond = amount / gameSeconds;
			result.remainingGameSeconds = gameSeconds;

			return result;
		}
	}
}
