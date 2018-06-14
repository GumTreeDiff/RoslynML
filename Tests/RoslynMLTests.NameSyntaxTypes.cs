using System.Xml.Linq;
using CommandLineApp;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public partial class RoslynMLTests
    {
        [TestMethod]
        public void TypeSyntax_IdentifierNameSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseName("var");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\">" +
                                "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">var</Token>" +
                            "</IdentifierName>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_QualifiedNameSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseName("a.b.c");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<QualifiedName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\">" +
                                "<QualifiedName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Left\">" +
                                    "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\">" +
                                        "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token>" +
                                    "</IdentifierName>" +
                                    "<Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"DotToken\">.</Token>" +
                                    "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Right\">" +
                                        "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">b</Token>" +
                                    "</IdentifierName>" +
                                "</QualifiedName>" +
                                "<Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"DotToken\">.</Token>" +
                                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\">" +
                                    "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">c</Token>" +
                                "</IdentifierName>" +
                            "</QualifiedName>", 
                            xElement.ToString(SaveOptions.DisableFormatting));
                              

            node = SyntaxFactory.ParseName("x.y");
            xElement = converter.Visit(node);
            Assert.AreEqual("<QualifiedName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\">" +
                                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\">" +
                                    "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token>" +
                                "</IdentifierName>" +
                                "<Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"DotToken\">.</Token>" +
                                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Right\">" +
                                    "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">y</Token>" +
                                "</IdentifierName>" +
                            "</QualifiedName>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_GenericNameSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseName("a<,>");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<GenericName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\">" +
                                "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token>" +
                                "<TypeArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"4\" part=\"TypeArgumentList\">" +
                                    "<Token kind=\"LessThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"LessThanToken\">&lt;</Token>" +
                                    "<SeparatedList_of_Type part=\"Arguments\">" +
                                        "<OmittedTypeArgument>" +
                                            "<Token kind=\"OmittedTypeArgumentToken\" part=\"OmittedTypeArgumentToken\"></Token>" +
                                        "</OmittedTypeArgument>" +
                                        "<OmittedTypeArgument>" +
                                            "<Token kind=\"OmittedTypeArgumentToken\" part=\"OmittedTypeArgumentToken\"></Token>" +
                                        "</OmittedTypeArgument>" +
                                    "</SeparatedList_of_Type>" +
                                    "<Token kind=\"GreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"GreaterThanToken\">&gt;</Token>" +
                                "</TypeArgumentList>" +
                            "</GenericName>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseName("a<x,t>");
            xElement = converter.Visit(node);
            Assert.AreEqual("<GenericName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\">" +
                                "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token>" +
                                "<TypeArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"TypeArgumentList\">" +
                                    "<Token kind=\"LessThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"LessThanToken\">&lt;</Token>" +
                                    "<SeparatedList_of_Type part=\"Arguments\">" +
                                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\">" +
                                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">x</Token>" +
                                        "</IdentifierName>" +
                                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\">" +
                                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">t</Token>" +
                                        "</IdentifierName>" +
                                    "</SeparatedList_of_Type>" +
                                    "<Token kind=\"GreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"GreaterThanToken\">&gt;</Token>" +
                                "</TypeArgumentList>" +
                            "</GenericName>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseName("a<x,>");
            xElement = converter.Visit(node);
            Assert.AreEqual("<GenericName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\">" +
                                "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token>" +
                                "<TypeArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"5\" part=\"TypeArgumentList\">" +
                                    "<Token kind=\"LessThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"LessThanToken\">&lt;</Token>" +
                                    "<SeparatedList_of_Type part=\"Arguments\">" +
                                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\">" +
                                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">x</Token>" +
                                        "</IdentifierName>" +
                                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\">" +
                                            "<Token kind=\"IdentifierToken\" part=\"Identifier\"></Token>" +
                                        "</IdentifierName>" +
                                    "</SeparatedList_of_Type>" +
                                    "<Token kind=\"GreaterThanToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"GreaterThanToken\">&gt;</Token>" +
                                "</TypeArgumentList>" +
                            "</GenericName>", xElement.ToString(SaveOptions.DisableFormatting));
        }



        [TestMethod]
        public void TypeSyntax_AliasQualifiedNameSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("global::c");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AliasQualifiedName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\">" +
                                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\" part=\"Alias\">" +
                                    "<Token kind=\"GlobalKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\" part=\"Identifier\">global</Token>" +
                                "</IdentifierName>" +
                                "<Token kind=\"ColonColonToken\" Operator=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"8\" part=\"ColonColonToken\">::</Token>" +
                                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Name\">" +
                                    "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Identifier\">c</Token>" +
                                "</IdentifierName>" +
                            "</AliasQualifiedName>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_PredefinedTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseTypeName("int");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\">" +
                                "<Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Keyword\">int</Token>" +
                            "</PredefinedType>", xElement.ToString(SaveOptions.DisableFormatting));


            node = SyntaxFactory.ParseTypeName("byte");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\">" +
                                "<Token kind=\"ByteKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\" part=\"Keyword\">byte</Token>" +
                            "</PredefinedType>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_ArrayTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseTypeName("int[1, 2]");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ArrayType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\">" +
                                "<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"ElementType\">" +
                                    "<Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Keyword\">int</Token>" +
                                "</PredefinedType>" +
                                "<List_of_ArrayRankSpecifier part=\"RankSpecifiers\">" +
                                    "<ArrayRankSpecifier startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"9\">" +
                                        "<Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"OpenBracketToken\">[</Token>" +
                                            "<SeparatedList_of_Expression part=\"Sizes\">" +
                                                "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\">" +
                                                    "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">1</Token>" +
                                                "</LiteralExpression>" +
                                                "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\">" +
                                                    "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Token\">2</Token>" +
                                                "</LiteralExpression>" +
                                            "</SeparatedList_of_Expression>" +
                                        "<Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseBracketToken\">]</Token>" +
                                    "</ArrayRankSpecifier>" +
                                "</List_of_ArrayRankSpecifier>" +
                            "</ArrayType>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseTypeName("int[1][2]");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ArrayType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\">" +
                                "<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"ElementType\">" +
                                    "<Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Keyword\">int</Token>" +
                                "</PredefinedType>" +
                                "<List_of_ArrayRankSpecifier part=\"RankSpecifiers\">" +
                                    "<ArrayRankSpecifier startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"6\">" +
                                        "<Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"OpenBracketToken\">[</Token>" +
                                            "<SeparatedList_of_Expression part=\"Sizes\">" +
                                                "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\">" +
                                                    "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">1</Token>" +
                                                "</LiteralExpression>" +
                                            "</SeparatedList_of_Expression>" +
                                        "<Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"CloseBracketToken\">]</Token>" +
                                    "</ArrayRankSpecifier>" +
                                    "<ArrayRankSpecifier startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"9\">" +
                                        "<Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBracketToken\">[</Token>" +
                                            "<SeparatedList_of_Expression part=\"Sizes\">" +
                                                "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\">" +
                                                    "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Token\">2</Token>" +
                                                "</LiteralExpression>" +
                                            "</SeparatedList_of_Expression>" +
                                        "<Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseBracketToken\">]</Token>" +
                                    "</ArrayRankSpecifier>" +
                                "</List_of_ArrayRankSpecifier>" +
                            "</ArrayType>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_PointerTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseTypeName("int*");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PointerType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\">" +
                                "<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"ElementType\">" +
                                    "<Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Keyword\">int</Token>" +
                                "</PredefinedType>" +
                                "<Token kind=\"AsteriskToken\" Operator=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"AsteriskToken\">*</Token>" +
                            "</PointerType>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TypeSyntax_NullableTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseTypeName("int?");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<NullableType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\">" +
                                "<PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"ElementType\">" +
                                    "<Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Keyword\">int</Token>" +
                                "</PredefinedType>" +
                                "<Token kind=\"QuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"QuestionToken\">?</Token>" +
                            "</NullableType>", xElement.ToString(SaveOptions.DisableFormatting));
        }
    }
}
