using System;
using System.Globalization;

namespace SpadStorePanel.Core.Utility
{
    public static class DateFormater
    {
        public static string ConvertToPersian(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            string FormattedDate = String.Empty;

            switch (pc.GetMonth(date))
            {
                case 1:
                    FormattedDate = $"فروردین {pc.GetYear(date)}";
                    break;
                case 2:
                    FormattedDate = $"اردیبهشت {pc.GetYear(date)}";
                    break;
                case 3:
                    FormattedDate = $"خرداد {pc.GetYear(date)}";
                    break;
                case 4:
                    FormattedDate = $"تیر {pc.GetYear(date)}";
                    break;
                case 5:
                    FormattedDate = $"مرداد {pc.GetYear(date)}";
                    break;
                case 6:
                    FormattedDate = $"شهریور {pc.GetYear(date)}";
                    break;
                case 7:
                    FormattedDate = $"مهر {pc.GetYear(date)}";
                    break;
                case 8:
                    FormattedDate = $"آبان {pc.GetYear(date)}";
                    break;
                case 9:
                    FormattedDate = $"آذر {pc.GetYear(date)}";
                    break;
                case 10:
                    FormattedDate = $"دی {pc.GetYear(date)}";
                    break;
                case 11:
                    FormattedDate = $"بهمن {pc.GetYear(date)}";
                    break;
                case 12:
                    FormattedDate = $"اسفند {pc.GetYear(date)}";
                    break;
            }

            return FormattedDate;
        }

        public static string ConvertToPersian_Month(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            string FormattedDate = String.Empty;

            switch (pc.GetMonth(date))
            {
                case 1:
                    FormattedDate = $"فروردین";
                    break;
                case 2:
                    FormattedDate = $"اردیبهشت";
                    break;
                case 3:
                    FormattedDate = $"خرداد";
                    break;
                case 4:
                    FormattedDate = $"تیر";
                    break;
                case 5:
                    FormattedDate = $"مرداد";
                    break;
                case 6:
                    FormattedDate = $"شهریور";
                    break;
                case 7:
                    FormattedDate = $"مهر";
                    break;
                case 8:
                    FormattedDate = $"آبان";
                    break;
                case 9:
                    FormattedDate = $"آذر";
                    break;
                case 10:
                    FormattedDate = $"دی";
                    break;
                case 11:
                    FormattedDate = $"بهمن";
                    break;
                case 12:
                    FormattedDate = $"اسفند";
                    break;
            }

            return FormattedDate;
        }
    }
}
