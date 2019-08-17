# Venturi77CallHijacker
 KoiVM,EazVM,AgileVM Patcher - Will Keep On Updating (This is a rushed release, i will add all the features tomorrow. Expect lots of updates!) Detailed Guide TomorrowTM

Team Venturi77 Members (Current):

TobitoFatito - https://github.com/TobitoFatitoNulled/

XSilent007 - https://github.com/XSilent007/

# Latest Update

Config system was updated to support parameter manipulation.

Example: https://i.imgur.com/ganYBMV.png

Took off the ContinueAdvanced from the config but will re-add it

in a few hours. For now use search by MDToken, if you used ContinueAdvanced.

# How To Use

https://github.com/TobitoFatitoNulled/Venturi77CallHijacker/wiki/Patching-KoiVM-CrackME

# Bugs:

Agile,Eaz injection is buggy some files wont inject, will fix really soon.

If you have a program to inject that is eaz/agile and wont work you can inject

it yourself with dnlib. How? 

On eaz you can follow the calls with the parameters like this

https://i.imgur.com/YLeVeTm.png

till you find this 

https://i.imgur.com/TWq3R3V.png

Then just Control+f and search for .Invoke till you find this method:

https://i.imgur.com/2kzcHMj.png

Now just edit il instructions, make sure the venturi dll is on the same directory

and that you have added it on dnlib and just edit the call like this https://i.imgur.com/0Ur15Bz.png

Click OK https://i.imgur.com/qDfliTe.png and after that make sure to click on the .HandleInvoke so you can see the dll.

Now you just save the assembly with keeping MDToken settings.

Injection done you can just make configs now to debug/patch

