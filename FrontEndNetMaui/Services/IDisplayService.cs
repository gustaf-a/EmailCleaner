using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndNetMaui.Services
{
    public interface IDisplayService
    {
        public void DisplayAlert(string title, string message, string cancel);
        public void DisplayAlert(string title, string message, string cancel, string accept);
    }
}
