using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using TuringMachineMT;

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
            //By default, we read the Home XML.
            ReadXMLDocument("Home");
        }

        #region Interface Buttons' Methods.
        /// <summary>
        /// Tells the ReadXMLDocument method to read the Credits.xaml file.
        /// </summary>
        /// <param name="sender">CreditsButton</param>
        /// <param name="e">OnClick EventArgs</param>
        private void ShowCredits(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("Credits");
        }

        /// <summary>
        /// Tells the ReadXMLDocument method to read the Home.xaml file.
        /// </summary>
        /// <param name="sender">CreditsButton</param>
        /// <param name="e">OnClick EventArgs</param>
        private void ShowHome(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("Home");
        }

        /// <summary>
        /// Tells the ReadXMLDocument method to read the About.xaml file.
        /// </summary>
        /// <param name="sender">CreditsButton</param>
        /// <param name="e">OnClick EventArgs</param>
        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("About");
        }

        /// <summary>
        /// Tells the ReadXMLDocument method to read the Instructions.xaml file.
        /// </summary>
        /// <param name="sender">CreditsButton</param>
        /// <param name="e">OnClick EventArgs</param>
        private void ShowInstructions(object sender, RoutedEventArgs e)
        {
            ReadXMLDocument("Instructions");
        }
        #endregion

        #region Control Methods
        /// <summary>
        /// Reads the specified XML document and displays its information.
        /// </summary>
        /// <param name="pageName">XML document's name.</param>
        private void ReadXMLDocument(string pageName) 
        {
            //Errase current information display.
            TitleTextBox.Text = string.Empty;
            Style contentStyle = ContentTextBlock.Style;
            Content.Children.Remove(ContentTextBlock);

            //Start reading the XML document
            XmlDocument doc = new XmlDocument();
            doc.Load("../../XMLDocuments/" + pageName + ".xml");

            XmlNodeList page = doc.GetElementsByTagName("Title");
            string title = page[0].InnerText;

            XmlNodeList content = doc.GetElementsByTagName("Content");
            string contentStr = content[0].InnerText;

            //Display the XML doument Information.
            TitleTextBox.Text = title;
            ContentTextBlock = new TextBlock { Text = contentStr, Style = contentStyle };
            Content.Children.Add(ContentTextBlock);
        }

        /// <summary>
        /// Initiallize send mail task.
        /// </summary>
        /// <param name="sender">ContactButton</param>
        /// <param name="e">Click EventArgs</param>
        private void SendMail(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:j.roberto-torres@hotmail.com, mauriciocunille@hotmail.com?subject=Multi-track Touring Machine");
        }
        
        /// <summary>
        /// Loads the xml file that contains the Multi-track Touring Machine's Formal Description.
        /// </summary>
        /// <param name="sender">LoadDescriptionButton</param>
        /// <param name="e">Click EventArgs</param>
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            string fileName = string.Empty;

            Microsoft.Win32.OpenFileDialog openFileD = new Microsoft.Win32.OpenFileDialog();
            openFileD.DefaultExt = ".xml";
            openFileD.Filter = "Xml Documents (*.xml)|*.xml";

            Nullable<bool> result = openFileD.ShowDialog();

            if (result == true)
            {
                fileName = openFileD.FileName;
            }

            FormalDescription description = new FormalDescription();
            description.Load(fileName);
        }
        #endregion


    }
}
