using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Core
{
    public partial class ReflectionDetailPage : ContentPage
    {
        ReflectionDetailViewModel ViewModel => this.BindingContext as ReflectionDetailViewModel;
        public ReflectionDetailPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            ViewModel.OnAppearing();
            base.OnAppearing();
        }

        private async Task OpenAnimation(View view, uint length = 250)
        {
            try
            {
                view.RotationX = -90;
                view.IsVisible = true;
                view.Opacity = 0;
                _ = view.FadeTo(1, length);
                await view.RotateXTo(0, length);
            }
            catch (Exception e)
            {

            }
           
        }

        private async Task CloseAnimation(View view, uint length = 250)
        {
            try
            {
                _ = view.FadeTo(0, length);
                await view.RotateXTo(-90, length);
                view.IsVisible = false;
            }
            catch (Exception e)
            {

            }
          
        }

        private async void reflectionExpander_Tapped(object sender, EventArgs e)
        {
            try
            {
                var expander = sender as Expander;
                var imgView = expander.FindByName<Grid>("ImageView");
                var detailsView = expander.FindByName<Grid>("DetailsView");

                if (expander.IsExpanded)
                {
                    await OpenAnimation(imgView);
                    await OpenAnimation(detailsView);
                }
                else
                {
                    await CloseAnimation(detailsView);
                    await CloseAnimation(imgView);
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        private async void audioExpander_Tapped(object sender, EventArgs e)
        {
            try
            {
                var expander = sender as Expander;
                var audioView = expander.FindByName<Grid>("AudioView");

                if (expander.IsExpanded)
                    await OpenAnimation(audioView);
                else
                    await CloseAnimation(audioView);
            }
            catch (Exception ex)
            {

            }

        }

        private async void workoutExpander_Tapped(object sender, EventArgs e)
        {
            try
            {
                var expander = sender as Expander;
                var workoutView = expander.FindByName<Grid>("WorkoutView");

                if (expander.IsExpanded)
                    await OpenAnimation(workoutView);
                else
                    await CloseAnimation(workoutView);
            }
            catch (Exception ex)
            {

            }

        }

        private async void penanceExpander_Tapped(object sender, EventArgs e)
        {
            try
            {
                var expander = sender as Expander;
                var penanceView = expander.FindByName<Grid>("PenanceView");

                if (expander.IsExpanded)
                    await OpenAnimation(penanceView);
                else
                    await CloseAnimation(penanceView);
            }
            catch (Exception ex)
            {

            }

        }

        private async void commentExpander_Tapped(object sender, EventArgs e)
        {
            try
            {
                var expander = sender as Expander;
                var commentView = expander.FindByName<Grid>("CommentView");

                if (expander.IsExpanded)
                    await OpenAnimation(commentView);
                else
                    await CloseAnimation(commentView);
            }
            catch (Exception ex)
            {

            }

        }

        
    }
}
