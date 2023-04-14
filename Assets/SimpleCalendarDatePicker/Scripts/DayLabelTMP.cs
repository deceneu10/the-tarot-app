using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;

namespace kha2dev.DatePicker
{
    public class DayLabelTMP : MonoBehaviour
    {
        [SerializeField] DayOfWeek day;
        [SerializeField] TMP_Text textLabel;

        void Start()
        {
            int differenceDay = (int) day - (int) DateTime.Now.DayOfWeek;
            DateTime dayDateTime =DateTime.Today.AddDays(differenceDay);
            gameObject.name = dayDateTime.ToString("dddd", new CultureInfo(CalendarDatePicker.CultureID));
            textLabel.text = dayDateTime.ToString("ddd", new CultureInfo(CalendarDatePicker.CultureID));
        }
    }
}
