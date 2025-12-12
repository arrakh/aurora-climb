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
When you reach the top, what you find might not be what you expect
+ Who are you?
    I am your best friend and your worst enemy -> END
+ What will I find at the top?
    What you always sought but never found -> END
    
=== npc1 ===
The mountain holds familiar secrets. What will you do with them?
+ What secrets?
    Memories. Fragments of your past, present and future -> END
+ Where are they?
    In corners light and dark, seek and you shall find -> END
Let them taste the fire, and the flames lead you up -> END

=== npc2 ===
There is still a long way to go. Are you sure you are ready?
+ I don't deserve this
    You keep making the same mistake every time -> END
+ I can't believe this
    The sooner you accept, the easier it will be -> END

=== npc3 ===
You don't look well, what happened?
+ Nothing
    -> nothing
+ I am haunted, terrified of the cruelty of this world
    You need space, you need to reflect -> END
-> END

= nothing
Are you sure? You don't seem okay
+ I am alright, it's just the weather
    If you say so -> END
+ Things happened, but it's alright, maybe, I guess...
    I hope you feel better -> END

=== npc4 ===
You need to be strong, you can't let emotions control you
+ It's hard, it's very, very hard
    -> hard
+ I know, I am trying to
    I don't see it, you are under an illusion, and that is hurting you -> END

= hard
Life is hard, you have to prepare yourself
+ I am trying my best! You don't understand how painful it is!
    I understand, but you are hurting yourself -> END
+ Fuck this preparing, it is unfair, it is WRONG!
    It's wrong only to you, you are allowing yourself to feel this pain -> END

=== npc5 ===
You will keep coming back to me, but your questions won’t have answers
+ Your actions and words don’t match
    Who said they have to? You can’t assume you know me -> END
+ All these conversations and yet nobody understands
    No one ever will, but try your best. I hope you succeed. -> END

=== npc6 ===
You are the biggest loser. Everyone is happy without you.
+ But I still have myself
    You have lost everything else. You are alone.
+ But my work matters
    For who? Your work won’t pay you back.
You are nobody without love. No one will save you. -> END
    
=== npc7 ===
It's been some time.
+ Grief is a cycle, it comes and goes
    -> grief
+ A bit, the sadness is there, but the suffering not so much
    -> life

= grief
How are you doing right now?
+ The triggers disturb me, otherwise I am okay
    Yes, try to forgive and forget, and carry the lessons forward -> END
+ I am slowly feeling the urge to create again
    That's good! You are on the path to acceptance, I am proud of you. -> END
    
= life
That's how it goes, doesn't it? That's life
+ The loops in my head have found their answers, and it points to the same thing
    Yes, you didn't deserve this, and you are better off now -> END
+ Let people choose what they think they deserve, I know I have to make a wiser choice
    Yes, you are capable of it, just think and choose carefully before giving in -> END