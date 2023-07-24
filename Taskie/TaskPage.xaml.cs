using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TaskieLib;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Taskie
{
    public sealed partial class TaskPage : Page
    {
        public TaskPage()
        {
            this.InitializeComponent();
        }

        public string listname { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                testname.Text = e.Parameter.ToString();
                listname = e.Parameter.ToString();
            }
            base.OnNavigatedTo(e);

            if (Tools.ReadList(listname) != null)
            {
                foreach (ListTask task in Tools.ReadList(listname))
                {
                    taskListView.Items.Add(task);
                }
            }
            
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            List<ListTask> tasks = new List<ListTask>();
            if (Tools.ReadList(listname) != null && (Tools.ReadList(listname)).Count > 0)
            {
                foreach (ListTask task2add in Tools.ReadList(listname))
                {
                    tasks.Add(task2add);
                }
            };
            ListTask task = new ListTask()
            {
                Name = args.QueryText,
                CreationDate = DateTime.Now,
                IsDone = false,
                SubTasks = new List<ListTask>()
            };
            tasks.Add(task);
            taskListView.Items.Add(task);
            Tools.SaveList(listname, tasks);
        }
    }
}
