using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using BaTrip.Client.Models;

namespace BaTrip.Client.ViewModels;

public partial class CalendarViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _notesTitle = "Notes";
    [ObservableProperty]
    private string _notesText;

    [ObservableProperty] 
    private string _monthTitle = "Месяц";
    [ObservableProperty] 
    private CalendarDay? _selectedDay;
    [ObservableProperty]
    private bool _isEnabledSaveButton = false;

    [ObservableProperty] 
    private ObservableCollection<CalendarDay> _calendarDays = new();

    // Дни недели для заголовка
    public string[] WeekDayShort => ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];

    private DateTime _currentDate;

    public CalendarViewModel()
    {
        _currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        GenerateCalendar();
    }

    [RelayCommand]
    private void SaveNotes()
    {
        if (NotesTitle != null && NotesText.Trim() != null)
        {
            SelectedDay.Notes = NotesText;
            SelectedDay.IsHaveNotes = true;
        }
        else
        {
            SelectedDay.IsHaveNotes = false;
        }
    }

    [RelayCommand]
    private void PrevMonth()
    {
        _currentDate = _currentDate.AddMonths(-1);
        GenerateCalendar();
    }

    [RelayCommand]
    private void NextMonth()
    {
        _currentDate = _currentDate.AddMonths(1);
        GenerateCalendar();
    }

    [RelayCommand]
    private void SelectDay(object? parameter)
    {
        if (parameter is CalendarDay day && day.IsClickable)
        {
            // Снимаем выделение со всех
            foreach (var d in CalendarDays)
                d.IsSelected = false;

            day.IsSelected = true;
            SelectedDay = day;

            IsEnabledSaveButton = true;

            NotesTitle = SelectedDay.CurrentDay;
            NotesText = SelectedDay.Notes;
        }
    }

    private void GenerateCalendar()
    {
        MonthTitle = _currentDate.ToString("MMMM yyyy");
        CalendarDays.Clear();

        var firstDay = new DateTime(_currentDate.Year, _currentDate.Month, 1);
        var startDay = GetMondayOfWeek(firstDay);
        var today = DateTime.Today;

        // (6 недель × 7 дней)
        for (int i = 0; i < 42; i++)
        {
            var date = startDay.AddDays(i);
            var isCurrentMonth = date.Month == _currentDate.Month;

            CalendarDays.Add(new CalendarDay(date, isCurrentMonth)
            {
                IsSelected = date.Date == today.Date && isCurrentMonth
            });
        }
    }

    // Возвращает понедельник недели, в которой находится дата
    private DateTime GetMondayOfWeek(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - System.DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }
}