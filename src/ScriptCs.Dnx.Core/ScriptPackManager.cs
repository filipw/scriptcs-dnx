using System;
using System.Collections.Generic;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public class ScriptPackManager : IScriptPackManager
    {
        private readonly IDictionary<Type, IScriptPackContext> _contexts = new Dictionary<Type, IScriptPackContext>();

        public ScriptPackManager(IEnumerable<IScriptPackContext> contexts)
        {
            if (contexts == null) throw new ArgumentNullException(nameof(contexts));

            foreach (var context in contexts)
            {
                _contexts[context.GetType()] = context;
            }
        }

        public TContext Get<TContext>() where TContext : IScriptPackContext
        {
            return (TContext)_contexts[typeof(TContext)];
        }
    }
}