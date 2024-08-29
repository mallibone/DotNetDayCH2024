namespace MauiFitnessApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public void OnRefreshButtonClicked(object sender, EventArgs e)
	{
		// Refresh the list of exercises
		WebView.Reload();
	}
}

