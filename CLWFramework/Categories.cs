﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CLWFramework.CLWFilters;
namespace CLWFramework
{
    public sealed class Categories
    {
        private static readonly Categories instance = new Categories();

        public static Categories Instance
        {
            get { return instance; }
        }

        private Dictionary<CategoryInfo, List<CategoryInfo>> categoriesDictionary;

        public Dictionary<CategoryInfo, List<CategoryInfo>> CategoriesDictionary
        {
            get { return categoriesDictionary; }
        }
        private Categories()
        {
            categoriesDictionary = new Dictionary<CategoryInfo, List<CategoryInfo>>();
        }
        public string GetSuffix(string parentSection, string childSection)
        {
            foreach (KeyValuePair<CategoryInfo, List<CategoryInfo>> parent in CategoriesDictionary)
            {
                if (parent.Key.Name == parentSection)
                {
                    if (childSection == null)
                    {
                        if (parent.Key.Name == parentSection)
                            return parent.Key.Suffix;
                    }
                    else
                    {
                        foreach (CategoryInfo info in parent.Value)
                        {
                            if (info.Name == childSection)
                                return info.Suffix;
                        }
                    }
                }
            }
            return "";
        }
        public void PopulateTreeView(ref TreeView tree_view)
        {
            TreeView new_view = new TreeView();
            tree_view.CheckBoxes = true;

            foreach (KeyValuePair<CategoryInfo, List<CategoryInfo>> parentSection in CategoriesDictionary)
            {
                TreeNode node = new TreeNode(parentSection.Key.Name);
                node.Tag = parentSection.Key;
                foreach (CategoryInfo childInfo in parentSection.Value)
                {
                    TreeNode childNode = new TreeNode(childInfo.Name);
                    childNode.Tag = childNode;
                    node.Nodes.Add(childNode);
                }
                tree_view.Nodes.Add(node);
            }
        }
        public void DownloadCategories()
        {
            categoriesDictionary.Clear();
            CategoriesFilter filter = new CategoriesFilter(ref categoriesDictionary);
            if(filter.ParseURL("http://chicago.craigslist.org"))
                filter.Populate();
        }

        public void FormatForCityDetails(out Dictionary<string, Dictionary<string, SubsectionDetails>> sections)
        {
            sections = new Dictionary<string, Dictionary<string, SubsectionDetails>>();
            foreach(KeyValuePair<CategoryInfo, List<CategoryInfo>> pair in categoriesDictionary)
            {
                Dictionary<string, SubsectionDetails> subDetails = new Dictionary<string, SubsectionDetails>();
                foreach (CategoryInfo info in pair.Value)
                    subDetails.Add(info.Name, new SubsectionDetails(info.Suffix));

                sections.Add(pair.Key.Name, subDetails);
            }
        }
    }
}
