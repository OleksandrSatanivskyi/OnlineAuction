using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortuneWheel.Domain.WheelsOfFortune
{
    public abstract class WheelOfFortune
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        protected WheelOfFortune(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public WheelOfFortune() { }
    }

}
