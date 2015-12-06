﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.NamingRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// The name of a C# generic parameter does not begin with the capital letter T.
    /// </summary>
    /// <remarks>
    /// A violation of this rule occurs when the name of a generic parameter does not begin with the capital letter T.
    /// Generic parameter names should always begin with T. For example, <c>T</c> or <c>TKey</c>.
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1654GenericParameterNamesMustBeginWithT : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1654GenericParameterNamesMustBeginWithT"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1654";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1654Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1654MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1654Description), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1654.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly Action<CompilationStartAnalysisContext> CompilationStartAction = HandleCompilationStart;
        private static readonly Action<SyntaxNodeAnalysisContext> TypeParameterAction = HandleTypeParameter;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(CompilationStartAction);
        }

        private static void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            context.RegisterSyntaxNodeActionHonorExclusions(TypeParameterAction, SyntaxKind.TypeParameter);
        }

        private static void HandleTypeParameter(SyntaxNodeAnalysisContext context)
        {
            var typeParameter = (TypeParameterSyntax)context.Node;
            if (typeParameter.Identifier.IsMissing)
            {
                return;
            }

            string name = typeParameter.Identifier.ValueText;
            if (name != null && !name.StartsWith("T", StringComparison.Ordinal))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, typeParameter.Identifier.GetLocation()));
            }
        }
    }
}
