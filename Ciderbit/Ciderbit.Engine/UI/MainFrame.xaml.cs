using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Controls;
using System.IO;
using Newtonsoft.Json;
using Ciderbit.Engine.UI.Models;
using Ciderbit.Engine.Libraries.Core;
using Ciderbit.Engine.Libraries.Compiler;
using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Engine.Libraries.Utilities;
using System.Text;
using System.Threading;

namespace Ciderbit.Engine.UI
{
	/// <summary>
	/// Interaction logic for MainFrame.xaml
	/// </summary>
	public partial class MainFrame : Window
	{
		public static AssemblyModel SelectedAssembly { get; set; }

		public static ProcessModel SelectedProcess { get; set; } = new ProcessModel();

		public static ObservableCollection<AssemblyModel> Assemblies { get; set; } = new ObservableCollection<AssemblyModel>();

		public MainFrame()
		{
			var consoleWriter = new ConsoleRedirector();
			consoleWriter.LineWritten += (o, e) =>
			{
				ConsoleOutputTextBox.Dispatcher.Invoke(() => ConsoleOutputTextBox.AppendText(e.Line + "\n"));
			};

			Console.SetOut(consoleWriter);

			//Fill assemblies
			foreach(var dir in Directory.GetDirectories(AppContext.BaseDirectory + @"Data\Scripts"))
			{
				var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(dir + @"\script.info"));

				var model = new AssemblyModel();
				model.Info = info;
				model.Path = dir;

				Assemblies.Add(model);
			}

			InitializeComponent();

			DataContext = this;
		}

		private void ButtonClose_MouseDown(object sender, MouseButtonEventArgs e) => Close();

		private void ButtonMinimalize_MouseDown(object sender, MouseButtonEventArgs e) => WindowState = WindowState.Minimized;

		private void WindowBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
				DragMove();
		}

		private void ExecuteButton_Click(object sender, RoutedEventArgs e)
		{
			if(SelectedAssembly == null)
			{
				Console.WriteLine("No assembly selected!");
				return;
			}

			if (SelectedProcess == null)
			{
				Console.WriteLine("No process selected!");
				return;
			}

			AssemblyListBox.IsEnabled = false;
			ExecuteButton.IsEnabled = false;
			TerminateButton.IsEnabled = true;
			ConnectButton.IsEnabled = false;

			Console.WriteLine($"Compiling: {SelectedAssembly.Info.Name}.");

			var path = SelectedAssembly.Path + @"\" + SelectedAssembly.Info.Name + ".cider";

			Compiler.Create(path,
				Directory.GetFiles(SelectedAssembly.Path, "*.cs", SearchOption.AllDirectories),
				Directory.GetFiles(SelectedAssembly.Path, "*.dll", SearchOption.AllDirectories),
				SelectedAssembly.Info.Namespace + "." + SelectedAssembly.Info.MainClass);

			Console.WriteLine($"Sending execution packet.");

			ConduitClient.Send(new ConduitPacket(ConduitPacketType.Execute, Encoding.Default.GetBytes(path)));
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Connecting to the TCP server.");

			if (!ConduitClient.Connect())
			{
				Console.WriteLine("Unable to connect to the TCP server.");
				return;
			}

			Console.WriteLine("Connected successfully.");

			AssemblyListBox.IsEnabled = true;
			ExecuteButton.IsEnabled = true;
			TerminateButton.IsEnabled = false;
			ConnectButton.IsEnabled = false;

			ConduitClient.DataReceived += (o, ev) =>
			{
				switch (ev.Packet.PacketType)
				{
					case ConduitPacketType.Print:
						Console.WriteLine(Encoding.Default.GetString(ev.Packet.Data));
						break;
					case ConduitPacketType.ProcessSelect:
						Console.WriteLine("Process selection received.");

						Dispatcher.Invoke(() =>
						{
							var pid = Convert.ToInt32(Encoding.Default.GetString(ev.Packet.Data));

							SelectedProcess.Process = Process.GetProcessById(pid);

							var icon = ProcessUtility.GetProcessIcon(SelectedProcess.Process);

							if (icon != null)
							{

								var source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height),
									BitmapSizeOptions.FromEmptyOptions());

								SelectedProcess.Icon = source;
							}
							else
								Console.WriteLine("Icon could not be resolved.");

							SelectedProcessIcon.Source = SelectedProcess.Icon;
							SelectedProcessName.Content = SelectedProcess.Process.ProcessName;
						});

						break;
				}
			};
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Sending termination packet.");

			ConduitClient.Send(new ConduitPacket(ConduitPacketType.Terminate));

			ExecuteButton.IsEnabled = true;
			AssemblyListBox.IsEnabled = true;
			TerminateButton.IsEnabled = false;
		}
	}
}
