// =======================================================
// Text Animator for Unity - Copyright (c) 2018-Today, Febucci SRL, febucci.com
// - LICENSE: https://www.textanimatorforgames.com/legal/eula
// - DOCUMENTATION: https://docs.febucci.com/text-animator-unity/
// - WEBSITE: https://www.textanimatorforgames.com/
// =======================================================

#if NANINOVEL_PRESENT
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Febucci.Parsing.Core;

[assembly: InternalsVisibleTo("Febucci.TextAnimatorForUnity.Naninovel.Runtime")]

namespace Febucci.TextAnimatorForUnity.TextMeshPro
{
    class NaninovelExpressionTagParser : TagParserBase
    {
        static Func<string, string> expressionEvaluator;

        public NaninovelExpressionTagParser() : base('<', '/', '>') { }

        internal static void RegisterExpressionEvaluator(Func<string, string> evaluator)
        {
            expressionEvaluator = evaluator;
        }

        public override bool TryProcessingTag(string textInsideBrackets, int tagLength, ref int realTextIndex, StringBuilder finalTextBuilder, int internalOrder)
        {
            if (textInsideBrackets[0] != ':') return false;

            if (expressionEvaluator == null)
                throw new InvalidOperationException("Naninovel expression evaluator is not registered");

            var result = expressionEvaluator(textInsideBrackets.Substring(1));
            if (string.IsNullOrEmpty(result)) return true;

            finalTextBuilder.Append(result);
            realTextIndex += result.Length;
            return true;
        }
    }
}
#endif
