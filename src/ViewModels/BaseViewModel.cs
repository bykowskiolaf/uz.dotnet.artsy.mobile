using CommunityToolkit.Mvvm.ComponentModel;

namespace artsy.mobile.ViewModels;

public partial class BaseViewModel : ObservableObject
{
	[ObservableProperty] [NotifyPropertyChangedFor(nameof(IsNotBusy))]
	bool _isBusy;

	[ObservableProperty] string _title = string.Empty;

	public bool IsNotBusy => !IsBusy;
}
