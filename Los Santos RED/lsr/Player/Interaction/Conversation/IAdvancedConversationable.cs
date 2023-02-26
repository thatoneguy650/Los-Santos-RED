using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IAdvancedConversationable
{
    PedExt ConversingPed { get; }
    void OnAdvancedConversationStopped();
    void PedReply(string v);
    void SaySpeech(SpeechData speechData, UIMenu menuToActivate);
    void TransitionToTransaction();
}

