﻿using Content.Shared.Chat.TypingIndicator;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.Chat.TypingIndicator;

public sealed class TypingIndicatorVisualizerSystem : VisualizerSystem<TypingIndicatorComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    protected override void OnAppearanceChange(EntityUid uid, TypingIndicatorComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;
        
        if (!_prototypeManager.TryIndex<TypingIndicatorPrototype>(component.Prototype, out var proto))
        {
            Logger.Error($"Unknown typing indicator id: {component.Prototype}");
            return;
        }

        // args.Component.TryGetData(TypingIndicatorVisuals.IsTyping, out bool isTyping); // Corvax-TypingIndicator
        var layerExists = args.Sprite.LayerMapTryGet(TypingIndicatorLayers.Base, out var layer);
        if (!layerExists)
            layer = args.Sprite.LayerMapReserveBlank(TypingIndicatorLayers.Base);
        
        args.Sprite.LayerSetRSI(layer, proto.SpritePath);
        args.Sprite.LayerSetState(layer, proto.TypingState);
        args.Sprite.LayerSetShader(layer, proto.Shader);
        args.Sprite.LayerSetOffset(layer, proto.Offset);
        // args.Sprite.LayerSetVisible(layer, isTyping); // Corvax-TypingIndicator
        // Corvax-TypingIndicator-Start
        args.Component.TryGetData(TypingIndicatorVisuals.State, out TypingIndicatorState state);
        args.Sprite.LayerSetVisible(layer, state != TypingIndicatorState.None);
        switch (state)
        {
            case TypingIndicatorState.Idle:
                args.Sprite.LayerSetState(layer, proto.IdleState);
                break;
            case TypingIndicatorState.Typing:
                args.Sprite.LayerSetState(layer, proto.TypingState);
                break;
        }
        // Corvax-TypingIndicator-End
    }
}
