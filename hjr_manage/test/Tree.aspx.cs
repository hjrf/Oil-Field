using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_Tree : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<List<String>> tree = GetTree();
        Response.Write(tree[1][0]);
    }
    private List<List<String>> GetTree()
    {
        List<List<String>> tree = new List<List<String>>();
        List<String> node1 = new List<string>();
        List<String> node2 = new List<string>();
        node1.Add("1");
        node2.Add("2");
        tree.Add(node1);
        tree.Add(node2);
        return tree;
    }
}