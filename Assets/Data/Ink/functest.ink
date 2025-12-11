// Single entry point for the climbing/lasagna test.
=== aurora_lasagna_test ===

The cliff is a vertical sheet of frozen midnight.
Above, the aurora stretches like a debugging overlay across the polar sky.

AURORA: "Back again, climber?"

{
- HasGlobalFlag("got_lasagna"):
    AURORA: "Ah. The one whose soul smells faintly of lasagna..."
- HasItem("lasagna"):
    AURORA: "And you came packing with a lasagna already, interesting"
    ~ AddGlobalFlag("got_lasagna")
- else:
    AURORA: "Fresh face. No lasagna-shaped scars yet."
}

YOU: "…How many things am I even carrying right now?"

You glance at an invisible UI panel only you can see.

YOU: "Apparently I'm holding {GetInventoryCount()} item(s),
and the universe bothers to track {GetTotalItemCount()} item(s) in total."

{HasItem("lasagna"):
    The comforting weight of lasagna presses against your pack.
- else:
    Your pack contains zero lasagnas. This feels like a design oversight.
}

AURORA: "So. What's it going to be?"

+ {not HasItem("lasagna")} [\"Uh… can you spawn a lasagna for me?\"] 
    -> conjure_lasagna
+ {HasItem("lasagna")} [\"Let me check how much lasagna I'm hoarding.\"] 
    -> inspect_lasagna
+ [\"Forget lasagna. I’ll just climb.\"] 
    -> climb_plain


= conjure_lasagna
AURORA: "Naturally. What *kind* of lasagna manifestation do you desire?"

+ [A real, heavy, inventory-ruining lasagna.]
    ~ AddItemWithAmount("lasagna", 1)
    ~ AddInventoryCount(1)
    ~ AddGlobalFlag("got_lasagna")
    The aurora folds into a perfectly rectangular slab of lasagna and drops it into your pack.
    YOU: "That seems… unsafely hot."
    YOU: "Okay, so now I have {GetItemCount("lasagna")} lasagna unit(s)."
    -> lasagna_menu

+ [A purely symbolic lasagna that somehow still counts as an item.]
    ~ AddItem("lasagna")
    ~ AddGlobalFlag("got_lasagna")
    AURORA: "Behold: Metaphysical Pasta."
    A faint icon of lasagna quietly appears in your inventory.
    YOU: "I'm scared to ask how many there are."
    YOU: "Apparently… {GetItemCount("lasagna")}."
    -> lasagna_menu


= inspect_lasagna
YOU: "Hang on, let me check my lasagna situation."

YOU: "The log says I'm holding {GetItemCount("lasagna")} lasagna object(s). 
That's either one meal or twenty-four bad decisions."

AURORA: "The first step is admitting you are lasagna-positive."

-> lasagna_menu


= lasagna_menu
AURORA: "So what will you do with this cheesy attachment?"

+ {HasItem("lasagna")} [Eat one lasagna right now.]
    -> eat_lasagna
+ {HasItem("lasagna")} [Throw the lasagna dramatically off the cliff.]
    -> drop_lasagna
+ [Keep climbing while clinging to your lasagna.]
    -> climb_with_lasagna


= eat_lasagna
YOU: "If this breaks causality, at least I'll die full."

~ RemoveItemWithAmount("lasagna", 1)
~ AddInventoryCount(-1)

You devour the lasagna with reckless, texture-agnostic enthusiasm.

YOU: "…I regret nothing."

YOU: "Inventory now reports {GetItemCount("lasagna")} lasagna remaining."
AURORA: "Attachment converted to calories. Very efficient."

-> final_ledge


= drop_lasagna
You hold the lasagna over the abyss like a cheesy offering to gravity.

YOU: "This is for character growth… and maybe performance."

~ RemoveItem("lasagna")
~ RemoveGlobalFlag("got_lasagna")
~ AddInventoryCount(-1)

The lasagna spins away, trailing shredded metaphor into the dark.

YOU: "Okay, now I'm at {GetInventoryCount()} item(s) on me."
YOU: "Universe still tracks {GetTotalItemCount()} item(s) overall, though. Show-off."

AURORA: "Sometimes letting go is just freeing a slot in your inventory."

-> final_ledge


= climb_with_lasagna
You readjust your pack so the lasagna sits comfortably against your back.

{HasItem("lasagna"):
    The dish radiates a faint, suspicious warmth.
- else:
    The lasagna seems to exist only as a flag in the sky labeled "got_lasagna".
}

AURORA: "Climbing while refusing to let go. Classic."

-> final_ledge


= climb_plain
You dig your fingers into the frozen rock and start climbing in stubborn silence.

{HasItem("lasagna"):
    Despite your denial, lasagna sloshes gently in your pack with every move.
- else:
    Your stomach growls, providing an ambient horror soundtrack.
}

AURORA: "Fine. We'll pretend this isn't secretly a lasagna tutorial."

-> final_ledge


= final_ledge
After a long, chilly ascent, you reach a narrow ledge halfway up the cliff.

The aurora hangs above you like a progress bar that forgot what it was loading.

{HasItem("lasagna"):
    The lasagna is still with you, heavier than any piece of gear.
- else:
    Only the memory of lasagna weighs on you now.
}

{HasGlobalFlag("got_lasagna"):
    AURORA: "You met lasagna, made choices, and the story remembered."
- else:
    AURORA: "You climbed without ever truly embracing lasagna. That's also a valid build."
}

YOU: "So what's the verdict? Am I ready to let go yet?"

AURORA: "Maybe. But first, a tiny safety net for the next run."

{HasItem("lasagna") == false:
    // Give them a tiny backup lasagna for testing AddItem again if needed.
    ~ AddItem("lasagna")
    YOU: "…Did you just slip a lasagna into my pocket?"
    AURORA: "Purely for QA purposes."
}

YOU: "Right. I'll pretend that makes sense."

The aurora reaches down and taps an invisible *Save & Quit* button in the sky.

-> END
