INCLUDE variables.ink
INCLUDE functions.ink
INCLUDE functest.ink

=== testpath ===
Hi, I'm a box!
-> choice

= choice
+ a box?
    Yes, a box. -> END
+ cool
    Right??!!!! -> END
* im sorry to hear that
    Sorry? what do you mean sorry??? 
    I'll have you know I graduated on top of my class in the Cuboid as the Top Cube. I've got 6 confirmed sides and 8 different points, way more than any other 3D shapes in the whole state. On top of that, the 12 lines surrounding my body is of high level and fidelity that it beats any other Cubers out there. You have NO idea who you are talking to you polygonal humanoid schmuck. You're in dead waters, kiddo. -> choice
    
    
=== npc0 ===
Good luck on the journey ahead, I hope the Aurora grants your wish.
+ What awaits me at the top?
    What you always sought but never found -> DONE
+ How do I get to the top?
    Seek the other Echoes, they will guide you -> DONE
    
=== npc1 ===
The mountain holds familiar secrets, they will slow you down 
+ What secrets?
    Memories. Fragments of your past, a burden on your soul -> npc1end
+ Where are they?
    In corners light and dark, seek and you shall find these objects -> npc1end

=== npc1end ===
{HasGlobalFlag("hasBurned-camera"):
    + I got the camera
        Pictures capture time in a bottle, it's an illusion that doesn't exist -> END
- else:
    Bring them to the fire, and the flames will give you strength -> END
}

=== npc2 ===
There is still a long way to go. Are you sure you are ready?
+ I don't deserve this
    You keep making the same mistake every time -> npc2end
+ I can't believe this
    The sooner you accept, the easier it will be -> npc2end
    
=== npc2end ===
{HasGlobalFlag("hasBurned-petfood"):
    + I miss the pets
        Animals understand our feelings, I know she sensed your tears that day -> END
- else:
    I forgot to feed my pets, maybe you can find their food. -> END
}

=== npc3 ===
You don't look well, what happened?
+ Nothing
    -> nothing
+ I am haunted, terrified of the cruelty of this world
    You need space, you need to reflect -> npc3end

= nothing
Are you sure? You don't seem okay
+ I am alright, it's just the weather
    If you say so -> npc3end
+ I feel betrayed, the closest person hurt me
    Actions can be wrong, but the intentions were not -> npc3end

=== npc3end ===
{HasGlobalFlag("hasBurned-medkit"):
    + I was sick once, they brought me medicines
        We all wish to be cared for, but we can't expect love -> END
- else:
    I lost the cold medicines somewhere, it's difficult without it-> END
}

=== npc4 ===
You need to be strong, you can't let emotions control you
+ It's hard, it's very, very hard
    -> hard
+ I know, I am trying to
    I don't see it, you are under an illusion, and that is hurting you -> npc4end

= hard
Life is hard, you have to prepare yourself
+ I am trying my best! You don't understand how painful it is!
    I understand, but you are hurting yourself -> npc4end
+ I hate this preparing, it is unfair, it is WRONG!
    It's wrong only to you, you are allowing yourself to feel this pain -> npc4end

=== npc4end ===
{HasGlobalFlag("hasBurned-books"):
    + We shared a love for stories
        Some tales end without resolution, you must find your own closure -> END
- else:
    There was a library in this mountain once, maybe some books still remain -> END
}

=== npc5 ===
You will keep coming back to me, but your questions won’t have answers
+ Your actions and words don’t match
    Who said they have to? You can’t assume you know me -> npc5end
+ All these conversations and yet nobody understands
    No one ever will, but try your best. I hope you succeed. -> npc5end

=== npc5end ===
{HasGlobalFlag("hasBurned-guitar"):
    + All my songs were dedicated to them
        Their ears find melody in somebody else's voice, you don't matter. -> END
- else:
    Someone used to play a guitar here, it made the climb less lonely -> END
}

=== npc6 ===
You are the biggest loser. Everyone is happy without you.
+ But I still have myself
    You have lost everything else. You are alone. -> npc6end
+ But my work matters
    For who? Your work won’t pay you back. -> npc6end

=== npc6end ===
{HasGlobalFlag("hasBurned-lasagna"):
    + They cooked, I used to wash the dishes
        The dishes are well taken care of, their partner makes sure they are clean -> END
- else:
    I left some food for hungry climbers, feel free to grab a bite if you find it -> END
}
    
=== npc7 ===
It's been some time.
+ Grief is a cycle, it comes and goes
    -> grief
+ A bit, the sadness is there, but the suffering not so much
    -> life

= grief
How are you doing right now?
+ The triggers disturb me, otherwise I am okay
    Yes, try to forgive and forget, and carry the lessons forward -> npc7end
+ I am slowly feeling the urge to create again
    That's good! You are on the path to acceptance, I am proud of you. -> npc7end
    
= life
That's how it goes, doesn't it? That's life
+ The loops in my head have found their answers, and it points to the same thing
    Yes, you didn't deserve this, and you are better off now -> npc7end
+ Let people choose what they think they deserve, I know I have to make a wiser choice
    Yes, you are capable of it, just think and choose carefully before giving in -> npc7end

=== npc7end ===
{HasGlobalFlag("hasBurned-dice"):
    + Games, my purpose, the last thing we shared
        Perhaps, but your love for games is your own, nobody can take it from you -> END
- else:
    At the end, remember why you came here -> END
}
