using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Views.TodoItem
{
    public static class SelectListConvenience
    {
        public static readonly SelectListItem[] ImportanceSelectListItems =
        {
            new() {Text = "High", Value = Importance.High.ToString()},
            new() {Text = "Medium", Value = Importance.Medium.ToString()},
            new() {Text = "Low", Value = Importance.Low.ToString()},
        };

        public static readonly SelectListItem[] RankSelectListItems =
        {
            new() { Text = "High", Value = Rank.High.ToString() },
            new() { Text = "Moderate", Value = Rank.Moderate.ToString() },
            new() { Text = "Low", Value = Rank.Low.ToString() },
            new() { Text = "None", Value = Rank.None.ToString() },
        };

        public static List<SelectListItem> UserSelectListItems(this ApplicationDbContext dbContext)
        {
            return dbContext.Users.Select(u => new SelectListItem {Text = u.UserName, Value = u.Id}).ToList();
        }
    }
}