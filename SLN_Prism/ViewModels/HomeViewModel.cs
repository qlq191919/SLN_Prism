using Prism.Mvvm;
using SLN_Prism.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.ViewModels
{
    public class HomeViewModel:BindableBase
    {
        public HomeViewModel()
        {
            Alarms = new Queue<Alarms>();
            Alarms.Enqueue(new Alarms() { level ="停机故障", address="机构1", message = "Alarmsaddadasda 1", time = DateTime.Now.AddMinutes(0) });
            Alarms.Enqueue(new Alarms() { level ="警告", address = "机构2", message = "Alarm 2daadaddada", time = DateTime.Now.AddMinutes(1) });
            Alarms.Enqueue(new Alarms() { level = "信息", address = "机构3", message = "Alarm 3ads adsa2q", time = DateTime.Now.AddMinutes(14) });
            
        }
        public Queue<Alarms> Alarms { get; set; }
    }
}
