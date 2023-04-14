using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

namespace kha2dev.DatePicker
{
    public class CalendarDatePicker : MonoBehaviour
    {
        [Header("Base Component")]
        [SerializeField] string cultureID = "en-US";
        [SerializeField] Color enableColor;
        [SerializeField] Color disableColor;
        [SerializeField] GameObject objectCalendarDatePicker;
        [SerializeField] Button titleButton;
        [SerializeField] TMP_Text titleText;
        [SerializeField] Button prevButton;
        [SerializeField] Button nextButton;

        [Header("Day Component")]
        [SerializeField] GameObject dayHolderObject;
        [SerializeField] GameObject dayObjectPrefab;
        [SerializeField] Transform parentListDayPicker;
        List<GameObject> listDayObject = new List<GameObject>();

        [Header("MonthYear Component")]
        [SerializeField] GameObject monthHolderObject;
        [SerializeField] GameObject yearHolderObject;
        [SerializeField] GameObject monthYearObjectPrefab;
        List<GameObject> listMonthYearObject = new List<GameObject>();
        
        public ParameterDatePicker param = new ParameterDatePicker();        
        public static string CultureID;
        public static Color EnableColor;
        public static Color DisableColor;

        bool isDoneInitialize = false;

        private void Initialize()
        {
            if (cultureID == "") cultureID = "en-US";
            CultureID = cultureID;
            EnableColor = enableColor;
            DisableColor = disableColor;

            isDoneInitialize = true;
        }

        /// <summary>
        /// Function to open CalendarDatePicker
        /// </summary>
        public void Show(Action<DateTime> callback = null, ParameterDatePicker param = null)
        {
            if (!isDoneInitialize) Initialize();
            if (param == null) param = new ParameterDatePicker();

            this.param = param;

            objectCalendarDatePicker.SetActive(true);

            dayHolderObject.SetActive(false);
            monthHolderObject.SetActive(false);
            yearHolderObject.SetActive(false);
            
            GetDay(callback);      
        }

        private void GetDay(Action<DateTime> callback = null)
        {
            dayHolderObject.SetActive(true);
            titleButton.onClick.RemoveAllListeners();
            titleButton.onClick.AddListener(()=> 
            {
                dayHolderObject.SetActive(false);
                GetMonth((dateTime)=>
                {
                    param.defaultDate = dateTime;
                    GetDay(callback);

                });
            });

            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddMonths(-1);
                GetDay(callback);
            });

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddMonths(1);
                GetDay(callback);
            });

            titleText.text = param.defaultDate.ToString("MMMM yyyy", new CultureInfo(CultureID));

            for (int i = 0; i < listDayObject.Count; i++)
            {
                Destroy(listDayObject[i]);
            }
            listDayObject.Clear();
            listDayObject = new List<GameObject>();

            for (int i = 0; i < DateTime.DaysInMonth(param.defaultDate.Year, param.defaultDate.Month); i++)
            {
                DateTime newDay = new DateTime(param.defaultDate.Year, param.defaultDate.Month, i + 1);

                if (i == 0)
                {
                    for (int j = 0; j < (int)newDay.DayOfWeek; j++)
                    {
                        GameObject newBlankObjectDay = Instantiate(dayObjectPrefab, parentListDayPicker);
                        newBlankObjectDay.GetComponent<DayDateMonoScript>().CreateDayDate();
                        listDayObject.Add(newBlankObjectDay);
                    }
                }
                
                GameObject newObjectDay = Instantiate(dayObjectPrefab, parentListDayPicker);
                
                bool isEnable = param.startDateFrom <= newDay &&
                                param.finishDateUntil >= newDay &&
                                !param.listDisableDate.Exists(disable => disable == newDay) &&
                                !(param.disableSunday && newDay.DayOfWeek == DayOfWeek.Sunday) &&
                                !(param.disableMonday && newDay.DayOfWeek == DayOfWeek.Monday) &&
                                !(param.disableTuesday && newDay.DayOfWeek == DayOfWeek.Tuesday) &&
                                !(param.disableWednesday && newDay.DayOfWeek == DayOfWeek.Wednesday) &&
                                !(param.disableThursday && newDay.DayOfWeek == DayOfWeek.Thursday) &&
                                !(param.disableFriday && newDay.DayOfWeek == DayOfWeek.Friday) &&
                                !(param.disableSaturday && newDay.DayOfWeek == DayOfWeek.Saturday);
                                
                newObjectDay.GetComponent<DayDateMonoScript>().CreateDayDate(newDay, isEnable,
                (date)=>
                {
                    
                    for (int j = 0; j < listDayObject.Count; j++)
                    {
                        Destroy(listDayObject[j]);
                    }
                    listDayObject.Clear();
                    listDayObject = new List<GameObject>();

                    callback?.Invoke(date);
                    Hide();
                });
                listDayObject.Add(newObjectDay);
            }
            
        }

        private void GetMonth(Action<DateTime> callback = null)
        {
            monthHolderObject.SetActive(true);
            titleButton.onClick.RemoveAllListeners();
            titleButton.onClick.AddListener(()=>
            {
                monthHolderObject.SetActive(false);
                GetYear((dateTime)=>
                {
                    param.defaultDate = dateTime;
                    GetMonth(callback);
                });
            });

            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddYears(-1);
                GetMonth(callback);
            });

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddYears(1);
                GetMonth(callback);
            });

            titleText.text = param.defaultDate.ToString("yyyy", new CultureInfo(CultureID));

            for (int i = 0; i < listMonthYearObject.Count; i++)
            {
                Destroy(listMonthYearObject[i]);
            }
            listMonthYearObject.Clear();
            listMonthYearObject = new List<GameObject>();
            
            for (int i = 0; i < 12; i++)
            {
                
                DateTime newMonth = new DateTime(param.defaultDate.Year, i + 1, 1);
                GameObject newMonthObject = Instantiate(monthYearObjectPrefab, monthHolderObject.transform);
                newMonthObject.GetComponent<MonthYearMonoScript>().CreateMonth(newMonth,
                (month)=>
                {                    
                    for (int j = 0; j < listMonthYearObject.Count; j++)
                    {
                        Destroy(listMonthYearObject[j]);
                    }
                    listMonthYearObject.Clear();
                    listMonthYearObject = new List<GameObject>();

                    monthHolderObject.SetActive(false);

                    callback?.Invoke(month);
                });
    
                listMonthYearObject.Add(newMonthObject);
            }
        }

        private void GetYear(Action<DateTime> callback = null)
        {
            yearHolderObject.SetActive(true);
            titleButton.onClick.RemoveAllListeners();

            int startFromYear = param.defaultDate.Year - (param.defaultDate.Year % 10);
            int endToYear = startFromYear + 10;

            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddYears(-10);
                GetYear(callback);
            });

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(()=>
            {
                param.defaultDate = param.defaultDate.AddYears(10);
                GetYear(callback);
            });

            titleText.text = string.Format("{0} - {1}", startFromYear, endToYear - 1);

            for (int i = 0; i < listMonthYearObject.Count; i++)
            {
                Destroy(listMonthYearObject[i]);
            }
            listMonthYearObject.Clear();
            listMonthYearObject = new List<GameObject>();
            
            for (int i = startFromYear; i < endToYear; i++)
            {                
                DateTime newYear = new DateTime(i, 1, 1);
                GameObject newMonthObject = Instantiate(monthYearObjectPrefab, yearHolderObject.transform);
                newMonthObject.GetComponent<MonthYearMonoScript>().CreateYear(newYear,
                (year)=>
                {                    
                    for (int j = 0; j < listMonthYearObject.Count; j++)
                    {
                        Destroy(listMonthYearObject[j]);
                    }
                    listMonthYearObject.Clear();
                    listMonthYearObject = new List<GameObject>();

                    yearHolderObject.SetActive(false);

                    callback?.Invoke(year);
                });
    
                listMonthYearObject.Add(newMonthObject);
            }
        }

        /// <summary>
        /// Function to Hide CalendarDatePicker
        /// </summary>
        public void Hide()
        {
            param = new ParameterDatePicker();
            objectCalendarDatePicker.SetActive(false);
        }
    }

    public class ParameterDatePicker
    {
        /// <summary>
        /// the month that will appear when CalendarDatePicker is opened for the first time
        /// </summary>
        public DateTime defaultDate = DateTime.Today;

        /// <summary>
        /// if startDateFrom is filled then, the date before startDateFrom will be disabled or unselectable
        /// </summary>
        public DateTime startDateFrom = DateTime.MinValue;

        /// <summary>
        /// if finishDateUntil is filled then, the date after finishDateUntil will be disabled or unselectable
        /// </summary>
        public DateTime finishDateUntil = DateTime.MaxValue;

        /// <summary>
        /// a list of dates that will be disabled or unselectable
        /// </summary>
        public List<DateTime> listDisableDate = new List<DateTime>();

        /// <summary>
        /// if disableSunday is set to true, every Sunday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableSunday = false;

        /// <summary>
        /// if disableMonday is set to true, every Monday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableMonday = false;

        /// <summary>
        /// if disableTuesday is set to true, every Tuesday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableTuesday = false;

        /// <summary>
        /// if disableWednesday is set to true, every Wednesday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableWednesday = false;

        /// <summary>
        /// if disableThursday is set to true, every Thursday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableThursday = false;

        /// <summary>
        /// if disableFriday is set to true, every Friday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableFriday = false;

        /// <summary>
        /// if disableSaturday is set to true, every Saturday will be disabled or unselectable, if it is not set, it will still be selectable
        /// </summary>
        public bool disableSaturday = false;
    }
}
