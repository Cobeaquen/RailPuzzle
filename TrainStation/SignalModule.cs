using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainStation
{
    public class SignalModifierModule : PowerModule
    {
        public SignalModifierModule(Cell cell, bool removable = true) : base(cell, removable)
        {
            texture = Textures.ModuleSignalModifier;
        }
    }
}
