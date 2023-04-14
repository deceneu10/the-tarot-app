using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using kha2dev.DatePicker;
using System;
using System.Globalization;

public class DemoCalendarDatePicker : MonoBehaviour
{
    [SerializeField] TMP_Text textResult;
    [SerializeField] CalendarDatePicker calendarDatePicker;

    private void Start()
    {


    }

    public void OpenDatePicker_Normal()
    {
        calendarDatePicker.Show(CallbackDatePicker);
    }

    private void OpenDatePicker_Limited()
    {        
        List<DateTime>disableDate = new List<DateTime>();
        disableDate.Add(new DateTime(2022, 3, 15));
        disableDate.Add(new DateTime(2022, 4, 4));
        disableDate.Add(new DateTime(2022, 4, 21));

        ParameterDatePicker dateLimited = new ParameterDatePicker()
        {
            defaultDate = new DateTime(2022, 3, 1),
            startDateFrom = new DateTime(2022, 2, 20),
            finishDateUntil = new DateTime(2022, 5, 20),
            listDisableDate = disableDate,
            disableFriday = true,
            disableSunday = true
        };

        calendarDatePicker.Show(CallbackDatePicker, dateLimited);
    }

    private void CallbackDatePicker(DateTime result)
    {
        textResult.text = "Result: " + result.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("ro"));
    }
}
