﻿using Microsoft.CodeAnalysis;

namespace csharp_cartographer_be._03.Models.Tokens
{
    public class SemanticData
    {
        public ISymbol? Symbol { get; set; }

        public string? SymbolName { get; set; }

        public string? SymbolKind { get; set; }

        public string? ContainingType { get; set; }

        public string? ContainingNamespace { get; set; }

        public string? TypeName { get; set; }

        public string? TypeKind { get; set; }

        public bool IsNullable { get; set; }
    }
}
