using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using System.Xml;

namespace MaquinaTuringMulticintas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            ReadXMLDocument("Home");
        }

        private void ShowCredits(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("Credits");
        }

        private void ShowHome(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("Home");
        }
        
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("About");
        }
        
        private void ReadXMLDocument(string pageName) 
        {
            TitleTextBox.Text = string.Empty;
            Style contentStyle = ContentTextBlock.Style;
            Content.Children.Remove(ContentTextBlock);

            XmlDocument doc = new XmlDocument();
            doc.Load("../../XMLDocuments/" + pageName + ".xml");

            XmlNodeList page = doc.GetElementsByTagName("Title");
            string title = page[0].InnerText;

            XmlNodeList content = doc.GetElementsByTagName("Content");
            string contentStr = content[0].InnerText;

            TitleTextBox.Text = title;
            ContentTextBlock = new TextBlock { Text = contentStr, Style = contentStyle };
            Content.Children.Add(ContentTextBlock);
        }

        

        
    }
}
