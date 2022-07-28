using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph;

public class AppGraph
{
    public int Id { get; set; }
    public int Value { get; set; }
    public List<AppGraph> Graphs { get; set; }

    public bool ConnectDirectlyToQ { get; set; }
    public bool ConnectDirectlyToQChecked { get; set; }
}

public static class Functions
{
    public static List<int> IsItTheSame(List<AppGraph> oldGraphs, int qNode)
    {
        var newGraph = new List<AppGraph>();

        foreach (var graph in oldGraphs)
        {
            newGraph.Add(new AppGraph()
            {
                Id = graph.Id,
                Value = 4 * graph.Value,
                ConnectDirectlyToQ = false,
                ConnectDirectlyToQChecked = false,
                Graphs = new List<AppGraph>(),
            });
        }

        foreach (var appGraph in oldGraphs)
        {
            foreach (var appGraphGraph in appGraph.Graphs)
            {
                var itemAdd = newGraph.First(c => c.Id == appGraphGraph.Id);
                newGraph.First(c => c.Id == appGraph.Id).Graphs.Add(itemAdd);
            }
        }

        newGraph[qNode - 1].ConnectDirectlyToQ = true;
        newGraph[qNode - 1].ConnectDirectlyToQChecked = true;

        // QList Generated
        var flag = false;
        do
        {
            foreach (var node in newGraph.Where(c => c.ConnectDirectlyToQ).ToList())
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
            if (i == qNode)
            {
                continue;
            }

            var node = newGraph.First(c => c.Id == i);

            if (node.Value >= node.Graphs.Count)
            {
                var newValue = node.Graphs.Count - 1;
                var decrease = node.Value - newValue;
                node.Value = newValue;

                var subNodes = node.Graphs.ToList();

                do
                {
                    var canIncrease = subNodes.Where(c => c.Value < c.Graphs.Count || c.Id == qNode).ToList();

                    if (canIncrease.Any(c => c.Id == qNode))
                    {
                        newGraph.First(c => c.Id == qNode).Value += decrease;
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

            if (Functions.CheckIsFinished(newGraph, qNode))
            {
                break;
            }
        }


        var result = new List<int>();

        foreach (var graph in newGraph)
        {
            var id = oldGraphs.Where(c => !result.Contains(c.Id)).FirstOrDefault(g => g.Value == graph.Value);
            if (id is null)
            {
                continue;
            }
            else
            {
                result.Add(id.Id);
            }
        }

        return result;
    }

    public static bool CheckIsFinished(List<AppGraph> graph, int qNodeId)
    {
        var nodesss = graph.Where(g => g.Id != qNodeId).ToList();

        foreach (var node in nodesss)
        {
            if (node.Value >= node.Graphs.Count)
            {
                return false;
            }
        }
        return true;
    }
}