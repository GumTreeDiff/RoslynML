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
        public void CasePatternSwitchLabelSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case 'a' when obj.Lenght > 0: }");
            var xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]));
            Assert.AreEqual("<CasePatternSwitchLabel startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"42\"><Token kind=\"CaseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"17\" part=\"Keyword\">case</Token><ConstantPattern startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\" part=\"Pattern\"><LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\" part=\"Expression\"><Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\" part=\"Token\">a</Token></LiteralExpression></ConstantPattern><WhenClause startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"41\" part=\"WhenClause\"><Token kind=\"WhenKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"26\" part=\"WhenKeyword\">when</Token><BinaryExpression kind=\"GreaterThanExpression\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"41\" part=\"Condition\"><MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"37\" part=\"Left\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Identifier\">obj</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"31\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"37\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"37\" part=\"Identifier\">Lenght</Token></IdentifierName></MemberAccessExpression><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"39\" part=\"OperatorToken\">&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"Token\">0</Token></LiteralExpression></BinaryExpression></WhenClause><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"42\" endLine=\"1\" endColumn=\"42\" part=\"ColonToken\">:</Token></CasePatternSwitchLabel>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case Shape s: }");
            xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]));
            Assert.AreEqual("<CasePatternSwitchLabel startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"26\"><Token kind=\"CaseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"17\" part=\"Keyword\">case</Token><DeclarationPattern startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"25\" part=\"Pattern\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Identifier\">Shape</Token></IdentifierName><SingleVariableDesignation startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Designation\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Identifier\">s</Token></SingleVariableDesignation></DeclarationPattern><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"26\" endLine=\"1\" endColumn=\"26\" part=\"ColonToken\">:</Token></CasePatternSwitchLabel>", xElement.ToString(SaveOptions.DisableFormatting));

            node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case Shape s when s.Lenght > 0: }");
            xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]));
            Assert.AreEqual("<CasePatternSwitchLabel startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"44\"><Token kind=\"CaseKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"17\" part=\"Keyword\">case</Token><DeclarationPattern startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"25\" part=\"Pattern\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Identifier\">Shape</Token></IdentifierName><SingleVariableDesignation startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Designation\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Identifier\">s</Token></SingleVariableDesignation></DeclarationPattern><WhenClause startLine=\"1\" startColumn=\"27\" endLine=\"1\" endColumn=\"43\" part=\"WhenClause\"><Token kind=\"WhenKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"27\" endLine=\"1\" endColumn=\"30\" part=\"WhenKeyword\">when</Token><BinaryExpression kind=\"GreaterThanExpression\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"43\" part=\"Condition\"><MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"39\" part=\"Left\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"32\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"32\" part=\"Identifier\">s</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"33\" endLine=\"1\" endColumn=\"33\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"34\" endLine=\"1\" endColumn=\"39\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"34\" endLine=\"1\" endColumn=\"39\" part=\"Identifier\">Lenght</Token></IdentifierName></MemberAccessExpression><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"OperatorToken\">&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"43\" endLine=\"1\" endColumn=\"43\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"43\" endLine=\"1\" endColumn=\"43\" part=\"Token\">0</Token></LiteralExpression></BinaryExpression></WhenClause><Token kind=\"ColonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"44\" endLine=\"1\" endColumn=\"44\" part=\"ColonToken\">:</Token></CasePatternSwitchLabel>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void WhenClauseSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case 'a' when obj.Lenght > 0: }");
            var xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]).WhenClause);
            Assert.AreEqual("<WhenClause startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"41\"><Token kind=\"WhenKeyword\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"26\" part=\"WhenKeyword\">when</Token><BinaryExpression kind=\"GreaterThanExpression\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"41\" part=\"Condition\"><MemberAccessExpression kind=\"SimpleMemberAccessExpression\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"37\" part=\"Left\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"28\" endLine=\"1\" endColumn=\"30\" part=\"Identifier\">obj</Token></IdentifierName><Token kind=\"DotToken\" Operator=\"true\" startLine=\"1\" startColumn=\"31\" endLine=\"1\" endColumn=\"31\" part=\"OperatorToken\">.</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"37\" part=\"Name\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"32\" endLine=\"1\" endColumn=\"37\" part=\"Identifier\">Lenght</Token></IdentifierName></MemberAccessExpression><Token kind=\"GreaterThanToken\" startLine=\"1\" startColumn=\"39\" endLine=\"1\" endColumn=\"39\" part=\"OperatorToken\">&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"Right\"><Token kind=\"NumericLiteralToken\" startLine=\"1\" startColumn=\"41\" endLine=\"1\" endColumn=\"41\" part=\"Token\">0</Token></LiteralExpression></BinaryExpression></WhenClause>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TupleTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseTypeName("(string, int)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<TupleType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"13\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><SeparatedList_of_TupleElement part=\"Elements\"><TupleElement startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Type\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Keyword\">string</Token></PredefinedType></TupleElement><TupleElement startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Keyword\">int</Token></PredefinedType></TupleElement></SeparatedList_of_TupleElement><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseParenToken\">)</Token></TupleType>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TupleExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("(string, int)");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<TupleExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"13\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><SeparatedList_of_Argument part=\"Arguments\"><Argument startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Keyword\">string</Token></PredefinedType></Argument><Argument startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Expression\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Keyword\">int</Token></PredefinedType></Argument></SeparatedList_of_Argument><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseParenToken\">)</Token></TupleExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void TupleElementSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var type = SyntaxFactory.ParseTypeName("(string, int)");
            var node = SyntaxFactory.TupleElement(type, SyntaxFactory.Identifier("a"));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<TupleElement startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"14\"><TupleType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"13\" part=\"Type\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><SeparatedList_of_TupleElement part=\"Elements\"><TupleElement startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Type\"><Token kind=\"StringKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"7\" part=\"Keyword\">string</Token></PredefinedType></TupleElement><TupleElement startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"12\" part=\"Keyword\">int</Token></PredefinedType></TupleElement></SeparatedList_of_TupleElement><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"CloseParenToken\">)</Token></TupleType><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Identifier\">a</Token></TupleElement>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ThrowExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("throw e");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ThrowExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\"><Token kind=\"ThrowKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\" part=\"ThrowKeyword\">throw</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"7\" endLine=\"1\" endColumn=\"7\" part=\"Identifier\">e</Token></IdentifierName></ThrowExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void SingleVariableDesignationSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.SingleVariableDesignation(SyntaxFactory.Identifier("a"));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<SingleVariableDesignation startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"Identifier\">a</Token></SingleVariableDesignation>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void DiscardDesignationSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.DiscardDesignation(SyntaxFactory.Token(SyntaxKind.UnderscoreToken));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<DiscardDesignation startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\"><Token kind=\"UnderscoreToken\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"UnderscoreToken\">_</Token></DiscardDesignation>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ParenthesizedVariableDesignationSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParenthesizedVariableDesignation(SyntaxFactory.Token(SyntaxKind.OpenParenToken), 
                SyntaxFactory.SeparatedList(new VariableDesignationSyntax[]
                {
                    SyntaxFactory.SingleVariableDesignation(SyntaxFactory.Identifier("a")),
                     SyntaxFactory.DiscardDesignation(SyntaxFactory.Token(SyntaxKind.UnderscoreToken))
                }),
                SyntaxFactory.Token(SyntaxKind.CloseParenToken));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ParenthesizedVariableDesignation startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"5\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"1\" part=\"OpenParenToken\">(</Token><SeparatedList_of_VariableDesignation part=\"Variables\"><SingleVariableDesignation startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"2\" part=\"Identifier\">a</Token></SingleVariableDesignation><DiscardDesignation startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\"><Token kind=\"UnderscoreToken\" Keyword=\"true\" Contextual=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"UnderscoreToken\">_</Token></DiscardDesignation></SeparatedList_of_VariableDesignation><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"5\" part=\"CloseParenToken\">)</Token></ParenthesizedVariableDesignation>", xElement.ToString(SaveOptions.DisableFormatting));
        }
        
        [TestMethod]
        public void RefExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.RefExpression(SyntaxFactory.Token(SyntaxKind.RefKeyword), SyntaxFactory.ParseExpression("s + d"));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<RefExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"8\"><Token kind=\"RefKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"RefKeyword\">ref</Token><BinaryExpression kind=\"AddExpression\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"8\" part=\"Expression\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Left\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"4\" part=\"Identifier\">s</Token></IdentifierName><Token kind=\"PlusToken\" Operator=\"true\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"OperatorToken\">+</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Right\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">d</Token></IdentifierName></BinaryExpression></RefExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void DeclarationExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (TupleExpressionSyntax)SyntaxFactory.ParseExpression("(int a, int b)");
            var xElement = converter.Visit(node.Arguments[0].Expression);
            Assert.AreEqual("<DeclarationExpression startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"6\"><PredefinedType TypeSyntax=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"4\" part=\"Type\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"2\" endLine=\"1\" endColumn=\"4\" part=\"Keyword\">int</Token></PredefinedType><SingleVariableDesignation startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Designation\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"6\" endLine=\"1\" endColumn=\"6\" part=\"Identifier\">a</Token></SingleVariableDesignation></DeclarationExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void IsPatternExpressionSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseExpression("obj is A a");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<IsPatternExpression startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"10\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"Identifier\">obj</Token></IdentifierName><Token kind=\"IsKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"5\" endLine=\"1\" endColumn=\"6\" part=\"IsKeyword\">is</Token><DeclarationPattern startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"10\" part=\"Pattern\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"Identifier\">A</Token></IdentifierName><SingleVariableDesignation startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Designation\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"10\" endLine=\"1\" endColumn=\"10\" part=\"Identifier\">a</Token></SingleVariableDesignation></DeclarationPattern></IsPatternExpression>", xElement.ToString(SaveOptions.DisableFormatting));
        }
        
        [TestMethod]
        public void ForEachVariableStatementSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.ParseStatement("foreach(var (a,b) in x){}");
            var xElement = converter.Visit(node);
            Assert.AreEqual("<ForEachVariableStatement startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"25\"><Token kind=\"ForEachKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"7\" part=\"ForEachKeyword\">foreach</Token><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"8\" endLine=\"1\" endColumn=\"8\" part=\"OpenParenToken\">(</Token><DeclarationExpression startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"17\" part=\"Variable\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"11\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"9\" endLine=\"1\" endColumn=\"11\" part=\"Identifier\">var</Token></IdentifierName><ParenthesizedVariableDesignation startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"17\" part=\"Designation\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"13\" endLine=\"1\" endColumn=\"13\" part=\"OpenParenToken\">(</Token><SeparatedList_of_VariableDesignation part=\"Variables\"><SingleVariableDesignation startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"14\" endLine=\"1\" endColumn=\"14\" part=\"Identifier\">a</Token></SingleVariableDesignation><SingleVariableDesignation startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"16\" endLine=\"1\" endColumn=\"16\" part=\"Identifier\">b</Token></SingleVariableDesignation></SeparatedList_of_VariableDesignation><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"17\" endLine=\"1\" endColumn=\"17\" part=\"CloseParenToken\">)</Token></ParenthesizedVariableDesignation></DeclarationExpression><Token kind=\"InKeyword\" Keyword=\"true\" TypeParameterVariance=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"20\" part=\"InKeyword\">in</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"22\" part=\"Expression\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"22\" endLine=\"1\" endColumn=\"22\" part=\"Identifier\">x</Token></IdentifierName><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"23\" endLine=\"1\" endColumn=\"23\" part=\"CloseParenToken\">)</Token><Block startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"25\" part=\"Statement\"><Token kind=\"OpenBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"24\" endLine=\"1\" endColumn=\"24\" part=\"OpenBraceToken\">{</Token><Token kind=\"CloseBraceToken\" Punctuation=\"true\" Language=\"true\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"CloseBraceToken\">}</Token></Block></ForEachVariableStatement>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void DeclarationPatternSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case Shape s: }");
            var xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]).Pattern);
            Assert.AreEqual("<DeclarationPattern startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"25\"><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"23\" part=\"Identifier\">Shape</Token></IdentifierName><SingleVariableDesignation startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Designation\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"25\" endLine=\"1\" endColumn=\"25\" part=\"Identifier\">s</Token></SingleVariableDesignation></DeclarationPattern>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void ConstantPatternSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (SwitchStatementSyntax)SyntaxFactory.ParseStatement("switch(obj){ case 'a' when obj.Lenght > 0: }");
            var xElement = converter.Visit(((CasePatternSwitchLabelSyntax)node.Sections[0].Labels[0]).Pattern);
            Assert.AreEqual("<ConstantPattern startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\"><LiteralExpression kind=\"CharacterLiteralExpression\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\" part=\"Expression\"><Token kind=\"CharacterLiteralToken\" startLine=\"1\" startColumn=\"19\" endLine=\"1\" endColumn=\"21\" part=\"Token\">a</Token></LiteralExpression></ConstantPattern>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void LocalFunctionStatementSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = (MethodDeclarationSyntax)SyntaxFactory.ParseCompilationUnit(@"
            void m()
            {
                int local() => 0;
            }").Members[0];
            var xElement = converter.Visit(node.Body.Statements[0]);
            Assert.AreEqual("<LocalFunctionStatement startLine=\"4\" startColumn=\"17\" endLine=\"4\" endColumn=\"33\"><PredefinedType TypeSyntax=\"true\" startLine=\"4\" startColumn=\"17\" endLine=\"4\" endColumn=\"19\" part=\"ReturnType\"><Token kind=\"IntKeyword\" Keyword=\"true\" startLine=\"4\" startColumn=\"17\" endLine=\"4\" endColumn=\"19\" part=\"Keyword\">int</Token></PredefinedType><Token kind=\"IdentifierToken\" startLine=\"4\" startColumn=\"21\" endLine=\"4\" endColumn=\"25\" part=\"Identifier\">local</Token><ParameterList startLine=\"4\" startColumn=\"26\" endLine=\"4\" endColumn=\"27\" part=\"ParameterList\"><Token kind=\"OpenParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"4\" startColumn=\"26\" endLine=\"4\" endColumn=\"26\" part=\"OpenParenToken\">(</Token><Token kind=\"CloseParenToken\" Punctuation=\"true\" Language=\"true\" startLine=\"4\" startColumn=\"27\" endLine=\"4\" endColumn=\"27\" part=\"CloseParenToken\">)</Token></ParameterList><ArrowExpressionClause startLine=\"4\" startColumn=\"29\" endLine=\"4\" endColumn=\"32\" part=\"ExpressionBody\"><Token kind=\"EqualsGreaterThanToken\" Operator=\"true\" startLine=\"4\" startColumn=\"29\" endLine=\"4\" endColumn=\"30\" part=\"ArrowToken\">=&gt;</Token><LiteralExpression kind=\"NumericLiteralExpression\" startLine=\"4\" startColumn=\"32\" endLine=\"4\" endColumn=\"32\" part=\"Expression\"><Token kind=\"NumericLiteralToken\" startLine=\"4\" startColumn=\"32\" endLine=\"4\" endColumn=\"32\" part=\"Token\">0</Token></LiteralExpression></ArrowExpressionClause><Token kind=\"SemicolonToken\" Punctuation=\"true\" Language=\"true\" startLine=\"4\" startColumn=\"33\" endLine=\"4\" endColumn=\"33\" part=\"SemicolonToken\">;</Token></LocalFunctionStatement>", xElement.ToString(SaveOptions.DisableFormatting));
        }

        [TestMethod]
        public void RefTypeSyntax_RoslynMLFromRoslyn_OK()
        {
            var converter = new RoslynML();
            var node = SyntaxFactory.RefType(SyntaxFactory.Token(SyntaxKind.RefKeyword), SyntaxFactory.ParseTypeName("var"));
            var xElement = converter.Visit(node);
            Assert.AreEqual("<RefType startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"6\"><Token kind=\"RefKeyword\" Keyword=\"true\" startLine=\"1\" startColumn=\"1\" endLine=\"1\" endColumn=\"3\" part=\"RefKeyword\">ref</Token><IdentifierName Name=\"true\" TypeSyntax=\"true\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"6\" part=\"Type\"><Token kind=\"IdentifierToken\" startLine=\"1\" startColumn=\"4\" endLine=\"1\" endColumn=\"6\" part=\"Identifier\">var</Token></IdentifierName></RefType>", xElement.ToString(SaveOptions.DisableFormatting));
        }
    }
}
