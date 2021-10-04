using Xamarin.Forms;

namespace Core
{
    public partial class NavigationBarControl : Grid
    {
        public string Title
        {
            get => title.Text;
            set => title.Text = value;
        }

        public string Icon
        {
            get => icon.ResourceId;
            set => icon.ResourceId = value;
        }

        public NavigationBarControl()
        {
            InitializeComponent();
        }
    }
}
