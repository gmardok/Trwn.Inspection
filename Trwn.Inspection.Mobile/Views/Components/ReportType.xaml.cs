using CommunityToolkit.Mvvm.ComponentModel;
using Trwn.Inspection.Models;

namespace Trwn.Inspection.Mobile.Views.Components;

public partial class ReportType : ContentView
{
    private bool _finalChecked = true;

    private bool _reChecked;

    private bool _duringChecked;

    public static readonly Dictionary<InspectionType, string> InspectionTypeLabels = new()
    {
        { InspectionType.Final, "FINAL RANDOM INSPECTION" },
        { InspectionType.ReInspection, "RE-INSPECTION" },
        { InspectionType.DuringProduction, "DURING PRODUCTION INSPECTION" }
    };

    public string SelectedReportTypeLabel => InspectionTypeLabels[Value];

    public ReportType()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(InspectionType),
        typeof(ReportType),
        InspectionType.Final,
        propertyChanged: OnValueChanged);

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ReportType)bindable;
        control.UpdateCheckedStates((InspectionType)newValue);
        control.OnPropertyChanged(nameof(SelectedReportTypeLabel));
    }

    private void UpdateCheckedStates(InspectionType value)
    {
        _finalChecked = value == InspectionType.Final;
        _reChecked = value == InspectionType.ReInspection;
        _duringChecked = value == InspectionType.DuringProduction;
        OnPropertyChanged(nameof(FinalChecked));
        OnPropertyChanged(nameof(ReChecked));
        OnPropertyChanged(nameof(DuringChecked));
    }

    public InspectionType Value
    {
        get => (InspectionType)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public bool FinalChecked
    {
        get => _finalChecked;
        set
        {
            if (!value || _finalChecked) return;
            Value = InspectionType.Final;
        }
    }

    public bool ReChecked
    {
        get => _reChecked;
        set
        {
            if (!value || _reChecked) return;
            Value = InspectionType.ReInspection;
        }
    }

    public bool DuringChecked
    {
        get => _duringChecked;
        set
        {
            if (!value || _duringChecked) return;
            Value = InspectionType.DuringProduction;
        }
    }
}