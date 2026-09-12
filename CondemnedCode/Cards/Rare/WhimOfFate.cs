using System.Reflection;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.StaticHoverTips;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Condemned.CondemnedCode.Cards.Rare;

public class WhimOfFate : CondemnedCard
{
    private class Effect
    {
        public Func<CardModel, bool> CanApply { get; }
        public Action<CardModel> Apply { get; }

        public Effect(Func<CardModel, bool> canApply, Action<CardModel> apply)
        {
            CanApply = canApply;
            Apply = apply;
        }
    }

    private static readonly List<Effect> _effects = new();

    static WhimOfFate()
    {
        var excludedKeywords = new HashSet<CardKeyword>
        {
            CardKeyword.Eternal,
            CardKeyword.Innate,
            CardKeyword.None,
            CardKeyword.Sly
        };

        foreach (CardKeyword keyword in Enum.GetValues(typeof(CardKeyword)))
        {
            if (excludedKeywords.Contains(keyword))
                continue;

            _effects.Add(new Effect(
                canApply: card => !card.Keywords.Contains(keyword),
                apply: card => card.AddKeyword(keyword)
            ));
        }

        foreach (FieldInfo field in typeof(CondemnedKeywords).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.FieldType == typeof(CardKeyword))
            {
                var keyword = (CardKeyword)field.GetValue(null);
                _effects.Add(new Effect(
                    canApply: card => !card.Keywords.Contains(keyword),
                    apply: card => card.AddKeyword(keyword)
                ));
            }
        }

        _effects.Add(new Effect(
            canApply: _ => true,
            apply: card => card.BaseReplayCount++
        ));
    }

    public WhimOfFate() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        WithTip(CondemnedStaticHoverTips.Keywords);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        List<CardModel> cards;

        if (IsUpgraded)
        {
            cards = Owner.PlayerCombatState.Hand.Cards.ToList();
        }
        else
        {
            var selected = await CardSelectCmd.FromCombatPile(
                choiceContext,
                PileType.Hand.GetPile(Owner),
                Owner,
                new CardSelectorPrefs(SelectionScreenPrompt, 1)
            );

            if (selected == null || selected.Count() == 0)
                return;

            cards = selected.ToList();
        }

        foreach (CardModel card in cards)
        {
            var applicable = _effects.Where(e => e.CanApply(card)).ToList();

            if (applicable.Count == 0)
                continue;
            
            int index = (int)Math.Floor(Owner.RunState.Rng.Niche.NextDouble() * applicable.Count);
            var chosenEffect = applicable[index];
            
            CondemnedKeywordModel.TriggerCardKeywordsModified(this);

            chosenEffect.Apply(card);
            CardCmd.Preview(card, 1f);
            await Cmd.Wait(0.6f);
        }
    }
}