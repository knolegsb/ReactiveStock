using Akka.Actor;
using GalaSoft.MvvmLight;
using OxyPlot;
using System.Collections.Generic;
using System;
using OxyPlot.Axes;
using ReactiveStock.ActorModel;
using ReactiveStock.ActorModel.Actors;

namespace ReactiveStock.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private IActorRef _chartingActorRef;
        private IActorRef _stockCoordinatorActorRef;
        private PlotModel _plotModel;

        public Dictionary<string, StockToggleButtonViewModel> StockButtonViewModels { get; set; }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { Set(() => PlotModel, ref _plotModel, value); }
        }

        public MainWindowViewModel()
        {
            SetUpChartModel();
            InitializeActors();
            CreateStockButtonViewModels();
        }

        private void SetUpChartModel()
        {
            _plotModel = new PlotModel
            {
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black
            };

            var stockDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Date",
                StringFormat = "HH:mm:ss"
            };

            _plotModel.Axes.Add(stockDateTimeAxis);

            var stockPriceAxis = new LinearAxis
            {
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Price"
            };

            _plotModel.Axes.Add(stockPriceAxis);
        }

        private void InitializeActors()
        {
            _chartingActorRef = ActorSystemReferences.ActorSystem.ActorOf(Props.Create(() => 
                                                new StockCoordinatorActor(_chartingActorRef)), "StockCoordinator");
        }

        private void CreateStockButtonViewModels()
        {
            StockButtonViewModels = new Dictionary<string, StockToggleButtonViewModel>();

            CreateStockButtonViewModel("AAAA");
            CreateStockButtonViewModel("BBBB");
            CreateStockButtonViewModel("CCCC");
        }

        private void CreateStockButtonViewModel(string stockSymbol)
        {
            var newViewModel = new StockToggleButtonViewModel(_stockCoordinatorActorRef, stockSymbol);
            StockButtonViewModels.Add(stockSymbol, newViewModel);
        }
    }
}