// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

// https://connelhooley.uk/blog/2017/04/30/f-sharp-to-c-sharp
module MyMath
// https://stackoverflow.com/questions/46914816/recursion-in-f-expected-type-int-but-got-type-int-list-int
let rec gcd a b=
    if b=0
        then 
            //printfn "a=%d b=%d" a b
            abs a
    else 
        //printfn "a=%d b=%d (%d:1(%d))" a b (a/b) (a%b)
        gcd b (a%b)
 
let rec lcm a b=(a*b)/(gcd a b)

// https://www.calculatorsoup.com/calculators/math/ratios.php
// 3900:2160=65:36
let ratio a b=(a/(gcd a b),b/(gcd a b))

[<EntryPoint>]
let main argv = 
    //printfn "%A" argv
    printfn "lcm 15 25=%d(%d)" (lcm 15 25) (15*25)
    printfn "gcd 15 25=%d" (gcd 15 25)
    printfn "gcd 7800/2 2160=%d" (gcd (7800/2) 2160)
    printfn "lcm 7800/2 2160=%d" (lcm (7800/2) 2160)
    printfn "ratio 3900:2160=%A" (ratio 3900 2160)
    0 // return an integer exit code

