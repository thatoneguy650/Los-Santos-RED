using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface TaskState
{
    string DebugName { get; }
    bool IsValid { get; }
    void Dispose();
    void Update();
    void Stop();
    void Start();
}

