using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace kha2dev.DatePicker
{
    public class DayDateMonoScript : MonoBehaviour
    {
        [SerializeField] Button buttonDay;
        [SerializeField] TMP_Text textDay;
        public DateTime dateTime = new DateTime();

        private bool isEnable
        {
            get
            {
                return buttonDay.interactable;
            }
            set
            {
                buttonDay.interactable = value;
                textDay.color = value ? CalendarDatePicker.EnableColor : CalendarDatePicker.DisableColor;
            }
        }

        /// <summary>
        /// Create Day Date in Calendar to Get Day
        /// </summary>
        public void CreateDayDate(DateTime dateTime = new DateTime(), bool isEnable = false, Action<DateTime> callback = null)
        {
            if (this.dateTime == dateTime)
            {
                this.isEnable = false;
                textDay.text = "";
                return;
            }

            this.dateTime = dateTime;
            this.isEnable = isEnable;

            buttonDay.onClick.AddListener(()=>
            {
                callback?.Invoke(this.dateTime);
            });
            
            textDay.text = dateTime.Day.ToString();
        }
    }
}