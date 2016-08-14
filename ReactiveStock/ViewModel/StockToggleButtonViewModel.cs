using Akka.Actor;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors.UI;
using ReactiveStock.ActorModel.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReactiveStock.ViewModel
{
    public class StockToggleButtonViewModel : ViewModelBase
    {
        private string _buttonText;
        public string StockSymbol { get; set; }
        public ICommand ToggleCommand { get; set; }
        public IActorRef StockToggleButtonActorRef { get; private set; }
        public string ButtonText
        {
            get { return _buttonText; }
            set { Set(() => ButtonText, ref _buttonText, value); }
        }

        public StockToggleButtonViewModel(IActorRef stockCoordinatorRef, string stockSymbol)
        {
            StockSymbol = stockSymbol;
            StockToggleButtonActorRef = ActorSystemReferences.ActorSystem.ActorOf(Props.Create(() => 
                                        new StockToggleButtonActor(StockToggleButtonActorRef, this, stockSymbol)));

            ToggleCommand = new RelayCommand(() => StockToggleButtonActorRef.Tell(new FlipToggleMessage()));
            UpdateButtonTextToOff();
        }

        public void UpdateButtonTextToOff()
        {
            ButtonText = ConstructButtonText(false);
        }

        public void UpdateButtonTextToOn()
        {
            ButtonText = ConstructButtonText(true);
        }

        private string ConstructButtonText(bool isToggledOn)
        {
            return string.Format("{0} {1}", StockSymbol, isToggledOn ? "(0)" : "(off)");
        }
    }
}
