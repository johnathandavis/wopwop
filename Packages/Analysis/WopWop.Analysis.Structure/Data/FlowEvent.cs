using Microsoft.CodeAnalysis.Text;

namespace WopWop.Analysis.Structure.Data;

public class FlowEvent
{
    private FlowEvent()
    {
    }

    public Datum Datum { get; private set; }
    public FlowEventType EventType { get; private set; }
    public TextSpan Span { get; private set; }
    public DeclarationDetails Declaration { get; private set; }
    public AssignmentDetails Assignment { get; private set; }
    public RetrievalDetails Retrieval { get; private set; }

    public class DeclarationDetails(string dataType, string valueExpression)
    {
        public string DataType => dataType;
        public string ValueExpression => valueExpression;
    }

    public class RetrievalDetails(string retrieverExpression, string retrievedIntoExpression)
    {
        public string RetrieverExpression => retrieverExpression;
        public string RetrievedIntoExpression => retrievedIntoExpression;
    }
    
    public class AssignmentDetails(string valueExpression)
    {
        public string ValueExpression => valueExpression;
    }

    public override string ToString() => EventType switch
    {
        FlowEventType.Declaration =>
            $"{Datum} DECLARED type = {Declaration.DataType}, value = {Declaration.ValueExpression}",
        FlowEventType.Assignment => $"{Datum} ASSIGNED {Assignment.ValueExpression}",
        FlowEventType.Retrieval =>
            $"{Datum} RETRIEVED by {Retrieval.RetrievedIntoExpression} as {Retrieval.RetrieverExpression}",
    };

    public static FlowEvent NewDeclaration(Datum datum, TextSpan span, string dataType, string valueExpression) => new FlowEvent()
        {
            Datum = datum,
            Span = span,
            EventType = FlowEventType.Declaration,
            Declaration = new DeclarationDetails(dataType, valueExpression)
        };
    
    public static FlowEvent NewAssignment(Datum datum, TextSpan span, string valueExpression) => new FlowEvent()
    {
        Datum = datum,
        Span = span,
        EventType = FlowEventType.Assignment,
        Assignment = new AssignmentDetails(valueExpression)
    };
    
    public static FlowEvent NewRetrieval(Datum datum, TextSpan span, string retrieverExpression, string retrievedIntoExpression) => new FlowEvent()
    {
        Datum = datum,
        Span = span,
        EventType = FlowEventType.Retrieval,
        Retrieval = new RetrievalDetails(retrieverExpression, retrievedIntoExpression)
    };
}