using Igneel.Compiling.Declarations;
using Igneel.Compiling.Preprocessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public class Scope
    {
        Scope _parent;               
        Dictionary<string, TypeDeclaration> _types = new Dictionary<string, TypeDeclaration>();        
        Dictionary<string, List<FunctionDeclaration>> _functions = new Dictionary<string, List<FunctionDeclaration>>();
        Dictionary<string, VariableDeclaration> _variables = new Dictionary<string, VariableDeclaration>();
        ErrorLog _log;

        public Scope()
        {

        }

        public Scope(Scope parent)
        {
            this._parent = parent;
        }

        public FunctionDeclaration CurrentFunction { get; set; }

        public ErrorLog Log
        {
            get { return _log ?? (_log = (_parent != null) ? _parent.Log : null); }
            set { _log = value; }
        }

        public Scope Parent { get { return _parent; } }      

        public void ProcessInclude(Include include)
        {

        }

        public TypeDeclaration GetType(string typeName)
        {
            return GetType(typeName, null);
        }

        public TypeDeclaration GetType(string typeName, TypeDeclaration[]genericParameters)
        {          
            TypeDeclaration type = null;
            if (_types.TryGetValue(typeName, out type))
            {
                if (genericParameters != null && type.IsGenericTypeDefinition)
                {
                    GenericTypeDefinition genTypeDef = (GenericTypeDefinition)type;
                    type = genTypeDef.ResolveGenerics(genericParameters);
                }
                type.IsUsed = true;
                if (!type.IsBuildIn && !type.IsChecked)
                {
                    type.CheckSemantic(this, Log);
                    type.IsChecked = true;
                }

            }
            else if ( _parent != null)
                type = _parent.GetType(typeName, genericParameters);          

            return type;
        }

        public void AddType(TypeDeclaration type)
        {
            _types.Add(type.Name, type);          
        }

        public void AddFunction(FunctionDeclaration function)
        {
            List<FunctionDeclaration> list;
            if(!_functions.TryGetValue(function.Name, out list))
            {
                list = new List<FunctionDeclaration>();
                _functions.Add(function.Name, list);
            }
            list.Add(function);            
        }        

        public FunctionDeclaration GetFunction(string name, params TypeDeclaration[] parameterTypes)
        {
            FunctionDeclaration function = null;
            List<FunctionDeclaration> list = null;
            if (_functions.TryGetValue(name, out list) && list.Count > 0)
            {
                foreach (var func in list)
                {
                    if (!func.IsBuildIn && !func.IsChecked)
                    {
                        func.CheckSemantic(this, Log);
                        func.IsChecked = true;
                    }
                    if (func.Match(parameterTypes))
                    {
                        if (func.IsGenericFunctionDefinition)
                        {
                            var funcDefinition = (GenericFunctionDefinition)func;

                            function = funcDefinition.MakeGenericFunction(parameterTypes);
                        }
                        else
                            function = func;

                        break;
                    };
                }
            }
            if(_parent!=null && function == null)
            {
                function = Parent.GetFunction(name, parameterTypes);
            }
            if (function != null)
                function.IsUsed = true;

            return function;
        }

        public void AddVariable(VariableDeclaration variableDeclaration)
        {
            _variables.Add(variableDeclaration.Name, variableDeclaration);
        }


        public bool ContainsVariable(string name, bool includeHeirarchy= true)
        {
            if (!_variables.ContainsKey(name))
            {
                if (_parent != null && includeHeirarchy)
                    return _parent.ContainsVariable(name);                 
                return false;
            }
            return true;
               
        }

        public VariableDeclaration GetVariable(string name)
        {
            VariableDeclaration v = null;
            if (!_variables.TryGetValue(name, out v))
            {
                if (_parent != null)
                    return _parent.GetVariable(name);
            }
            if (v != null)
            {
                if (!v.IsChecked)
                {
                    v.CheckSemantic(this, Log);
                    v.IsChecked = true;
                }
                v.IsUsed = true;
            }
            return v;
        }

        public bool ContainsType(string name)
        {
            if (!_types.ContainsKey(name))
            {
                if (_parent != null)
                    return _parent.ContainsType(name);
                return false;
            }
            return true;
               
        }
    }
}
