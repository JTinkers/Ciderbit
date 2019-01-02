using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.IO;
using Ciderbit.Engine.UI.Models;
using Ciderbit.Engine.Libraries.Compiler;
using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Engine.Libraries.Utilities;
using System.Text;

namespace Ciderbit.Engine.UI
{
	public partial class MainFrame : Window
	{
		public static Script SelectedScript { get; set; }

		public static ProcessModel SelectedProcess { get; set; } = new ProcessModel();

		public static ObservableCollection<Script> Scripts { get; set; } = new ObservableCollection<Script>(Engine.GetScripts());

		public MainFrame()
		{
			var consoleWriter = new ConsoleRedirector();

			consoleWriter.TextWritten += (o, e) => 
				ConsoleOutputTextBox.Dispatcher.Invoke(() => ConsoleOutputTextBox.AppendText(e.Text + "\n"));

			Console.SetOut(consoleWriter);

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
			if(SelectedScript == null)
			{
				Console.WriteLine("No assembly selected!");
				return;
			}

			Engine.ExecuteScript(SelectedScript);

			ScriptListBox.IsEnabled = false;
			ExecuteButton.IsEnabled = false;
			TerminateButton.IsEnabled = true;
			ConnectButton.IsEnabled = false;
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

			ScriptListBox.IsEnabled = true;
			ExecuteButton.IsEnabled = true;
			TerminateButton.IsEnabled = false;
			ConnectButton.IsEnabled = false;
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{
			Engine.TerminateScript();

			ExecuteButton.IsEnabled = true;
			ScriptListBox.IsEnabled = true;
			TerminateButton.IsEnabled = false;
		}
	}
}
