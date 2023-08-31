using HealthHarmony.ViewModel;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace HealthHarmony.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVindowViewModel VM;
        public MainWindow()
        {
            InitializeComponent();

            VM = this.DataContext as MainVindowViewModel;

            UsersList_DataGrid.Events().PreviewMouseLeftButtonDown
                .Subscribe(VM.OnMouseLeftButtonDown);

            UsersList_DataGrid.Events().MouseLeftButtonUp
                .Subscribe(x => 
                {
                    VM.OnMouseLeftButtonUp(x);
                    UsersList_DataGrid.SelectedIndex = -1;
                });

            VM.SubscribeToSortDirectionsClear(UsersList_DataGrid);

            UsersList_DataGrid.SelectedIndex = -1;
        }
    }
}
