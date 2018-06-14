using System;
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
        public void ExpressionSyntax_LiteralExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("3");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">3</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("'3'");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Token\">3</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("false");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"FalseLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><Token kind=\"FalseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"Token\">false</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("true");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"TrueLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><Token kind=\"TrueKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\" part=\"Token\">true</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("null");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"NullLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><Token kind=\"NullKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\" part=\"Token\">null</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("__arglist");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"ArgListExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\"><Token kind=\"ArgListKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\" part=\"Token\">__arglist</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("\"3\"");
            xElement = converter.Visit(node);
            Assert.AreEqual("<LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Token\">3</Token></LiteralExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ParenthesizedExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("(3)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ParenthesizedExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"CloseParenToken\">)</Token></ParenthesizedExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_PrefixUnaryExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("++3");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"PreIncrementExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><Token kind=\"PlusPlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">++</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("+3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"UnaryPlusExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">+</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("-3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"UnaryMinusExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"MinusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">-</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("~3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"BitwiseNotExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"TildeToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">~</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("!3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"LogicalNotExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"ExclamationToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">!</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("&3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"AddressOfExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"AmpersandToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">&amp;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("*3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"PointerIndirectionExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\"><Token kind=\"AsteriskToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OperatorToken\">*</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("--3");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PrefixUnaryExpression kind=\"PreDecrementExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><Token kind=\"MinusMinusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">--</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Token\">3</Token></LiteralExpression></PrefixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_PostfixUnaryExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("3++");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<PostfixUnaryExpression kind=\"PostIncrementExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">3</Token></LiteralExpression><Token kind=\"PlusPlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">++</Token></PostfixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("3--");
            xElement = converter.Visit(node);
            Assert.AreEqual("<PostfixUnaryExpression kind=\"PostDecrementExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Operand\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">3</Token></LiteralExpression><Token kind=\"MinusMinusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">--</Token></PostfixUnaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_AwaitExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("await x");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AwaitExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\"><Token kind=\"AwaitKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"AwaitKeyword\">await</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">x</Token></IdentifierName></AwaitExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_MemberAccessExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("x.r");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">r</Token></IdentifierName></MemberAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x->r");
            xElement = converter.Visit(node);
            Assert.AreEqual("<MemberAccessExpression kind=\"PointerMemberAccessExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"MinusGreaterThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">-&gt;</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Identifier\">r</Token></IdentifierName></MemberAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ConditionalAccessExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("x?.r");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ConditionalAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"QuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">?</Token><MemberBindingExpression startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"WhenNotNull\"><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Identifier\">r</Token></IdentifierName></MemberBindingExpression></ConditionalAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_MemberBindingExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (ConditionalAccessExpressionSyntax)Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression("m?.r");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ConditionalAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">m</Token></IdentifierName><Token kind=\"QuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">?</Token><MemberBindingExpression startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"WhenNotNull\"><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Identifier\">r</Token></IdentifierName></MemberBindingExpression></ConditionalAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ElementBindingExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (ConditionalAccessExpressionSyntax)Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression("m?[r]");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ConditionalAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">m</Token></IdentifierName><Token kind=\"QuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OperatorToken\">?</Token><ElementBindingExpression startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"WhenNotNull\"><BracketedArgumentList startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Identifier\">r</Token></IdentifierName></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ElementBindingExpression></ConditionalAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ImplicitElementAccessSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            Func<string, ImplicitElementAccessSyntax> getElementBinding = delegate (string s)
            {
                var objectCreation = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression(s);
                var assigment = (AssignmentExpressionSyntax)objectCreation.Initializer.Expressions[0];
                return (ImplicitElementAccessSyntax)assigment.Left;
            };

            var node = getElementBinding("new A { [\"a\"] = { a = 0} }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ImplicitElementAccess startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"13\"><BracketedArgumentList startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"13\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Token\">a</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ImplicitElementAccess>", xElement.ToString(SaveOptions.DisableFormatting));

            node = getElementBinding("new A { [\"hj\", \"vb\"] = { a = 0} }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ImplicitElementAccess startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"20\"><BracketedArgumentList startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"20\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"13\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"13\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"13\" part=\"Token\">hj</Token></LiteralExpression></Argument><Argument startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"19\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"19\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"19\" part=\"Token\">vb</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"20\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ImplicitElementAccess>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_BinaryExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("1 * 2");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"MultiplyExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"AsteriskToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">*</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 / 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"DivideExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"SlashToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">/</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 % 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"ModuloExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"PercentToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">%</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 + 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"AddExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">+</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 - 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"SubtractExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"MinusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">-</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 << 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"LeftShiftExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"LessThanLessThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&lt;&lt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 >> 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"RightShiftExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"GreaterThanGreaterThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&gt;&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 < 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"LessThanExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"LessThanToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">&lt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 > 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"GreaterThanExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 <= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"LessThanOrEqualExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"LessThanEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&lt;=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 >= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"GreaterThanOrEqualExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"GreaterThanEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&gt;=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 == 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"EqualsExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"EqualsEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">==</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 != 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"NotEqualsExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"ExclamationEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">!=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 & 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"BitwiseAndExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"AmpersandToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">&amp;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 ^ 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"ExclusiveOrExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"CaretToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">^</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 | 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"BitwiseOrExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"BarToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">|</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 && 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"LogicalAndExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"AmpersandAmpersandToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&amp;&amp;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 || 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"LogicalOrExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"BarBarToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">||</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("1 ?? 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<BinaryExpression kind=\"CoalesceExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Token\">1</Token></LiteralExpression><Token kind=\"QuestionQuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">??</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></BinaryExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_AssignmentExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("x *= 2");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"MultiplyAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"AsteriskEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">*=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x /= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"DivideAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"SlashEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">/=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x %= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"ModuloAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"PercentEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">%=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x += 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"AddAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"PlusEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">+=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x -= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"SubtractAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"MinusEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">-=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x <<= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"LeftShiftAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"LessThanLessThanEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"OperatorToken\">&lt;&lt;=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x >>= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"RightShiftAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"GreaterThanGreaterThanEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"OperatorToken\">&gt;&gt;=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x = 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"OperatorToken\">=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x ^= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"ExclusiveOrAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CaretEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">^=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x &= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"AndAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"AmpersandEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">&amp;=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("x |= 2");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AssignmentExpression kind=\"OrAssignmentExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"BarEqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"OperatorToken\">|=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">2</Token></LiteralExpression></AssignmentExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ConditionalExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("x ? 3 : 2");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ConditionalExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Condition\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"QuestionToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"QuestionToken\">?</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"WhenTrue\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Token\">3</Token></LiteralExpression><Token kind=\"ColonToken\" Operator=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"ColonToken\">:</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"WhenFalse\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Token\">2</Token></LiteralExpression></ConditionalExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ThisExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("this");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ThisExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><Token kind=\"ThisKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\" part=\"Token\">this</Token></ThisExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_BaseExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("base");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<BaseExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\"><Token kind=\"BaseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"4\" part=\"Token\">base</Token></BaseExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_MakeRefExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("__makeref(x)");
            var xElement = converter.Visit(node);
            Assert.AreEqual(
            "<MakeRefExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\">" +
                "<Token kind=\"MakeRefKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\" part=\"Keyword\">__makeref</Token>" +
                "<Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"OpenParenToken\">(</Token>" +
                "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Expression\">" +
                    "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Identifier\">x</Token>" +
                "</IdentifierName>" +
                "<Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"CloseParenToken\">)</Token>" +
            "</MakeRefExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_RefTypeExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("__reftype(x)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<RefTypeExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"RefTypeKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\" part=\"Keyword\">__reftype</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"CloseParenToken\">)</Token></RefTypeExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_RefValueExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("__refvalue(x, int)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<RefValueExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"18\"><Token kind=\"RefValueKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"10\" part=\"Keyword\">__refvalue</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CommaToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"Comma\">,</Token><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"17\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"17\" part=\"Keyword\">int</Token></PredefinedType><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"CloseParenToken\">)</Token></RefValueExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_CheckedExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("unchecked(y)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<CheckedExpression kind=\"UncheckedExpression\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"UncheckedKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\" part=\"Keyword\">unchecked</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Identifier\">y</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"CloseParenToken\">)</Token></CheckedExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("checked(x)");
            xElement = converter.Visit(node);
            Assert.AreEqual("<CheckedExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"10\"><Token kind=\"CheckedKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\" part=\"Keyword\">checked</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"CloseParenToken\">)</Token></CheckedExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_DefaultExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("default(x)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<DefaultExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"10\"><Token kind=\"DefaultKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\" part=\"Keyword\">default</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"CloseParenToken\">)</Token></DefaultExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_TypeOfExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("typeof(x)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<TypeOfExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\"><Token kind=\"TypeOfKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\" part=\"Keyword\">typeof</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseParenToken\">)</Token></TypeOfExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_SizeOfExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("sizeof(x)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<SizeOfExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"9\"><Token kind=\"SizeOfKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\" part=\"Keyword\">sizeof</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenParenToken\">(</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseParenToken\">)</Token></SizeOfExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_InvocationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("a(r,e)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<InvocationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token></IdentifierName><ArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"ArgumentList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">r</Token></IdentifierName></Argument><Argument startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">e</Token></IdentifierName></Argument></SeparatedList_of_Argument><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"CloseParenToken\">)</Token></ArgumentList></InvocationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ArgumentSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (InvocationExpressionSyntax)Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression("f(arg: ref 3)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<InvocationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"13\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">f</Token></IdentifierName><ArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"13\" part=\"ArgumentList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"12\"><NameColon startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"6\" part=\"NameColon\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">arg</Token></IdentifierName><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"ColonToken\">:</Token></NameColon><Token kind=\"RefKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"10\" part=\"RefOrOutKeyword\">ref</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Token\">3</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseParenToken\">)</Token></ArgumentList></InvocationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (InvocationExpressionSyntax)Microsoft.CodeAnalysis.CSharp.SyntaxFactory.ParseExpression("f(out x)");
            xElement = converter.Visit(node);
            Assert.AreEqual("<InvocationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">f</Token></IdentifierName><ArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"8\" part=\"ArgumentList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"7\"><Token kind=\"OutKeyword\" Keyword=\"true\" TypeParameterVariance=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"RefOrOutKeyword\">out</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">x</Token></IdentifierName></Argument></SeparatedList_of_Argument><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"CloseParenToken\">)</Token></ArgumentList></InvocationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ElementAccessSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (ElementAccessExpressionSyntax)SyntaxFactory.ParseExpression("x[\"a\"]");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ElementAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><BracketedArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Token\">a</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ElementAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (ElementAccessExpressionSyntax)SyntaxFactory.ParseExpression("x['a']");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ElementAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><BracketedArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\"><LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Expression\"><Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"5\" part=\"Token\">a</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ElementAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (ElementAccessExpressionSyntax)SyntaxFactory.ParseExpression("x[\"hj\", \"vb\"]");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ElementAccessExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"13\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">x</Token></IdentifierName><BracketedArgumentList startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"13\" part=\"ArgumentList\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"6\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"6\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"6\" part=\"Token\">hj</Token></LiteralExpression></Argument><Argument startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\"><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Token\">vb</Token></LiteralExpression></Argument></SeparatedList_of_Argument><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseBracketToken\">]</Token></BracketedArgumentList></ElementAccessExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_CastExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("(int)x");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<CastExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"4\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"4\" part=\"Keyword\">int</Token></PredefinedType><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"CloseParenToken\">)</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Identifier\">x</Token></IdentifierName></CastExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_AnonymousMethodExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = SyntaxFactory.ParseExpression("async delegate { return 4; }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousMethodExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"28\"><Token kind=\"AsyncKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"AsyncKeyword\">async</Token><Token kind=\"DelegateKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"14\" part=\"DelegateKeyword\">delegate</Token><Block startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"28\" part=\"Body\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"OpenBraceToken\">{</Token><List_of_Statement part=\"Statements\"><ReturnStatement startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"26\"><Token kind=\"ReturnKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"23\" part=\"ReturnKeyword\">return</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Token\">4</Token></LiteralExpression><Token kind=\"SemicolonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"SemicolonToken\">;</Token></ReturnStatement></List_of_Statement><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"28\" part=\"CloseBraceToken\">}</Token></Block></AnonymousMethodExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("async delegate(T x) { return 4; }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousMethodExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"33\"><Token kind=\"AsyncKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"AsyncKeyword\">async</Token><Token kind=\"DelegateKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"14\" part=\"DelegateKeyword\">delegate</Token><ParameterList startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"19\" part=\"ParameterList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Parameter part=\"Parameters\"><Parameter startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"18\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Identifier\">T</Token></IdentifierName><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"Identifier\">x</Token></Parameter></SeparatedList_of_Parameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"CloseParenToken\">)</Token></ParameterList><Block startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"33\" part=\"Body\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"21\" part=\"OpenBraceToken\">{</Token><List_of_Statement part=\"Statements\"><ReturnStatement startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"31\"><Token kind=\"ReturnKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"28\" part=\"ReturnKeyword\">return</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"30\" endLine=\"1\" endColumn=\"30\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"30\" endLine=\"1\" endColumn=\"30\" part=\"Token\">4</Token></LiteralExpression><Token kind=\"SemicolonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"31\" part=\"SemicolonToken\">;</Token></ReturnStatement></List_of_Statement><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"33\" part=\"CloseBraceToken\">}</Token></Block></AnonymousMethodExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("delegate(T r, K t) { yield break; }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousMethodExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"35\"><Token kind=\"DelegateKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\" part=\"DelegateKeyword\">delegate</Token><ParameterList startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"18\" part=\"ParameterList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Parameter part=\"Parameters\"><Parameter startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Identifier\">T</Token></IdentifierName><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Identifier\">r</Token></Parameter><Parameter startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"17\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Identifier\">K</Token></IdentifierName><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"Identifier\">t</Token></Parameter></SeparatedList_of_Parameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"CloseParenToken\">)</Token></ParameterList><Block startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"35\" part=\"Body\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"20\" part=\"OpenBraceToken\">{</Token><List_of_Statement part=\"Statements\"><YieldStatement kind=\"YieldBreakStatement\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"33\"><Token kind=\"YieldKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"26\" part=\"YieldKeyword\">yield</Token><Token kind=\"BreakKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"32\" part=\"ReturnOrBreakKeyword\">break</Token><Token kind=\"SemicolonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"33\" part=\"SemicolonToken\">;</Token></YieldStatement></List_of_Statement><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"35\" endLine=\"1\" endColumn=\"35\" part=\"CloseBraceToken\">}</Token></Block></AnonymousMethodExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_SimpleLambdaExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("async e => 4 }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<SimpleLambdaExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"AsyncKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"AsyncKeyword\">async</Token><Parameter startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Parameter\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">e</Token></Parameter><Token kind=\"EqualsGreaterThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"10\" part=\"ArrowToken\">=&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Body\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"Token\">4</Token></LiteralExpression></SimpleLambdaExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("e => 5");
            xElement = converter.Visit(node);
            Assert.AreEqual("<SimpleLambdaExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><Parameter startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Parameter\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">e</Token></Parameter><Token kind=\"EqualsGreaterThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"3\" endLine=\"1\" endColumn=\"4\" part=\"ArrowToken\">=&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Body\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Token\">5</Token></LiteralExpression></SimpleLambdaExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ParenthesizedLambdaExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("async (e) => 4");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ParenthesizedLambdaExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"14\"><Token kind=\"AsyncKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"AsyncKeyword\">async</Token><ParameterList startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"9\" part=\"ParameterList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Parameter part=\"Parameters\"><Parameter startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">e</Token></Parameter></SeparatedList_of_Parameter><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseParenToken\">)</Token></ParameterList><Token kind=\"EqualsGreaterThanToken\" Operator=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"12\" part=\"ArrowToken\">=&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Body\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Token\">4</Token></LiteralExpression></ParenthesizedLambdaExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_InitializerExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();

            var node = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new F { 4, 'r', \"hello world\" }");
            var xElement = converter.Visit(node.Initializer);
            Assert.AreEqual(
            "<InitializerExpression kind=\"CollectionInitializerExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"31\">" +
                "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBraceToken\">{</Token>" +
                "<SeparatedList_of_Expression part=\"Expressions\">" +
                    "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\">" +
                        "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Token\">4</Token>" +
                    "</LiteralExpression>" +
                    "<LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"14\">" +
                        "<Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"14\" part=\"Token\">r</Token>" +
                    "</LiteralExpression>" +
                    "<LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"29\">" +
                        "<Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"29\" part=\"Token\">hello world</Token>" +
                    "</LiteralExpression>" +
                "</SeparatedList_of_Expression>" +
                "<Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"31\" part=\"CloseBraceToken\">}</Token>" +
            "</InitializerExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new F { 4, {3, 'r'}, \"hello world\" }");
            xElement = converter.Visit(node.Initializer);
            Assert.AreEqual(
            "<InitializerExpression kind=\"CollectionInitializerExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"36\">" +
                "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBraceToken\">{</Token>" +
                "<SeparatedList_of_Expression part=\"Expressions\">" +
                    "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\">" +
                        "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Token\">4</Token>" +
                    "</LiteralExpression>" +
                    "<InitializerExpression kind=\"ComplexElementInitializerExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"19\">" +
                        "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"OpenBraceToken\">{</Token>" +
                        "<SeparatedList_of_Expression part=\"Expressions\">" +
                            "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\">" +
                                "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"Token\">3</Token>" +
                            "</LiteralExpression>" +
                            "<LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"18\">" +
                                "<Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"18\" part=\"Token\">r</Token>" +
                            "</LiteralExpression>" +
                        "</SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"CloseBraceToken\">}</Token>" +
                    "</InitializerExpression>" +
                    "<LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"34\">" +
                        "<Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"34\" part=\"Token\">hello world</Token>" +
                    "</LiteralExpression>" +
                "</SeparatedList_of_Expression>" +
                "<Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"36\" endLine=\"1\" endColumn=\"36\" part=\"CloseBraceToken\">}</Token>" +
            "</InitializerExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            var node2 = (ArrayCreationExpressionSyntax)SyntaxFactory.ParseExpression("new object[] { 4, 'r', \"hello world\" }");
            xElement = converter.Visit(node.Initializer);
            Assert.AreEqual("<InitializerExpression kind=\"CollectionInitializerExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"36\">" +
                                "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBraceToken\">{</Token>" +
                                "<SeparatedList_of_Expression part=\"Expressions\">" +
                                    "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\">" +
                                        "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"Token\">4</Token>" +
                                    "</LiteralExpression>" +
                                    "<InitializerExpression kind=\"ComplexElementInitializerExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"19\">" +
                                        "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"OpenBraceToken\">{</Token>" +
                                        "<SeparatedList_of_Expression part=\"Expressions\">" +
                                            "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\">" +
                                                "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"Token\">3</Token>" +
                                            "</LiteralExpression>" +
                                            "<LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"18\">" +
                                                "<Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"18\" part=\"Token\">r</Token>" +
                                            "</LiteralExpression>" +
                                        "</SeparatedList_of_Expression>" +
                                        "<Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"CloseBraceToken\">}</Token>" +
                                    "</InitializerExpression>" +
                                    "<LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"34\">" +
                                        "<Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"34\" part=\"Token\">hello world</Token>" +
                                    "</LiteralExpression>" +
                                "</SeparatedList_of_Expression>" +
                                "<Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"36\" endLine=\"1\" endColumn=\"36\" part=\"CloseBraceToken\">}</Token>" +
                            "</InitializerExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new F { Text = 4, Prop = 'r', PropTex = \"hello world\" }");
            xElement = converter.Visit(node.Initializer);
            Assert.AreEqual(
            "<InitializerExpression kind=\"ObjectInitializerExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"55\">" +
                "<Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBraceToken\">{</Token>" +
                "<SeparatedList_of_Expression part=\"Expressions\">" +
                    "<AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"16\">" +
                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Left\">" +
                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Identifier\">Text</Token>" +
                        "</IdentifierName>" +
                        "<Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"OperatorToken\">=</Token>" +
                        "<LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Right\">" +
                            "<Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Token\">4</Token>" +
                        "</LiteralExpression>" +
                    "</AssignmentExpression>" +
                    "<AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"28\">" +
                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"22\" part=\"Left\">" +
                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"22\" part=\"Identifier\">Prop</Token>" +
                        "</IdentifierName>" +
                        "<Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"24\" part=\"OperatorToken\">=</Token>" +
                        "<LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"28\" part=\"Right\">" +
                            "<Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"28\" part=\"Token\">r</Token>" +
                        "</LiteralExpression>" +
                    "</AssignmentExpression>" +
                    "<AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"53\">" +
                        "<IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"37\" part=\"Left\">" +
                            "<Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"37\" part=\"Identifier\">PropTex</Token>" +
                        "</IdentifierName>" +
                        "<Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"39\" part=\"OperatorToken\">=</Token>" +
                        "<LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"53\" part=\"Right\">" +
                            "<Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"53\" part=\"Token\">hello world</Token>" +
                        "</LiteralExpression>" +
                    "</AssignmentExpression>" +
                "</SeparatedList_of_Expression>" +
                "<Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"55\" endLine=\"1\" endColumn=\"55\" part=\"CloseBraceToken\">}</Token>" +
            "</InitializerExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ObjectCreationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new F() { Text = 4, Prop = 'r', PropTex = \"hello world\" }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"57\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">F</Token></IdentifierName><ArgumentList startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"7\" part=\"ArgumentList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"OpenParenToken\">(</Token><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"CloseParenToken\">)</Token></ArgumentList><InitializerExpression kind=\"ObjectInitializerExpression\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"57\" part=\"Initializer\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_Expression part=\"Expressions\"><AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"18\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"14\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"14\" part=\"Identifier\">Text</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"OperatorToken\">=</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"Token\">4</Token></LiteralExpression></AssignmentExpression><AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"30\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"24\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"24\" part=\"Identifier\">Prop</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"OperatorToken\">=</Token><LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Right\"><Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Token\">r</Token></LiteralExpression></AssignmentExpression><AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"55\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"39\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"39\" part=\"Identifier\">PropTex</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"OperatorToken\">=</Token><LiteralExpression kind=\"StringLiteralExpression\" startLine=\"1\" startColumn=\"43\" endLine=\"1\" endColumn=\"55\" part=\"Right\"><Token kind=\"StringLiteralToken\" startLine=\"1\" startColumn=\"43\" endLine=\"1\" endColumn=\"55\" part=\"Token\">hello world</Token></LiteralExpression></AssignmentExpression></SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"57\" endLine=\"1\" endColumn=\"57\" part=\"CloseBraceToken\">}</Token></InitializerExpression></ObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (ObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new B(c, f){ A = 3+4 }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"22\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">B</Token></IdentifierName><ArgumentList startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"11\" part=\"ArgumentList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">c</Token></IdentifierName></Argument><Argument startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Identifier\">f</Token></IdentifierName></Argument></SeparatedList_of_Argument><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"CloseParenToken\">)</Token></ArgumentList><InitializerExpression kind=\"ObjectInitializerExpression\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"22\" part=\"Initializer\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_Expression part=\"Expressions\"><AssignmentExpression kind=\"SimpleAssignmentExpression\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"20\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Identifier\">A</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"OperatorToken\">=</Token><BinaryExpression kind=\"AddExpression\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"20\" part=\"Right\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"Left\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"18\" endLine=\"1\" endColumn=\"18\" part=\"Token\">3</Token></LiteralExpression><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"OperatorToken\">+</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"20\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"20\" endLine=\"1\" endColumn=\"20\" part=\"Token\">4</Token></LiteralExpression></BinaryExpression></AssignmentExpression></SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"22\" part=\"CloseBraceToken\">}</Token></InitializerExpression></ObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_AnonymousObjectMemberDeclaratorSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (AnonymousObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new { Text = 4 }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"16\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_AnonymousObjectMemberDeclarator part=\"Initializers\"><AnonymousObjectMemberDeclarator startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"14\"><NameEquals startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\" part=\"NameEquals\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"10\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"10\" part=\"Identifier\">Text</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"EqualsToken\">=</Token></NameEquals><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Token\">4</Token></LiteralExpression></AnonymousObjectMemberDeclarator></SeparatedList_of_AnonymousObjectMemberDeclarator><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"CloseBraceToken\">}</Token></AnonymousObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (AnonymousObjectCreationExpressionSyntax)SyntaxFactory.ParseExpression("new { a.Text }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"14\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_AnonymousObjectMemberDeclarator part=\"Initializers\"><AnonymousObjectMemberDeclarator startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\"><MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">a</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Identifier\">Text</Token></IdentifierName></MemberAccessExpression></AnonymousObjectMemberDeclarator></SeparatedList_of_AnonymousObjectMemberDeclarator><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"CloseBraceToken\">}</Token></AnonymousObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_AnonymousObjectCreationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("new { Text = 4 }");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"16\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_AnonymousObjectMemberDeclarator part=\"Initializers\"><AnonymousObjectMemberDeclarator startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"14\"><NameEquals startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\" part=\"NameEquals\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"10\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"10\" part=\"Identifier\">Text</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"EqualsToken\">=</Token></NameEquals><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Token\">4</Token></LiteralExpression></AnonymousObjectMemberDeclarator></SeparatedList_of_AnonymousObjectMemberDeclarator><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"CloseBraceToken\">}</Token></AnonymousObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("new { a.Text, Y = 6 }");
            xElement = converter.Visit(node);
            Assert.AreEqual("<AnonymousObjectCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"21\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_AnonymousObjectMemberDeclarator part=\"Initializers\"><AnonymousObjectMemberDeclarator startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\"><MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">a</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"12\" part=\"Identifier\">Text</Token></IdentifierName></MemberAccessExpression></AnonymousObjectMemberDeclarator><AnonymousObjectMemberDeclarator startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"19\"><NameEquals startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"17\" part=\"NameEquals\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"Identifier\">Y</Token></IdentifierName><Token kind=\"EqualsToken\" Operator=\"true\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"EqualsToken\">=</Token></NameEquals><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"19\" part=\"Token\">6</Token></LiteralExpression></AnonymousObjectMemberDeclarator></SeparatedList_of_AnonymousObjectMemberDeclarator><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"21\" part=\"CloseBraceToken\">}</Token></AnonymousObjectCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ArrayCreationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("new A[4, 6]");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ArrayCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"11\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><ArrayType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"11\" part=\"Type\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"ElementType\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">A</Token></IdentifierName><List_of_ArrayRankSpecifier part=\"RankSpecifiers\"><ArrayRankSpecifier startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"11\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Expression part=\"Sizes\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Token\">4</Token></LiteralExpression><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Token\">6</Token></LiteralExpression></SeparatedList_of_Expression><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"CloseBracketToken\">]</Token></ArrayRankSpecifier></List_of_ArrayRankSpecifier></ArrayType></ArrayCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("new A[]{ 4, 5, 6}");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ArrayCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"17\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><ArrayType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"7\" part=\"Type\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"ElementType\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"Identifier\">A</Token></IdentifierName><List_of_ArrayRankSpecifier part=\"RankSpecifiers\"><ArrayRankSpecifier startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"7\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Expression part=\"Sizes\"><OmittedArraySizeExpression><Token kind=\"OmittedArraySizeExpressionToken\" part=\"OmittedArraySizeExpressionToken\"></Token></OmittedArraySizeExpression></SeparatedList_of_Expression><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"CloseBracketToken\">]</Token></ArrayRankSpecifier></List_of_ArrayRankSpecifier></ArrayType><InitializerExpression kind=\"ArrayInitializerExpression\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"17\" part=\"Initializer\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_Expression part=\"Expressions\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Token\">4</Token></LiteralExpression><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"Token\">5</Token></LiteralExpression><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Token\">6</Token></LiteralExpression></SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"CloseBraceToken\">}</Token></InitializerExpression></ArrayCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_ImplicitArrayCreationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("new [, ,]{4, 6}");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ImplicitArrayCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"15\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBracketToken\">[</Token><TokenList part=\"Commas\"><Token kind=\"CommaToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\">,</Token><Token kind=\"CommaToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\">,</Token></TokenList><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"9\" part=\"CloseBracketToken\">]</Token><InitializerExpression kind=\"ArrayInitializerExpression\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"15\" part=\"Initializer\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_Expression part=\"Expressions\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Token\">4</Token></LiteralExpression><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Token\">6</Token></LiteralExpression></SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"15\" endLine=\"1\" endColumn=\"15\" part=\"CloseBraceToken\">}</Token></InitializerExpression></ImplicitArrayCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));

            node = SyntaxFactory.ParseExpression("new []{4, 6}");
            xElement = converter.Visit(node);
            Assert.AreEqual("<ImplicitArrayCreationExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"12\"><Token kind=\"NewKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"NewKeyword\">new</Token><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"OpenBracketToken\">[</Token><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"CloseBracketToken\">]</Token><InitializerExpression kind=\"ArrayInitializerExpression\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"12\" part=\"Initializer\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"OpenBraceToken\">{</Token><SeparatedList_of_Expression part=\"Expressions\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Token\">4</Token></LiteralExpression><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"11\" endLine=\"1\" endColumn=\"11\" part=\"Token\">6</Token></LiteralExpression></SeparatedList_of_Expression><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"12\" endLine=\"1\" endColumn=\"12\" part=\"CloseBraceToken\">}</Token></InitializerExpression></ImplicitArrayCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ExpressionSyntax_StackAllocArrayCreationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            Func<string, StackAllocArrayCreationExpressionSyntax> function = delegate (string s)
            {
                var node = (LocalDeclarationStatementSyntax)SyntaxFactory.ParseStatement(s);
                return (StackAllocArrayCreationExpressionSyntax)node.Declaration.Variables[0].Initializer.Value;
            };

            var n = function("int* f = stackalloc int[2]");
            var xElement = converter.Visit(n);
            Assert.AreEqual("<StackAllocArrayCreationExpression startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"26\"><Token kind=\"StackAllocKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"19\" part=\"StackAllocKeyword\">stackalloc</Token><ArrayType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"26\" part=\"Type\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"23\" part=\"ElementType\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"21\" endLine=\"1\" endColumn=\"23\" part=\"Keyword\">int</Token></PredefinedType><List_of_ArrayRankSpecifier part=\"RankSpecifiers\"><ArrayRankSpecifier startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"26\"><Token kind=\"OpenBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"24\" part=\"OpenBracketToken\">[</Token><SeparatedList_of_Expression part=\"Sizes\"><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Token\">2</Token></LiteralExpression></SeparatedList_of_Expression><Token kind=\"CloseBracketToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"CloseBracketToken\">]</Token></ArrayRankSpecifier></List_of_ArrayRankSpecifier></ArrayType></StackAllocArrayCreationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }
    }
}
