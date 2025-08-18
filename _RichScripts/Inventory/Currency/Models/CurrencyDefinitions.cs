namespace RichPackage.InventorySystem.Currency
{
    /// <summary>
    /// A group of <see cref="CurrencyDefinition"/>s.
    /// </summary>
    public sealed class CurrencyDefinitions : ScriptableCollection<CurrencyDefinition>
    {
        public CurrencyDefinition this[UniqueID key] => this[key.ID];

        public CurrencyDefinition this[string key]
        {
            get
            {
                int length = this.Count;
                for (int i = 0; i < length; i++)
                {
                    if (this[i].Id.Equals(key))
                        return this[i];
                }

                //throw new KeyNotFoundException(name);
                return null;
            }
        }
    }
}
