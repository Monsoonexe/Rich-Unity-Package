using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.InventorySystem.Currency
{
    /// <summary>
    /// A group of <see cref="CurrencyDefinition"/>s.
    /// </summary>
    public class CurrencyCollection : RichScriptableObject, IReadOnlyList<CurrencyDefinition>
    {
        [SerializeField, Required]
        private CurrencyDefinition[] currencies = System.Array.Empty<CurrencyDefinition>();

        public CurrencyDefinition this[int index] { get => currencies[index]; }

        public int Count { get => currencies.Length; }

        public IEnumerator<CurrencyDefinition> GetEnumerator()
        {
            for (int i = 0; i < currencies.Length; i++)
            {
                yield return currencies[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => currencies.GetEnumerator();

        public CurrencyDefinition this[UniqueID key] => this[key.ID];

        public CurrencyDefinition this[string key]
        {
            get
            {
                for (int i = 0; i < currencies.Length; i++)
                {
                    if (currencies[i].Id.Equals(key))
                        return currencies[i];
                }

                //throw new KeyNotFoundException(name);
                return null;
            }
        }
    }
}
