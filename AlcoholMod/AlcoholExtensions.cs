using ModComponentAPI;

namespace AlcoholMod
{
	internal static class AlcoholExtensions
	{
		internal static void Apply(this AlcoholComponent  _this, float normalizedValue)
		{
			float amountConsumed = _this.AmountTotal * normalizedValue;
			_this.AmountRemaining -= amountConsumed;
			ModHealthManager.DrankAlcohol(amountConsumed, _this.UptakeSeconds);
		}

		internal static void UpdateAlcoholValues(FoodItem foodItem)
		{
			AlcoholComponent alcoholComponent = ModComponentUtils.ComponentUtils.GetComponent<AlcoholComponent>(foodItem);
			if (alcoholComponent != null)
			{
				ModComponentUtils.CopyFieldHandler.UpdateFieldValues<AlcoholComponent>(alcoholComponent);
				alcoholComponent.AmountRemaining = foodItem.m_CaloriesRemaining / foodItem.m_CaloriesTotal * alcoholComponent.AmountTotal;
			}
		}
	}
}
