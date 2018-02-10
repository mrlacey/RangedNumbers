using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using RangedNumbers;

namespace MvvmDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get;}

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel = new MainViewModel();
        }

        private void IncreaseClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.Value ++;
        }

        private void DecreaseClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Value > 0)
            {
                ViewModel.Value -= 1;
            }
        }

        private void RealWorldClicked(object sender, RoutedEventArgs e)
        {
            // TODO: Need to cause this to call RaisePropertyChanged - will it require that SetValue actually returns a new instance and constructors ahve boundary checks?
            ViewModel.Value.SetValue(-1);
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private RangedInt _value;

        public MainViewModel()
        {
            Value = (0, 0, 10);
        }

        public RangedInt Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
