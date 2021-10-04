
using System;
using Xamarin.Forms;

namespace Core
{
    public partial class DialogModalPage : ContentPage
    {
        protected DialogModalPage()
        {
            InitializeComponent();
        }

        public DialogModalPage(string title = "", string description = "", string txtBtnCancel = "", string txtBtnOK = "", Action<bool> action = null) : this()
        {
            lblTitle.Text = title;
            lblDescription.Text = description;
            btnCancel.Text = txtBtnCancel;
            btnOk.Text = txtBtnOK;

            btnOk.CommandParameter = true;
            btnOk.Command = new Command<bool>(action);

            btnCancel.CommandParameter = false;
            btnCancel.Command = new Command<bool>(action);
        }
    }
}