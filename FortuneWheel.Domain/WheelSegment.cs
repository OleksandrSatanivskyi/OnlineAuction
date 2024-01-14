using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortuneWheel.Domain
{
    public class WheelSegment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float Points { get; set; }
        public string HexColor { get; set; }
    }

}
