using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;

namespace MyEditableComboBox
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region INotifyPropertyChanged Properties
        private ObservableCollection<string> itemList;
        public ObservableCollection<string> ItemList
        {
            get { return itemList; }
            set { SetField(ref this.itemList, value, nameof(ItemList)); }
        }
        private string newItem;
        public string NewItem
        {
            get
            {
                return newItem;
            }
            set
            {
                if (newItem != value)
                {
                    newItem = value;
                    var item = ItemList.SingleOrDefault(x => x == newItem);
                    if (item == null)
                        ItemList.Insert(0,newItem);
                    SelectedItem = newItem;
                }
            }
        }
        private string selectedItem;
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    if (selectedItem == ItemListToXml.DELETE_COMMAND)
                    {
                        ItemList.Clear();
                        ItemList.Add(ItemListToXml.DELETE_COMMAND);
                    }
                    SetField(ref this.selectedItem, value, nameof(SelectedItem));
                }
            }
        }
        #endregion

        public ItemListToXml ItemListToXml { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ItemListToXml = new ItemListToXml();
            ItemList = ItemListToXml.Load(ref selectedItem);
            SelectedItem = selectedItem;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow()
        {
            ItemListToXml.Save(SelectedItem,ItemList);
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        /// <summary>
        /// ComboBox_KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Return)
            {
                string newItemValue = ((System.Windows.Controls.TextBox)e.OriginalSource).Text;
                var item = ItemList.SingleOrDefault(x => x == newItemValue);
                if (item == null)
                    ItemList.Insert(0,newItemValue);
            }
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// SetField
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void OnPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        #endregion
    }
}
