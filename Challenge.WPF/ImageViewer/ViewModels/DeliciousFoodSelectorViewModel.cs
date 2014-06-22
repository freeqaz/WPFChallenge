using Caliburn.Micro;
using MemBus;
using MetroImageViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroImageViewer.ViewModels
{
    /// <summary>
    /// A helpful ViewModel for helping us find something to eat!
    /// </summary>
    public class DeliciousFoodSelectorViewModel : Screen
    {
        private IBus _bus;
        
        private ObservableCollection<string> _sizeItems;
        public ObservableCollection<string> SizeOptions
        {
            get { return _sizeItems; }
            set
            {
                _sizeItems = value;
                NotifyOfPropertyChange(() => SizeOptions);
            }
        }

        private ObservableCollection<string> _tasteOptions;
        public ObservableCollection<string> TasteOptions
        {
            get { return _tasteOptions; }
            set
            {
                _tasteOptions = value;
                NotifyOfPropertyChange(() => TasteOptions);
            }
        }

        private ObservableCollection<string> _textureOptions;
        public ObservableCollection<string> TextureOptions
        {
            get { return _textureOptions; }
            set
            {
                _textureOptions = value;
                NotifyOfPropertyChange(() => TextureOptions);
            }
        }

        private ObservableCollection<string> _hotLevelsOptions;
        public ObservableCollection<string> HotLevelsOptions
        {
            get { return _hotLevelsOptions; }
            set
            {
                _hotLevelsOptions = value;
                NotifyOfPropertyChange(() => HotLevelsOptions);
            }
        }

        private ObservableCollection<string> _hungerOptions;
        public ObservableCollection<string> HungerOptions
        {
            get { return _hungerOptions; }
            set
            {
                _hungerOptions = value;
                NotifyOfPropertyChange(() => HungerOptions);
            }
        }

        private ObservableCollection<string> _foodStyleOptions;
        public ObservableCollection<string> FoodStyleOptions
        {
            get { return _foodStyleOptions; }
            set
            {
                _foodStyleOptions = value;
                NotifyOfPropertyChange(() => FoodStyleOptions);
            }
        }

        private string _selectedSize;
        public string SelectedSize
        {
            get { return _selectedSize; }
            set
            {
                _selectedSize = value;
                NotifyOfPropertyChange(() => SelectedSize);
            }
        }

        private string _selectedTaste;
        public string SelectedTaste
        {
            get { return _selectedTaste; }
            set
            {
                _selectedTaste = value;
                NotifyOfPropertyChange(() => SelectedTaste);
            }
        }

        private string _selectedTexture;
        public string SelectedTexture
        {
            get { return _selectedTexture; }
            set
            {
                _selectedTexture = value;
                NotifyOfPropertyChange(() => SelectedTexture);
            }
        }

        private string _selectedHotLevel;
        public string SelectedHotLevel
        {
            get { return _selectedHotLevel; }
            set
            {
                _selectedHotLevel = value;
                NotifyOfPropertyChange(() => SelectedHotLevel);
            }
        }

        private string _selectedHunger;
        public string SelectedHunger
        {
            get { return _selectedHunger; }
            set
            {
                _selectedHunger = value;
                NotifyOfPropertyChange(() => SelectedHunger);
            }
        }

        private string _selectedFoodStyle;
        public string SelectedFoodStyle
        {
            get { return _selectedFoodStyle; }
            set
            {
                _selectedFoodStyle = value;
                NotifyOfPropertyChange(() => SelectedFoodStyle);
            }
        }

        public DeliciousFoodSelectorViewModel(IBus bus)
        {

            SizeOptions = new BindableCollection<string>()
            {
                "ONE OF A KIND",
                "SMALL BATCH",
                "LARGE BATCH",
                "MASS MARKET"
            };

            TasteOptions = new BindableCollection<string>()
            {
                "SAVORY",
                "SWEET",
                "UMAMI"
            };

            TextureOptions = new BindableCollection<string>()
            {
                "CRUNCHY",
                "MUSHY",
                "SMOOTH"
            };

            HotLevelsOptions = new BindableCollection<string>()
            {
                "SPICY",
                "MILD"
            };

            HungerOptions = new BindableCollection<string>()
            {
                "A LITTLE",
                "A LOT"
            };

            FoodStyleOptions = new BindableCollection<string>()
            {
                "BREAKFAST",
                "BRUNCH",
                "LUNCH",
                "SNACK",
                "DINNER"
            };

            _bus = bus;
        }

        public void GoToNextPage()
        {
            _bus.Publish(new NavigateToPageMessage(NavigateToPageMessage.Pages.NotDeliciousFood));
        }

        /// <summary>
        /// Sets every option to random!
        /// </summary>
        public void RandomizeOptions()
        {
            Random r = new Random();

            SelectedSize = SizeOptions[r.Next(0, SizeOptions.Count)];
            SelectedTaste = TasteOptions[r.Next(0, TasteOptions.Count)];
            SelectedTexture = TextureOptions[r.Next(0, TextureOptions.Count)];
            SelectedHunger = HungerOptions[r.Next(0, HungerOptions.Count)];
            SelectedHotLevel = HotLevelsOptions[r.Next(0, HotLevelsOptions.Count)];
            SelectedFoodStyle = FoodStyleOptions[r.Next(0, FoodStyleOptions.Count)];
        }
    }
}
