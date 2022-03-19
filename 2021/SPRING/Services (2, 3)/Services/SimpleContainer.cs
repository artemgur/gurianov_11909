using System;
using System.Collections.Generic;

namespace Services
{
    public class SimpleContainer
    {
        private static readonly Dictionary<Type, Type> registeredObjects = new ();

        public static dynamic Resolve<TKey>()
        {
            return Activator.CreateInstance(registeredObjects[typeof(TKey)]);
        }

        public static void Register<TKey, TConcrete>() where TConcrete : TKey
        {
            registeredObjects[typeof(TKey)] = typeof (TConcrete);
        }
    }
}