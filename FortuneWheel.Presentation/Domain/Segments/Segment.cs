using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune.Domain.Segments
{
    public class Segment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ColorHex { get; set; }
    }

}
