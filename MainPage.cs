using Microsoft.Maui.Controls;

namespace artsy.mobile
{
	public class MainPage : ContentPage
	{
		int count = 0;
		Button counterBtn;

		public MainPage()
		{
			var image = new Image
			{
				Source = "dotnet_bot.png",
				HeightRequest = 185,
				Aspect = Aspect.AspectFit
			};
			SemanticProperties.SetDescription(image, "dot net bot in a hovercraft number nine");

			var headline = new Label
			{
				Text = "Hello, World!",
				Style = (Style)Application.Current.Resources["Headline"]
			};
			SemanticProperties.SetHeadingLevel(headline, SemanticHeadingLevel.Level1);

			var subHeadline = new Label
			{
				Text = "Welcome to \n.NET Multi-platform App UI",
				Style = (Style)Application.Current.Resources["SubHeadline"]
			};
			SemanticProperties.SetHeadingLevel(subHeadline, SemanticHeadingLevel.Level2);
			SemanticProperties.SetDescription(subHeadline, "Welcome to dot net Multi platform App U I");

			counterBtn = new Button
			{
				Text = "Click me",
				HorizontalOptions = LayoutOptions.Fill
			};
			SemanticProperties.SetHint(counterBtn, "Counts the number of times you click");
			counterBtn.Clicked += OnCounterClicked;

			var layout = new VerticalStackLayout
			{
				Padding = new Thickness(30, 0),
				Spacing = 25,
				Children = { image, headline, subHeadline, counterBtn }
			};

			Content = new ScrollView { Content = layout };
		}

		private void OnCounterClicked(object? sender, EventArgs e)
		{
			count++;
			counterBtn.Text = $"Clicked {count} times";
		}
	}
}
