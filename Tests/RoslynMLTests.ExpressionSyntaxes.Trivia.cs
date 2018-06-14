using System;
using System.Linq;
using System.Xml.Linq;
using CommandLineApp;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    partial class RoslynMLTests
    {
        [TestMethod]
        public void StructuredTriviaSyntax_SkippedTokensTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var incompleteMember = (IncompleteMemberSyntax)SyntaxFactory.ParseCompilationUnit("[Serializable] public virtual a").Members[0];
            var node = SyntaxFactory.SkippedTokensTrivia(incompleteMember.Modifiers);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<SkippedTokensTrivia startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"14\"><TokenList part=\"Tokens\"><Token kind=\"PublicKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\">public</Token><Token kind=\"VirtualKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"14\">virtual</Token></TokenList></SkippedTokensTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_DocumentationCommentTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<foo />").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<DocumentationCommentTrivia kind=\"SingleLineDocumentationCommentTrivia\" Trivia=\"true\" DocumentationCommentTrivia=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"10\"><List_of_XmlNode part=\"Content\"><XmlEmptyElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"10\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"LocalName\">foo</Token></XmlName><Token kind=\"SlashGreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"SlashGreaterThanToken\">/&gt;</Token></XmlEmptyElement></List_of_XmlNode><Token kind=\"EndOfDocumentationCommentToken\" part=\"EndOfComment\"></Token></DocumentationCommentTrivia>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("/**<foo />*/").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node);
            Assert.AreEqual("<DocumentationCommentTrivia kind=\"MultiLineDocumentationCommentTrivia\" Trivia=\"true\" DocumentationCommentTrivia=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"12\"><List_of_XmlNode part=\"Content\"><XmlEmptyElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"10\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"LocalName\">foo</Token></XmlName><Token kind=\"SlashGreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"SlashGreaterThanToken\">/&gt;</Token></XmlEmptyElement></List_of_XmlNode><Token kind=\"EndOfDocumentationCommentToken\" part=\"EndOfComment\"></Token></DocumentationCommentTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"int\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<TypeCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<TypeCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"32\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"32\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"32\" part=\"Keyword\">int</Token></PredefinedType></TypeCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void QualifiedCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"N.A\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<QualifiedCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<QualifiedCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"32\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Container\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Identifier\">N</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"2\" startColumn=\"31\" endLine=\"2\" endColumn=\"31\" part=\"DotToken\">.</Token><NameMemberCref startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"32\" part=\"Member\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"32\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"32\" part=\"Identifier\">A</Token></IdentifierName></NameMemberCref></QualifiedCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void NameMemberCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"N(int)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<NameMemberCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<NameMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"35\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Identifier\">N</Token></IdentifierName><CrefParameterList startLine=\"2\" startColumn=\"31\" endLine=\"2\" endColumn=\"35\" part=\"Parameters\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"31\" endLine=\"2\" endColumn=\"31\" part=\"OpenParenToken\">(</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"34\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"34\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"32\" endLine=\"2\" endColumn=\"34\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"35\" part=\"CloseParenToken\">)</Token></CrefParameterList></NameMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));

            var text3 = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"N\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text3).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<NameMemberCrefSyntax>().Single();
            xElement = converter.Visit(node);
            Assert.AreEqual("<NameMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"30\" part=\"Identifier\">N</Token></IdentifierName></NameMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void IndexerMemberCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"this[int]\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<IndexerMemberCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<IndexerMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"38\"><Token kind=\"ThisKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"33\" part=\"ThisKeyword\">this</Token><CrefBracketedParameterList startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"38\" part=\"Parameters\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"34\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"38\" endLine=\"2\" endColumn=\"38\" part=\"CloseBracketToken\">]</Token></CrefBracketedParameterList></IndexerMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));

            var text3 = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"this[]\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text3).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<IndexerMemberCrefSyntax>().Single();
            xElement = converter.Visit(node);
            Assert.AreEqual("<IndexerMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"35\"><Token kind=\"ThisKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"33\" part=\"ThisKeyword\">this</Token><CrefBracketedParameterList startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"35\" part=\"Parameters\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"34\" part=\"OpenBracketToken\">[</Token><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"35\" part=\"CloseBracketToken\">]</Token></CrefBracketedParameterList></IndexerMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));

            text3 = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"this\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text3).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<IndexerMemberCrefSyntax>().Single();
            xElement = converter.Visit(node);
            Assert.AreEqual("<IndexerMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"33\"><Token kind=\"ThisKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"33\" part=\"ThisKeyword\">this</Token></IndexerMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void OperatorMemberCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"operator +(int)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<OperatorMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"44\"><Token kind=\"OperatorKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"37\" part=\"OperatorKeyword\">operator</Token><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"2\" startColumn=\"39\" endLine=\"2\" endColumn=\"39\" part=\"OperatorToken\">+</Token><CrefParameterList startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"44\" part=\"Parameters\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"40\" part=\"OpenParenToken\">(</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"44\" endLine=\"2\" endColumn=\"44\" part=\"CloseParenToken\">)</Token></CrefParameterList></OperatorMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));

            var text3 = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"operator +\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text3).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single();
            xElement = converter.Visit(node);
            Assert.AreEqual("<OperatorMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"39\"><Token kind=\"OperatorKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"37\" part=\"OperatorKeyword\">operator</Token><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"2\" startColumn=\"39\" endLine=\"2\" endColumn=\"39\" part=\"OperatorToken\">+</Token></OperatorMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ConversionOperatorMemberCrefSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"implicit operator N(int)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<ConversionOperatorMemberCrefSyntax>().Single();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ConversionOperatorMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"53\"><Token kind=\"ImplicitKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"37\" part=\"ImplicitOrExplicitKeyword\">implicit</Token><Token kind=\"OperatorKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"39\" endLine=\"2\" endColumn=\"46\" part=\"OperatorKeyword\">operator</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"48\" endLine=\"2\" endColumn=\"48\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"48\" endLine=\"2\" endColumn=\"48\" part=\"Identifier\">N</Token></IdentifierName><CrefParameterList startLine=\"2\" startColumn=\"49\" endLine=\"2\" endColumn=\"53\" part=\"Parameters\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"49\" endLine=\"2\" endColumn=\"49\" part=\"OpenParenToken\">(</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"53\" endLine=\"2\" endColumn=\"53\" part=\"CloseParenToken\">)</Token></CrefParameterList></ConversionOperatorMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));

            text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"explicit operator N(int)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<ConversionOperatorMemberCrefSyntax>().Single();
            xElement = converter.Visit(node);
            Assert.AreEqual("<ConversionOperatorMemberCref startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"53\"><Token kind=\"ExplicitKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"30\" endLine=\"2\" endColumn=\"37\" part=\"ImplicitOrExplicitKeyword\">explicit</Token><Token kind=\"OperatorKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"39\" endLine=\"2\" endColumn=\"46\" part=\"OperatorKeyword\">operator</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"2\" startColumn=\"48\" endLine=\"2\" endColumn=\"48\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"2\" startColumn=\"48\" endLine=\"2\" endColumn=\"48\" part=\"Identifier\">N</Token></IdentifierName><CrefParameterList startLine=\"2\" startColumn=\"49\" endLine=\"2\" endColumn=\"53\" part=\"Parameters\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"49\" endLine=\"2\" endColumn=\"49\" part=\"OpenParenToken\">(</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"53\" endLine=\"2\" endColumn=\"53\" part=\"CloseParenToken\">)</Token></CrefParameterList></ConversionOperatorMemberCref>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void CrefBracketedParameterListSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"this[int, string]\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<IndexerMemberCrefSyntax>().Single().Parameters;
            var xElement = converter.Visit(node);
            Assert.AreEqual("<CrefBracketedParameterList startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"46\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"34\" endLine=\"2\" endColumn=\"34\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"35\" endLine=\"2\" endColumn=\"37\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter><CrefParameter startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"45\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"45\" part=\"Type\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"45\" part=\"Keyword\">string</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"46\" endLine=\"2\" endColumn=\"46\" part=\"CloseBracketToken\">]</Token></CrefBracketedParameterList>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void CrefParameterListSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"operator +(int, string)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single().Parameters;
            var xElement = converter.Visit(node);
            Assert.AreEqual("<CrefParameterList startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"52\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"40\" endLine=\"2\" endColumn=\"40\" part=\"OpenParenToken\">(</Token><SeparatedList_of_CrefParameter part=\"Parameters\"><CrefParameter startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter><CrefParameter startLine=\"2\" startColumn=\"46\" endLine=\"2\" endColumn=\"51\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"46\" endLine=\"2\" endColumn=\"51\" part=\"Type\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"46\" endLine=\"2\" endColumn=\"51\" part=\"Keyword\">string</Token></PredefinedType></CrefParameter></SeparatedList_of_CrefParameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"2\" startColumn=\"52\" endLine=\"2\" endColumn=\"52\" part=\"CloseParenToken\">)</Token></CrefParameterList>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void CrefParameterSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"operator +(ref int, out string)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            var namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            var node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single().Parameters.Parameters[0];
            var xElement = converter.Visit(node);
            Assert.AreEqual("<CrefParameter startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"47\"><Token kind=\"RefKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"RefOrOutKeyword\">ref</Token><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"45\" endLine=\"2\" endColumn=\"47\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"45\" endLine=\"2\" endColumn=\"47\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter>", xElement.ToString(SaveOptions.DisableFormatting));

            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single().Parameters.Parameters[1];
            xElement = converter.Visit(node);
            Assert.AreEqual("<CrefParameter startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"59\"><Token kind=\"OutKeyword\" Keyword=\"true\" TypeParameterVariance=\"true\" startLine=\"2\" startColumn=\"50\" endLine=\"2\" endColumn=\"52\" part=\"RefOrOutKeyword\">out</Token><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"54\" endLine=\"2\" endColumn=\"59\" part=\"Type\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"54\" endLine=\"2\" endColumn=\"59\" part=\"Keyword\">string</Token></PredefinedType></CrefParameter>", xElement.ToString(SaveOptions.DisableFormatting));

            text = "namespace N { " + Environment.NewLine +
                       "    /// <summary> <see cref=\"operator +(int, string)\"/> </summary>" + Environment.NewLine +
                       "    class A { } }";
            namespaceNode = SyntaxFactory.ParseCompilationUnit(text).Members[0];
            node = namespaceNode.DescendantNodes(descendIntoTrivia: true).OfType<OperatorMemberCrefSyntax>().Single().Parameters.Parameters[0];
            xElement = converter.Visit(node);
            Assert.AreEqual("<CrefParameter startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\"><PredefinedType TypeSyntax=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"2\" startColumn=\"41\" endLine=\"2\" endColumn=\"43\" part=\"Keyword\">int</Token></PredefinedType></CrefParameter>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlElementSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message id=\"5\" oid=\"3\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.Single());
            Assert.AreEqual("<XmlElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"48\"><XmlElementStartTag startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"27\" part=\"StartTag\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"LocalName\">message</Token></XmlName><List_of_XmlAttribute part=\"Attributes\"><XmlTextAttribute startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"18\"><XmlName startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">id</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\">5</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute><XmlTextAttribute startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"26\"><XmlName startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"22\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"22\" part=\"LocalName\">oid</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"23\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"24\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\">3</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute></List_of_XmlAttribute><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"27\" endLine=\"1\" endColumn=\"27\" part=\"GreaterThanToken\">&gt;</Token></XmlElementStartTag><List_of_XmlNode part=\"Content\"><XmlText startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"38\"><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"38\">Hello World</Token></TokenList></XmlText></List_of_XmlNode><XmlElementEndTag startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"48\" part=\"EndTag\"><Token kind=\"LessThanSlashToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"40\" part=\"LessThanSlashToken\">&lt;/</Token><XmlName startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"47\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"47\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"48\" endLine=\"1\" endColumn=\"48\" part=\"GreaterThanToken\">&gt;</Token></XmlElementEndTag></XmlElement>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message id=\"5\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.Single());
            Assert.AreEqual("<XmlElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"40\"><XmlElementStartTag startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"19\" part=\"StartTag\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"LocalName\">message</Token></XmlName><List_of_XmlAttribute part=\"Attributes\"><XmlTextAttribute startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"18\"><XmlName startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">id</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\">5</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute></List_of_XmlAttribute><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"GreaterThanToken\">&gt;</Token></XmlElementStartTag><List_of_XmlNode part=\"Content\"><XmlText startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"30\"><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"30\">Hello World</Token></TokenList></XmlText></List_of_XmlNode><XmlElementEndTag startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"40\" part=\"EndTag\"><Token kind=\"LessThanSlashToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"32\" part=\"LessThanSlashToken\">&lt;/</Token><XmlName startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"39\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"39\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"40\" endLine=\"1\" endColumn=\"40\" part=\"GreaterThanToken\">&gt;</Token></XmlElementEndTag></XmlElement>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message>Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.Single());
            Assert.AreEqual("<XmlElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"33\"><XmlElementStartTag startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"12\" part=\"StartTag\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"GreaterThanToken\">&gt;</Token></XmlElementStartTag><List_of_XmlNode part=\"Content\"><XmlText startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"23\"><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"23\">Hello World</Token></TokenList></XmlText></List_of_XmlNode><XmlElementEndTag startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"33\" part=\"EndTag\"><Token kind=\"LessThanSlashToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"25\" part=\"LessThanSlashToken\">&lt;/</Token><XmlName startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"32\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"32\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"33\" part=\"GreaterThanToken\">&gt;</Token></XmlElementEndTag></XmlElement>", xElement.ToString(SaveOptions.DisableFormatting));
        }
 
        [TestMethod]
        public void XmlElementStartTagSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message id=\"5\" oid=\"3\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().StartTag);
            Assert.AreEqual("<XmlElementStartTag startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"27\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"LocalName\">message</Token></XmlName><List_of_XmlAttribute part=\"Attributes\"><XmlTextAttribute startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"18\"><XmlName startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">id</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\">5</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute><XmlTextAttribute startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"26\"><XmlName startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"22\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"22\" part=\"LocalName\">oid</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"23\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"24\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\">3</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute></List_of_XmlAttribute><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"27\" endLine=\"1\" endColumn=\"27\" part=\"GreaterThanToken\">&gt;</Token></XmlElementStartTag>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message>Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().StartTag);
            Assert.AreEqual("<XmlElementStartTag startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"12\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"GreaterThanToken\">&gt;</Token></XmlElementStartTag>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlElementEndTagSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message id=\"5\" oid=\"3\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().EndTag);
            Assert.AreEqual("<XmlElementEndTag startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"48\"><Token kind=\"LessThanSlashToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"40\" part=\"LessThanSlashToken\">&lt;/</Token><XmlName startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"47\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"47\" part=\"LocalName\">message</Token></XmlName><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"48\" endLine=\"1\" endColumn=\"48\" part=\"GreaterThanToken\">&gt;</Token></XmlElementEndTag>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlEmptyElementSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<foo id=\"3\"/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single());
            Assert.AreEqual("<XmlEmptyElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"16\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"LocalName\">foo</Token></XmlName><List_of_XmlAttribute part=\"Attributes\"><XmlTextAttribute startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"14\"><XmlName startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"LocalName\">id</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\">3</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute></List_of_XmlAttribute><Token kind=\"SlashGreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"16\" part=\"SlashGreaterThanToken\">/&gt;</Token></XmlEmptyElement>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<foo />").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single());
            Assert.AreEqual("<XmlEmptyElement startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"10\"><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"LessThanToken\">&lt;</Token><XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"LocalName\">foo</Token></XmlName><Token kind=\"SlashGreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"SlashGreaterThanToken\">/&gt;</Token></XmlEmptyElement>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlNameSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<d:foo/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Name);
            Assert.AreEqual("<XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"9\"><XmlPrefix startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"6\" part=\"Prefix\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Prefix\">d</Token><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"ColonToken\">:</Token></XmlPrefix><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"9\" part=\"LocalName\">foo</Token></XmlName>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<foo />").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Name);
            Assert.AreEqual("<XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"LocalName\">foo</Token></XmlName>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<a:foo id=\"3\"/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Name);
            Assert.AreEqual("<XmlName startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"9\"><XmlPrefix startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"6\" part=\"Prefix\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Prefix\">a</Token><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"ColonToken\">:</Token></XmlPrefix><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"9\" part=\"LocalName\">foo</Token></XmlName>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlPrefixSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<d:foo/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Name.Prefix);
            Assert.AreEqual("<XmlPrefix startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"6\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Prefix\">d</Token><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"ColonToken\">:</Token></XmlPrefix>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<a:foo id=\"3\"/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Name.Prefix);
            Assert.AreEqual("<XmlPrefix startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"6\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Prefix\">a</Token><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"ColonToken\">:</Token></XmlPrefix>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlAttributeSyntax_XmlTextAttributeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<message id=\"5 r\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().StartTag.Attributes[0]);
            Assert.AreEqual("<XmlTextAttribute startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"20\"><XmlName startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">id</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"StartQuoteToken\">\"</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"19\">5 r</Token></TokenList><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"20\" part=\"EndQuoteToken\">\"</Token></XmlTextAttribute>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlAttributeSyntax_XmlCrefAttributeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<see cref=\"M\">Hello World</message>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().StartTag.Attributes[0]);
            Assert.AreEqual("<XmlCrefAttribute startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"16\"><XmlName startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"LocalName\">cref</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"StartQuoteToken\">\"</Token><NameMemberCref startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Cref\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Identifier\">M</Token></IdentifierName></NameMemberCref><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"EndQuoteToken\">\"</Token></XmlCrefAttribute>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlAttributeSyntax_XmlNameAttributeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<param name=\"M\"/>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlEmptyElementSyntax>().Single().Attributes[0]);
            Assert.AreEqual("<XmlNameAttribute startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"18\"><XmlName startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">name</Token></XmlName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"EqualsToken\">=</Token><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"StartQuoteToken\">\"</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"Identifier\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"Identifier\">M</Token></IdentifierName><Token kind=\"DoubleQuoteToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"EndQuoteToken\">\"</Token></XmlNameAttribute>", xElement.ToString(SaveOptions.DisableFormatting));
        }
 
        [TestMethod]
        public void XmlTextSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<foo>bar</foo>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlElementSyntax>().Single().Content[0]);
            Assert.AreEqual("<XmlText startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"11\"><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"11\">bar</Token></TokenList></XmlText>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlCDataSectionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<![CDATA[this is a test of &some; cdata]]>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlCDataSectionSyntax>().Single());
            Assert.AreEqual("<XmlCDataSection startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"45\"><Token kind=\"XmlCDataStartToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"12\" part=\"StartCDataToken\">&lt;![CDATA[</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"42\">this is a test of &amp;some; cdata</Token></TokenList><Token kind=\"XmlCDataEndToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"43\" endLine=\"1\" endColumn=\"45\" part=\"EndCDataToken\">]]&gt;</Token></XmlCDataSection>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlProcessingInstructionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<?proc-inst this is a test of &some; processinginstruction?>").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlProcessingInstructionSyntax>().Single());
            Assert.AreEqual("<XmlProcessingInstruction startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"63\"><Token kind=\"XmlProcessingInstructionStartToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"5\" part=\"StartProcessingInstructionToken\">&lt;?</Token><XmlName startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"14\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"14\" part=\"LocalName\">proc-inst</Token></XmlName><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"61\"> this is a test of &amp;some; processinginstruction</Token></TokenList><Token kind=\"XmlProcessingInstructionEndToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"62\" endLine=\"1\" endColumn=\"63\" part=\"EndProcessingInstructionToken\">?&gt;</Token></XmlProcessingInstruction>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void XmlCommentSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (DocumentationCommentTriviaSyntax)SyntaxFactory.ParseSyntaxTree("///<!--this is a test of &some; comment-->").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node.Content.OfType<XmlCommentSyntax>().Single());
            Assert.AreEqual("<XmlComment startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"42\"><Token kind=\"XmlCommentStartToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"7\" part=\"LessThanExclamationMinusMinusToken\">&lt;!--</Token><TokenList part=\"TextTokens\"><Token kind=\"XmlTextLiteralToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"39\">this is a test of &amp;some; comment</Token></TokenList><Token kind=\"XmlCommentEndToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"40\" endLine=\"1\" endColumn=\"42\" part=\"MinusMinusGreaterThanToken\">--&gt;</Token></XmlComment>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_IfDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (IfDirectiveTriviaSyntax)SyntaxFactory.ParseSyntaxTree("#if DEBUG Console.WriteLine(\"Debug version\"); #endif").GetCompilationUnitRoot().EndOfFileToken.LeadingTrivia.Single().GetStructure();
            var xElement = converter.Visit(node);
            Assert.AreEqual("<IfDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"52\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"IfKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"3\" part=\"IfKeyword\">if</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"9\" part=\"Condition\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"9\" part=\"Identifier\">DEBUG</Token></IdentifierName><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></IfDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_ElifDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ElifDirectiveTrivia(SyntaxFactory.IdentifierName("VC7"), true, true, true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ElifDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"ElifKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"5\" part=\"ElifKeyword\">elif</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"8\" part=\"Condition\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">VC7</Token></IdentifierName><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></ElifDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }
 
        [TestMethod]
        public void StructuredTriviaSyntax_ElseDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ElseDirectiveTrivia(true, true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ElseDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"ElseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"5\" part=\"ElseKeyword\">else</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></ElseDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        // 
        [TestMethod]
        public void StructuredTriviaSyntax_EndIfDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.EndIfDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<EndIfDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"EndIfKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"EndIfKeyword\">endif</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></EndIfDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_RegionDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.RegionDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<RegionDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"RegionKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"RegionKeyword\">region</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></RegionDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_EndRegionDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.EndRegionDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<EndRegionDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"10\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"EndRegionKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"10\" part=\"EndRegionKeyword\">endregion</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></EndRegionDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_ErrorDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ErrorDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ErrorDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"ErrorKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"ErrorKeyword\">error</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></ErrorDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_WarningDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.WarningDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<WarningDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"WarningKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"8\" part=\"WarningKeyword\">warning</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></WarningDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_BadDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.BadDirectiveTrivia(SyntaxFactory.Identifier("unknown"), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<BadDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">unknown</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></BadDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }
 
        [TestMethod]
        public void StructuredTriviaSyntax_DefineDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.DefineDirectiveTrivia(SyntaxFactory.Identifier("DEBUG"), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<DefineDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"DefineKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"DefineKeyword\">define</Token><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"12\" part=\"Name\">DEBUG</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></DefineDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_UndefDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.UndefDirectiveTrivia(SyntaxFactory.Identifier("DEBUG"), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<UndefDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"11\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"UndefKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"UndefKeyword\">undef</Token><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"11\" part=\"Name\">DEBUG</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></UndefDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_LineDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.LineDirectiveTrivia(SyntaxFactory.Literal("200",200), SyntaxFactory.Literal("\"Special\""), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<LineDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"21\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"LineKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"5\" part=\"LineKeyword\">line</Token><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"8\" part=\"Line\">200</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"21\" part=\"File\">\"Special\"</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></LineDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_PragmaWarningDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var separatedSyntaxList = SyntaxFactory.SeparatedList(new ExpressionSyntax[] { SyntaxFactory.IdentifierName("warning-list"), SyntaxFactory.IdentifierName("CS3021") });
            var node = SyntaxFactory.PragmaWarningDirectiveTrivia(SyntaxFactory.Token(SyntaxKind.DisableKeyword), separatedSyntaxList, true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PragmaWarningDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"40\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"PragmaKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"PragmaKeyword\">pragma</Token><Token kind=\"WarningKeyword\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"14\" part=\"WarningKeyword\">warning</Token><Token kind=\"DisableKeyword\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"21\" part=\"DisableOrRestoreKeyword\">disable</Token><SeparatedList_of_Expression part=\"ErrorCodes\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"33\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"33\" part=\"Identifier\">warning-list</Token></IdentifierName><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"35\" endLine=\"1\" endColumn=\"40\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"35\" endLine=\"1\" endColumn=\"40\" part=\"Identifier\">CS3021</Token></IdentifierName></SeparatedList_of_Expression><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></PragmaWarningDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_PragmaChecksumDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.PragmaChecksumDirectiveTrivia(SyntaxFactory.Literal("\"file.cs\""), SyntaxFactory.Literal("\"{3673e4ca-6098-4ec1-890f-8fceb2a794a2}\""), SyntaxFactory.Literal("\"{012345678AB}\""), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PragmaChecksumDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"91\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"PragmaKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"PragmaKeyword\">pragma</Token><Token kind=\"ChecksumKeyword\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"15\" part=\"ChecksumKeyword\">checksum</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"28\" part=\"File\">\"file.cs\"</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"29\" endLine=\"1\" endColumn=\"72\" part=\"Guid\">\"{3673e4ca-6098-4ec1-890f-8fceb2a794a2}\"</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"73\" endLine=\"1\" endColumn=\"91\" part=\"Bytes\">\"{012345678AB}\"</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></PragmaChecksumDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }
 
        [TestMethod]
        public void StructuredTriviaSyntax_ReferenceDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ReferenceDirectiveTrivia(SyntaxFactory.Literal("DEBUG"), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ReferenceDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"ReferenceKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"ReferenceKeyword\">r</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"9\" part=\"File\">DEBUG</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></ReferenceDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_LoadDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.LoadDirectiveTrivia(SyntaxFactory.Literal("DEBUG"), true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<LoadDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"LoadKeyword\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"5\" part=\"LoadKeyword\">load</Token><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"12\" part=\"File\">DEBUG</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></LoadDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void StructuredTriviaSyntax_ShebangDirectiveTriviaSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ShebangDirectiveTrivia(true);
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ShebangDirectiveTrivia Trivia=\"true\" PreprocessorDirective=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"HashToken\" Punctuation=\"true\" Language=\"true\" Preprocessor=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"HashToken\">#</Token><Token kind=\"ExclamationToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"ExclamationToken\">!</Token><Token kind=\"EndOfDirectiveToken\" part=\"EndOfDirectiveToken\"></Token></ShebangDirectiveTrivia>", xElement.ToString(SaveOptions.DisableFormatting));
        }
    }
}
