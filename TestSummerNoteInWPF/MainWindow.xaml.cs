using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TestSummerNoteInWPF
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {


            var htmlAgent = new HtmlAgent();
            wb1.ObjectForScripting = htmlAgent;


            string curDir = Directory.GetCurrentDirectory();
            wb1.Navigated += Wb1_Navigated;
            this.wb1.Navigate(new Uri(String.Format("file:///{0}/Assets/sample.html", curDir)));




        }

        private void Wb1_Navigated(object sender, NavigationEventArgs e)
        {

        }


    }
    
    [ComVisible(true)]
    public class HtmlAgent
    {
        public void ShowMessageFromHTML(string msg)
        {
            MessageBox.Show(msg);
        }

        public string UploadImage(string img)
        {

            var imageBase64=img.Split(new string[] { ";base64," }, StringSplitOptions.None)[1];
          
          
            var image = Base64ToImage(imageBase64).Source as BitmapSource;
            var filename = Guid.NewGuid().ToString("N");
           
            using (var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory +"Assets"+ System.IO.Path.DirectorySeparatorChar + filename + ".png", FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }

            return filename + ".png";
        }


        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
           
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = new Image();
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    image.Source = BitmapFrame.Create(stream,
                                                      BitmapCreateOptions.None,
                                                      BitmapCacheOption.OnLoad);
                }

                return image;
            }
        }
        

    }


}
