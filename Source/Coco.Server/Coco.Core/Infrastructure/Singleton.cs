using System;
using System.Collections.Generic;

namespace Coco.Core.Infrastructure
{
    public class Singleton<T>
    {
        private static T _instance;
        public static IDictionary<Type, object> Singletons { get; }

        static Singleton()
        {
            Singletons = new Dictionary<Type, object>();
        }

        public static T Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
                Singletons[typeof(T)] = value;
            }
        }
    }
}
