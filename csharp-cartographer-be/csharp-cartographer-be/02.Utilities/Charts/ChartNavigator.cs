﻿using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._02.Utilities.Charts
{
    public static class ChartNavigator
    {
        #region Identifiers
        public static bool IsUsingDirective(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "UsingDirective";
        }

        public static bool IsInterfaceDeclaration(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "InterfaceDeclaration";
        }

        public static bool IsClassDeclaration(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "ClassDeclaration";
        }

        public static bool IsConstructorDeclaration(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "ConstructorDeclaration";
        }

        public static bool IsMethodDeclaration(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "MethodDeclaration";
        }

        public static bool IsVariableDeclaration(NavToken token)
        {
            return token.Charts.Count > 3 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "VariableDeclarator" &&
                token.Charts[2].Label == "VariableDeclaration" &&
                token.Charts[3].Label == "LocalDeclarationStatement";
        }

        public static bool IsParameter(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "Parameter";
        }

        public static bool IsParameterType(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "Parameter";
        }

        public static bool IsField(NavToken token)
        {
            return token.Charts.Count > 3 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "VariableDeclarator" &&
                token.Charts[2].Label == "VariableDeclaration" &&
                token.Charts[3].Label == "FieldDeclaration";
        }

        public static bool IsProperty(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "PropertyDeclaration";
        }

        public static bool IsNamespaceDeclaration(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "NamespaceDeclaration";
        }

        public static bool IsForEachVariable(NavToken token)
        {
            return token.Charts.Count > 1 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "ForEachStatement";
        }

        public static bool IsTypeArgument(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "TypeArgumentList";
        }

        public static bool IsBaseType(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "SimpleBaseType";
        }

        public static bool IsInvocation(NavToken token)
        {
            return token.Charts.Count > 3 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "SimpleMemberAccessExpression" &&
                token.Charts[3].Label == "InvocationExpression";
        }

        public static bool IsAttribute(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "Attribute";
        }

        public static bool IsPropertyTypeClass(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "PropertyDeclaration";
        }

        public static bool IsNameColon(NavToken token)
        {
            return token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "NameColon";
        }

        public static bool IsExpression(NavToken token)
        {
            bool isExpression = false;

            if (token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName" &&
                token.Charts[2].Label == "InvocationExpression")
            {
                isExpression = true;
            }

            if (token.Charts.Count > 3 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "GenericName" &&
                token.Charts[2].Label == "SimpleMemberAccessExpression" &&
                token.Charts[3].Label == "InvocationExpression")
            {
                isExpression = true;
            }

            return isExpression;
        }

        public static bool IsDataType(NavToken token)
        {
            bool isIdentifier = false;
            bool isDeclaration = false;

            if (token.Charts.Count > 2 &&
                token.Charts[0].Label == "IdentifierToken" &&
                token.Charts[1].Label == "IdentifierName")
            {
                isIdentifier = true;
            }

            if (token.Charts.Count > 2 &&
                (token.Charts[2].Label == "MethodDeclaration" || token.Charts[2].Label == "VariableDeclaration" || token.Charts[2].Label == "PropertyDeclaration"))
            {
                isDeclaration = true;
            }

            if (token.Charts.Count > 2 &&
                token.Charts[2].Label == "NullableType")
            {
                isDeclaration = true;
            }

            if (isIdentifier && isDeclaration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
