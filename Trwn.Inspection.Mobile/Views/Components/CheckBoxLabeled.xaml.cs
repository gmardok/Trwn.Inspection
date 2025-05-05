namespace Trwn.Inspection.Mobile.Views.Components;

public partial class CheckBoxLabeled : ContentView
{
	public CheckBoxLabeled()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(CheckBoxLabeled), string.Empty);

    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBoxLabeled), false);

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    private void label_Click(object sender, TappedEventArgs args)
    {
        IsChecked = !IsChecked;
    }
}