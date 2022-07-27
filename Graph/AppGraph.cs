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