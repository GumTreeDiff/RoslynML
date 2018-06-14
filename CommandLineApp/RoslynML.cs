using System;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace CommandLineApp
{
    public partial class RoslynML : CSharpSyntaxVisitor<XElement>
    {
    	private static bool IsOperator(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.TildeToken:
                case SyntaxKind.ExclamationToken:
                case SyntaxKind.PercentToken:
                case SyntaxKind.CaretToken:
                case SyntaxKind.AmpersandToken:
                case SyntaxKind.AsteriskToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.EqualsToken:
                case SyntaxKind.BarToken:
                case SyntaxKind.ColonToken:
                case SyntaxKind.LessThanToken:
                case SyntaxKind.GreaterThanToken:
                case SyntaxKind.DotToken:
                case SyntaxKind.QuestionToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.BarBarToken:
                case SyntaxKind.AmpersandAmpersandToken:
                case SyntaxKind.MinusMinusToken:
                case SyntaxKind.PlusPlusToken:
                case SyntaxKind.ColonColonToken:
                case SyntaxKind.QuestionQuestionToken:
                case SyntaxKind.MinusGreaterThanToken:
                case SyntaxKind.ExclamationEqualsToken:
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.EqualsGreaterThanToken:
                case SyntaxKind.LessThanEqualsToken:
                case SyntaxKind.LessThanLessThanToken:
                case SyntaxKind.LessThanLessThanEqualsToken:
                case SyntaxKind.GreaterThanEqualsToken:
                case SyntaxKind.GreaterThanGreaterThanToken:
                case SyntaxKind.GreaterThanGreaterThanEqualsToken:
                case SyntaxKind.SlashEqualsToken:
                case SyntaxKind.AsteriskEqualsToken:
                case SyntaxKind.BarEqualsToken:
                case SyntaxKind.AmpersandEqualsToken:
                case SyntaxKind.PlusEqualsToken:
                case SyntaxKind.MinusEqualsToken:
                case SyntaxKind.CaretEqualsToken:
                case SyntaxKind.PercentEqualsToken:
                    return true;
        
                default:
                    return false;
            }
        }
    
        private static void ClassificationForPunctuation(XElement element, Microsoft.CodeAnalysis.SyntaxToken token)
        {
            if (IsOperator(token.Kind()))
            {
                // special cases...
                switch (token.Kind())
                {
                    case SyntaxKind.LessThanToken:
                    case SyntaxKind.GreaterThanToken:
                        // the < and > tokens of a type parameter list should be classified as
                        // punctuation; otherwise, they're operators.
                        if (token.Parent != null)
                        {
                            if (token.Parent.Kind() == SyntaxKind.TypeParameterList ||
                                token.Parent.Kind() == SyntaxKind.TypeArgumentList)
                            {
                                element.Add(new XAttribute("Punctuation", true));
    							if (SyntaxFacts.IsLanguagePunctuation(token.Kind()))
    							{
    							    element.Add(new XAttribute("Language", true));
    							}
    							if (SyntaxFacts.IsPreprocessorPunctuation(token.Kind()))
    							{
    							    element.Add(new XAttribute("Preprocessor", true));
    							}
                            }
                        }
    
                        break;
                    case SyntaxKind.ColonToken:
                        // the : for inheritance/implements or labels should be classified as
                        // punctuation; otherwise, it's from a conditional operator.
                        if (token.Parent != null)
                        {
                            if (token.Parent.Kind() != SyntaxKind.ConditionalExpression)
                            {
                                element.Add(new XAttribute("Punctuation", true));
                            }
    						else
    						{
    							element.Add(new XAttribute("Operator", true));
    						}
    						if (SyntaxFacts.IsLanguagePunctuation(token.Kind()))
    						{
    						    element.Add(new XAttribute("Language", true));
    						}
    						if (SyntaxFacts.IsPreprocessorPunctuation(token.Kind()))
    						{
    						    element.Add(new XAttribute("Preprocessor", true));
    						}
                        }
                        break;
    				default:
    				    element.Add(new XAttribute("Operator", true));
    					break;
                }            
            }
            else
            {
                element.Add(new XAttribute("Punctuation", true));
    			if (SyntaxFacts.IsLanguagePunctuation(token.Kind()))
    			{
    			    element.Add(new XAttribute("Language", true));
    			}
    			if (SyntaxFacts.IsPreprocessorPunctuation(token.Kind()))
    			{
    			    element.Add(new XAttribute("Preprocessor", true));
    			}
            }
        }
    
        /// <summary>
        /// Annotates an element with syntax metadata.
        /// </summary>
        /// <param name="element">the XML element being serialized.</param>
        /// <param name="node">the node being represented by the serializing XML element.</param>
        protected virtual void Annotate(XElement element, Microsoft.CodeAnalysis.SyntaxToken node)
        {
            element.Add(new XAttribute("kind", Enum.GetName(typeof (SyntaxKind), node.Kind())));
    		if (SyntaxFacts.IsKeywordKind(node.Kind()))
            {
                element.Add(new XAttribute("Keyword", true));
    			if (SyntaxFacts.IsAccessorDeclarationKeyword(node.Kind()))
    			{
    			    element.Add(new XAttribute("AccessorDeclaration", true));
    			}
    			if (SyntaxFacts.IsContextualKeyword(node.Kind()))
    			{
    			    element.Add(new XAttribute("Contextual", true));
    			}    
    			if (SyntaxFacts.IsQueryContextualKeyword(node.Kind()))
    			{
    			    element.Add(new XAttribute("Query", true));
    			}
    			if (SyntaxFacts.IsTypeParameterVarianceKeyword(node.Kind()))
    			{
    			    element.Add(new XAttribute("TypeParameterVariance", true));
    			}
            }
            else if (SyntaxFacts.IsPunctuation(node.Kind()))
            {
                ClassificationForPunctuation(element, node);
            }
    
            // if (SyntaxFacts.IsPrefixUnaryExpressionOperatorToken(node.Kind()))
            // {
            //     element.Add(new XAttribute("PrefixUnaryOperator", true));
            // }
    		// 
            // if (SyntaxFacts.IsPostfixUnaryExpressionToken(node.Kind()))
            // {
            //     element.Add(new XAttribute("PostfixUnaryOperator", true));
            // }
    
            // if (SyntaxFacts.IsLiteralExpression(node.Kind()))
            // {
            //     element.Add(new XAttribute("Literal", true));
            // }
    
            // if (SyntaxFacts.IsInstanceExpression(node.Kind()))
            // {
            //     element.Add(new XAttribute("InstanceExpression", true));
            // }
    
            // if (SyntaxFacts.IsBinaryExpressionOperatorToken(node.Kind()))
            // {
            //     element.Add(new XAttribute("BinaryExpressionOperator", true));
            // }
    		// 
            // if (SyntaxFacts.IsAssignmentExpressionOperatorToken(node.Kind()))
            // {
            //     element.Add(new XAttribute("AssignmentExpressionOperator", true));
            // }		
    
    		if(node.ToString() != "")
    		{
    			element.Add(new XAttribute("startLine", node.GetLocation().GetLineSpan().StartLinePosition.Line + 1));
    			element.Add(new XAttribute("startColumn", node.GetLocation().GetLineSpan().StartLinePosition.Character + 1));
    			element.Add(new XAttribute("endLine", node.GetLocation().GetLineSpan().EndLinePosition.Line + 1));
    			element.Add(new XAttribute("endColumn", node.GetLocation().GetLineSpan().EndLinePosition.Character));
    		}
        }
    
        /// <summary>
        /// Annotates an element with syntax metadata.
        /// </summary>
        /// <param name="element">the XML element being serialized.</param>
        /// <param name="node">the node being represented by the serializing XML element.</param>
        protected virtual void Annotate(XElement element, Microsoft.CodeAnalysis.SyntaxNode node)
        {
            element.Add(new XAttribute("kind", Enum.GetName(typeof (SyntaxKind), node.Kind())));
            //if (SyntaxFacts.IsAliasQualifier(node))
            //{
            //    element.Add(new XAttribute("Keyword", true));
            //}
        	//
            if (SyntaxFacts.IsKeywordKind(node.Kind()))
            {
                element.Add(new XAttribute("Keyword", true));
            }
        
            //if (SyntaxFacts.IsReservedKeyword(node.Kind()))
            //{
            //    element.Add(new XAttribute("ReservedKeyword", true));
            //}
        	//
            //if (SyntaxFacts.IsAttributeTargetSpecifier(node.Kind()))
            //{
            //    element.Add(new XAttribute("AttributeTargetSpecifier", true));
            //}
        	
            //if (SyntaxFacts.IsAccessibilityModifier(node.Kind()))
            //{
            //    element.Add(new XAttribute("AccessibilityModifier", true));
            //}
        
            //if (SyntaxFacts.IsPreprocessorKeyword(node.Kind()))
            //{
            //    element.Add(new XAttribute("PreprocessorKeyword", true));
            //}        
        
            if (SyntaxFacts.IsTrivia(node.Kind()))
            {
                element.Add(new XAttribute("Trivia", true));
            }
        
            if (SyntaxFacts.IsPreprocessorDirective(node.Kind()))
            {
                element.Add(new XAttribute("PreprocessorDirective", true));
            }
        
            if (SyntaxFacts.IsName(node.Kind()))
            {
                element.Add(new XAttribute("Name", true));
            }
        
            if (SyntaxFacts.IsPredefinedType(node.Kind()))
            {
                element.Add(new XAttribute("PredefinedType", true));
            }
        
            if (SyntaxFacts.IsTypeSyntax(node.Kind()))
            {
                element.Add(new XAttribute("TypeSyntax", true));
            }
        
            if (SyntaxFacts.IsTypeDeclaration(node.Kind()))
            {
                element.Add(new XAttribute("TypeDeclaration", true));
            }
        
            // if (SyntaxFacts.IsAssignmentExpression(node.Kind()))
            // {
            //     element.Add(new XAttribute("AssignmentExpression", true));
            // }       
        
            if (SyntaxFacts.IsDocumentationCommentTrivia(node.Kind()))
            {
                element.Add(new XAttribute("DocumentationCommentTrivia", true));
            }		
    
    		if(node.ToString() != "")
    		{
    			element.Add(new XAttribute("startLine", node.GetLocation().GetLineSpan().StartLinePosition.Line + 1));
    			element.Add(new XAttribute("startColumn", node.GetLocation().GetLineSpan().StartLinePosition.Character + 1));
    			element.Add(new XAttribute("endLine", node.GetLocation().GetLineSpan().EndLinePosition.Line + 1));
    			element.Add(new XAttribute("endColumn", node.GetLocation().GetLineSpan().EndLinePosition.Character));
    		}
        }
    
        /// <summary>
        /// Called when the visitor visits a AttributeArgumentListSyntax node.
        /// </summary>
        public virtual XElement Visit(Microsoft.CodeAnalysis.SyntaxToken node)
        {
    		var result = new XElement("Token");
            result.Add(new XText(node.ValueText));
    		this.Annotate(result, node);
            return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AttributeArgumentSyntax node.
        /// </summary>
        public override XElement VisitAttributeArgument(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax node)
        {
    		var result = new XElement("AttributeArgument");
    		//NameEquals
    		if(node.NameEquals != null)
    		{
    			var xNameEquals = this.Visit(node.NameEquals);
    			xNameEquals.Add(new XAttribute("part", "NameEquals"));
    			result.Add(xNameEquals);
    		}
    		//NameColon
    		if(node.NameColon != null)
    		{
    			var xNameColon = this.Visit(node.NameColon);
    			xNameColon.Add(new XAttribute("part", "NameColon"));
    			result.Add(xNameColon);
    		}
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a NameEqualsSyntax node.
        /// </summary>
        public override XElement VisitNameEquals(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax node)
        {
    		var result = new XElement("NameEquals");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeParameterListSyntax node.
        /// </summary>
        public override XElement VisitTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax node)
        {
    		var result = new XElement("TypeParameterList");
    		//LessThanToken
    		var xLessThanToken = this.Visit(node.LessThanToken);
    		xLessThanToken.Add(new XAttribute("part", "LessThanToken"));
    		result.Add(xLessThanToken);
    		//Parameters
    		if(node.Parameters.Count > 0)
    		{
    			var xParameters = new XElement("SeparatedList_of_TypeParameter");
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			foreach(var x in node.Parameters)
    			{
    				var xElement = this.Visit(x);
    				xParameters.Add(xElement);
    			}
    			result.Add(xParameters);
    		}
    		//GreaterThanToken
    		var xGreaterThanToken = this.Visit(node.GreaterThanToken);
    		xGreaterThanToken.Add(new XAttribute("part", "GreaterThanToken"));
    		result.Add(xGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeParameterSyntax node.
        /// </summary>
        public override XElement VisitTypeParameter(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax node)
        {
    		var result = new XElement("TypeParameter");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//VarianceKeyword
    		if(node.VarianceKeyword != null && node.VarianceKeyword.Kind() != SyntaxKind.None)
    		{
    			var xVarianceKeyword = this.Visit(node.VarianceKeyword);
    			xVarianceKeyword.Add(new XAttribute("part", "VarianceKeyword"));
    			result.Add(xVarianceKeyword);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BaseListSyntax node.
        /// </summary>
        public override XElement VisitBaseList(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax node)
        {
    		var result = new XElement("BaseList");
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//Types
    		if(node.Types.Count > 0)
    		{
    			var xTypes = new XElement("SeparatedList_of_BaseType");
    			xTypes.Add(new XAttribute("part", "Types"));
    			foreach(var x in node.Types)
    			{
    				var xElement = this.Visit(x);
    				xTypes.Add(xElement);
    			}
    			result.Add(xTypes);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeParameterConstraintClauseSyntax node.
        /// </summary>
        public override XElement VisitTypeParameterConstraintClause(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax node)
        {
    		var result = new XElement("TypeParameterConstraintClause");
    		//WhereKeyword
    		var xWhereKeyword = this.Visit(node.WhereKeyword);
    		xWhereKeyword.Add(new XAttribute("part", "WhereKeyword"));
    		result.Add(xWhereKeyword);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//Constraints
    		if(node.Constraints.Count > 0)
    		{
    			var xConstraints = new XElement("SeparatedList_of_TypeParameterConstraint");
    			xConstraints.Add(new XAttribute("part", "Constraints"));
    			foreach(var x in node.Constraints)
    			{
    				var xElement = this.Visit(x);
    				xConstraints.Add(xElement);
    			}
    			result.Add(xConstraints);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ExplicitInterfaceSpecifierSyntax node.
        /// </summary>
        public override XElement VisitExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax node)
        {
    		var result = new XElement("ExplicitInterfaceSpecifier");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//DotToken
    		var xDotToken = this.Visit(node.DotToken);
    		xDotToken.Add(new XAttribute("part", "DotToken"));
    		result.Add(xDotToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConstructorInitializerSyntax node.
        /// </summary>
        public override XElement VisitConstructorInitializer(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax node)
        {
    		var result = new XElement("ConstructorInitializer");
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//ThisOrBaseKeyword
    		var xThisOrBaseKeyword = this.Visit(node.ThisOrBaseKeyword);
    		xThisOrBaseKeyword.Add(new XAttribute("part", "ThisOrBaseKeyword"));
    		result.Add(xThisOrBaseKeyword);
    		//ArgumentList
    		var xArgumentList = this.Visit(node.ArgumentList);
    		xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    		result.Add(xArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArrowExpressionClauseSyntax node.
        /// </summary>
        public override XElement VisitArrowExpressionClause(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax node)
        {
    		var result = new XElement("ArrowExpressionClause");
    		//ArrowToken
    		var xArrowToken = this.Visit(node.ArrowToken);
    		xArrowToken.Add(new XAttribute("part", "ArrowToken"));
    		result.Add(xArrowToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AccessorListSyntax node.
        /// </summary>
        public override XElement VisitAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax node)
        {
    		var result = new XElement("AccessorList");
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Accessors
    		if(node.Accessors.Count > 0)
    		{
    			var xAccessors = new XElement("List_of_AccessorDeclaration");
    			xAccessors.Add(new XAttribute("part", "Accessors"));
    			foreach(var x in node.Accessors)
    			{
    				var xElement = this.Visit(x);
    				xAccessors.Add(xElement);
    			}
    			result.Add(xAccessors);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AccessorDeclarationSyntax node.
        /// </summary>
        public override XElement VisitAccessorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax node)
        {
    		var result = new XElement("AccessorDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ParameterSyntax node.
        /// </summary>
        public override XElement VisitParameter(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax node)
        {
    		var result = new XElement("Parameter");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Type
    		if(node.Type != null)
    		{
    			var xType = this.Visit(node.Type);
    			xType.Add(new XAttribute("part", "Type"));
    			result.Add(xType);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//Default
    		if(node.Default != null)
    		{
    			var xDefault = this.Visit(node.Default);
    			xDefault.Add(new XAttribute("part", "Default"));
    			result.Add(xDefault);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CrefParameterSyntax node.
        /// </summary>
        public override XElement VisitCrefParameter(Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax node)
        {
    		var result = new XElement("CrefParameter");
    		//RefOrOutKeyword
    		if(node.RefOrOutKeyword != null && node.RefOrOutKeyword.Kind() != SyntaxKind.None)
    		{
    			var xRefOrOutKeyword = this.Visit(node.RefOrOutKeyword);
    			xRefOrOutKeyword.Add(new XAttribute("part", "RefOrOutKeyword"));
    			result.Add(xRefOrOutKeyword);
    		}
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlElementStartTagSyntax node.
        /// </summary>
        public override XElement VisitXmlElementStartTag(Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax node)
        {
    		var result = new XElement("XmlElementStartTag");
    		//LessThanToken
    		var xLessThanToken = this.Visit(node.LessThanToken);
    		xLessThanToken.Add(new XAttribute("part", "LessThanToken"));
    		result.Add(xLessThanToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//Attributes
    		if(node.Attributes.Count > 0)
    		{
    			var xAttributes = new XElement("List_of_XmlAttribute");
    			xAttributes.Add(new XAttribute("part", "Attributes"));
    			foreach(var x in node.Attributes)
    			{
    				var xElement = this.Visit(x);
    				xAttributes.Add(xElement);
    			}
    			result.Add(xAttributes);
    		}
    		//GreaterThanToken
    		var xGreaterThanToken = this.Visit(node.GreaterThanToken);
    		xGreaterThanToken.Add(new XAttribute("part", "GreaterThanToken"));
    		result.Add(xGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlElementEndTagSyntax node.
        /// </summary>
        public override XElement VisitXmlElementEndTag(Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax node)
        {
    		var result = new XElement("XmlElementEndTag");
    		//LessThanSlashToken
    		var xLessThanSlashToken = this.Visit(node.LessThanSlashToken);
    		xLessThanSlashToken.Add(new XAttribute("part", "LessThanSlashToken"));
    		result.Add(xLessThanSlashToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//GreaterThanToken
    		var xGreaterThanToken = this.Visit(node.GreaterThanToken);
    		xGreaterThanToken.Add(new XAttribute("part", "GreaterThanToken"));
    		result.Add(xGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlNameSyntax node.
        /// </summary>
        public override XElement VisitXmlName(Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax node)
        {
    		var result = new XElement("XmlName");
    		//Prefix
    		if(node.Prefix != null)
    		{
    			var xPrefix = this.Visit(node.Prefix);
    			xPrefix.Add(new XAttribute("part", "Prefix"));
    			result.Add(xPrefix);
    		}
    		//LocalName
    		var xLocalName = this.Visit(node.LocalName);
    		xLocalName.Add(new XAttribute("part", "LocalName"));
    		result.Add(xLocalName);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlPrefixSyntax node.
        /// </summary>
        public override XElement VisitXmlPrefix(Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax node)
        {
    		var result = new XElement("XmlPrefix");
    		//Prefix
    		var xPrefix = this.Visit(node.Prefix);
    		xPrefix.Add(new XAttribute("part", "Prefix"));
    		result.Add(xPrefix);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeArgumentListSyntax node.
        /// </summary>
        public override XElement VisitTypeArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax node)
        {
    		var result = new XElement("TypeArgumentList");
    		//LessThanToken
    		var xLessThanToken = this.Visit(node.LessThanToken);
    		xLessThanToken.Add(new XAttribute("part", "LessThanToken"));
    		result.Add(xLessThanToken);
    		//Arguments
    		if(node.Arguments.Count > 0)
    		{
    			var xArguments = new XElement("SeparatedList_of_Type");
    			xArguments.Add(new XAttribute("part", "Arguments"));
    			foreach(var x in node.Arguments)
    			{
    				var xElement = this.Visit(x);
    				xArguments.Add(xElement);
    			}
    			result.Add(xArguments);
    		}
    		//GreaterThanToken
    		var xGreaterThanToken = this.Visit(node.GreaterThanToken);
    		xGreaterThanToken.Add(new XAttribute("part", "GreaterThanToken"));
    		result.Add(xGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArrayRankSpecifierSyntax node.
        /// </summary>
        public override XElement VisitArrayRankSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax node)
        {
    		var result = new XElement("ArrayRankSpecifier");
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Sizes
    		if(node.Sizes.Count > 0)
    		{
    			var xSizes = new XElement("SeparatedList_of_Expression");
    			xSizes.Add(new XAttribute("part", "Sizes"));
    			foreach(var x in node.Sizes)
    			{
    				var xElement = this.Visit(x);
    				xSizes.Add(xElement);
    			}
    			result.Add(xSizes);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TupleElementSyntax node.
        /// </summary>
        public override XElement VisitTupleElement(Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax node)
        {
    		var result = new XElement("TupleElement");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Identifier
    		if(node.Identifier != null && node.Identifier.Kind() != SyntaxKind.None)
    		{
    			var xIdentifier = this.Visit(node.Identifier);
    			xIdentifier.Add(new XAttribute("part", "Identifier"));
    			result.Add(xIdentifier);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArgumentSyntax node.
        /// </summary>
        public override XElement VisitArgument(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax node)
        {
    		var result = new XElement("Argument");
    		//NameColon
    		if(node.NameColon != null)
    		{
    			var xNameColon = this.Visit(node.NameColon);
    			xNameColon.Add(new XAttribute("part", "NameColon"));
    			result.Add(xNameColon);
    		}
    		//RefOrOutKeyword
    		if(node.RefOrOutKeyword != null && node.RefOrOutKeyword.Kind() != SyntaxKind.None)
    		{
    			var xRefOrOutKeyword = this.Visit(node.RefOrOutKeyword);
    			xRefOrOutKeyword.Add(new XAttribute("part", "RefOrOutKeyword"));
    			result.Add(xRefOrOutKeyword);
    		}
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a NameColonSyntax node.
        /// </summary>
        public override XElement VisitNameColon(Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax node)
        {
    		var result = new XElement("NameColon");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AnonymousObjectMemberDeclaratorSyntax node.
        /// </summary>
        public override XElement VisitAnonymousObjectMemberDeclarator(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax node)
        {
    		var result = new XElement("AnonymousObjectMemberDeclarator");
    		//NameEquals
    		if(node.NameEquals != null)
    		{
    			var xNameEquals = this.Visit(node.NameEquals);
    			xNameEquals.Add(new XAttribute("part", "NameEquals"));
    			result.Add(xNameEquals);
    		}
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a QueryBodySyntax node.
        /// </summary>
        public override XElement VisitQueryBody(Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax node)
        {
    		var result = new XElement("QueryBody");
    		//Clauses
    		if(node.Clauses.Count > 0)
    		{
    			var xClauses = new XElement("List_of_QueryClause");
    			xClauses.Add(new XAttribute("part", "Clauses"));
    			foreach(var x in node.Clauses)
    			{
    				var xElement = this.Visit(x);
    				xClauses.Add(xElement);
    			}
    			result.Add(xClauses);
    		}
    		//SelectOrGroup
    		var xSelectOrGroup = this.Visit(node.SelectOrGroup);
    		xSelectOrGroup.Add(new XAttribute("part", "SelectOrGroup"));
    		result.Add(xSelectOrGroup);
    		//Continuation
    		if(node.Continuation != null)
    		{
    			var xContinuation = this.Visit(node.Continuation);
    			xContinuation.Add(new XAttribute("part", "Continuation"));
    			result.Add(xContinuation);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a JoinIntoClauseSyntax node.
        /// </summary>
        public override XElement VisitJoinIntoClause(Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax node)
        {
    		var result = new XElement("JoinIntoClause");
    		//IntoKeyword
    		var xIntoKeyword = this.Visit(node.IntoKeyword);
    		xIntoKeyword.Add(new XAttribute("part", "IntoKeyword"));
    		result.Add(xIntoKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OrderingSyntax node.
        /// </summary>
        public override XElement VisitOrdering(Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax node)
        {
    		var result = new XElement("Ordering");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//AscendingOrDescendingKeyword
    		if(node.AscendingOrDescendingKeyword != null && node.AscendingOrDescendingKeyword.Kind() != SyntaxKind.None)
    		{
    			var xAscendingOrDescendingKeyword = this.Visit(node.AscendingOrDescendingKeyword);
    			xAscendingOrDescendingKeyword.Add(new XAttribute("part", "AscendingOrDescendingKeyword"));
    			result.Add(xAscendingOrDescendingKeyword);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a QueryContinuationSyntax node.
        /// </summary>
        public override XElement VisitQueryContinuation(Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax node)
        {
    		var result = new XElement("QueryContinuation");
    		//IntoKeyword
    		var xIntoKeyword = this.Visit(node.IntoKeyword);
    		xIntoKeyword.Add(new XAttribute("part", "IntoKeyword"));
    		result.Add(xIntoKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//Body
    		var xBody = this.Visit(node.Body);
    		xBody.Add(new XAttribute("part", "Body"));
    		result.Add(xBody);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a WhenClauseSyntax node.
        /// </summary>
        public override XElement VisitWhenClause(Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax node)
        {
    		var result = new XElement("WhenClause");
    		//WhenKeyword
    		var xWhenKeyword = this.Visit(node.WhenKeyword);
    		xWhenKeyword.Add(new XAttribute("part", "WhenKeyword"));
    		result.Add(xWhenKeyword);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterpolationAlignmentClauseSyntax node.
        /// </summary>
        public override XElement VisitInterpolationAlignmentClause(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax node)
        {
    		var result = new XElement("InterpolationAlignmentClause");
    		//CommaToken
    		var xCommaToken = this.Visit(node.CommaToken);
    		xCommaToken.Add(new XAttribute("part", "CommaToken"));
    		result.Add(xCommaToken);
    		//Value
    		var xValue = this.Visit(node.Value);
    		xValue.Add(new XAttribute("part", "Value"));
    		result.Add(xValue);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterpolationFormatClauseSyntax node.
        /// </summary>
        public override XElement VisitInterpolationFormatClause(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax node)
        {
    		var result = new XElement("InterpolationFormatClause");
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//FormatStringToken
    		var xFormatStringToken = this.Visit(node.FormatStringToken);
    		xFormatStringToken.Add(new XAttribute("part", "FormatStringToken"));
    		result.Add(xFormatStringToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a VariableDeclarationSyntax node.
        /// </summary>
        public override XElement VisitVariableDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax node)
        {
    		var result = new XElement("VariableDeclaration");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Variables
    		if(node.Variables.Count > 0)
    		{
    			var xVariables = new XElement("SeparatedList_of_VariableDeclarator");
    			xVariables.Add(new XAttribute("part", "Variables"));
    			foreach(var x in node.Variables)
    			{
    				var xElement = this.Visit(x);
    				xVariables.Add(xElement);
    			}
    			result.Add(xVariables);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a VariableDeclaratorSyntax node.
        /// </summary>
        public override XElement VisitVariableDeclarator(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax node)
        {
    		var result = new XElement("VariableDeclarator");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//ArgumentList
    		if(node.ArgumentList != null)
    		{
    			var xArgumentList = this.Visit(node.ArgumentList);
    			xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    			result.Add(xArgumentList);
    		}
    		//Initializer
    		if(node.Initializer != null)
    		{
    			var xInitializer = this.Visit(node.Initializer);
    			xInitializer.Add(new XAttribute("part", "Initializer"));
    			result.Add(xInitializer);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EqualsValueClauseSyntax node.
        /// </summary>
        public override XElement VisitEqualsValueClause(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax node)
        {
    		var result = new XElement("EqualsValueClause");
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    		//Value
    		var xValue = this.Visit(node.Value);
    		xValue.Add(new XAttribute("part", "Value"));
    		result.Add(xValue);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ElseClauseSyntax node.
        /// </summary>
        public override XElement VisitElseClause(Microsoft.CodeAnalysis.CSharp.Syntax.ElseClauseSyntax node)
        {
    		var result = new XElement("ElseClause");
    		//ElseKeyword
    		var xElseKeyword = this.Visit(node.ElseKeyword);
    		xElseKeyword.Add(new XAttribute("part", "ElseKeyword"));
    		result.Add(xElseKeyword);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SwitchSectionSyntax node.
        /// </summary>
        public override XElement VisitSwitchSection(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax node)
        {
    		var result = new XElement("SwitchSection");
    		//Labels
    		if(node.Labels.Count > 0)
    		{
    			var xLabels = new XElement("List_of_SwitchLabel");
    			xLabels.Add(new XAttribute("part", "Labels"));
    			foreach(var x in node.Labels)
    			{
    				var xElement = this.Visit(x);
    				xLabels.Add(xElement);
    			}
    			result.Add(xLabels);
    		}
    		//Statements
    		if(node.Statements.Count > 0)
    		{
    			var xStatements = new XElement("List_of_Statement");
    			xStatements.Add(new XAttribute("part", "Statements"));
    			foreach(var x in node.Statements)
    			{
    				var xElement = this.Visit(x);
    				xStatements.Add(xElement);
    			}
    			result.Add(xStatements);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CatchClauseSyntax node.
        /// </summary>
        public override XElement VisitCatchClause(Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax node)
        {
    		var result = new XElement("CatchClause");
    		//CatchKeyword
    		var xCatchKeyword = this.Visit(node.CatchKeyword);
    		xCatchKeyword.Add(new XAttribute("part", "CatchKeyword"));
    		result.Add(xCatchKeyword);
    		//Declaration
    		if(node.Declaration != null)
    		{
    			var xDeclaration = this.Visit(node.Declaration);
    			xDeclaration.Add(new XAttribute("part", "Declaration"));
    			result.Add(xDeclaration);
    		}
    		//Filter
    		if(node.Filter != null)
    		{
    			var xFilter = this.Visit(node.Filter);
    			xFilter.Add(new XAttribute("part", "Filter"));
    			result.Add(xFilter);
    		}
    		//Block
    		var xBlock = this.Visit(node.Block);
    		xBlock.Add(new XAttribute("part", "Block"));
    		result.Add(xBlock);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CatchDeclarationSyntax node.
        /// </summary>
        public override XElement VisitCatchDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax node)
        {
    		var result = new XElement("CatchDeclaration");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Identifier
    		if(node.Identifier != null && node.Identifier.Kind() != SyntaxKind.None)
    		{
    			var xIdentifier = this.Visit(node.Identifier);
    			xIdentifier.Add(new XAttribute("part", "Identifier"));
    			result.Add(xIdentifier);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CatchFilterClauseSyntax node.
        /// </summary>
        public override XElement VisitCatchFilterClause(Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax node)
        {
    		var result = new XElement("CatchFilterClause");
    		//WhenKeyword
    		var xWhenKeyword = this.Visit(node.WhenKeyword);
    		xWhenKeyword.Add(new XAttribute("part", "WhenKeyword"));
    		result.Add(xWhenKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//FilterExpression
    		var xFilterExpression = this.Visit(node.FilterExpression);
    		xFilterExpression.Add(new XAttribute("part", "FilterExpression"));
    		result.Add(xFilterExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a FinallyClauseSyntax node.
        /// </summary>
        public override XElement VisitFinallyClause(Microsoft.CodeAnalysis.CSharp.Syntax.FinallyClauseSyntax node)
        {
    		var result = new XElement("FinallyClause");
    		//FinallyKeyword
    		var xFinallyKeyword = this.Visit(node.FinallyKeyword);
    		xFinallyKeyword.Add(new XAttribute("part", "FinallyKeyword"));
    		result.Add(xFinallyKeyword);
    		//Block
    		var xBlock = this.Visit(node.Block);
    		xBlock.Add(new XAttribute("part", "Block"));
    		result.Add(xBlock);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CompilationUnitSyntax node.
        /// </summary>
        public override XElement VisitCompilationUnit(Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax node)
        {
    		var result = new XElement("CompilationUnit");
    		//Externs
    		if(node.Externs.Count > 0)
    		{
    			var xExterns = new XElement("List_of_ExternAliasDirective");
    			xExterns.Add(new XAttribute("part", "Externs"));
    			foreach(var x in node.Externs)
    			{
    				var xElement = this.Visit(x);
    				xExterns.Add(xElement);
    			}
    			result.Add(xExterns);
    		}
    		//Usings
    		if(node.Usings.Count > 0)
    		{
    			var xUsings = new XElement("List_of_UsingDirective");
    			xUsings.Add(new XAttribute("part", "Usings"));
    			foreach(var x in node.Usings)
    			{
    				var xElement = this.Visit(x);
    				xUsings.Add(xElement);
    			}
    			result.Add(xUsings);
    		}
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("List_of_MemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//EndOfFileToken
    		var xEndOfFileToken = this.Visit(node.EndOfFileToken);
    		xEndOfFileToken.Add(new XAttribute("part", "EndOfFileToken"));
    		result.Add(xEndOfFileToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ExternAliasDirectiveSyntax node.
        /// </summary>
        public override XElement VisitExternAliasDirective(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax node)
        {
    		var result = new XElement("ExternAliasDirective");
    		//ExternKeyword
    		var xExternKeyword = this.Visit(node.ExternKeyword);
    		xExternKeyword.Add(new XAttribute("part", "ExternKeyword"));
    		result.Add(xExternKeyword);
    		//AliasKeyword
    		var xAliasKeyword = this.Visit(node.AliasKeyword);
    		xAliasKeyword.Add(new XAttribute("part", "AliasKeyword"));
    		result.Add(xAliasKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a UsingDirectiveSyntax node.
        /// </summary>
        public override XElement VisitUsingDirective(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax node)
        {
    		var result = new XElement("UsingDirective");
    		//UsingKeyword
    		var xUsingKeyword = this.Visit(node.UsingKeyword);
    		xUsingKeyword.Add(new XAttribute("part", "UsingKeyword"));
    		result.Add(xUsingKeyword);
    		//StaticKeyword
    		if(node.StaticKeyword != null && node.StaticKeyword.Kind() != SyntaxKind.None)
    		{
    			var xStaticKeyword = this.Visit(node.StaticKeyword);
    			xStaticKeyword.Add(new XAttribute("part", "StaticKeyword"));
    			result.Add(xStaticKeyword);
    		}
    		//Alias
    		if(node.Alias != null)
    		{
    			var xAlias = this.Visit(node.Alias);
    			xAlias.Add(new XAttribute("part", "Alias"));
    			result.Add(xAlias);
    		}
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AttributeListSyntax node.
        /// </summary>
        public override XElement VisitAttributeList(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax node)
        {
    		var result = new XElement("AttributeList");
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Target
    		if(node.Target != null)
    		{
    			var xTarget = this.Visit(node.Target);
    			xTarget.Add(new XAttribute("part", "Target"));
    			result.Add(xTarget);
    		}
    		//Attributes
    		if(node.Attributes.Count > 0)
    		{
    			var xAttributes = new XElement("SeparatedList_of_Attribute");
    			xAttributes.Add(new XAttribute("part", "Attributes"));
    			foreach(var x in node.Attributes)
    			{
    				var xElement = this.Visit(x);
    				xAttributes.Add(xElement);
    			}
    			result.Add(xAttributes);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AttributeTargetSpecifierSyntax node.
        /// </summary>
        public override XElement VisitAttributeTargetSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax node)
        {
    		var result = new XElement("AttributeTargetSpecifier");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AttributeSyntax node.
        /// </summary>
        public override XElement VisitAttribute(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax node)
        {
    		var result = new XElement("Attribute");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//ArgumentList
    		if(node.ArgumentList != null)
    		{
    			var xArgumentList = this.Visit(node.ArgumentList);
    			xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    			result.Add(xArgumentList);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AttributeArgumentListSyntax node.
        /// </summary>
        public override XElement VisitAttributeArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax node)
        {
    		var result = new XElement("AttributeArgumentList");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Arguments
    		if(node.Arguments.Count > 0)
    		{
    			var xArguments = new XElement("SeparatedList_of_AttributeArgument");
    			xArguments.Add(new XAttribute("part", "Arguments"));
    			foreach(var x in node.Arguments)
    			{
    				var xElement = this.Visit(x);
    				xArguments.Add(xElement);
    			}
    			result.Add(xArguments);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DelegateDeclarationSyntax node.
        /// </summary>
        public override XElement VisitDelegateDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax node)
        {
    		var result = new XElement("DelegateDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//DelegateKeyword
    		var xDelegateKeyword = this.Visit(node.DelegateKeyword);
    		xDelegateKeyword.Add(new XAttribute("part", "DelegateKeyword"));
    		result.Add(xDelegateKeyword);
    		//ReturnType
    		var xReturnType = this.Visit(node.ReturnType);
    		xReturnType.Add(new XAttribute("part", "ReturnType"));
    		result.Add(xReturnType);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EnumMemberDeclarationSyntax node.
        /// </summary>
        public override XElement VisitEnumMemberDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax node)
        {
    		var result = new XElement("EnumMemberDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//EqualsValue
    		if(node.EqualsValue != null)
    		{
    			var xEqualsValue = this.Visit(node.EqualsValue);
    			xEqualsValue.Add(new XAttribute("part", "EqualsValue"));
    			result.Add(xEqualsValue);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IncompleteMemberSyntax node.
        /// </summary>
        public override XElement VisitIncompleteMember(Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax node)
        {
    		var result = new XElement("IncompleteMember");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Type
    		if(node.Type != null)
    		{
    			var xType = this.Visit(node.Type);
    			xType.Add(new XAttribute("part", "Type"));
    			result.Add(xType);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a GlobalStatementSyntax node.
        /// </summary>
        public override XElement VisitGlobalStatement(Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax node)
        {
    		var result = new XElement("GlobalStatement");
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a NamespaceDeclarationSyntax node.
        /// </summary>
        public override XElement VisitNamespaceDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax node)
        {
    		var result = new XElement("NamespaceDeclaration");
    		//NamespaceKeyword
    		var xNamespaceKeyword = this.Visit(node.NamespaceKeyword);
    		xNamespaceKeyword.Add(new XAttribute("part", "NamespaceKeyword"));
    		result.Add(xNamespaceKeyword);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Externs
    		if(node.Externs.Count > 0)
    		{
    			var xExterns = new XElement("List_of_ExternAliasDirective");
    			xExterns.Add(new XAttribute("part", "Externs"));
    			foreach(var x in node.Externs)
    			{
    				var xElement = this.Visit(x);
    				xExterns.Add(xElement);
    			}
    			result.Add(xExterns);
    		}
    		//Usings
    		if(node.Usings.Count > 0)
    		{
    			var xUsings = new XElement("List_of_UsingDirective");
    			xUsings.Add(new XAttribute("part", "Usings"));
    			foreach(var x in node.Usings)
    			{
    				var xElement = this.Visit(x);
    				xUsings.Add(xElement);
    			}
    			result.Add(xUsings);
    		}
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("List_of_MemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EnumDeclarationSyntax node.
        /// </summary>
        public override XElement VisitEnumDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax node)
        {
    		var result = new XElement("EnumDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//EnumKeyword
    		var xEnumKeyword = this.Visit(node.EnumKeyword);
    		xEnumKeyword.Add(new XAttribute("part", "EnumKeyword"));
    		result.Add(xEnumKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//BaseList
    		if(node.BaseList != null)
    		{
    			var xBaseList = this.Visit(node.BaseList);
    			xBaseList.Add(new XAttribute("part", "BaseList"));
    			result.Add(xBaseList);
    		}
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("SeparatedList_of_EnumMemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ClassDeclarationSyntax node.
        /// </summary>
        public override XElement VisitClassDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax node)
        {
    		var result = new XElement("ClassDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//BaseList
    		if(node.BaseList != null)
    		{
    			var xBaseList = this.Visit(node.BaseList);
    			xBaseList.Add(new XAttribute("part", "BaseList"));
    			result.Add(xBaseList);
    		}
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("List_of_MemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a StructDeclarationSyntax node.
        /// </summary>
        public override XElement VisitStructDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax node)
        {
    		var result = new XElement("StructDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//BaseList
    		if(node.BaseList != null)
    		{
    			var xBaseList = this.Visit(node.BaseList);
    			xBaseList.Add(new XAttribute("part", "BaseList"));
    			result.Add(xBaseList);
    		}
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("List_of_MemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterfaceDeclarationSyntax node.
        /// </summary>
        public override XElement VisitInterfaceDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax node)
        {
    		var result = new XElement("InterfaceDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//BaseList
    		if(node.BaseList != null)
    		{
    			var xBaseList = this.Visit(node.BaseList);
    			xBaseList.Add(new XAttribute("part", "BaseList"));
    			result.Add(xBaseList);
    		}
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Members
    		if(node.Members.Count > 0)
    		{
    			var xMembers = new XElement("List_of_MemberDeclaration");
    			xMembers.Add(new XAttribute("part", "Members"));
    			foreach(var x in node.Members)
    			{
    				var xElement = this.Visit(x);
    				xMembers.Add(xElement);
    			}
    			result.Add(xMembers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a FieldDeclarationSyntax node.
        /// </summary>
        public override XElement VisitFieldDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax node)
        {
    		var result = new XElement("FieldDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Declaration
    		var xDeclaration = this.Visit(node.Declaration);
    		xDeclaration.Add(new XAttribute("part", "Declaration"));
    		result.Add(xDeclaration);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EventFieldDeclarationSyntax node.
        /// </summary>
        public override XElement VisitEventFieldDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax node)
        {
    		var result = new XElement("EventFieldDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//EventKeyword
    		var xEventKeyword = this.Visit(node.EventKeyword);
    		xEventKeyword.Add(new XAttribute("part", "EventKeyword"));
    		result.Add(xEventKeyword);
    		//Declaration
    		var xDeclaration = this.Visit(node.Declaration);
    		xDeclaration.Add(new XAttribute("part", "Declaration"));
    		result.Add(xDeclaration);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a MethodDeclarationSyntax node.
        /// </summary>
        public override XElement VisitMethodDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax node)
        {
    		var result = new XElement("MethodDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//ReturnType
    		var xReturnType = this.Visit(node.ReturnType);
    		xReturnType.Add(new XAttribute("part", "ReturnType"));
    		result.Add(xReturnType);
    		//ExplicitInterfaceSpecifier
    		if(node.ExplicitInterfaceSpecifier != null)
    		{
    			var xExplicitInterfaceSpecifier = this.Visit(node.ExplicitInterfaceSpecifier);
    			xExplicitInterfaceSpecifier.Add(new XAttribute("part", "ExplicitInterfaceSpecifier"));
    			result.Add(xExplicitInterfaceSpecifier);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OperatorDeclarationSyntax node.
        /// </summary>
        public override XElement VisitOperatorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax node)
        {
    		var result = new XElement("OperatorDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//ReturnType
    		var xReturnType = this.Visit(node.ReturnType);
    		xReturnType.Add(new XAttribute("part", "ReturnType"));
    		result.Add(xReturnType);
    		//OperatorKeyword
    		var xOperatorKeyword = this.Visit(node.OperatorKeyword);
    		xOperatorKeyword.Add(new XAttribute("part", "OperatorKeyword"));
    		result.Add(xOperatorKeyword);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConversionOperatorDeclarationSyntax node.
        /// </summary>
        public override XElement VisitConversionOperatorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax node)
        {
    		var result = new XElement("ConversionOperatorDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//ImplicitOrExplicitKeyword
    		var xImplicitOrExplicitKeyword = this.Visit(node.ImplicitOrExplicitKeyword);
    		xImplicitOrExplicitKeyword.Add(new XAttribute("part", "ImplicitOrExplicitKeyword"));
    		result.Add(xImplicitOrExplicitKeyword);
    		//OperatorKeyword
    		var xOperatorKeyword = this.Visit(node.OperatorKeyword);
    		xOperatorKeyword.Add(new XAttribute("part", "OperatorKeyword"));
    		result.Add(xOperatorKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConstructorDeclarationSyntax node.
        /// </summary>
        public override XElement VisitConstructorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax node)
        {
    		var result = new XElement("ConstructorDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//Initializer
    		if(node.Initializer != null)
    		{
    			var xInitializer = this.Visit(node.Initializer);
    			xInitializer.Add(new XAttribute("part", "Initializer"));
    			result.Add(xInitializer);
    		}
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DestructorDeclarationSyntax node.
        /// </summary>
        public override XElement VisitDestructorDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax node)
        {
    		var result = new XElement("DestructorDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//TildeToken
    		var xTildeToken = this.Visit(node.TildeToken);
    		xTildeToken.Add(new XAttribute("part", "TildeToken"));
    		result.Add(xTildeToken);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PropertyDeclarationSyntax node.
        /// </summary>
        public override XElement VisitPropertyDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax node)
        {
    		var result = new XElement("PropertyDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//ExplicitInterfaceSpecifier
    		if(node.ExplicitInterfaceSpecifier != null)
    		{
    			var xExplicitInterfaceSpecifier = this.Visit(node.ExplicitInterfaceSpecifier);
    			xExplicitInterfaceSpecifier.Add(new XAttribute("part", "ExplicitInterfaceSpecifier"));
    			result.Add(xExplicitInterfaceSpecifier);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//AccessorList
    		if(node.AccessorList != null)
    		{
    			var xAccessorList = this.Visit(node.AccessorList);
    			xAccessorList.Add(new XAttribute("part", "AccessorList"));
    			result.Add(xAccessorList);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//Initializer
    		if(node.Initializer != null)
    		{
    			var xInitializer = this.Visit(node.Initializer);
    			xInitializer.Add(new XAttribute("part", "Initializer"));
    			result.Add(xInitializer);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EventDeclarationSyntax node.
        /// </summary>
        public override XElement VisitEventDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax node)
        {
    		var result = new XElement("EventDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//EventKeyword
    		var xEventKeyword = this.Visit(node.EventKeyword);
    		xEventKeyword.Add(new XAttribute("part", "EventKeyword"));
    		result.Add(xEventKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//ExplicitInterfaceSpecifier
    		if(node.ExplicitInterfaceSpecifier != null)
    		{
    			var xExplicitInterfaceSpecifier = this.Visit(node.ExplicitInterfaceSpecifier);
    			xExplicitInterfaceSpecifier.Add(new XAttribute("part", "ExplicitInterfaceSpecifier"));
    			result.Add(xExplicitInterfaceSpecifier);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//AccessorList
    		var xAccessorList = this.Visit(node.AccessorList);
    		xAccessorList.Add(new XAttribute("part", "AccessorList"));
    		result.Add(xAccessorList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IndexerDeclarationSyntax node.
        /// </summary>
        public override XElement VisitIndexerDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax node)
        {
    		var result = new XElement("IndexerDeclaration");
    		//AttributeLists
    		if(node.AttributeLists.Count > 0)
    		{
    			var xAttributeLists = new XElement("List_of_AttributeList");
    			xAttributeLists.Add(new XAttribute("part", "AttributeLists"));
    			foreach(var x in node.AttributeLists)
    			{
    				var xElement = this.Visit(x);
    				xAttributeLists.Add(xElement);
    			}
    			result.Add(xAttributeLists);
    		}
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//ExplicitInterfaceSpecifier
    		if(node.ExplicitInterfaceSpecifier != null)
    		{
    			var xExplicitInterfaceSpecifier = this.Visit(node.ExplicitInterfaceSpecifier);
    			xExplicitInterfaceSpecifier.Add(new XAttribute("part", "ExplicitInterfaceSpecifier"));
    			result.Add(xExplicitInterfaceSpecifier);
    		}
    		//ThisKeyword
    		var xThisKeyword = this.Visit(node.ThisKeyword);
    		xThisKeyword.Add(new XAttribute("part", "ThisKeyword"));
    		result.Add(xThisKeyword);
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//AccessorList
    		if(node.AccessorList != null)
    		{
    			var xAccessorList = this.Visit(node.AccessorList);
    			xAccessorList.Add(new XAttribute("part", "AccessorList"));
    			result.Add(xAccessorList);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SimpleBaseTypeSyntax node.
        /// </summary>
        public override XElement VisitSimpleBaseType(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax node)
        {
    		var result = new XElement("SimpleBaseType");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConstructorConstraintSyntax node.
        /// </summary>
        public override XElement VisitConstructorConstraint(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax node)
        {
    		var result = new XElement("ConstructorConstraint");
    		//NewKeyword
    		var xNewKeyword = this.Visit(node.NewKeyword);
    		xNewKeyword.Add(new XAttribute("part", "NewKeyword"));
    		result.Add(xNewKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ClassOrStructConstraintSyntax node.
        /// </summary>
        public override XElement VisitClassOrStructConstraint(Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax node)
        {
    		var result = new XElement("ClassOrStructConstraint");
    		//ClassOrStructKeyword
    		var xClassOrStructKeyword = this.Visit(node.ClassOrStructKeyword);
    		xClassOrStructKeyword.Add(new XAttribute("part", "ClassOrStructKeyword"));
    		result.Add(xClassOrStructKeyword);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeConstraintSyntax node.
        /// </summary>
        public override XElement VisitTypeConstraint(Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax node)
        {
    		var result = new XElement("TypeConstraint");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ParameterListSyntax node.
        /// </summary>
        public override XElement VisitParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax node)
        {
    		var result = new XElement("ParameterList");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Parameters
    		if(node.Parameters.Count > 0)
    		{
    			var xParameters = new XElement("SeparatedList_of_Parameter");
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			foreach(var x in node.Parameters)
    			{
    				var xElement = this.Visit(x);
    				xParameters.Add(xElement);
    			}
    			result.Add(xParameters);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BracketedParameterListSyntax node.
        /// </summary>
        public override XElement VisitBracketedParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax node)
        {
    		var result = new XElement("BracketedParameterList");
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Parameters
    		if(node.Parameters.Count > 0)
    		{
    			var xParameters = new XElement("SeparatedList_of_Parameter");
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			foreach(var x in node.Parameters)
    			{
    				var xElement = this.Visit(x);
    				xParameters.Add(xElement);
    			}
    			result.Add(xParameters);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SkippedTokensTriviaSyntax node.
        /// </summary>
        public override XElement VisitSkippedTokensTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.SkippedTokensTriviaSyntax node)
        {
    		var result = new XElement("SkippedTokensTrivia");
    		//Tokens
    		if(node.Tokens.Count > 0)
    		{
    			var xTokens = new XElement("TokenList");
    			xTokens.Add(new XAttribute("part", "Tokens"));
    			foreach(var x in node.Tokens)
    			{
    				var xElement = this.Visit(x);
    				xTokens.Add(xElement);
    			}
    			result.Add(xTokens);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DocumentationCommentTriviaSyntax node.
        /// </summary>
        public override XElement VisitDocumentationCommentTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax node)
        {
    		var result = new XElement("DocumentationCommentTrivia");
    		//Content
    		if(node.Content.Count > 0)
    		{
    			var xContent = new XElement("List_of_XmlNode");
    			xContent.Add(new XAttribute("part", "Content"));
    			foreach(var x in node.Content)
    			{
    				var xElement = this.Visit(x);
    				xContent.Add(xElement);
    			}
    			result.Add(xContent);
    		}
    		//EndOfComment
    		var xEndOfComment = this.Visit(node.EndOfComment);
    		xEndOfComment.Add(new XAttribute("part", "EndOfComment"));
    		result.Add(xEndOfComment);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EndIfDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitEndIfDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.EndIfDirectiveTriviaSyntax node)
        {
    		var result = new XElement("EndIfDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//EndIfKeyword
    		var xEndIfKeyword = this.Visit(node.EndIfKeyword);
    		xEndIfKeyword.Add(new XAttribute("part", "EndIfKeyword"));
    		result.Add(xEndIfKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a RegionDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitRegionDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.RegionDirectiveTriviaSyntax node)
        {
    		var result = new XElement("RegionDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//RegionKeyword
    		var xRegionKeyword = this.Visit(node.RegionKeyword);
    		xRegionKeyword.Add(new XAttribute("part", "RegionKeyword"));
    		result.Add(xRegionKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EndRegionDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitEndRegionDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.EndRegionDirectiveTriviaSyntax node)
        {
    		var result = new XElement("EndRegionDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//EndRegionKeyword
    		var xEndRegionKeyword = this.Visit(node.EndRegionKeyword);
    		xEndRegionKeyword.Add(new XAttribute("part", "EndRegionKeyword"));
    		result.Add(xEndRegionKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ErrorDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitErrorDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.ErrorDirectiveTriviaSyntax node)
        {
    		var result = new XElement("ErrorDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//ErrorKeyword
    		var xErrorKeyword = this.Visit(node.ErrorKeyword);
    		xErrorKeyword.Add(new XAttribute("part", "ErrorKeyword"));
    		result.Add(xErrorKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a WarningDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitWarningDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.WarningDirectiveTriviaSyntax node)
        {
    		var result = new XElement("WarningDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//WarningKeyword
    		var xWarningKeyword = this.Visit(node.WarningKeyword);
    		xWarningKeyword.Add(new XAttribute("part", "WarningKeyword"));
    		result.Add(xWarningKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BadDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitBadDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.BadDirectiveTriviaSyntax node)
        {
    		var result = new XElement("BadDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DefineDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitDefineDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.DefineDirectiveTriviaSyntax node)
        {
    		var result = new XElement("DefineDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//DefineKeyword
    		var xDefineKeyword = this.Visit(node.DefineKeyword);
    		xDefineKeyword.Add(new XAttribute("part", "DefineKeyword"));
    		result.Add(xDefineKeyword);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a UndefDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitUndefDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.UndefDirectiveTriviaSyntax node)
        {
    		var result = new XElement("UndefDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//UndefKeyword
    		var xUndefKeyword = this.Visit(node.UndefKeyword);
    		xUndefKeyword.Add(new XAttribute("part", "UndefKeyword"));
    		result.Add(xUndefKeyword);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LineDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitLineDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.LineDirectiveTriviaSyntax node)
        {
    		var result = new XElement("LineDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//LineKeyword
    		var xLineKeyword = this.Visit(node.LineKeyword);
    		xLineKeyword.Add(new XAttribute("part", "LineKeyword"));
    		result.Add(xLineKeyword);
    		//Line
    		var xLine = this.Visit(node.Line);
    		xLine.Add(new XAttribute("part", "Line"));
    		result.Add(xLine);
    		//File
    		if(node.File != null && node.File.Kind() != SyntaxKind.None)
    		{
    			var xFile = this.Visit(node.File);
    			xFile.Add(new XAttribute("part", "File"));
    			result.Add(xFile);
    		}
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PragmaWarningDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitPragmaWarningDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.PragmaWarningDirectiveTriviaSyntax node)
        {
    		var result = new XElement("PragmaWarningDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//PragmaKeyword
    		var xPragmaKeyword = this.Visit(node.PragmaKeyword);
    		xPragmaKeyword.Add(new XAttribute("part", "PragmaKeyword"));
    		result.Add(xPragmaKeyword);
    		//WarningKeyword
    		var xWarningKeyword = this.Visit(node.WarningKeyword);
    		xWarningKeyword.Add(new XAttribute("part", "WarningKeyword"));
    		result.Add(xWarningKeyword);
    		//DisableOrRestoreKeyword
    		var xDisableOrRestoreKeyword = this.Visit(node.DisableOrRestoreKeyword);
    		xDisableOrRestoreKeyword.Add(new XAttribute("part", "DisableOrRestoreKeyword"));
    		result.Add(xDisableOrRestoreKeyword);
    		//ErrorCodes
    		if(node.ErrorCodes.Count > 0)
    		{
    			var xErrorCodes = new XElement("SeparatedList_of_Expression");
    			xErrorCodes.Add(new XAttribute("part", "ErrorCodes"));
    			foreach(var x in node.ErrorCodes)
    			{
    				var xElement = this.Visit(x);
    				xErrorCodes.Add(xElement);
    			}
    			result.Add(xErrorCodes);
    		}
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PragmaChecksumDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitPragmaChecksumDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.PragmaChecksumDirectiveTriviaSyntax node)
        {
    		var result = new XElement("PragmaChecksumDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//PragmaKeyword
    		var xPragmaKeyword = this.Visit(node.PragmaKeyword);
    		xPragmaKeyword.Add(new XAttribute("part", "PragmaKeyword"));
    		result.Add(xPragmaKeyword);
    		//ChecksumKeyword
    		var xChecksumKeyword = this.Visit(node.ChecksumKeyword);
    		xChecksumKeyword.Add(new XAttribute("part", "ChecksumKeyword"));
    		result.Add(xChecksumKeyword);
    		//File
    		var xFile = this.Visit(node.File);
    		xFile.Add(new XAttribute("part", "File"));
    		result.Add(xFile);
    		//Guid
    		var xGuid = this.Visit(node.Guid);
    		xGuid.Add(new XAttribute("part", "Guid"));
    		result.Add(xGuid);
    		//Bytes
    		var xBytes = this.Visit(node.Bytes);
    		xBytes.Add(new XAttribute("part", "Bytes"));
    		result.Add(xBytes);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ReferenceDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitReferenceDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.ReferenceDirectiveTriviaSyntax node)
        {
    		var result = new XElement("ReferenceDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//ReferenceKeyword
    		var xReferenceKeyword = this.Visit(node.ReferenceKeyword);
    		xReferenceKeyword.Add(new XAttribute("part", "ReferenceKeyword"));
    		result.Add(xReferenceKeyword);
    		//File
    		var xFile = this.Visit(node.File);
    		xFile.Add(new XAttribute("part", "File"));
    		result.Add(xFile);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LoadDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitLoadDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.LoadDirectiveTriviaSyntax node)
        {
    		var result = new XElement("LoadDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//LoadKeyword
    		var xLoadKeyword = this.Visit(node.LoadKeyword);
    		xLoadKeyword.Add(new XAttribute("part", "LoadKeyword"));
    		result.Add(xLoadKeyword);
    		//File
    		var xFile = this.Visit(node.File);
    		xFile.Add(new XAttribute("part", "File"));
    		result.Add(xFile);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ShebangDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitShebangDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.ShebangDirectiveTriviaSyntax node)
        {
    		var result = new XElement("ShebangDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//ExclamationToken
    		var xExclamationToken = this.Visit(node.ExclamationToken);
    		xExclamationToken.Add(new XAttribute("part", "ExclamationToken"));
    		result.Add(xExclamationToken);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ElseDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitElseDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.ElseDirectiveTriviaSyntax node)
        {
    		var result = new XElement("ElseDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//ElseKeyword
    		var xElseKeyword = this.Visit(node.ElseKeyword);
    		xElseKeyword.Add(new XAttribute("part", "ElseKeyword"));
    		result.Add(xElseKeyword);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IfDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitIfDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.IfDirectiveTriviaSyntax node)
        {
    		var result = new XElement("IfDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//IfKeyword
    		var xIfKeyword = this.Visit(node.IfKeyword);
    		xIfKeyword.Add(new XAttribute("part", "IfKeyword"));
    		result.Add(xIfKeyword);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ElifDirectiveTriviaSyntax node.
        /// </summary>
        public override XElement VisitElifDirectiveTrivia(Microsoft.CodeAnalysis.CSharp.Syntax.ElifDirectiveTriviaSyntax node)
        {
    		var result = new XElement("ElifDirectiveTrivia");
    		//HashToken
    		var xHashToken = this.Visit(node.HashToken);
    		xHashToken.Add(new XAttribute("part", "HashToken"));
    		result.Add(xHashToken);
    		//ElifKeyword
    		var xElifKeyword = this.Visit(node.ElifKeyword);
    		xElifKeyword.Add(new XAttribute("part", "ElifKeyword"));
    		result.Add(xElifKeyword);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//EndOfDirectiveToken
    		var xEndOfDirectiveToken = this.Visit(node.EndOfDirectiveToken);
    		xEndOfDirectiveToken.Add(new XAttribute("part", "EndOfDirectiveToken"));
    		result.Add(xEndOfDirectiveToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeCrefSyntax node.
        /// </summary>
        public override XElement VisitTypeCref(Microsoft.CodeAnalysis.CSharp.Syntax.TypeCrefSyntax node)
        {
    		var result = new XElement("TypeCref");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a QualifiedCrefSyntax node.
        /// </summary>
        public override XElement VisitQualifiedCref(Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedCrefSyntax node)
        {
    		var result = new XElement("QualifiedCref");
    		//Container
    		var xContainer = this.Visit(node.Container);
    		xContainer.Add(new XAttribute("part", "Container"));
    		result.Add(xContainer);
    		//DotToken
    		var xDotToken = this.Visit(node.DotToken);
    		xDotToken.Add(new XAttribute("part", "DotToken"));
    		result.Add(xDotToken);
    		//Member
    		var xMember = this.Visit(node.Member);
    		xMember.Add(new XAttribute("part", "Member"));
    		result.Add(xMember);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a NameMemberCrefSyntax node.
        /// </summary>
        public override XElement VisitNameMemberCref(Microsoft.CodeAnalysis.CSharp.Syntax.NameMemberCrefSyntax node)
        {
    		var result = new XElement("NameMemberCref");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//Parameters
    		if(node.Parameters != null)
    		{
    			var xParameters = this.Visit(node.Parameters);
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			result.Add(xParameters);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IndexerMemberCrefSyntax node.
        /// </summary>
        public override XElement VisitIndexerMemberCref(Microsoft.CodeAnalysis.CSharp.Syntax.IndexerMemberCrefSyntax node)
        {
    		var result = new XElement("IndexerMemberCref");
    		//ThisKeyword
    		var xThisKeyword = this.Visit(node.ThisKeyword);
    		xThisKeyword.Add(new XAttribute("part", "ThisKeyword"));
    		result.Add(xThisKeyword);
    		//Parameters
    		if(node.Parameters != null)
    		{
    			var xParameters = this.Visit(node.Parameters);
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			result.Add(xParameters);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OperatorMemberCrefSyntax node.
        /// </summary>
        public override XElement VisitOperatorMemberCref(Microsoft.CodeAnalysis.CSharp.Syntax.OperatorMemberCrefSyntax node)
        {
    		var result = new XElement("OperatorMemberCref");
    		//OperatorKeyword
    		var xOperatorKeyword = this.Visit(node.OperatorKeyword);
    		xOperatorKeyword.Add(new XAttribute("part", "OperatorKeyword"));
    		result.Add(xOperatorKeyword);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Parameters
    		if(node.Parameters != null)
    		{
    			var xParameters = this.Visit(node.Parameters);
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			result.Add(xParameters);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConversionOperatorMemberCrefSyntax node.
        /// </summary>
        public override XElement VisitConversionOperatorMemberCref(Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorMemberCrefSyntax node)
        {
    		var result = new XElement("ConversionOperatorMemberCref");
    		//ImplicitOrExplicitKeyword
    		var xImplicitOrExplicitKeyword = this.Visit(node.ImplicitOrExplicitKeyword);
    		xImplicitOrExplicitKeyword.Add(new XAttribute("part", "ImplicitOrExplicitKeyword"));
    		result.Add(xImplicitOrExplicitKeyword);
    		//OperatorKeyword
    		var xOperatorKeyword = this.Visit(node.OperatorKeyword);
    		xOperatorKeyword.Add(new XAttribute("part", "OperatorKeyword"));
    		result.Add(xOperatorKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Parameters
    		if(node.Parameters != null)
    		{
    			var xParameters = this.Visit(node.Parameters);
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			result.Add(xParameters);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CrefParameterListSyntax node.
        /// </summary>
        public override XElement VisitCrefParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterListSyntax node)
        {
    		var result = new XElement("CrefParameterList");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Parameters
    		if(node.Parameters.Count > 0)
    		{
    			var xParameters = new XElement("SeparatedList_of_CrefParameter");
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			foreach(var x in node.Parameters)
    			{
    				var xElement = this.Visit(x);
    				xParameters.Add(xElement);
    			}
    			result.Add(xParameters);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CrefBracketedParameterListSyntax node.
        /// </summary>
        public override XElement VisitCrefBracketedParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.CrefBracketedParameterListSyntax node)
        {
    		var result = new XElement("CrefBracketedParameterList");
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Parameters
    		if(node.Parameters.Count > 0)
    		{
    			var xParameters = new XElement("SeparatedList_of_CrefParameter");
    			xParameters.Add(new XAttribute("part", "Parameters"));
    			foreach(var x in node.Parameters)
    			{
    				var xElement = this.Visit(x);
    				xParameters.Add(xElement);
    			}
    			result.Add(xParameters);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlElementSyntax node.
        /// </summary>
        public override XElement VisitXmlElement(Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementSyntax node)
        {
    		var result = new XElement("XmlElement");
    		//StartTag
    		var xStartTag = this.Visit(node.StartTag);
    		xStartTag.Add(new XAttribute("part", "StartTag"));
    		result.Add(xStartTag);
    		//Content
    		if(node.Content.Count > 0)
    		{
    			var xContent = new XElement("List_of_XmlNode");
    			xContent.Add(new XAttribute("part", "Content"));
    			foreach(var x in node.Content)
    			{
    				var xElement = this.Visit(x);
    				xContent.Add(xElement);
    			}
    			result.Add(xContent);
    		}
    		//EndTag
    		var xEndTag = this.Visit(node.EndTag);
    		xEndTag.Add(new XAttribute("part", "EndTag"));
    		result.Add(xEndTag);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlEmptyElementSyntax node.
        /// </summary>
        public override XElement VisitXmlEmptyElement(Microsoft.CodeAnalysis.CSharp.Syntax.XmlEmptyElementSyntax node)
        {
    		var result = new XElement("XmlEmptyElement");
    		//LessThanToken
    		var xLessThanToken = this.Visit(node.LessThanToken);
    		xLessThanToken.Add(new XAttribute("part", "LessThanToken"));
    		result.Add(xLessThanToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//Attributes
    		if(node.Attributes.Count > 0)
    		{
    			var xAttributes = new XElement("List_of_XmlAttribute");
    			xAttributes.Add(new XAttribute("part", "Attributes"));
    			foreach(var x in node.Attributes)
    			{
    				var xElement = this.Visit(x);
    				xAttributes.Add(xElement);
    			}
    			result.Add(xAttributes);
    		}
    		//SlashGreaterThanToken
    		var xSlashGreaterThanToken = this.Visit(node.SlashGreaterThanToken);
    		xSlashGreaterThanToken.Add(new XAttribute("part", "SlashGreaterThanToken"));
    		result.Add(xSlashGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlTextSyntax node.
        /// </summary>
        public override XElement VisitXmlText(Microsoft.CodeAnalysis.CSharp.Syntax.XmlTextSyntax node)
        {
    		var result = new XElement("XmlText");
    		//TextTokens
    		if(node.TextTokens.Count > 0)
    		{
    			var xTextTokens = new XElement("TokenList");
    			xTextTokens.Add(new XAttribute("part", "TextTokens"));
    			foreach(var x in node.TextTokens)
    			{
    				var xElement = this.Visit(x);
    				xTextTokens.Add(xElement);
    			}
    			result.Add(xTextTokens);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlCDataSectionSyntax node.
        /// </summary>
        public override XElement VisitXmlCDataSection(Microsoft.CodeAnalysis.CSharp.Syntax.XmlCDataSectionSyntax node)
        {
    		var result = new XElement("XmlCDataSection");
    		//StartCDataToken
    		var xStartCDataToken = this.Visit(node.StartCDataToken);
    		xStartCDataToken.Add(new XAttribute("part", "StartCDataToken"));
    		result.Add(xStartCDataToken);
    		//TextTokens
    		if(node.TextTokens.Count > 0)
    		{
    			var xTextTokens = new XElement("TokenList");
    			xTextTokens.Add(new XAttribute("part", "TextTokens"));
    			foreach(var x in node.TextTokens)
    			{
    				var xElement = this.Visit(x);
    				xTextTokens.Add(xElement);
    			}
    			result.Add(xTextTokens);
    		}
    		//EndCDataToken
    		var xEndCDataToken = this.Visit(node.EndCDataToken);
    		xEndCDataToken.Add(new XAttribute("part", "EndCDataToken"));
    		result.Add(xEndCDataToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlProcessingInstructionSyntax node.
        /// </summary>
        public override XElement VisitXmlProcessingInstruction(Microsoft.CodeAnalysis.CSharp.Syntax.XmlProcessingInstructionSyntax node)
        {
    		var result = new XElement("XmlProcessingInstruction");
    		//StartProcessingInstructionToken
    		var xStartProcessingInstructionToken = this.Visit(node.StartProcessingInstructionToken);
    		xStartProcessingInstructionToken.Add(new XAttribute("part", "StartProcessingInstructionToken"));
    		result.Add(xStartProcessingInstructionToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//TextTokens
    		if(node.TextTokens.Count > 0)
    		{
    			var xTextTokens = new XElement("TokenList");
    			xTextTokens.Add(new XAttribute("part", "TextTokens"));
    			foreach(var x in node.TextTokens)
    			{
    				var xElement = this.Visit(x);
    				xTextTokens.Add(xElement);
    			}
    			result.Add(xTextTokens);
    		}
    		//EndProcessingInstructionToken
    		var xEndProcessingInstructionToken = this.Visit(node.EndProcessingInstructionToken);
    		xEndProcessingInstructionToken.Add(new XAttribute("part", "EndProcessingInstructionToken"));
    		result.Add(xEndProcessingInstructionToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlCommentSyntax node.
        /// </summary>
        public override XElement VisitXmlComment(Microsoft.CodeAnalysis.CSharp.Syntax.XmlCommentSyntax node)
        {
    		var result = new XElement("XmlComment");
    		//LessThanExclamationMinusMinusToken
    		var xLessThanExclamationMinusMinusToken = this.Visit(node.LessThanExclamationMinusMinusToken);
    		xLessThanExclamationMinusMinusToken.Add(new XAttribute("part", "LessThanExclamationMinusMinusToken"));
    		result.Add(xLessThanExclamationMinusMinusToken);
    		//TextTokens
    		if(node.TextTokens.Count > 0)
    		{
    			var xTextTokens = new XElement("TokenList");
    			xTextTokens.Add(new XAttribute("part", "TextTokens"));
    			foreach(var x in node.TextTokens)
    			{
    				var xElement = this.Visit(x);
    				xTextTokens.Add(xElement);
    			}
    			result.Add(xTextTokens);
    		}
    		//MinusMinusGreaterThanToken
    		var xMinusMinusGreaterThanToken = this.Visit(node.MinusMinusGreaterThanToken);
    		xMinusMinusGreaterThanToken.Add(new XAttribute("part", "MinusMinusGreaterThanToken"));
    		result.Add(xMinusMinusGreaterThanToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlTextAttributeSyntax node.
        /// </summary>
        public override XElement VisitXmlTextAttribute(Microsoft.CodeAnalysis.CSharp.Syntax.XmlTextAttributeSyntax node)
        {
    		var result = new XElement("XmlTextAttribute");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    		//StartQuoteToken
    		var xStartQuoteToken = this.Visit(node.StartQuoteToken);
    		xStartQuoteToken.Add(new XAttribute("part", "StartQuoteToken"));
    		result.Add(xStartQuoteToken);
    		//TextTokens
    		if(node.TextTokens.Count > 0)
    		{
    			var xTextTokens = new XElement("TokenList");
    			xTextTokens.Add(new XAttribute("part", "TextTokens"));
    			foreach(var x in node.TextTokens)
    			{
    				var xElement = this.Visit(x);
    				xTextTokens.Add(xElement);
    			}
    			result.Add(xTextTokens);
    		}
    		//EndQuoteToken
    		var xEndQuoteToken = this.Visit(node.EndQuoteToken);
    		xEndQuoteToken.Add(new XAttribute("part", "EndQuoteToken"));
    		result.Add(xEndQuoteToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlCrefAttributeSyntax node.
        /// </summary>
        public override XElement VisitXmlCrefAttribute(Microsoft.CodeAnalysis.CSharp.Syntax.XmlCrefAttributeSyntax node)
        {
    		var result = new XElement("XmlCrefAttribute");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    		//StartQuoteToken
    		var xStartQuoteToken = this.Visit(node.StartQuoteToken);
    		xStartQuoteToken.Add(new XAttribute("part", "StartQuoteToken"));
    		result.Add(xStartQuoteToken);
    		//Cref
    		var xCref = this.Visit(node.Cref);
    		xCref.Add(new XAttribute("part", "Cref"));
    		result.Add(xCref);
    		//EndQuoteToken
    		var xEndQuoteToken = this.Visit(node.EndQuoteToken);
    		xEndQuoteToken.Add(new XAttribute("part", "EndQuoteToken"));
    		result.Add(xEndQuoteToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a XmlNameAttributeSyntax node.
        /// </summary>
        public override XElement VisitXmlNameAttribute(Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameAttributeSyntax node)
        {
    		var result = new XElement("XmlNameAttribute");
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    		//StartQuoteToken
    		var xStartQuoteToken = this.Visit(node.StartQuoteToken);
    		xStartQuoteToken.Add(new XAttribute("part", "StartQuoteToken"));
    		result.Add(xStartQuoteToken);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//EndQuoteToken
    		var xEndQuoteToken = this.Visit(node.EndQuoteToken);
    		xEndQuoteToken.Add(new XAttribute("part", "EndQuoteToken"));
    		result.Add(xEndQuoteToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ParenthesizedExpressionSyntax node.
        /// </summary>
        public override XElement VisitParenthesizedExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax node)
        {
    		var result = new XElement("ParenthesizedExpression");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TupleExpressionSyntax node.
        /// </summary>
        public override XElement VisitTupleExpression(Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax node)
        {
    		var result = new XElement("TupleExpression");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Arguments
    		if(node.Arguments.Count > 0)
    		{
    			var xArguments = new XElement("SeparatedList_of_Argument");
    			xArguments.Add(new XAttribute("part", "Arguments"));
    			foreach(var x in node.Arguments)
    			{
    				var xElement = this.Visit(x);
    				xArguments.Add(xElement);
    			}
    			result.Add(xArguments);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PrefixUnaryExpressionSyntax node.
        /// </summary>
        public override XElement VisitPrefixUnaryExpression(Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax node)
        {
    		var result = new XElement("PrefixUnaryExpression");
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Operand
    		var xOperand = this.Visit(node.Operand);
    		xOperand.Add(new XAttribute("part", "Operand"));
    		result.Add(xOperand);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AwaitExpressionSyntax node.
        /// </summary>
        public override XElement VisitAwaitExpression(Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax node)
        {
    		var result = new XElement("AwaitExpression");
    		//AwaitKeyword
    		var xAwaitKeyword = this.Visit(node.AwaitKeyword);
    		xAwaitKeyword.Add(new XAttribute("part", "AwaitKeyword"));
    		result.Add(xAwaitKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PostfixUnaryExpressionSyntax node.
        /// </summary>
        public override XElement VisitPostfixUnaryExpression(Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax node)
        {
    		var result = new XElement("PostfixUnaryExpression");
    		//Operand
    		var xOperand = this.Visit(node.Operand);
    		xOperand.Add(new XAttribute("part", "Operand"));
    		result.Add(xOperand);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a MemberAccessExpressionSyntax node.
        /// </summary>
        public override XElement VisitMemberAccessExpression(Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax node)
        {
    		var result = new XElement("MemberAccessExpression");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConditionalAccessExpressionSyntax node.
        /// </summary>
        public override XElement VisitConditionalAccessExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax node)
        {
    		var result = new XElement("ConditionalAccessExpression");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//WhenNotNull
    		var xWhenNotNull = this.Visit(node.WhenNotNull);
    		xWhenNotNull.Add(new XAttribute("part", "WhenNotNull"));
    		result.Add(xWhenNotNull);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a MemberBindingExpressionSyntax node.
        /// </summary>
        public override XElement VisitMemberBindingExpression(Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax node)
        {
    		var result = new XElement("MemberBindingExpression");
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ElementBindingExpressionSyntax node.
        /// </summary>
        public override XElement VisitElementBindingExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax node)
        {
    		var result = new XElement("ElementBindingExpression");
    		//ArgumentList
    		var xArgumentList = this.Visit(node.ArgumentList);
    		xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    		result.Add(xArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ImplicitElementAccessSyntax node.
        /// </summary>
        public override XElement VisitImplicitElementAccess(Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax node)
        {
    		var result = new XElement("ImplicitElementAccess");
    		//ArgumentList
    		var xArgumentList = this.Visit(node.ArgumentList);
    		xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    		result.Add(xArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BinaryExpressionSyntax node.
        /// </summary>
        public override XElement VisitBinaryExpression(Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax node)
        {
    		var result = new XElement("BinaryExpression");
    		//Left
    		var xLeft = this.Visit(node.Left);
    		xLeft.Add(new XAttribute("part", "Left"));
    		result.Add(xLeft);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Right
    		var xRight = this.Visit(node.Right);
    		xRight.Add(new XAttribute("part", "Right"));
    		result.Add(xRight);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AssignmentExpressionSyntax node.
        /// </summary>
        public override XElement VisitAssignmentExpression(Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax node)
        {
    		var result = new XElement("AssignmentExpression");
    		//Left
    		var xLeft = this.Visit(node.Left);
    		xLeft.Add(new XAttribute("part", "Left"));
    		result.Add(xLeft);
    		//OperatorToken
    		var xOperatorToken = this.Visit(node.OperatorToken);
    		xOperatorToken.Add(new XAttribute("part", "OperatorToken"));
    		result.Add(xOperatorToken);
    		//Right
    		var xRight = this.Visit(node.Right);
    		xRight.Add(new XAttribute("part", "Right"));
    		result.Add(xRight);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConditionalExpressionSyntax node.
        /// </summary>
        public override XElement VisitConditionalExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax node)
        {
    		var result = new XElement("ConditionalExpression");
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//QuestionToken
    		var xQuestionToken = this.Visit(node.QuestionToken);
    		xQuestionToken.Add(new XAttribute("part", "QuestionToken"));
    		result.Add(xQuestionToken);
    		//WhenTrue
    		var xWhenTrue = this.Visit(node.WhenTrue);
    		xWhenTrue.Add(new XAttribute("part", "WhenTrue"));
    		result.Add(xWhenTrue);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//WhenFalse
    		var xWhenFalse = this.Visit(node.WhenFalse);
    		xWhenFalse.Add(new XAttribute("part", "WhenFalse"));
    		result.Add(xWhenFalse);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LiteralExpressionSyntax node.
        /// </summary>
        public override XElement VisitLiteralExpression(Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax node)
        {
    		var result = new XElement("LiteralExpression");
    		//Token
    		var xToken = this.Visit(node.Token);
    		xToken.Add(new XAttribute("part", "Token"));
    		result.Add(xToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a MakeRefExpressionSyntax node.
        /// </summary>
        public override XElement VisitMakeRefExpression(Microsoft.CodeAnalysis.CSharp.Syntax.MakeRefExpressionSyntax node)
        {
    		var result = new XElement("MakeRefExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a RefTypeExpressionSyntax node.
        /// </summary>
        public override XElement VisitRefTypeExpression(Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax node)
        {
    		var result = new XElement("RefTypeExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a RefValueExpressionSyntax node.
        /// </summary>
        public override XElement VisitRefValueExpression(Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax node)
        {
    		var result = new XElement("RefValueExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//Comma
    		var xComma = this.Visit(node.Comma);
    		xComma.Add(new XAttribute("part", "Comma"));
    		result.Add(xComma);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CheckedExpressionSyntax node.
        /// </summary>
        public override XElement VisitCheckedExpression(Microsoft.CodeAnalysis.CSharp.Syntax.CheckedExpressionSyntax node)
        {
    		var result = new XElement("CheckedExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DefaultExpressionSyntax node.
        /// </summary>
        public override XElement VisitDefaultExpression(Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax node)
        {
    		var result = new XElement("DefaultExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TypeOfExpressionSyntax node.
        /// </summary>
        public override XElement VisitTypeOfExpression(Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax node)
        {
    		var result = new XElement("TypeOfExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SizeOfExpressionSyntax node.
        /// </summary>
        public override XElement VisitSizeOfExpression(Microsoft.CodeAnalysis.CSharp.Syntax.SizeOfExpressionSyntax node)
        {
    		var result = new XElement("SizeOfExpression");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InvocationExpressionSyntax node.
        /// </summary>
        public override XElement VisitInvocationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax node)
        {
    		var result = new XElement("InvocationExpression");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//ArgumentList
    		var xArgumentList = this.Visit(node.ArgumentList);
    		xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    		result.Add(xArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ElementAccessExpressionSyntax node.
        /// </summary>
        public override XElement VisitElementAccessExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax node)
        {
    		var result = new XElement("ElementAccessExpression");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//ArgumentList
    		var xArgumentList = this.Visit(node.ArgumentList);
    		xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    		result.Add(xArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DeclarationExpressionSyntax node.
        /// </summary>
        public override XElement VisitDeclarationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax node)
        {
    		var result = new XElement("DeclarationExpression");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Designation
    		var xDesignation = this.Visit(node.Designation);
    		xDesignation.Add(new XAttribute("part", "Designation"));
    		result.Add(xDesignation);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CastExpressionSyntax node.
        /// </summary>
        public override XElement VisitCastExpression(Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax node)
        {
    		var result = new XElement("CastExpression");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a RefExpressionSyntax node.
        /// </summary>
        public override XElement VisitRefExpression(Microsoft.CodeAnalysis.CSharp.Syntax.RefExpressionSyntax node)
        {
    		var result = new XElement("RefExpression");
    		//RefKeyword
    		var xRefKeyword = this.Visit(node.RefKeyword);
    		xRefKeyword.Add(new XAttribute("part", "RefKeyword"));
    		result.Add(xRefKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InitializerExpressionSyntax node.
        /// </summary>
        public override XElement VisitInitializerExpression(Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax node)
        {
    		var result = new XElement("InitializerExpression");
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Expressions
    		if(node.Expressions.Count > 0)
    		{
    			var xExpressions = new XElement("SeparatedList_of_Expression");
    			xExpressions.Add(new XAttribute("part", "Expressions"));
    			foreach(var x in node.Expressions)
    			{
    				var xElement = this.Visit(x);
    				xExpressions.Add(xElement);
    			}
    			result.Add(xExpressions);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ObjectCreationExpressionSyntax node.
        /// </summary>
        public override XElement VisitObjectCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ObjectCreationExpressionSyntax node)
        {
    		var result = new XElement("ObjectCreationExpression");
    		//NewKeyword
    		var xNewKeyword = this.Visit(node.NewKeyword);
    		xNewKeyword.Add(new XAttribute("part", "NewKeyword"));
    		result.Add(xNewKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//ArgumentList
    		if(node.ArgumentList != null)
    		{
    			var xArgumentList = this.Visit(node.ArgumentList);
    			xArgumentList.Add(new XAttribute("part", "ArgumentList"));
    			result.Add(xArgumentList);
    		}
    		//Initializer
    		if(node.Initializer != null)
    		{
    			var xInitializer = this.Visit(node.Initializer);
    			xInitializer.Add(new XAttribute("part", "Initializer"));
    			result.Add(xInitializer);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AnonymousObjectCreationExpressionSyntax node.
        /// </summary>
        public override XElement VisitAnonymousObjectCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax node)
        {
    		var result = new XElement("AnonymousObjectCreationExpression");
    		//NewKeyword
    		var xNewKeyword = this.Visit(node.NewKeyword);
    		xNewKeyword.Add(new XAttribute("part", "NewKeyword"));
    		result.Add(xNewKeyword);
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Initializers
    		if(node.Initializers.Count > 0)
    		{
    			var xInitializers = new XElement("SeparatedList_of_AnonymousObjectMemberDeclarator");
    			xInitializers.Add(new XAttribute("part", "Initializers"));
    			foreach(var x in node.Initializers)
    			{
    				var xElement = this.Visit(x);
    				xInitializers.Add(xElement);
    			}
    			result.Add(xInitializers);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArrayCreationExpressionSyntax node.
        /// </summary>
        public override XElement VisitArrayCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax node)
        {
    		var result = new XElement("ArrayCreationExpression");
    		//NewKeyword
    		var xNewKeyword = this.Visit(node.NewKeyword);
    		xNewKeyword.Add(new XAttribute("part", "NewKeyword"));
    		result.Add(xNewKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Initializer
    		if(node.Initializer != null)
    		{
    			var xInitializer = this.Visit(node.Initializer);
    			xInitializer.Add(new XAttribute("part", "Initializer"));
    			result.Add(xInitializer);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ImplicitArrayCreationExpressionSyntax node.
        /// </summary>
        public override XElement VisitImplicitArrayCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitArrayCreationExpressionSyntax node)
        {
    		var result = new XElement("ImplicitArrayCreationExpression");
    		//NewKeyword
    		var xNewKeyword = this.Visit(node.NewKeyword);
    		xNewKeyword.Add(new XAttribute("part", "NewKeyword"));
    		result.Add(xNewKeyword);
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Commas
    		if(node.Commas.Count > 0)
    		{
    			var xCommas = new XElement("TokenList");
    			xCommas.Add(new XAttribute("part", "Commas"));
    			foreach(var x in node.Commas)
    			{
    				var xElement = this.Visit(x);
    				xCommas.Add(xElement);
    			}
    			result.Add(xCommas);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    		//Initializer
    		var xInitializer = this.Visit(node.Initializer);
    		xInitializer.Add(new XAttribute("part", "Initializer"));
    		result.Add(xInitializer);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a StackAllocArrayCreationExpressionSyntax node.
        /// </summary>
        public override XElement VisitStackAllocArrayCreationExpression(Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax node)
        {
    		var result = new XElement("StackAllocArrayCreationExpression");
    		//StackAllocKeyword
    		var xStackAllocKeyword = this.Visit(node.StackAllocKeyword);
    		xStackAllocKeyword.Add(new XAttribute("part", "StackAllocKeyword"));
    		result.Add(xStackAllocKeyword);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a QueryExpressionSyntax node.
        /// </summary>
        public override XElement VisitQueryExpression(Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax node)
        {
    		var result = new XElement("QueryExpression");
    		//FromClause
    		var xFromClause = this.Visit(node.FromClause);
    		xFromClause.Add(new XAttribute("part", "FromClause"));
    		result.Add(xFromClause);
    		//Body
    		var xBody = this.Visit(node.Body);
    		xBody.Add(new XAttribute("part", "Body"));
    		result.Add(xBody);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OmittedArraySizeExpressionSyntax node.
        /// </summary>
        public override XElement VisitOmittedArraySizeExpression(Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax node)
        {
    		var result = new XElement("OmittedArraySizeExpression");
    		//OmittedArraySizeExpressionToken
    		var xOmittedArraySizeExpressionToken = this.Visit(node.OmittedArraySizeExpressionToken);
    		xOmittedArraySizeExpressionToken.Add(new XAttribute("part", "OmittedArraySizeExpressionToken"));
    		result.Add(xOmittedArraySizeExpressionToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterpolatedStringExpressionSyntax node.
        /// </summary>
        public override XElement VisitInterpolatedStringExpression(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax node)
        {
    		var result = new XElement("InterpolatedStringExpression");
    		//StringStartToken
    		var xStringStartToken = this.Visit(node.StringStartToken);
    		xStringStartToken.Add(new XAttribute("part", "StringStartToken"));
    		result.Add(xStringStartToken);
    		//Contents
    		if(node.Contents.Count > 0)
    		{
    			var xContents = new XElement("List_of_InterpolatedStringContent");
    			xContents.Add(new XAttribute("part", "Contents"));
    			foreach(var x in node.Contents)
    			{
    				var xElement = this.Visit(x);
    				xContents.Add(xElement);
    			}
    			result.Add(xContents);
    		}
    		//StringEndToken
    		var xStringEndToken = this.Visit(node.StringEndToken);
    		xStringEndToken.Add(new XAttribute("part", "StringEndToken"));
    		result.Add(xStringEndToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IsPatternExpressionSyntax node.
        /// </summary>
        public override XElement VisitIsPatternExpression(Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax node)
        {
    		var result = new XElement("IsPatternExpression");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//IsKeyword
    		var xIsKeyword = this.Visit(node.IsKeyword);
    		xIsKeyword.Add(new XAttribute("part", "IsKeyword"));
    		result.Add(xIsKeyword);
    		//Pattern
    		var xPattern = this.Visit(node.Pattern);
    		xPattern.Add(new XAttribute("part", "Pattern"));
    		result.Add(xPattern);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ThrowExpressionSyntax node.
        /// </summary>
        public override XElement VisitThrowExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax node)
        {
    		var result = new XElement("ThrowExpression");
    		//ThrowKeyword
    		var xThrowKeyword = this.Visit(node.ThrowKeyword);
    		xThrowKeyword.Add(new XAttribute("part", "ThrowKeyword"));
    		result.Add(xThrowKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PredefinedTypeSyntax node.
        /// </summary>
        public override XElement VisitPredefinedType(Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax node)
        {
    		var result = new XElement("PredefinedType");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArrayTypeSyntax node.
        /// </summary>
        public override XElement VisitArrayType(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax node)
        {
    		var result = new XElement("ArrayType");
    		//ElementType
    		var xElementType = this.Visit(node.ElementType);
    		xElementType.Add(new XAttribute("part", "ElementType"));
    		result.Add(xElementType);
    		//RankSpecifiers
    		if(node.RankSpecifiers.Count > 0)
    		{
    			var xRankSpecifiers = new XElement("List_of_ArrayRankSpecifier");
    			xRankSpecifiers.Add(new XAttribute("part", "RankSpecifiers"));
    			foreach(var x in node.RankSpecifiers)
    			{
    				var xElement = this.Visit(x);
    				xRankSpecifiers.Add(xElement);
    			}
    			result.Add(xRankSpecifiers);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a PointerTypeSyntax node.
        /// </summary>
        public override XElement VisitPointerType(Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax node)
        {
    		var result = new XElement("PointerType");
    		//ElementType
    		var xElementType = this.Visit(node.ElementType);
    		xElementType.Add(new XAttribute("part", "ElementType"));
    		result.Add(xElementType);
    		//AsteriskToken
    		var xAsteriskToken = this.Visit(node.AsteriskToken);
    		xAsteriskToken.Add(new XAttribute("part", "AsteriskToken"));
    		result.Add(xAsteriskToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a NullableTypeSyntax node.
        /// </summary>
        public override XElement VisitNullableType(Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax node)
        {
    		var result = new XElement("NullableType");
    		//ElementType
    		var xElementType = this.Visit(node.ElementType);
    		xElementType.Add(new XAttribute("part", "ElementType"));
    		result.Add(xElementType);
    		//QuestionToken
    		var xQuestionToken = this.Visit(node.QuestionToken);
    		xQuestionToken.Add(new XAttribute("part", "QuestionToken"));
    		result.Add(xQuestionToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TupleTypeSyntax node.
        /// </summary>
        public override XElement VisitTupleType(Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax node)
        {
    		var result = new XElement("TupleType");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Elements
    		if(node.Elements.Count > 0)
    		{
    			var xElements = new XElement("SeparatedList_of_TupleElement");
    			xElements.Add(new XAttribute("part", "Elements"));
    			foreach(var x in node.Elements)
    			{
    				var xElement = this.Visit(x);
    				xElements.Add(xElement);
    			}
    			result.Add(xElements);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OmittedTypeArgumentSyntax node.
        /// </summary>
        public override XElement VisitOmittedTypeArgument(Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax node)
        {
    		var result = new XElement("OmittedTypeArgument");
    		//OmittedTypeArgumentToken
    		var xOmittedTypeArgumentToken = this.Visit(node.OmittedTypeArgumentToken);
    		xOmittedTypeArgumentToken.Add(new XAttribute("part", "OmittedTypeArgumentToken"));
    		result.Add(xOmittedTypeArgumentToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a RefTypeSyntax node.
        /// </summary>
        public override XElement VisitRefType(Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax node)
        {
    		var result = new XElement("RefType");
    		//RefKeyword
    		var xRefKeyword = this.Visit(node.RefKeyword);
    		xRefKeyword.Add(new XAttribute("part", "RefKeyword"));
    		result.Add(xRefKeyword);
    		//ReadOnlyKeyword
    		if(node.ReadOnlyKeyword != null && node.ReadOnlyKeyword.Kind() != SyntaxKind.None)
    		{
    			var xReadOnlyKeyword = this.Visit(node.ReadOnlyKeyword);
    			xReadOnlyKeyword.Add(new XAttribute("part", "ReadOnlyKeyword"));
    			result.Add(xReadOnlyKeyword);
    		}
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a QualifiedNameSyntax node.
        /// </summary>
        public override XElement VisitQualifiedName(Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax node)
        {
    		var result = new XElement("QualifiedName");
    		//Left
    		var xLeft = this.Visit(node.Left);
    		xLeft.Add(new XAttribute("part", "Left"));
    		result.Add(xLeft);
    		//DotToken
    		var xDotToken = this.Visit(node.DotToken);
    		xDotToken.Add(new XAttribute("part", "DotToken"));
    		result.Add(xDotToken);
    		//Right
    		var xRight = this.Visit(node.Right);
    		xRight.Add(new XAttribute("part", "Right"));
    		result.Add(xRight);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AliasQualifiedNameSyntax node.
        /// </summary>
        public override XElement VisitAliasQualifiedName(Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax node)
        {
    		var result = new XElement("AliasQualifiedName");
    		//Alias
    		var xAlias = this.Visit(node.Alias);
    		xAlias.Add(new XAttribute("part", "Alias"));
    		result.Add(xAlias);
    		//ColonColonToken
    		if(node.ColonColonToken != null && node.ColonColonToken.Kind() != SyntaxKind.None)
    		{
    			var xColonColonToken = this.Visit(node.ColonColonToken);
    			xColonColonToken.Add(new XAttribute("part", "ColonColonToken"));
    			result.Add(xColonColonToken);
    		}
    		//Name
    		var xName = this.Visit(node.Name);
    		xName.Add(new XAttribute("part", "Name"));
    		result.Add(xName);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IdentifierNameSyntax node.
        /// </summary>
        public override XElement VisitIdentifierName(Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax node)
        {
    		var result = new XElement("IdentifierName");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a GenericNameSyntax node.
        /// </summary>
        public override XElement VisitGenericName(Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax node)
        {
    		var result = new XElement("GenericName");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeArgumentList
    		var xTypeArgumentList = this.Visit(node.TypeArgumentList);
    		xTypeArgumentList.Add(new XAttribute("part", "TypeArgumentList"));
    		result.Add(xTypeArgumentList);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ThisExpressionSyntax node.
        /// </summary>
        public override XElement VisitThisExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ThisExpressionSyntax node)
        {
    		var result = new XElement("ThisExpression");
    		//Token
    		var xToken = this.Visit(node.Token);
    		xToken.Add(new XAttribute("part", "Token"));
    		result.Add(xToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BaseExpressionSyntax node.
        /// </summary>
        public override XElement VisitBaseExpression(Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionSyntax node)
        {
    		var result = new XElement("BaseExpression");
    		//Token
    		var xToken = this.Visit(node.Token);
    		xToken.Add(new XAttribute("part", "Token"));
    		result.Add(xToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a AnonymousMethodExpressionSyntax node.
        /// </summary>
        public override XElement VisitAnonymousMethodExpression(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousMethodExpressionSyntax node)
        {
    		var result = new XElement("AnonymousMethodExpression");
    		//AsyncKeyword
    		if(node.AsyncKeyword != null && node.AsyncKeyword.Kind() != SyntaxKind.None)
    		{
    			var xAsyncKeyword = this.Visit(node.AsyncKeyword);
    			xAsyncKeyword.Add(new XAttribute("part", "AsyncKeyword"));
    			result.Add(xAsyncKeyword);
    		}
    		//DelegateKeyword
    		var xDelegateKeyword = this.Visit(node.DelegateKeyword);
    		xDelegateKeyword.Add(new XAttribute("part", "DelegateKeyword"));
    		result.Add(xDelegateKeyword);
    		//ParameterList
    		if(node.ParameterList != null)
    		{
    			var xParameterList = this.Visit(node.ParameterList);
    			xParameterList.Add(new XAttribute("part", "ParameterList"));
    			result.Add(xParameterList);
    		}
    		//Body
    		var xBody = this.Visit(node.Body);
    		xBody.Add(new XAttribute("part", "Body"));
    		result.Add(xBody);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SimpleLambdaExpressionSyntax node.
        /// </summary>
        public override XElement VisitSimpleLambdaExpression(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleLambdaExpressionSyntax node)
        {
    		var result = new XElement("SimpleLambdaExpression");
    		//AsyncKeyword
    		if(node.AsyncKeyword != null && node.AsyncKeyword.Kind() != SyntaxKind.None)
    		{
    			var xAsyncKeyword = this.Visit(node.AsyncKeyword);
    			xAsyncKeyword.Add(new XAttribute("part", "AsyncKeyword"));
    			result.Add(xAsyncKeyword);
    		}
    		//Parameter
    		var xParameter = this.Visit(node.Parameter);
    		xParameter.Add(new XAttribute("part", "Parameter"));
    		result.Add(xParameter);
    		//ArrowToken
    		var xArrowToken = this.Visit(node.ArrowToken);
    		xArrowToken.Add(new XAttribute("part", "ArrowToken"));
    		result.Add(xArrowToken);
    		//Body
    		var xBody = this.Visit(node.Body);
    		xBody.Add(new XAttribute("part", "Body"));
    		result.Add(xBody);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ParenthesizedLambdaExpressionSyntax node.
        /// </summary>
        public override XElement VisitParenthesizedLambdaExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax node)
        {
    		var result = new XElement("ParenthesizedLambdaExpression");
    		//AsyncKeyword
    		if(node.AsyncKeyword != null && node.AsyncKeyword.Kind() != SyntaxKind.None)
    		{
    			var xAsyncKeyword = this.Visit(node.AsyncKeyword);
    			xAsyncKeyword.Add(new XAttribute("part", "AsyncKeyword"));
    			result.Add(xAsyncKeyword);
    		}
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//ArrowToken
    		var xArrowToken = this.Visit(node.ArrowToken);
    		xArrowToken.Add(new XAttribute("part", "ArrowToken"));
    		result.Add(xArrowToken);
    		//Body
    		var xBody = this.Visit(node.Body);
    		xBody.Add(new XAttribute("part", "Body"));
    		result.Add(xBody);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ArgumentListSyntax node.
        /// </summary>
        public override XElement VisitArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax node)
        {
    		var result = new XElement("ArgumentList");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Arguments
    		if(node.Arguments.Count > 0)
    		{
    			var xArguments = new XElement("SeparatedList_of_Argument");
    			xArguments.Add(new XAttribute("part", "Arguments"));
    			foreach(var x in node.Arguments)
    			{
    				var xElement = this.Visit(x);
    				xArguments.Add(xElement);
    			}
    			result.Add(xArguments);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BracketedArgumentListSyntax node.
        /// </summary>
        public override XElement VisitBracketedArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax node)
        {
    		var result = new XElement("BracketedArgumentList");
    		//OpenBracketToken
    		var xOpenBracketToken = this.Visit(node.OpenBracketToken);
    		xOpenBracketToken.Add(new XAttribute("part", "OpenBracketToken"));
    		result.Add(xOpenBracketToken);
    		//Arguments
    		if(node.Arguments.Count > 0)
    		{
    			var xArguments = new XElement("SeparatedList_of_Argument");
    			xArguments.Add(new XAttribute("part", "Arguments"));
    			foreach(var x in node.Arguments)
    			{
    				var xElement = this.Visit(x);
    				xArguments.Add(xElement);
    			}
    			result.Add(xArguments);
    		}
    		//CloseBracketToken
    		var xCloseBracketToken = this.Visit(node.CloseBracketToken);
    		xCloseBracketToken.Add(new XAttribute("part", "CloseBracketToken"));
    		result.Add(xCloseBracketToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a FromClauseSyntax node.
        /// </summary>
        public override XElement VisitFromClause(Microsoft.CodeAnalysis.CSharp.Syntax.FromClauseSyntax node)
        {
    		var result = new XElement("FromClause");
    		//FromKeyword
    		var xFromKeyword = this.Visit(node.FromKeyword);
    		xFromKeyword.Add(new XAttribute("part", "FromKeyword"));
    		result.Add(xFromKeyword);
    		//Type
    		if(node.Type != null)
    		{
    			var xType = this.Visit(node.Type);
    			xType.Add(new XAttribute("part", "Type"));
    			result.Add(xType);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//InKeyword
    		var xInKeyword = this.Visit(node.InKeyword);
    		xInKeyword.Add(new XAttribute("part", "InKeyword"));
    		result.Add(xInKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LetClauseSyntax node.
        /// </summary>
        public override XElement VisitLetClause(Microsoft.CodeAnalysis.CSharp.Syntax.LetClauseSyntax node)
        {
    		var result = new XElement("LetClause");
    		//LetKeyword
    		var xLetKeyword = this.Visit(node.LetKeyword);
    		xLetKeyword.Add(new XAttribute("part", "LetKeyword"));
    		result.Add(xLetKeyword);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//EqualsToken
    		var xEqualsToken = this.Visit(node.EqualsToken);
    		xEqualsToken.Add(new XAttribute("part", "EqualsToken"));
    		result.Add(xEqualsToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a JoinClauseSyntax node.
        /// </summary>
        public override XElement VisitJoinClause(Microsoft.CodeAnalysis.CSharp.Syntax.JoinClauseSyntax node)
        {
    		var result = new XElement("JoinClause");
    		//JoinKeyword
    		var xJoinKeyword = this.Visit(node.JoinKeyword);
    		xJoinKeyword.Add(new XAttribute("part", "JoinKeyword"));
    		result.Add(xJoinKeyword);
    		//Type
    		if(node.Type != null)
    		{
    			var xType = this.Visit(node.Type);
    			xType.Add(new XAttribute("part", "Type"));
    			result.Add(xType);
    		}
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//InKeyword
    		var xInKeyword = this.Visit(node.InKeyword);
    		xInKeyword.Add(new XAttribute("part", "InKeyword"));
    		result.Add(xInKeyword);
    		//InExpression
    		var xInExpression = this.Visit(node.InExpression);
    		xInExpression.Add(new XAttribute("part", "InExpression"));
    		result.Add(xInExpression);
    		//OnKeyword
    		var xOnKeyword = this.Visit(node.OnKeyword);
    		xOnKeyword.Add(new XAttribute("part", "OnKeyword"));
    		result.Add(xOnKeyword);
    		//LeftExpression
    		var xLeftExpression = this.Visit(node.LeftExpression);
    		xLeftExpression.Add(new XAttribute("part", "LeftExpression"));
    		result.Add(xLeftExpression);
    		//EqualsKeyword
    		var xEqualsKeyword = this.Visit(node.EqualsKeyword);
    		xEqualsKeyword.Add(new XAttribute("part", "EqualsKeyword"));
    		result.Add(xEqualsKeyword);
    		//RightExpression
    		var xRightExpression = this.Visit(node.RightExpression);
    		xRightExpression.Add(new XAttribute("part", "RightExpression"));
    		result.Add(xRightExpression);
    		//Into
    		if(node.Into != null)
    		{
    			var xInto = this.Visit(node.Into);
    			xInto.Add(new XAttribute("part", "Into"));
    			result.Add(xInto);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a WhereClauseSyntax node.
        /// </summary>
        public override XElement VisitWhereClause(Microsoft.CodeAnalysis.CSharp.Syntax.WhereClauseSyntax node)
        {
    		var result = new XElement("WhereClause");
    		//WhereKeyword
    		var xWhereKeyword = this.Visit(node.WhereKeyword);
    		xWhereKeyword.Add(new XAttribute("part", "WhereKeyword"));
    		result.Add(xWhereKeyword);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a OrderByClauseSyntax node.
        /// </summary>
        public override XElement VisitOrderByClause(Microsoft.CodeAnalysis.CSharp.Syntax.OrderByClauseSyntax node)
        {
    		var result = new XElement("OrderByClause");
    		//OrderByKeyword
    		var xOrderByKeyword = this.Visit(node.OrderByKeyword);
    		xOrderByKeyword.Add(new XAttribute("part", "OrderByKeyword"));
    		result.Add(xOrderByKeyword);
    		//Orderings
    		if(node.Orderings.Count > 0)
    		{
    			var xOrderings = new XElement("SeparatedList_of_Ordering");
    			xOrderings.Add(new XAttribute("part", "Orderings"));
    			foreach(var x in node.Orderings)
    			{
    				var xElement = this.Visit(x);
    				xOrderings.Add(xElement);
    			}
    			result.Add(xOrderings);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SelectClauseSyntax node.
        /// </summary>
        public override XElement VisitSelectClause(Microsoft.CodeAnalysis.CSharp.Syntax.SelectClauseSyntax node)
        {
    		var result = new XElement("SelectClause");
    		//SelectKeyword
    		var xSelectKeyword = this.Visit(node.SelectKeyword);
    		xSelectKeyword.Add(new XAttribute("part", "SelectKeyword"));
    		result.Add(xSelectKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a GroupClauseSyntax node.
        /// </summary>
        public override XElement VisitGroupClause(Microsoft.CodeAnalysis.CSharp.Syntax.GroupClauseSyntax node)
        {
    		var result = new XElement("GroupClause");
    		//GroupKeyword
    		var xGroupKeyword = this.Visit(node.GroupKeyword);
    		xGroupKeyword.Add(new XAttribute("part", "GroupKeyword"));
    		result.Add(xGroupKeyword);
    		//GroupExpression
    		var xGroupExpression = this.Visit(node.GroupExpression);
    		xGroupExpression.Add(new XAttribute("part", "GroupExpression"));
    		result.Add(xGroupExpression);
    		//ByKeyword
    		var xByKeyword = this.Visit(node.ByKeyword);
    		xByKeyword.Add(new XAttribute("part", "ByKeyword"));
    		result.Add(xByKeyword);
    		//ByExpression
    		var xByExpression = this.Visit(node.ByExpression);
    		xByExpression.Add(new XAttribute("part", "ByExpression"));
    		result.Add(xByExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DeclarationPatternSyntax node.
        /// </summary>
        public override XElement VisitDeclarationPattern(Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationPatternSyntax node)
        {
    		var result = new XElement("DeclarationPattern");
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Designation
    		var xDesignation = this.Visit(node.Designation);
    		xDesignation.Add(new XAttribute("part", "Designation"));
    		result.Add(xDesignation);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ConstantPatternSyntax node.
        /// </summary>
        public override XElement VisitConstantPattern(Microsoft.CodeAnalysis.CSharp.Syntax.ConstantPatternSyntax node)
        {
    		var result = new XElement("ConstantPattern");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterpolatedStringTextSyntax node.
        /// </summary>
        public override XElement VisitInterpolatedStringText(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringTextSyntax node)
        {
    		var result = new XElement("InterpolatedStringText");
    		//TextToken
    		var xTextToken = this.Visit(node.TextToken);
    		xTextToken.Add(new XAttribute("part", "TextToken"));
    		result.Add(xTextToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a InterpolationSyntax node.
        /// </summary>
        public override XElement VisitInterpolation(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationSyntax node)
        {
    		var result = new XElement("Interpolation");
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//AlignmentClause
    		if(node.AlignmentClause != null)
    		{
    			var xAlignmentClause = this.Visit(node.AlignmentClause);
    			xAlignmentClause.Add(new XAttribute("part", "AlignmentClause"));
    			result.Add(xAlignmentClause);
    		}
    		//FormatClause
    		if(node.FormatClause != null)
    		{
    			var xFormatClause = this.Visit(node.FormatClause);
    			xFormatClause.Add(new XAttribute("part", "FormatClause"));
    			result.Add(xFormatClause);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BlockSyntax node.
        /// </summary>
        public override XElement VisitBlock(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax node)
        {
    		var result = new XElement("Block");
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Statements
    		if(node.Statements.Count > 0)
    		{
    			var xStatements = new XElement("List_of_Statement");
    			xStatements.Add(new XAttribute("part", "Statements"));
    			foreach(var x in node.Statements)
    			{
    				var xElement = this.Visit(x);
    				xStatements.Add(xElement);
    			}
    			result.Add(xStatements);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LocalFunctionStatementSyntax node.
        /// </summary>
        public override XElement VisitLocalFunctionStatement(Microsoft.CodeAnalysis.CSharp.Syntax.LocalFunctionStatementSyntax node)
        {
    		var result = new XElement("LocalFunctionStatement");
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//ReturnType
    		var xReturnType = this.Visit(node.ReturnType);
    		xReturnType.Add(new XAttribute("part", "ReturnType"));
    		result.Add(xReturnType);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//TypeParameterList
    		if(node.TypeParameterList != null)
    		{
    			var xTypeParameterList = this.Visit(node.TypeParameterList);
    			xTypeParameterList.Add(new XAttribute("part", "TypeParameterList"));
    			result.Add(xTypeParameterList);
    		}
    		//ParameterList
    		var xParameterList = this.Visit(node.ParameterList);
    		xParameterList.Add(new XAttribute("part", "ParameterList"));
    		result.Add(xParameterList);
    		//ConstraintClauses
    		if(node.ConstraintClauses.Count > 0)
    		{
    			var xConstraintClauses = new XElement("List_of_TypeParameterConstraintClause");
    			xConstraintClauses.Add(new XAttribute("part", "ConstraintClauses"));
    			foreach(var x in node.ConstraintClauses)
    			{
    				var xElement = this.Visit(x);
    				xConstraintClauses.Add(xElement);
    			}
    			result.Add(xConstraintClauses);
    		}
    		//Body
    		if(node.Body != null)
    		{
    			var xBody = this.Visit(node.Body);
    			xBody.Add(new XAttribute("part", "Body"));
    			result.Add(xBody);
    		}
    		//ExpressionBody
    		if(node.ExpressionBody != null)
    		{
    			var xExpressionBody = this.Visit(node.ExpressionBody);
    			xExpressionBody.Add(new XAttribute("part", "ExpressionBody"));
    			result.Add(xExpressionBody);
    		}
    		//SemicolonToken
    		if(node.SemicolonToken != null && node.SemicolonToken.Kind() != SyntaxKind.None)
    		{
    			var xSemicolonToken = this.Visit(node.SemicolonToken);
    			xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    			result.Add(xSemicolonToken);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LocalDeclarationStatementSyntax node.
        /// </summary>
        public override XElement VisitLocalDeclarationStatement(Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax node)
        {
    		var result = new XElement("LocalDeclarationStatement");
    		//Modifiers
    		if(node.Modifiers.Count > 0)
    		{
    			var xModifiers = new XElement("TokenList");
    			xModifiers.Add(new XAttribute("part", "Modifiers"));
    			foreach(var x in node.Modifiers)
    			{
    				var xElement = this.Visit(x);
    				xModifiers.Add(xElement);
    			}
    			result.Add(xModifiers);
    		}
    		//Declaration
    		var xDeclaration = this.Visit(node.Declaration);
    		xDeclaration.Add(new XAttribute("part", "Declaration"));
    		result.Add(xDeclaration);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ExpressionStatementSyntax node.
        /// </summary>
        public override XElement VisitExpressionStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax node)
        {
    		var result = new XElement("ExpressionStatement");
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a EmptyStatementSyntax node.
        /// </summary>
        public override XElement VisitEmptyStatement(Microsoft.CodeAnalysis.CSharp.Syntax.EmptyStatementSyntax node)
        {
    		var result = new XElement("EmptyStatement");
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LabeledStatementSyntax node.
        /// </summary>
        public override XElement VisitLabeledStatement(Microsoft.CodeAnalysis.CSharp.Syntax.LabeledStatementSyntax node)
        {
    		var result = new XElement("LabeledStatement");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a GotoStatementSyntax node.
        /// </summary>
        public override XElement VisitGotoStatement(Microsoft.CodeAnalysis.CSharp.Syntax.GotoStatementSyntax node)
        {
    		var result = new XElement("GotoStatement");
    		//GotoKeyword
    		var xGotoKeyword = this.Visit(node.GotoKeyword);
    		xGotoKeyword.Add(new XAttribute("part", "GotoKeyword"));
    		result.Add(xGotoKeyword);
    		//CaseOrDefaultKeyword
    		if(node.CaseOrDefaultKeyword != null && node.CaseOrDefaultKeyword.Kind() != SyntaxKind.None)
    		{
    			var xCaseOrDefaultKeyword = this.Visit(node.CaseOrDefaultKeyword);
    			xCaseOrDefaultKeyword.Add(new XAttribute("part", "CaseOrDefaultKeyword"));
    			result.Add(xCaseOrDefaultKeyword);
    		}
    		//Expression
    		if(node.Expression != null)
    		{
    			var xExpression = this.Visit(node.Expression);
    			xExpression.Add(new XAttribute("part", "Expression"));
    			result.Add(xExpression);
    		}
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a BreakStatementSyntax node.
        /// </summary>
        public override XElement VisitBreakStatement(Microsoft.CodeAnalysis.CSharp.Syntax.BreakStatementSyntax node)
        {
    		var result = new XElement("BreakStatement");
    		//BreakKeyword
    		var xBreakKeyword = this.Visit(node.BreakKeyword);
    		xBreakKeyword.Add(new XAttribute("part", "BreakKeyword"));
    		result.Add(xBreakKeyword);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ContinueStatementSyntax node.
        /// </summary>
        public override XElement VisitContinueStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ContinueStatementSyntax node)
        {
    		var result = new XElement("ContinueStatement");
    		//ContinueKeyword
    		var xContinueKeyword = this.Visit(node.ContinueKeyword);
    		xContinueKeyword.Add(new XAttribute("part", "ContinueKeyword"));
    		result.Add(xContinueKeyword);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ReturnStatementSyntax node.
        /// </summary>
        public override XElement VisitReturnStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ReturnStatementSyntax node)
        {
    		var result = new XElement("ReturnStatement");
    		//ReturnKeyword
    		var xReturnKeyword = this.Visit(node.ReturnKeyword);
    		xReturnKeyword.Add(new XAttribute("part", "ReturnKeyword"));
    		result.Add(xReturnKeyword);
    		//Expression
    		if(node.Expression != null)
    		{
    			var xExpression = this.Visit(node.Expression);
    			xExpression.Add(new XAttribute("part", "Expression"));
    			result.Add(xExpression);
    		}
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ThrowStatementSyntax node.
        /// </summary>
        public override XElement VisitThrowStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ThrowStatementSyntax node)
        {
    		var result = new XElement("ThrowStatement");
    		//ThrowKeyword
    		var xThrowKeyword = this.Visit(node.ThrowKeyword);
    		xThrowKeyword.Add(new XAttribute("part", "ThrowKeyword"));
    		result.Add(xThrowKeyword);
    		//Expression
    		if(node.Expression != null)
    		{
    			var xExpression = this.Visit(node.Expression);
    			xExpression.Add(new XAttribute("part", "Expression"));
    			result.Add(xExpression);
    		}
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a YieldStatementSyntax node.
        /// </summary>
        public override XElement VisitYieldStatement(Microsoft.CodeAnalysis.CSharp.Syntax.YieldStatementSyntax node)
        {
    		var result = new XElement("YieldStatement");
    		//YieldKeyword
    		var xYieldKeyword = this.Visit(node.YieldKeyword);
    		xYieldKeyword.Add(new XAttribute("part", "YieldKeyword"));
    		result.Add(xYieldKeyword);
    		//ReturnOrBreakKeyword
    		var xReturnOrBreakKeyword = this.Visit(node.ReturnOrBreakKeyword);
    		xReturnOrBreakKeyword.Add(new XAttribute("part", "ReturnOrBreakKeyword"));
    		result.Add(xReturnOrBreakKeyword);
    		//Expression
    		if(node.Expression != null)
    		{
    			var xExpression = this.Visit(node.Expression);
    			xExpression.Add(new XAttribute("part", "Expression"));
    			result.Add(xExpression);
    		}
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a WhileStatementSyntax node.
        /// </summary>
        public override XElement VisitWhileStatement(Microsoft.CodeAnalysis.CSharp.Syntax.WhileStatementSyntax node)
        {
    		var result = new XElement("WhileStatement");
    		//WhileKeyword
    		var xWhileKeyword = this.Visit(node.WhileKeyword);
    		xWhileKeyword.Add(new XAttribute("part", "WhileKeyword"));
    		result.Add(xWhileKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DoStatementSyntax node.
        /// </summary>
        public override XElement VisitDoStatement(Microsoft.CodeAnalysis.CSharp.Syntax.DoStatementSyntax node)
        {
    		var result = new XElement("DoStatement");
    		//DoKeyword
    		var xDoKeyword = this.Visit(node.DoKeyword);
    		xDoKeyword.Add(new XAttribute("part", "DoKeyword"));
    		result.Add(xDoKeyword);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    		//WhileKeyword
    		var xWhileKeyword = this.Visit(node.WhileKeyword);
    		xWhileKeyword.Add(new XAttribute("part", "WhileKeyword"));
    		result.Add(xWhileKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//SemicolonToken
    		var xSemicolonToken = this.Visit(node.SemicolonToken);
    		xSemicolonToken.Add(new XAttribute("part", "SemicolonToken"));
    		result.Add(xSemicolonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ForStatementSyntax node.
        /// </summary>
        public override XElement VisitForStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax node)
        {
    		var result = new XElement("ForStatement");
    		//ForKeyword
    		var xForKeyword = this.Visit(node.ForKeyword);
    		xForKeyword.Add(new XAttribute("part", "ForKeyword"));
    		result.Add(xForKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Declaration
    		if(node.Declaration != null)
    		{
    			var xDeclaration = this.Visit(node.Declaration);
    			xDeclaration.Add(new XAttribute("part", "Declaration"));
    			result.Add(xDeclaration);
    		}
    		//Initializers
    		if(node.Initializers.Count > 0)
    		{
    			var xInitializers = new XElement("SeparatedList_of_Expression");
    			xInitializers.Add(new XAttribute("part", "Initializers"));
    			foreach(var x in node.Initializers)
    			{
    				var xElement = this.Visit(x);
    				xInitializers.Add(xElement);
    			}
    			result.Add(xInitializers);
    		}
    		//FirstSemicolonToken
    		var xFirstSemicolonToken = this.Visit(node.FirstSemicolonToken);
    		xFirstSemicolonToken.Add(new XAttribute("part", "FirstSemicolonToken"));
    		result.Add(xFirstSemicolonToken);
    		//Condition
    		if(node.Condition != null)
    		{
    			var xCondition = this.Visit(node.Condition);
    			xCondition.Add(new XAttribute("part", "Condition"));
    			result.Add(xCondition);
    		}
    		//SecondSemicolonToken
    		var xSecondSemicolonToken = this.Visit(node.SecondSemicolonToken);
    		xSecondSemicolonToken.Add(new XAttribute("part", "SecondSemicolonToken"));
    		result.Add(xSecondSemicolonToken);
    		//Incrementors
    		if(node.Incrementors.Count > 0)
    		{
    			var xIncrementors = new XElement("SeparatedList_of_Expression");
    			xIncrementors.Add(new XAttribute("part", "Incrementors"));
    			foreach(var x in node.Incrementors)
    			{
    				var xElement = this.Visit(x);
    				xIncrementors.Add(xElement);
    			}
    			result.Add(xIncrementors);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a UsingStatementSyntax node.
        /// </summary>
        public override XElement VisitUsingStatement(Microsoft.CodeAnalysis.CSharp.Syntax.UsingStatementSyntax node)
        {
    		var result = new XElement("UsingStatement");
    		//UsingKeyword
    		var xUsingKeyword = this.Visit(node.UsingKeyword);
    		xUsingKeyword.Add(new XAttribute("part", "UsingKeyword"));
    		result.Add(xUsingKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Declaration
    		if(node.Declaration != null)
    		{
    			var xDeclaration = this.Visit(node.Declaration);
    			xDeclaration.Add(new XAttribute("part", "Declaration"));
    			result.Add(xDeclaration);
    		}
    		//Expression
    		if(node.Expression != null)
    		{
    			var xExpression = this.Visit(node.Expression);
    			xExpression.Add(new XAttribute("part", "Expression"));
    			result.Add(xExpression);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a FixedStatementSyntax node.
        /// </summary>
        public override XElement VisitFixedStatement(Microsoft.CodeAnalysis.CSharp.Syntax.FixedStatementSyntax node)
        {
    		var result = new XElement("FixedStatement");
    		//FixedKeyword
    		var xFixedKeyword = this.Visit(node.FixedKeyword);
    		xFixedKeyword.Add(new XAttribute("part", "FixedKeyword"));
    		result.Add(xFixedKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Declaration
    		var xDeclaration = this.Visit(node.Declaration);
    		xDeclaration.Add(new XAttribute("part", "Declaration"));
    		result.Add(xDeclaration);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CheckedStatementSyntax node.
        /// </summary>
        public override XElement VisitCheckedStatement(Microsoft.CodeAnalysis.CSharp.Syntax.CheckedStatementSyntax node)
        {
    		var result = new XElement("CheckedStatement");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Block
    		var xBlock = this.Visit(node.Block);
    		xBlock.Add(new XAttribute("part", "Block"));
    		result.Add(xBlock);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a UnsafeStatementSyntax node.
        /// </summary>
        public override XElement VisitUnsafeStatement(Microsoft.CodeAnalysis.CSharp.Syntax.UnsafeStatementSyntax node)
        {
    		var result = new XElement("UnsafeStatement");
    		//UnsafeKeyword
    		var xUnsafeKeyword = this.Visit(node.UnsafeKeyword);
    		xUnsafeKeyword.Add(new XAttribute("part", "UnsafeKeyword"));
    		result.Add(xUnsafeKeyword);
    		//Block
    		var xBlock = this.Visit(node.Block);
    		xBlock.Add(new XAttribute("part", "Block"));
    		result.Add(xBlock);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a LockStatementSyntax node.
        /// </summary>
        public override XElement VisitLockStatement(Microsoft.CodeAnalysis.CSharp.Syntax.LockStatementSyntax node)
        {
    		var result = new XElement("LockStatement");
    		//LockKeyword
    		var xLockKeyword = this.Visit(node.LockKeyword);
    		xLockKeyword.Add(new XAttribute("part", "LockKeyword"));
    		result.Add(xLockKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a IfStatementSyntax node.
        /// </summary>
        public override XElement VisitIfStatement(Microsoft.CodeAnalysis.CSharp.Syntax.IfStatementSyntax node)
        {
    		var result = new XElement("IfStatement");
    		//IfKeyword
    		var xIfKeyword = this.Visit(node.IfKeyword);
    		xIfKeyword.Add(new XAttribute("part", "IfKeyword"));
    		result.Add(xIfKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Condition
    		var xCondition = this.Visit(node.Condition);
    		xCondition.Add(new XAttribute("part", "Condition"));
    		result.Add(xCondition);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    		//Else
    		if(node.Else != null)
    		{
    			var xElse = this.Visit(node.Else);
    			xElse.Add(new XAttribute("part", "Else"));
    			result.Add(xElse);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SwitchStatementSyntax node.
        /// </summary>
        public override XElement VisitSwitchStatement(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchStatementSyntax node)
        {
    		var result = new XElement("SwitchStatement");
    		//SwitchKeyword
    		var xSwitchKeyword = this.Visit(node.SwitchKeyword);
    		xSwitchKeyword.Add(new XAttribute("part", "SwitchKeyword"));
    		result.Add(xSwitchKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//OpenBraceToken
    		var xOpenBraceToken = this.Visit(node.OpenBraceToken);
    		xOpenBraceToken.Add(new XAttribute("part", "OpenBraceToken"));
    		result.Add(xOpenBraceToken);
    		//Sections
    		if(node.Sections.Count > 0)
    		{
    			var xSections = new XElement("List_of_SwitchSection");
    			xSections.Add(new XAttribute("part", "Sections"));
    			foreach(var x in node.Sections)
    			{
    				var xElement = this.Visit(x);
    				xSections.Add(xElement);
    			}
    			result.Add(xSections);
    		}
    		//CloseBraceToken
    		var xCloseBraceToken = this.Visit(node.CloseBraceToken);
    		xCloseBraceToken.Add(new XAttribute("part", "CloseBraceToken"));
    		result.Add(xCloseBraceToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a TryStatementSyntax node.
        /// </summary>
        public override XElement VisitTryStatement(Microsoft.CodeAnalysis.CSharp.Syntax.TryStatementSyntax node)
        {
    		var result = new XElement("TryStatement");
    		//TryKeyword
    		var xTryKeyword = this.Visit(node.TryKeyword);
    		xTryKeyword.Add(new XAttribute("part", "TryKeyword"));
    		result.Add(xTryKeyword);
    		//Block
    		var xBlock = this.Visit(node.Block);
    		xBlock.Add(new XAttribute("part", "Block"));
    		result.Add(xBlock);
    		//Catches
    		if(node.Catches.Count > 0)
    		{
    			var xCatches = new XElement("List_of_CatchClause");
    			xCatches.Add(new XAttribute("part", "Catches"));
    			foreach(var x in node.Catches)
    			{
    				var xElement = this.Visit(x);
    				xCatches.Add(xElement);
    			}
    			result.Add(xCatches);
    		}
    		//Finally
    		if(node.Finally != null)
    		{
    			var xFinally = this.Visit(node.Finally);
    			xFinally.Add(new XAttribute("part", "Finally"));
    			result.Add(xFinally);
    		}
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ForEachStatementSyntax node.
        /// </summary>
        public override XElement VisitForEachStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax node)
        {
    		var result = new XElement("ForEachStatement");
    		//ForEachKeyword
    		var xForEachKeyword = this.Visit(node.ForEachKeyword);
    		xForEachKeyword.Add(new XAttribute("part", "ForEachKeyword"));
    		result.Add(xForEachKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Type
    		var xType = this.Visit(node.Type);
    		xType.Add(new XAttribute("part", "Type"));
    		result.Add(xType);
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    		//InKeyword
    		var xInKeyword = this.Visit(node.InKeyword);
    		xInKeyword.Add(new XAttribute("part", "InKeyword"));
    		result.Add(xInKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ForEachVariableStatementSyntax node.
        /// </summary>
        public override XElement VisitForEachVariableStatement(Microsoft.CodeAnalysis.CSharp.Syntax.ForEachVariableStatementSyntax node)
        {
    		var result = new XElement("ForEachVariableStatement");
    		//ForEachKeyword
    		var xForEachKeyword = this.Visit(node.ForEachKeyword);
    		xForEachKeyword.Add(new XAttribute("part", "ForEachKeyword"));
    		result.Add(xForEachKeyword);
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Variable
    		var xVariable = this.Visit(node.Variable);
    		xVariable.Add(new XAttribute("part", "Variable"));
    		result.Add(xVariable);
    		//InKeyword
    		var xInKeyword = this.Visit(node.InKeyword);
    		xInKeyword.Add(new XAttribute("part", "InKeyword"));
    		result.Add(xInKeyword);
    		//Expression
    		var xExpression = this.Visit(node.Expression);
    		xExpression.Add(new XAttribute("part", "Expression"));
    		result.Add(xExpression);
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    		//Statement
    		var xStatement = this.Visit(node.Statement);
    		xStatement.Add(new XAttribute("part", "Statement"));
    		result.Add(xStatement);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a SingleVariableDesignationSyntax node.
        /// </summary>
        public override XElement VisitSingleVariableDesignation(Microsoft.CodeAnalysis.CSharp.Syntax.SingleVariableDesignationSyntax node)
        {
    		var result = new XElement("SingleVariableDesignation");
    		//Identifier
    		var xIdentifier = this.Visit(node.Identifier);
    		xIdentifier.Add(new XAttribute("part", "Identifier"));
    		result.Add(xIdentifier);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DiscardDesignationSyntax node.
        /// </summary>
        public override XElement VisitDiscardDesignation(Microsoft.CodeAnalysis.CSharp.Syntax.DiscardDesignationSyntax node)
        {
    		var result = new XElement("DiscardDesignation");
    		//UnderscoreToken
    		var xUnderscoreToken = this.Visit(node.UnderscoreToken);
    		xUnderscoreToken.Add(new XAttribute("part", "UnderscoreToken"));
    		result.Add(xUnderscoreToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a ParenthesizedVariableDesignationSyntax node.
        /// </summary>
        public override XElement VisitParenthesizedVariableDesignation(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedVariableDesignationSyntax node)
        {
    		var result = new XElement("ParenthesizedVariableDesignation");
    		//OpenParenToken
    		var xOpenParenToken = this.Visit(node.OpenParenToken);
    		xOpenParenToken.Add(new XAttribute("part", "OpenParenToken"));
    		result.Add(xOpenParenToken);
    		//Variables
    		if(node.Variables.Count > 0)
    		{
    			var xVariables = new XElement("SeparatedList_of_VariableDesignation");
    			xVariables.Add(new XAttribute("part", "Variables"));
    			foreach(var x in node.Variables)
    			{
    				var xElement = this.Visit(x);
    				xVariables.Add(xElement);
    			}
    			result.Add(xVariables);
    		}
    		//CloseParenToken
    		var xCloseParenToken = this.Visit(node.CloseParenToken);
    		xCloseParenToken.Add(new XAttribute("part", "CloseParenToken"));
    		result.Add(xCloseParenToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CasePatternSwitchLabelSyntax node.
        /// </summary>
        public override XElement VisitCasePatternSwitchLabel(Microsoft.CodeAnalysis.CSharp.Syntax.CasePatternSwitchLabelSyntax node)
        {
    		var result = new XElement("CasePatternSwitchLabel");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Pattern
    		var xPattern = this.Visit(node.Pattern);
    		xPattern.Add(new XAttribute("part", "Pattern"));
    		result.Add(xPattern);
    		//WhenClause
    		if(node.WhenClause != null)
    		{
    			var xWhenClause = this.Visit(node.WhenClause);
    			xWhenClause.Add(new XAttribute("part", "WhenClause"));
    			result.Add(xWhenClause);
    		}
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a CaseSwitchLabelSyntax node.
        /// </summary>
        public override XElement VisitCaseSwitchLabel(Microsoft.CodeAnalysis.CSharp.Syntax.CaseSwitchLabelSyntax node)
        {
    		var result = new XElement("CaseSwitchLabel");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//Value
    		var xValue = this.Visit(node.Value);
    		xValue.Add(new XAttribute("part", "Value"));
    		result.Add(xValue);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    	/// <summary>
        /// Called when the visitor visits a DefaultSwitchLabelSyntax node.
        /// </summary>
        public override XElement VisitDefaultSwitchLabel(Microsoft.CodeAnalysis.CSharp.Syntax.DefaultSwitchLabelSyntax node)
        {
    		var result = new XElement("DefaultSwitchLabel");
    		//Keyword
    		var xKeyword = this.Visit(node.Keyword);
    		xKeyword.Add(new XAttribute("part", "Keyword"));
    		result.Add(xKeyword);
    		//ColonToken
    		var xColonToken = this.Visit(node.ColonToken);
    		xColonToken.Add(new XAttribute("part", "ColonToken"));
    		result.Add(xColonToken);
    
    		this.Annotate(result, node);
    
        	var kindAttribute = result.Attribute("kind");
            if (kindAttribute?.Value == result.Name.LocalName)
                kindAttribute.Remove();
    
    		return result;
        }
    
    }
}
// Generated helper templates
// Generated items
