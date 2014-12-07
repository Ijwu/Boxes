using System;
using System.Collections.Generic;

namespace Boxes.Objective
{
    public delegate void ObjectiveAreaElapsed(object sender, ObjectiveAreaElapsedArgs args);

    public class ObjectiveAreaElapsedArgs : EventArgs
    {
    }
}