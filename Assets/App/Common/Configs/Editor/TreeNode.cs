using System.Collections.Generic;
using App.Common.Configs.External;

namespace App.Common.Configs.Editor
{
    public class TreeNode
    {
        public string Name;
        public string Path;
        public bool IsFolder;
        public bool Expanded = true;

        public GameConfig Config;

        public List<TreeNode> Children = new();
    }
}