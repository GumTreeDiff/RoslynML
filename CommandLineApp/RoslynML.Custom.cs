using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace CommandLineApp
{
    partial class RoslynML
    {
        /// <summary>
        /// Loads the content given as a full path or a textual content.
        /// </summary>
        /// <param name="pathOrContent">full path or textual content</param>
        /// <param name="path">informs if the content is given as a full path or textual content.</param>
        /// <returns>XML-like representation of the Roslyn-based AST.</returns>
        public virtual XElement Load(string pathOrContent, bool path = true)
        {
            string content = path ? System.IO.File.ReadAllText(pathOrContent) : pathOrContent;
            var ast = SyntaxFactory.ParseCompilationUnit(content).SyntaxTree.GetRoot();
            return this.Visit(ast);
        }

        /// <summary>
        /// Converts an XML-like AST representation to the XML-like AST representation internally expected by GumTree.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual XElement Gumtreefy(XElement source)
        {
            return this.CoreGumtreefy(source).Single();
        }

        private IEnumerable<XElement> CoreGumtreefy(XElement source)
        {
            if (source.Name.LocalName.Contains("_of_") || source.Name.LocalName == "TokenList")
            {
                foreach (var child in source.Elements())
                {
                    var rChildren = this.CoreGumtreefy(child).ToArray();
                    foreach (var rChild in rChildren)
                    {
                        yield return rChild;
                    }
                }
                yield break;
            } 

            var result = new XElement("tree");

            var kindAtt = source.Attribute("kind");
            var kind = kindAtt != null ? kindAtt.Value : source.Name.LocalName;
            var kindVal = (SyntaxKind)Enum.Parse(typeof(SyntaxKind), kind);

            result.Add(new XAttribute("type", ((int)kindVal).ToString(CultureInfo.InvariantCulture)));
            result.Add(new XAttribute("typeLabel", kind));

            if (source.Name.LocalName == "Token")
                result.Add(new XAttribute("label", source.Value ?? ""));

            var startLineAtt = source.Attribute("startLine");
            if (startLineAtt != null)
                result.Add(new XAttribute("line_before", startLineAtt.Value));

            var startColumnAtt = source.Attribute("startColumn");
            if (startColumnAtt != null)
                result.Add(new XAttribute("col_before", startColumnAtt.Value));

            var endLineAtt = source.Attribute("endLine");
            if (endLineAtt != null)
                result.Add(new XAttribute("line_after", endLineAtt.Value));

            var endColumnAtt = source.Attribute("endColumn");
            if (endColumnAtt != null)
                result.Add(new XAttribute("col_after", endColumnAtt.Value));

            foreach (var child in source.Elements())
            {
                result.Add(this.CoreGumtreefy(child).ToArray());
            }

            yield return result;
        }

        /// <summary>
        /// Sets the ID for all elements in the AST.
        /// </summary>
        /// <param name="root">AST root.</param>
        public virtual void SetRoslynMLIDs(XElement root)
        {
            int i = 0;
            foreach (var item in root.PostOrder(n => n.Elements()))
            {
                item.Add(new XAttribute("RmID", i++.ToString(CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Sets the ID for elements used for GumTree's AST.
        /// </summary>
        /// <param name="root">AST root.</param>
        public virtual void SetGumTreefiedIDs(XElement root)
        {
            int i = 0;
            foreach (var item in root.PostOrder(n => n.Elements()).Where(n => !n.Name.LocalName.Contains("_of_") && n.Name.LocalName != "TokenList"))
            {
                item.Add(new XAttribute("GtID", i++.ToString(CultureInfo.InvariantCulture)));
            }
        }
        
        /// <summary>
        /// Removes from the tree those elements that do not satisfy a given filter.
        /// </summary>
        /// <param name="source">tree root.</param>
        /// <param name="selector">predicate to determine what element types remain (true) or not (false).</param>
        public virtual void Prune(XElement source, Func<XElement, bool> selector)
        {
            foreach (var item in source.Elements().ToList())
            {
                if (!selector(item))
                    item.Remove();
                else
                    this.Prune(item, selector);
            }
        }
    }
}
