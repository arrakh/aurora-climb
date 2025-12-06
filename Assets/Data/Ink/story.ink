INCLUDE variables.ink

=== testpath ===
Hi, I'm a box!

= choice
+ a box?
    Yes, a box. -> END
+ cool
    Right??!!!! -> END
* im sorry to hear that
    Sorry? what do you mean sorry??? 
    I'll have you know I graduated on top of my class in the Cuboid as the Top Cube. I've got 6 confirmed sides and 8 different points, way more than any other 3D shapes in the whole state. On top of that, the 12 lines surrounding my body is of high level and fidelity that it beats any other Cubers out there. You have NO idea who you are talking to you polygonal humanoid schmuck. You're in dead waters, kiddo. -> choice
    
    
=== one ===
How do you feel?
+ I don't deserve this
    You keep making the same mistake every time -> END
+ I can't believe this
    You have to, this is what happened -> END
-> END

=== two ===
You don't seem well, what happened?
+ Nothing
    -> nothing
+ I didn't expect things to turn out this way
    Expectations always hurt, but such is reality -> END
-> END

= nothing
Are you sure? You don't seem okay
+ I am alright, it's just the weather
    If you say so -> END
+ Things happened, but it's alright, maybe, I guess...
    I hope you feel better -> END
        
=== three ===
You need to be strong, you can't let emotions control you
+ It's hard, it's very very hard
    -> hard
+ I know, I am trying to
    I don't see it, you are under a illusion, and that is hurting you -> END

= hard
Life is hard, you have to prepare yourself
+ I am trying my best! You don't understand how painful it is!
    I understand, but you are hurting yourself -> END
+ Fuck this preparing, it's cruel, it is unfair, it is WRONG!
    It's wrong only to you, you are allowing yourself to feel this pain -> END

=== four ===
How are you doing now? Are things better?
+ I keep having panic attacks all night
    -> panic
+ I don't feel happiness anymore
    -> unhappiness
    
= panic
 You do realise things were never what you thought them to be, right?
+ I do, but it doesn't help me
    Then realise that this is not what you deserve, and move on -> END
+ Yes, but then why were things like that? Were all those words and tears fake?
    Maybe it was, maybe not, but the answer won't help you -> END

= unhappiness
Try to meet new people, participate in activities, and focus on yourself
+ I have lost motivation
    You had a life before this happened, and a life after, right? Do what has always made you feel happy -> END
+ I think these are distractions, nothing else
    Think of them as medicines that help you recover faster, that make you feel better about yourself -> END
    
=== five ===
It's been some time, has the grief subsided?
+ Grief is a cycle, it comes and goes
    -> grief
+ A bit, the sadness is there, but the suffering not so much
    -> life

= grief
How are you doing these days?
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

-> DONE