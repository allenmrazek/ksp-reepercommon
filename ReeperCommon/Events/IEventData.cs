using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReeperCommon.Events
{
    public interface IEventData<T>
    {
        void Add(Action<T> evt);
        void Remove(Action<T> evt); 

        void Fire(T value);
    }
}
