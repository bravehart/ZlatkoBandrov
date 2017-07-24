using System;
using System.Collections.Generic;
using System.Linq;
using ZlatkoBandrov.Entities.Models;

namespace ZlatkoBandrov.BusinessLogic.Extensions.EmployeeTableListExtensions
{
    public static class EmployeeTableListExtensions
    {
        public static List<EmployeeTableItem> WithFilter(this List<EmployeeTableItem> list, SearchWithOrderModel filter)
        {
            if (list == null || filter == null)
            {
                return list;
            }

            string[] searchTerms = filter.SearhTerms.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            list = list
                .Where(p =>
                    searchTerms.Any(term => p.EmployeeID.ToString().Contains(term))
                    || searchTerms.Any(term => p.Age.ToString().Contains(term))
                    || searchTerms.Any(term => !string.IsNullOrEmpty(p.Email) && p.Email.Contains(term))
                    || searchTerms.Any(term => !string.IsNullOrEmpty(p.FullName) && p.FullName.Contains(term))
                    || searchTerms.Any(term => !string.IsNullOrEmpty(p.Role) && p.Role.Contains(term))
                    || searchTerms.Any(term => p.Teams != null && p.Teams.Contains(term))
                    )
                .ToList();

            return list;
        }

        public static List<EmployeeTableItem> WithOrder(this List<EmployeeTableItem> list, SearchWithOrderModel orderData)
        {
            if (list == null || orderData == null)
            {
                return list;
            }
            // Make list sort by direction
            if (orderData.SortDirection.Equals("ASC", System.StringComparison.InvariantCultureIgnoreCase))
            {
                list = list.OrderBy(p => p.GetType().GetProperty(orderData.OrderBy).GetValue(p, null)).ToList();
            }
            else
            {
                list = list.OrderByDescending(p => p.GetType().GetProperty(orderData.OrderBy).GetValue(p, null)).ToList();
            }
            return list;
        }
    }
}
