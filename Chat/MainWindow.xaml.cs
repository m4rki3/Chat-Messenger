using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chat;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string clientName;
    private const int serverPort = 11000;
    private Client client;
    public MainWindow()
    {
        InitializeComponent();
        client = new(IPAddress.Parse("127.0.0.1"), serverPort);
    }
    ~MainWindow()
    {
        client.Dispose();
    }
    private void SetNameButtonClick(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(SetNameTextBox.Text)
         && !string.IsNullOrWhiteSpace(SetNameTextBox.Text))
        {
            clientName = SetNameTextBox.Text;
            MessageButton.IsEnabled = true;
            MessageTextBox.IsEnabled = true;
            SetNameTextBox.IsEnabled = false;
            SetNameButton.IsEnabled = false;
        }
    }
    private void MessageButtonClick(object sender, RoutedEventArgs e)
    {
        client.SendMessage(clientName, MessageTextBox.Text);
    }
}
