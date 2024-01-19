using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortuneWheel.Domain
{
    public class WheelOfFortune
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public WheelType WheelType { get; set; }
        public List<WheelSegment> Segments { get; set; }
    }

}
