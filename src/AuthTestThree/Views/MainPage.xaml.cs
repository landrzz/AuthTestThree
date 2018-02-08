using System.Threading.Tasks;

using AuthTestThree.Models;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;

namespace AuthTestThree.Views
{
    public partial class MainPage : ContentPage
    {
        TodoItemManager manager;

        public MainPage()
        {
            InitializeComponent();


          
            manager = TodoItemManager.DefaultManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set syncItems to true in order to synchronize the data on startup when running in offline mode
            //await RefreshItems(true, syncItems: false);
            TodoItem item = new TodoItem {  Id = "1234", Name = "AlexLanders" };
            await AddItem(item);
        }

        // Data methods
        async Task AddItem(TodoItem item)
        {
            await manager.SaveTaskAsync(item);
            //todoList.ItemsSource = await manager.GetTodoItemsAsync();
        }
    }
}
