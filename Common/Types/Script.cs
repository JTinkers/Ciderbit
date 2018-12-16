using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Types
{
    /// <summary>
    /// Class responsible for holding and executing compiled assembly togheter with info related to it.
    /// </summary>
    public class Script
    {
        /// <summary>
        /// Contents of info file deserialized into an object.
        /// </summary>
        public ScriptInfo Info;

        /// <summary>
        /// Attached compiled assembly for execution.
        /// </summary>
        public Assembly Assembly;

        public Script(Assembly assembly = null, ScriptInfo info = null)
        {
            Assembly = assembly;
            Info = info;
        }

        /// <summary>
        /// Execute the attached assembly using hints from the info file.
        /// </summary>
        public void Execute()
        {
            var method = Assembly.GetType(Info.EntryClass, true, false).GetMethod(Info.EntryMethod, BindingFlags.Public | BindingFlags.Static);

            method.Invoke(null, null);
        }
    }
}
