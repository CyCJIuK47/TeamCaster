using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeamCaster.Core.Interactivity;
using TeamCaster.Core.Interactivity.IO;
using TeamCaster.Core.Network;
using TeamCaster.MVVM.Commands;
using TeamCaster.MVVM.ViewModels.Base;

namespace TeamCaster.MVVM.ViewModels
{
    class InteractivityViewModel : ViewModel
    {
        private ActionRecorder _actionRecorder;

        public MouseSimulatorViewModel MouseSimulatorViewModel { get; set; } = new MouseSimulatorViewModel();

        public RelayCommand LeftDoubleClick { get; set; }

        public RelayCommand LeftClick { get; set; }

        public RelayCommand RightClick { get; set; }

        public InteractivityViewModel(INetworkDataSender networkDataSender)
        {
            _actionRecorder = new ActionRecorder(networkDataSender);

            LeftClick = new RelayCommand(
                (object sender) =>
                {
                    Canvas targetControl = (Canvas)sender;
                    Point position = Mouse.GetPosition(targetControl);

                    _actionRecorder.RecordEvent(position.X / targetControl.ActualWidth,
                                                position.Y / targetControl.ActualHeight,
                                                MouseEventType.LeftButtonClick);

                }
                );

            RightClick = new RelayCommand(
                (object sender) =>
                {
                    Canvas targetControl = (Canvas)sender;
                    Point position = Mouse.GetPosition(targetControl);

                    _actionRecorder.RecordEvent(position.X / targetControl.ActualWidth,
                                                position.Y / targetControl.ActualHeight,
                                                MouseEventType.RightButtonClick);

                }
                );

        }

    }
}
