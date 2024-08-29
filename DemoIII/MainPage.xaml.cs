using System.Diagnostics;

namespace DemoIII;

public partial class MainPage : ContentPage
{

	IBleService _bleService = new BleService();
	// IBleService _bleService = new FakeBleService();
	public MainPage()
	{
		InitializeComponent();

  		_bleService.HeartRateValue += async (sender, heartRate) =>
		{
			MainThread.BeginInvokeOnMainThread(() => { hybridWebView.SendRawMessage($"{heartRate}"); });
			Debug.WriteLine(heartRate);
		};
	}
	private void OnSendMessageButtonClicked(object sender, EventArgs e)
	{
		hybridWebView.SendRawMessage($"Hello from C#!");
	}

	private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
	{
		await DisplayAlert("Raw Message Received", e.Message, "OK");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		ActivityIndicator.IsRunning = true;
		ActivityIndicator.IsVisible = true;
		await _bleService.ConnectToPolar();
		ActivityIndicator.IsRunning = false;
		ActivityIndicator.IsVisible = false;
	}
}

