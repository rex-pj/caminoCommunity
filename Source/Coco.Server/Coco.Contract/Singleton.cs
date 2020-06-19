using System;
using System.Collections.Generic;

namespace Coco.Contract
{
    public class Singleton<T>
    {
        private static T instance;
        public static IDictionary<Type, object> AllSingletons { get; }

        static Singleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }
}
