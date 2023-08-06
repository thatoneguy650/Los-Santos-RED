using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LicenseQuestion
{
    public LicenseQuestion()
    {
    }

    public LicenseQuestion(string text, int order, List<LicenseAnswer> possibleAnswers)
    {
        Text = text;
        Order = order;
        PossibleAnswers = possibleAnswers;
    }

    public string Text { get; set; }
    public int Order { get; set; }
    public List<LicenseAnswer> PossibleAnswers { get; set; } = new List<LicenseAnswer>();
}

