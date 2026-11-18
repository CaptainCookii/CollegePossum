// =======================================================
// Text Animator for Unity - Copyright (c) 2018-Today, Febucci SRL, febucci.com
// - LICENSE: https://www.textanimatorforgames.com/legal/eula
// - DOCUMENTATION: https://docs.febucci.com/text-animator-unity/
// - WEBSITE: https://www.textanimatorforgames.com/
// =======================================================

#if NANINOVEL_PRESENT
using Febucci.TextAnimatorForUnity.TextMeshPro;
using Naninovel;
using UnityEngine;

namespace Febucci.TextAnimatorForUnity.Naninovel
{
    static class NaninovelExpressionParserRegistration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Register()
        {
            NaninovelExpressionTagParser.RegisterExpressionEvaluator(expression =>
                global::Naninovel.ExpressionEvaluator.Evaluate<string>(
                    expression,
                    new() { OnError = error => Engine.Err(error) }));
        }
    }
}
#endif
