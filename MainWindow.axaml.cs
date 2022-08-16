using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    public partial class MainWindow : Window
    {
        private QuickCollectionWithIList _qc = new();
        private QuickCollectionWithoutIList _qc2 = new();
        

        public MainWindow()
        {
            InitializeComponent();
            Task.Run(WrapperTask);
        }

        public async Task WrapperTask()
        {
            try
            {
                await TestStuffAsync();
            }
            catch(ArgumentException aex)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    ExampleTextBlock.Text = $"Argument Exception: {aex}";
                });
            }
        }

        public async Task TestStuffAsync()
        {
            Random r = new Random();
            await Task.Delay(1000);
            // can't access styled properties outside of Dispatcher UI thread
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                DataContext = _qc;
            });

            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                if (r.Next(4) == 0 && _qc.Count > 0)
                {
                    int index = r.Next(0, _qc.Count);
                    _qc.RemoveAt(index);
                }
                else
                {
                    _qc.AddItem(new QuickModel()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Description = Guid.NewGuid().ToString()
                    });
                }
            }

            while (_qc.Count > 0) 
            {
                await Task.Delay(500);
                int index = r.Next(0, _qc.Count);
                _qc.RemoveAt(index);

            }

            // can't access styled properties outside of Dispatcher UI thread
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                DataContext = _qc2;
            });

            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                if (r.Next(4) == 0 && _qc2.Count > 0)
                {
                    int index = r.Next(0, _qc2.Count);
                    var item = _qc2[index];
                    _qc2.RemoveItem(item);
                }
                else
                {
                    _qc2.AddItem(new QuickModel()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Description = Guid.NewGuid().ToString()
                    });
                }
            }

            while (_qc2.Count > 0)
            {
                await Task.Delay(500);
                int index = r.Next(0, _qc2.Count);
                var item = _qc2[index];
                _qc2.RemoveItem(item);
            }
        }

        private void MainWindow_DataContextChanged(object? sender, EventArgs e)
        {
            
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.DataContext == _qc)
            {
                _qc.AddItem(new QuickModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString()
                });
            }
            if (this.DataContext == _qc2)
            {
                _qc2.AddItem(new QuickModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString()
                });
            }
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(sender is Button);
            var b = sender as Button;
            Debug.Assert(b is not null);

            if (DataContext == _qc)
                _qc.Remove(b.DataContext);
            else if (DataContext == _qc2 && b.DataContext is QuickModel qm)
                _qc2.RemoveItem(qm);
        }
    }
}
