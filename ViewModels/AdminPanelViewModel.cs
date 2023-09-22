using Rater.Models;
namespace Rater.ViewModels
{
    public class AdminPanelViewModel
    {
        public IEnumerable<UserRow> UserRows { get; set; }
        public Dictionary<int, bool> Admins { get; set; }
    }
}
