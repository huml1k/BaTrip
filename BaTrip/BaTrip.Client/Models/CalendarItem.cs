using BaTrip.Client.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using DayOfWeek = BaTrip.Client.Enums.DayOfWeek;

namespace BaTrip.Client.Models;

public class CalendarItem
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int DaysOfMonth { get; set; }
    public CalendarDay SelectedDay { get; set; }

    public CalendarItem()
    {
        Year = DateTime.Today.Year;
        Month = DateTime.Today.Month;
        DaysOfMonth = DateTime.DaysInMonth(Year, Month);
        SelectedDay = new CalendarDay();
    }

    public CalendarItem(int year, int month)
    {
        Month = month;
        Year = year;
        DaysOfMonth = DateTime.DaysInMonth(year, month);
        SelectedDay = new CalendarDay();
    }

}

public partial class CalendarDay : ObservableObject
{
    [ObservableProperty] 
    private int _number;
    [ObservableProperty] 
    private string _currentDay = string.Empty;
    [ObservableProperty] 
    private DayOfWeek _dayOfWeek;
    [ObservableProperty] 
    private MonthOfYear _monthOfYear;
    [ObservableProperty] 
    private bool _isHaveNotes;
    [ObservableProperty] 
    private string _notes = string.Empty;

    [ObservableProperty] 
    private bool _isCurrentMonth;
    [ObservableProperty] 
    private bool _isToday;
    [ObservableProperty] 
    private bool _isSelected;
    [ObservableProperty] 
    private bool _isClickable;

    public CalendarDay() { }

    public CalendarDay(DateTime date, bool isCurrentMonth)
    {
        UpdateFromDate(date, isCurrentMonth);
    }

    public void UpdateFromDate(DateTime date, bool isCurrentMonth)
    {
        Number = date.Day;
        CurrentDay = date.ToString("D");
        IsCurrentMonth = isCurrentMonth;
        IsToday = date.Date == DateTime.Today;
        IsClickable = isCurrentMonth;

        DayOfWeek = ConvertDayOfWeek(date.DayOfWeek);
        MonthOfYear = (MonthOfYear)date.Month;
    }

    private DayOfWeek ConvertDayOfWeek(System.DayOfWeek systemDay) => systemDay switch
    {
        System.DayOfWeek.Monday => DayOfWeek.Понедельник,
        System.DayOfWeek.Tuesday => DayOfWeek.Вторник,
        System.DayOfWeek.Wednesday => DayOfWeek.Среда,
        System.DayOfWeek.Thursday => DayOfWeek.Четверг,
        System.DayOfWeek.Friday => DayOfWeek.Пятница,
        System.DayOfWeek.Saturday => DayOfWeek.Суббота,
        System.DayOfWeek.Sunday => DayOfWeek.Воскресенье,
        _ => DayOfWeek.Воскресенье
    };
}
