﻿using CSharpCIA.CSharpCIA.Nodes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCIA.Test.Nodes
{
    public class FieldNode : Node
    {
        public List<MODIFIERS> modifiers;

        public override Type Type => typeof(FieldNode);

        public FieldNode(uint id, string name, string sourcePath, List<Connection> connections) : base(id, name, sourcePath, connections)
        {
        }
    }
}
