
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;



namespace JWTValidetor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		List<string> listpasswords = new List<string>();
		BackgroundWorker worker = new BackgroundWorker();
		public string JWTToken { get; set; }
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BtnCheckValid_Click(object sender, RoutedEventArgs e)
		{
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;

			worker.DoWork += BruteForce;
			worker.RunWorkerCompleted += RunWorkerCompleted;
			worker.ProgressChanged += ProgressChanged;
			worker.RunWorkerAsync(TxtJWTToken.Text);
		}

		private void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{

		}

		private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			WriteToTextBox("Finish");
		}

		private void BruteForce(object sender, DoWorkEventArgs e)
		{
			if (string.IsNullOrEmpty(e.Argument.ToString()))
			{
				MessageBox.Show("Jwt is Empty!");
				return;
			}
			JWTToken = e.Argument.ToString();
			foreach (var item in listpasswords)
			{
				var ret = invalidnew(item);
				if (ret)
				{
					MessageBox.Show("Passwod Find " + item);
					WriteToTextBox("Find Password " + item);
					return;
				}
				else
					WriteToTextBox("Faild " + item);
			}
		}

		private void WriteToTextBox(string text)
		{
			if (this.LogShow.Dispatcher.CheckAccess())
			{
				this.LogShow.Items.Add(text);
			}
			else
			{
				this.LogShow.Dispatcher.Invoke(
					System.Windows.Threading.DispatcherPriority.Normal,
					new Action(() => { LogShow.Items.Add(text); }));
			}
		}
		public bool invalidnew(string secret)
		{



			try
			{
				string jsonPayload = Jwt.JsonWebToken.Decode(JWTToken, secret);

				return true;
			}
			catch
			{


			}
			return false;
		}

		private void BtnSelectList_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



			// Set filter for file extension and default file extension 
			dlg.DefaultExt = ".png";
			dlg.Filter = "TXT Files (*.txt)|*.txt";

			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();
			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string filename = dlg.FileName;
				TxtAddressFile.Text = filename;
				listpasswords.AddRange(System.IO.File.ReadAllLines(filename).ToList());

			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Github && twitter : @begininvoke");
		}
	}
}
