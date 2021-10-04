using Android.Text;
using Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRederer))]
namespace Droid
{
    public class CustomEntryRederer : EntryRenderer
    {
        public CustomEntryRederer(Android.Content.Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;

            Control.SetBackgroundResource(Android.Resource.Color.Transparent);
            Control.InputType = Control.InputType | InputTypes.TextFlagNoSuggestions;
        }
    }
}
