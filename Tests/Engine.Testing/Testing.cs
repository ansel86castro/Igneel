using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace D3D9Testing
{
    public class TestAttribute:Attribute
    {
    }

    public class TestMethodAttribute : Attribute
    {

    }

    public class TestManager
    {
        public static Action Cleaner;
        public static List<TestContainer> Tests = new List<TestContainer>();

        static TestManager()
        {
            foreach (var type in typeof(TestManager).Assembly.GetTypes())
            {
                if (type.GetCustomAttribute<TestAttribute>() != null)
                {
                    object instance = Activator.CreateInstance(type);
                    TestContainer t = new TestContainer() { Name = type.Name, Instance = instance };
                    Tests.Add(t);

                    foreach (var method in type.GetMethods())
                    {
                        if (method.GetCustomAttribute<TestMethodAttribute>() != null)
                        {
                            t.Testes.Add(new ActionContainer
                                {
                                    Name = method.Name,
                                    Action = (Action)Delegate.CreateDelegate(typeof(Action), instance, method)
                                });
                        }
                    }
                }
            }
        }
    }

    public class TestContainer
    {
        public object Instance;
        public string Name;
        public List<ActionContainer> Testes = new List<ActionContainer>();

        public override string ToString()
        {
            return Name;
        }
    }

    public class ActionContainer
    {
        public string Name;
        public Action Action;

        public override string ToString()
        {
            return Name;
        }
    }
}
