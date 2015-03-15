using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel
{
    public static class Generator
    {
        static class GenId<T>
        {
            public static int objectId;
        }

        private static int objectId;

        public static int GenerateId()
        {
            return ++objectId;
        }

        public static int GenerateId<T>()
        {
            return ++GenId<T>.objectId;
        }


        public static string GenerateStringId()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
