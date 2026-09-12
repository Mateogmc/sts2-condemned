using BaseLib.Extensions;
using BaseLib.Utils;
using Condemned.CondemnedCode.Cards;
using Condemned.CondemnedCode.Character;
using Condemned.CondemnedCode.Core;
using Condemned.CondemnedCode.DynamicVars;
using Condemned.CondemnedCode.Keywords;
using Condemned.CondemnedCode.Powers;
using Condemned.CondemnedCode.Vfx;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Condemned.CondemnedCode.Cards.Rare;

public class Dispossess : CondemnedCard
{

    public Dispossess() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(4, 1);
        WithVar(new DispossessVar(4).WithUpgrade(1));
        WithPower<DispossessPower>(1);
        WithKeyword(CondemnedKeywords.Brittle);
        WithTip(StaticHoverTip.Fatal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);

        var attackCommand = await DamageCmd.Attack(DynamicVars["Dispossess"].PreviewValue)
            .FromCard(this, play)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        if (attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
        {
            if (CombatState.RunState.CurrentRoom is CombatRoom combatRoom)
            {
                CurseCardPool pool = ModelDb.CardPool<CurseCardPool>();

                CardCreationOptions options = new CardCreationOptions(
                    new[] { pool },
                    CardCreationSource.Other,
                    CardRarityOddsType.Uniform);

                CurseCardReward reward = new CurseCardReward(
                    options,
                    3,
                    Owner);

                combatRoom.AddExtraReward(Owner, reward);
                
                await CommonActions.Apply<DispossessPower>(choiceContext, Owner.Creature, this, DynamicVars.Power<DispossessPower>().BaseValue);
            }
        }
    }
}