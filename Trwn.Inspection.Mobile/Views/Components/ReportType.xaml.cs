using CommunityToolkit.Mvvm.ComponentModel;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.Views.Components;

public partial class ReportType : ContentView
{
    private bool _finalChecked = true;

    private bool _reChecked;

    private bool _duringChecked;

    public ReportType()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(InspectionType), typeof(ReportType), InspectionType.Final);

    public InspectionType Value
    {
        get => (InspectionType)GetValue(ValueProperty);
        set
        {
            _finalChecked = value == InspectionType.Final;
            _reChecked = value == InspectionType.ReInspection;
            _duringChecked = value == InspectionType.DuringProduction;
            SetValue(ValueProperty, value);
        }
    }

    public bool FinalChecked
    {
        get => _finalChecked;
        set
        {
            if (!value || _finalChecked) return;
            Value = InspectionType.Final;
            OnPropertyChanged(nameof(FinalChecked));
            OnPropertyChanged(nameof(ReChecked));
            OnPropertyChanged(nameof(DuringChecked));
        }
    }

    public bool ReChecked
    {
        get => _reChecked;
        set
        {
            if (!value || _reChecked) return;
            Value = InspectionType.ReInspection;
            OnPropertyChanged(nameof(FinalChecked));
            OnPropertyChanged(nameof(ReChecked));
            OnPropertyChanged(nameof(DuringChecked));
        }
    }

    public bool DuringChecked
    {
        get => _duringChecked;
        set
        {
            if (!value || _duringChecked) return;
            Value = InspectionType.DuringProduction;
            OnPropertyChanged(nameof(FinalChecked));
            OnPropertyChanged(nameof(ReChecked));
            OnPropertyChanged(nameof(DuringChecked));
        }
    }
}