using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;

namespace DemoIII;

public partial class MainViewModel : ObservableObject
{
	public MainViewModel()
	{
		_bleService.HeartRateValue += (sender, heartRate) =>
		{
			HeartRate = heartRate;
		};
	}
	public BleService _bleService { get; } = new();

	private int _heartRate;
	public int HeartRate
	{
		get => _heartRate;
		set => SetProperty(ref _heartRate, value);
	}
	private bool _isShowingBleConfig = true;
	public bool IsShowingBleConfig
	{
		get => _isShowingBleConfig;
		set
		{
			if (SetProperty(ref _isShowingBleConfig, value))
			{
				OnPropertyChanged(nameof(IsShowingBleConfig));
			}
		}
	}

	private bool _isShowingHybridWeb = false;
	public bool IsShowingHybridWeb
	{
		get => _isShowingHybridWeb;
		set => SetProperty(ref _isShowingHybridWeb, value);
	}

    [RelayCommand]
	public async Task ScanForDevices()
	{
		await _bleService.ConnectToPolar();
	}
}

