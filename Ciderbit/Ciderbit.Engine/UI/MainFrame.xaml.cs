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
		public static string SearchBoxText { get; set; } = "";

		public static AssemblyModel SelectedAssembly { get; set; }

		public static ObservableCollection<AssemblyModel> Assemblies { get; set; } = new ObservableCollection<AssemblyModel>();

		public static ProcessModel SelectedProcess { get; set; }

		private static ObservableCollection<ProcessModel> processes { get; set; } = new ObservableCollection<ProcessModel>();

		public static ICollectionView Processes
		{
			get
			{
				var view = CollectionViewSource.GetDefaultView(processes);

				if (string.IsNullOrEmpty(SearchBoxText))
					return view;

				view.Filter = (object model) => ((ProcessModel)model).Process.ProcessName.ToLower().Contains(SearchBoxText.ToLower());

				return view;
			}
		}

		public MainFrame()
		{
			var consoleWriter = new ConsoleRedirector();
			consoleWriter.LineWritten += (o, e) =>
			{
				ConsoleOutputTextBox.AppendText(e.Line + "\n");
			};

			Console.SetOut(consoleWriter);

			//Fill processes
			foreach (var process in Process.GetProcesses().OrderBy(x => x.ProcessName))
			{
				processes.Add(new ProcessModel());
				processes.Last().Process = process;

				var icon = ProcessUtility.GetProcessIcon(processes.Last().Process);

				if (icon == null)
					continue;

				var source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height),
					BitmapSizeOptions.FromEmptyOptions());

				processes.Last().Icon = source;
			}

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

			this.DataContext = this;
		}

		private void ButtonClose_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		private void ButtonMinimalize_MouseDown(object sender, MouseButtonEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

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

			ProcessListBox.IsEnabled = false;
			AssemblyListBox.IsEnabled = false;
			ExecuteButton.IsEnabled = false;
			TerminateButton.IsEnabled = true;
			ProcessSearchBox.IsEnabled = false;
			ConnectButton.IsEnabled = false;

			Console.WriteLine($"Compiling: {SelectedAssembly.Path + @"\" + SelectedAssembly.Info.Name + ".cider"}");

			var path = SelectedAssembly.Path + @"\" + SelectedAssembly.Info.Name + ".cider";

			Compiler.Create(path,
				Directory.GetFiles(SelectedAssembly.Path, "*.cs", SearchOption.AllDirectories),
				Directory.GetFiles(SelectedAssembly.Path, "*.dll", SearchOption.AllDirectories),
				SelectedAssembly.Info.Namespace + "." + SelectedAssembly.Info.MainClass);

			Conduit.Send(new ConduitPacket(ConduitPacketType.Execute, Encoding.Default.GetBytes(path)));

			Console.WriteLine($"Sending execution packet.");
		}

		private void ProcessSearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ProcessListBox.ItemsSource = Processes;
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Connecting to the TCP server.");

			if (!Conduit.Connect())
			{
				Console.WriteLine("Unable to connect to the TCP server.");
				return;
			}

			Console.WriteLine("Connected successfully.");

			ProcessListBox.IsEnabled = true;
			AssemblyListBox.IsEnabled = true;
			ExecuteButton.IsEnabled = true;
			TerminateButton.IsEnabled = false;
			ProcessSearchBox.IsEnabled = true;
			ConnectButton.IsEnabled = false;
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Sending termination packet.");

			Conduit.Send(new ConduitPacket(ConduitPacketType.Terminate));

			ExecuteButton.IsEnabled = true;
			AssemblyListBox.IsEnabled = true;
			TerminateButton.IsEnabled = false;
		}
	}
}
