using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Cards.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Condemned.CondemnedCode.Utils
{
    public static class SigilCardFactory
    {
        private static readonly Dictionary<string, Type> _sigilTypes;
        private static readonly MethodInfo _createCardMethod;

        static SigilCardFactory()
        {
            // 1. Buscar todas las clases SigilOf* en el namespace Token
            _sigilTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CondemnedCard)))
                .Where(t => t.Namespace == "Condemned.CondemnedCode.Cards.Token")
                .Where(t => t.Name.StartsWith("SigilOf"))
                .ToDictionary(
                    t => t.Name.Substring(7), // "SigilOfExhaust" → "Exhaust"
                    t => t
                );

            // 2. Buscar el método estático o de instancia CreateCard<T>(Player) de forma precisa
            _createCardMethod = typeof(CombatState)
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .FirstOrDefault(m =>
                    m.Name == "CreateCard" &&
                    m.IsGenericMethod &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Player));

            if (_createCardMethod == null)
                throw new InvalidOperationException(
                    "No se encontró el método CreateCard<T>(Player) en CombatState.");
        }

        /// <summary>
        /// Crea una carta de tipo SigilOf[Keyword] usando la instancia de CombatState.
        /// </summary>
        /// <param name="keyword">Ej. "Exhaust"</param>
        /// <param name="owner">Dueño de la carta</param>
        /// <param name="combatState">Instancia de CombatState (requerida si el método no es estático)</param>
        public static CondemnedCard CreateSigilCard(string keyword, Player owner, ICombatState combatState)
        {
            if (string.IsNullOrEmpty(keyword))
                throw new ArgumentException("Keyword no puede ser nulo o vacío.", nameof(keyword));

            if (!_sigilTypes.TryGetValue(keyword, out var sigilType))
                throw new ArgumentException($"No se encontró una clase SigilOf{keyword} en el namespace Token");

            // Hacer el método genérico con el tipo específico
            var genericMethod = _createCardMethod.MakeGenericMethod(sigilType);

            // Determinar target: null si es estático, la instancia si es de instancia
            object target = _createCardMethod.IsStatic ? null : combatState;
            if (!_createCardMethod.IsStatic && combatState == null)
                throw new ArgumentNullException(nameof(combatState),
                    "El método CreateCard es de instancia y se requiere una instancia de CombatState.");

            // Invocar y devolver la carta creada
            var card = genericMethod.Invoke(target, new object[] { owner });

            return card as CondemnedCard
                ?? throw new InvalidOperationException($"CreateCard devolvió null o tipo incorrecto para {sigilType.Name}");
        }
    }
}