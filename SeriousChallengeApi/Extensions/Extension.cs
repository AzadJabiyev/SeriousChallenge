using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousChallengeApi.Extensions
{
    public static class Extension
    {
        public static Dictionary<DayOfWeek, DateTime> GetWeekDaysDates(this DateTime date)
        {
            var mondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);

            var weekDaysDates = new Dictionary<DayOfWeek, DateTime>();

            for (int i = 1; i <= 7; i++)
            {
                if (i == 7)
                {
                    weekDaysDates.Add(DayOfWeek.Sunday, mondayOfLastWeek.AddDays(i - 1));
                    continue;
                }

                weekDaysDates.Add((DayOfWeek)i, mondayOfLastWeek.AddDays(i - 1));
            }

            return weekDaysDates;
        }

       
    }
}
