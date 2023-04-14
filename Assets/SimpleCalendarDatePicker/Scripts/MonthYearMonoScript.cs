using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;


namespace kha2dev.DatePicker
{
    public class MonthYearMonoScript : MonoBehaviour
    {
        [SerializeField] Button buttonMonthYear;
        [SerializeField] TMP_Text textMonthYear;
        public DateTime dateTime = new DateTime();
        

        /// <summary>
        /// Create Month in Calendar to Get Month in Year
        /// </summary>
        public void CreateMonth(DateTime dateTime = new DateTime(), Action<DateTime> callback = null)
        {
            this.dateTime = dateTime;

            buttonMonthYear.onClick.AddListener(()=>
            {
                callback?.Invoke(this.dateTime);
            });
            
            textMonthYear.text = dateTime.ToString("MMMM\nyyyy", new CultureInfo(CalendarDatePicker.CultureID));
        }
        
        /// <summary>
        /// Create Year in Calendar to Get Year
        /// </summary>
        public void CreateYear(DateTime dateTime = new DateTime(), Action<DateTime> callback = null)
        {
            this.dateTime = dateTime;

            buttonMonthYear.onClick.AddListener(()=>
            {
                callback?.Invoke(this.dateTime);
            });
            
            textMonthYear.text = dateTime.ToString("yyyy", new CultureInfo(CalendarDatePicker.CultureID));
        }
    }
}