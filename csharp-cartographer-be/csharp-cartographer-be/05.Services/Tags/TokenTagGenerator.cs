using csharp_cartographer_be._03.Models.Tags;
using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Tags
{
    public class TokenTagGenerator : ITokenTagGenerator
    {
        private static readonly List<string> _punctuationChars =
        [
            ".",
            ",",
            ";",
            ":",
            "?"
        ];

        private static readonly List<string> _delimiterChars =
        [
            "(",
            ")",
            "[",
            "]",
            "{",
            "}",
            "<",
            ">"
        ];

        private static readonly List<string> _primitiveTypes =
        [
            "StringKeyword",
            "DecimalKeyword",
            "DoubleKeyword",
            "IntKeyword",
            "CharKeyword",
            "FloatKeyword",
            "BoolKeyword"
        ];

        private static readonly List<string> _accessModifiers =
        [
            "PublicKeyword",
            "PrivateKeyword",
            "ProtectedKeyword",
            "InternalKeyword"
        ];

        private static readonly List<string> _modifiers =
        [
            "AbstractKeyword",
            "AsyncKeyword",
            "ConstKeyword",
            "OverrideKeyword",
            "ReadOnlyKeyword",
            "SealedKeyword",
            "StaticKeyword",
            "VirtualKeyword",
            "VolatileKeyword"
        ];

        private static readonly List<string> _literalKinds =
        [
            "NumericLiteralToken",
            "StringLiteralToken"
        ];

        private static readonly List<string> _genericCollections =
        [
            "List",
        ];

        public void GenerateTokenTags(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                AddTokenTags(token);
            }
        }

        private static void AddTokenTags(NavToken token)
        {
            AddIdentifierTagIfNeeded(token);
            AddKeywordTagIfNeeded(token);

            AddAccessModifierTagIfNeeded(token);
            AddModifierTagIfNeeded(token);
            AddPrimitiveTypeTagIfNeeded(token);
            AddParameterTagIfNeeded(token);
            AddParameterTypeTagIfNeeded(token);
            AddStringLiteralTagIfNeeded(token);
            AddNumericLiteralTagIfNeeded(token);
            AddGenericTypeArgumentTagIfNeeded(token);
            AddGenericCollectionTagIfNeeded(token);
            AddAttributeTagIfNeeded(token);
            AddPunctuationTagIfNeeded(token);
            AddDelimiterTagIfNeeded(token);
            AddPropertyTagIfNeeded(token);
            AddFieldTagIfNeeded(token);
            AddBaseTypeTagIfNeeded(token);
            AddConstructorTagIfNeeded(token);
        }

        private static void AddAccessModifierTagIfNeeded(NavToken token)
        {
            if (_accessModifiers.Contains(token.RoslynKind))
            {
                token.Tags.Add(new AccessModifierTag());
            }
        }

        private static void AddModifierTagIfNeeded(NavToken token)
        {
            if (_modifiers.Contains(token.RoslynKind))
            {
                token.Tags.Add(new ModifierTag());
            }
        }

        private static void AddIdentifierTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind == "IdentifierToken")
            {
                token.Tags.Add(new IdentifierTag());
            }
        }

        private static void AddKeywordTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind.Contains("Keyword"))
            {
                token.Tags.Add(new KeywordTag());
            }
        }

        private static void AddStringLiteralTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind == "StringLiteralToken")
            {
                token.Tags.Add(new StringLiteralTag());
            }
        }

        private static void AddNumericLiteralTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind == "NumericLiteralToken")
            {
                token.Tags.Add(new NumericLiteralTag());
            }
        }

        private static void AddGenericCollectionTagIfNeeded(NavToken token)
        {
            if (_genericCollections.Contains(token.Text)
                && token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "GenericName")
            {
                token.Tags.Add(new GenericCollectionTag());
            }
        }

        private static void AddAttributeTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "IdentifierName"
                && token.GrandParentNodeKind == "Attribute")
            {
                token.Tags.Add(new AttributeTag());
            }
        }

        private static void AddGenericTypeArgumentTagIfNeeded(NavToken token)
        {
            bool isGenericType = false;

            if (_primitiveTypes.Contains(token.RoslynKind)
                && token.ParentNodeKind == "PredefinedType"
                && token.GrandParentNodeKind == "TypeArgumentList"
                && token.GreatGrandParentNodeKind == "GenericName")
            {
                isGenericType = true;
            }

            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "IdentifierName"
                && token.GrandParentNodeKind == "TypeArgumentList"
                && token.GreatGrandParentNodeKind == "GenericName")
            {
                isGenericType = true;
            }

            if (isGenericType)
            {
                token.Tags.Add(new GenericTypeArgumentTag());
            }
        }

        private static void AddParameterTagIfNeeded(NavToken token)
        {
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "Parameter"
                && token.GrandParentNodeKind == "ParameterList")
            {
                token.Tags.Add(new ParameterTag());
            }
        }

        private static void AddParameterTypeTagIfNeeded(NavToken token)
        {
            bool isParamType = false;

            // non-primitive types
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "IdentifierName"
                && token.GrandParentNodeKind == "Parameter"
                && token.GreatGrandParentNodeKind == "ParameterList")
            {
                isParamType = true;
            }

            // primitive types
            if (_primitiveTypes.Contains(token.RoslynKind)
                && token.ParentNodeKind == "PredefinedType"
                && token.GrandParentNodeKind == "Parameter"
                && token.GreatGrandParentNodeKind == "ParameterList")
            {
                isParamType = true;
            }

            if (isParamType)
            {
                token.Tags.Add(new ParameterTypeTag());
            }
        }

        private static void AddPrimitiveTypeTagIfNeeded(NavToken token)
        {
            if (_primitiveTypes.Contains(token.RoslynKind)
                && token.ParentNodeKind == "PredefinedType")
            {
                token.Tags.Add(new PrimitiveTypeTag());
            }
        }

        private static void AddPunctuationTagIfNeeded(NavToken token)
        {
            // punctuation
            if (_punctuationChars.Contains(token.Text))
            {
                token.Tags.Add(new PunctuationTag());
            }
        }

        private static void AddDelimiterTagIfNeeded(NavToken token)
        {
            // delimiters
            bool isAligatorClip = false;

            if (token.Text == "<" || token.Text == ">")
            {
                isAligatorClip = true;
            }

            if (_delimiterChars.Contains(token.Text) && !isAligatorClip)
            {
                token.Tags.Add(new DelimiterTag());
            }
            else if (_delimiterChars.Contains(token.Text) && isAligatorClip)
            {
                if (token.ParentNodeKind != "LessThanExpression"
                    && token.ParentNodeKind != "GreaterThanExpression")
                {
                    token.Tags.Add(new DelimiterTag());
                }
            }
        }

        private static void AddPropertyTagIfNeeded(NavToken token)
        {
            // properties
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "PropertyDeclaration")
            {
                token.Tags.Add(new PropertyTag());
            }
        }

        private static void AddConstructorTagIfNeeded(NavToken token)
        {
            // constructors
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "ConstructorDeclaration")
            {
                token.Tags.Add(new ConstructorTag());
            }
        }

        private static void AddFieldTagIfNeeded(NavToken token)
        {
            // fields
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "VariableDeclarator"
                && token.GrandParentNodeKind == "VariableDeclaration"
                && token.GreatGrandParentNodeKind == "FieldDeclaration")
            {
                token.Tags.Add(new FieldTag());
            }
        }

        private static void AddBaseTypeTagIfNeeded(NavToken token)
        {
            // base types
            if (token.RoslynKind == "IdentifierToken"
                && token.ParentNodeKind == "IdentifierName"
                && token.GrandParentNodeKind == "SimpleBaseType"
                && token.GreatGrandParentNodeKind == "BaseList")
            {
                token.Tags.Add(new BaseTypeTag());
            }
        }

        //private static void AddReturnTypeTagIfNeeded(NavToken token)
        //{
        //    // return types
        //    if ()
        //    {
        //        token.Tags.Add(new ReturnTypeTag());
        //    }
        //}
    }
}
