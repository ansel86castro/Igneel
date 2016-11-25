using System;

namespace Igneel.Assets
{
    public class ResourceNotFoundException : Exception
    {
        private string _name;

        public ResourceNotFoundException(string name)
        {            
            this._name = name;
        }

        public ResourceNotFoundException(string name, string message)
            :base(message)
        {
            this._name = name;
        }

        public ResourceNotFoundException()
        {
            // TODO: Complete member initialization
        }

        public string ResourceName { get { return _name; } }
    }
}
