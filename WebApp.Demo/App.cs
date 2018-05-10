using Granular.Host;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppDemo
{
    public partial class App
    {
        public static void Main()
        {
            ApplicationHost.Initialize(new WebApplicationHost());

            var app = new Application();
            app.Run();

            var w = new MainWindow();
            w.Show();

            // After building (Ctrl + Shift + B) this project, 
            // browse to the /bin/Debug or /bin/Release folder.

            // A new bridge/ folder has been created and
            // contains your projects JavaScript files. 

            // Open the bridge/index.html file in a browser by
            // Right-Click > Open With..., then choose a
            // web browser from the list

            // This application will then run in the browser.
        }
    }
}