using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using Autodesk.Revit.UI;

namespace LockDimensionsForAllProjectTypes
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI : Window
    {
        ExternalEvent _lockEvent;
        LockHandler _LockHandler;
        UnLockHandler _UnLockHandeler;
        ExternalEvent _unLockEvent;

        public UI(ExternalEvent lockEvent, LockHandler lockHandler, ExternalEvent unLockEvent, UnLockHandler unLockHandler)
        {
            InitializeComponent();
            _lockEvent = lockEvent;
            _unLockEvent = unLockEvent;
            _LockHandler = lockHandler;
            _UnLockHandeler = unLockHandler;
        }

        private void UnLockButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _UnLockHandeler.ShowMessage = true;
            _unLockEvent.Raise();

        }

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _LockHandler.ShowMessage = true;
            _lockEvent.Raise();
        }
    }
}
