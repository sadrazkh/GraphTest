// See https://aka.ms/new-console-template for more information


using System.Runtime.InteropServices;
using Graph;

Console.WriteLine("Hello, World!");

Console.WriteLine("N : ");
var n = int.Parse(Console.ReadLine() ?? string.Empty);

// START Initialize Nodes
var nodes = new List<AppGraph>();
for (int i = 1; i <= n; i++)
{
    nodes.Add(new AppGraph()
    {
        Id = i,
        Value = 0,
        Graphs = new List<AppGraph>(),
        ConnectDirectlyToQ = false,
        ConnectDirectlyToQChecked = false,
    });
}
// END Initialize Nodes

// START Initial Relation Of Nodes
int cleanFlag = 0;
for (int i = 1; i <= n; i++)
{
    var input = Console.ReadLine()!.Split(' ').ToList();

    cleanFlag += 1;

    var mainNode = nodes.First(c => c.Id == i);
    for (int k = i; k < input.Count; k++)
    {
        if (int.Parse(input[k]) == 1)
        {
            var subNode = nodes.First(c => c.Id == k + 1);
            subNode.Graphs.Add(mainNode);
            mainNode.Graphs.Add(subNode);
        }
    }
}

foreach (var node in nodes)
{
    node.Graphs.DistinctBy(c => c.Id);
}

// END Initial Relation Of Nodes

// START Set Value Of Nodes
var values = Console.ReadLine()!.Split(' ').ToList();
for (int i = 0; i < values.Count; i++)
{
    nodes[i].Value = int.Parse(values[i]);
}
// END Set Value Of Nodes

// Which Node Is Q
Console.WriteLine("Enter Id Of Q :");
int q = int.Parse(Console.ReadLine()!);


nodes[q - 1].ConnectDirectlyToQ = true;
nodes[q - 1].ConnectDirectlyToQChecked = true;

// QList Generated
var flag = false;
do
{
    foreach (var node in nodes.Where(c => c.ConnectDirectlyToQ).ToList())
    {

        var nodess = node.Graphs;
        foreach (var appGraph in nodess)
        {
            appGraph.ConnectDirectlyToQ = true;
            appGraph.ConnectDirectlyToQChecked = true;
        }

        var ids = nodess.Where(c => c.ConnectDirectlyToQ).ToList();
        if (ids.All(c => c.ConnectDirectlyToQChecked == true)
            && ids.SelectMany(s => s.Graphs).All(s => s.ConnectDirectlyToQChecked == true))
        {
            flag = true;
        }
    }

} while (!flag);


for (int i = 1; true; i++)
{
    if (i == q)
    {
        continue;
    }

    var node = nodes.First(c => c.Id == i);

    if (node.Value >= node.Graphs.Count)
    {
        var newValue = node.Graphs.Count - 1;
        var decrease = node.Value - newValue;
        node.Value = newValue;

        var subNodes = node.Graphs.ToList();

        do
        {
            var canIncrease = subNodes.Where(c => c.Value < c.Graphs.Count || c.Id == q).ToList();

            if (canIncrease.Any(c => c.Id == q))
            {
                nodes.First(c => c.Id == q).Value += decrease;
                decrease = 0;
            }

            foreach (var graph in canIncrease)
            {
                if (decrease <= 0)
                {
                    break;
                }
                graph.Value += 1;
                decrease -= 1;
            }

            if (canIncrease.Count == 0 && decrease > 0)
            {
                var sacrificerNode = node.Graphs.FirstOrDefault(c => c.ConnectDirectlyToQ);
                sacrificerNode.Value += decrease;

                decrease = 0;

            }
        } while (decrease > 0);


    }

    if (Functions.CheckIsFinished(nodes, q))
    {
        break;
    }
}

foreach (var node in nodes)
{
    Console.WriteLine($"{node.Id} -> {node.Value}");
}

var sameGraph = Functions.IsItTheSame(nodes, q);

if (sameGraph != null && sameGraph.Count > 0)
{
    Console.WriteLine("Same Node Is : ");
    foreach (var i in sameGraph)
    {
        Console.WriteLine($"Node Number {i} -> Value Is = {nodes.First(c => c.Id == i).Value}");
    }
}
else
{
    Console.WriteLine("Finished");
}


Console.ReadLine();




