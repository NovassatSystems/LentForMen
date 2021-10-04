using System.Windows.Input;
using Xamarin.Forms;

namespace Core
{
    public partial class CustomButtonControl : ContentView
    {
        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(CustomButtonControl), propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is CustomButtonControl self && newValue is ICommand command)
                    self.button.Command = command;
            });

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter), typeof(object), typeof(CustomButtonControl), propertyChanged: (bindable, oldValue, newValue) =>
             {
                 if (bindable is CustomButtonControl self)
                     self.button.CommandParameter = newValue;
             });

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        #endregion

        #region Text
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
           nameof(Text), typeof(string), typeof(CustomButtonControl), default(string), propertyChanged: (bindable, oldValue, newValue) =>
           {
               if (bindable is CustomButtonControl self && newValue is string text)
                   self.button.Text = text.ToUpper();
           });

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        double _buttonHeight;
        public double ButtonHeight
        {
            get => button.HeightRequest;
            set => button.HeightRequest = value;
        }

        public CustomButtonControl()
        {
            InitializeComponent();
        }
    }
}
