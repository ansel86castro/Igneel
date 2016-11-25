using System;

namespace Igneel.Utilities
{
    public class Assert
    {
        public void NotNull(object item)
        {
            if (item == null)
                throw new NullReferenceException();
        }
    }
}
