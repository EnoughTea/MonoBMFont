using System;
using System.Collections.Generic;

namespace MonoBMFont {
    /// <summary> Extension methods for <see cref="IDictionary{TKey,TValue}" />. </summary>
    public static class DictionaryExtensions {
        /// <summary>
        ///     Returns <paramref name="defaultValue" /> if the given <paramref name="key" />
        ///     is not present within the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dict">The dictionary to search for value.</param>
        /// <param name="key">The key to look for.</param>
        /// <param name="defaultValue">The default value to be returned if the specified key is not present.</param>
        /// <returns>
        ///     Value matching specified <paramref name="key" /> or
        ///     <paramref name="defaultValue" /> if none is found.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="dict"/> is <see langword="null"/>
        /// -or- <paramref name="key"/> is <see langword="null"/></exception>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            TValue defaultValue = default(TValue)) {
            if (dict == null) {
                throw new ArgumentNullException(nameof(dict));
            }
            if (ReferenceEquals(key, null)) {
                throw new ArgumentNullException(nameof(key));
            }

            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }
}