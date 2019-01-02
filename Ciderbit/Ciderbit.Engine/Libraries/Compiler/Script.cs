using Newtonsoft.Json;
using System.IO;

namespace Ciderbit.Engine.Libraries.Compiler
{
	public class Script
	{
		public ScriptInfo Info { get; set; }

		public string Path { get; set; }

		public string OutputPath => Path + Info.Name + ".cider";

		public Script(string path)
		{
			Path = path;
			Info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(path + @"\script.info"));
		}

		public string[] GetReferences() => Directory.GetFiles(Path, "*.dll");

		public void Compile() => Compiler.Create(OutputPath,
			Directory.GetFiles(Path, "*.cs", SearchOption.AllDirectories),
			Directory.GetFiles(Path, "*.dll", SearchOption.AllDirectories),
			Info.Namespace + "." + Info.MainClass);
	}
}
