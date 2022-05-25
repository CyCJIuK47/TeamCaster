using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCaster.Core.Architecture.Containers;
using TeamCaster.Core.Interactivity;
using TeamCaster.Core.Interactivity.IO;
using TeamCaster.MVVM.ViewModels.Base;


namespace TeamCaster.MVVM.ViewModels
{
    class MouseSimulatorViewModel : ViewModel
    {
        private MouseSimulator _mouseSimulator;

        public DataProvider<MouseEvent> MouseEventProvider { get; set; }

        public MouseSimulatorViewModel()
        {
            _mouseSimulator = new MouseSimulator();
            MouseEventProvider = new DataProvider<MouseEvent>();

            MouseEventProvider.DataAvailable += OnMouseEvent;

        }

        private void OnMouseEvent(object sender, MouseEvent mouseEvent)
        {
            _mouseSimulator.SimulateEvent(mouseEvent);
        }

    }
}
