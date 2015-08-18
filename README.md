#Audio data hiding
##What's audio data hiding?
Audio data hiding has been widely applied in many areas such as audio watermarking for copyright protection, steganography, covert communication, and broadcast monitoring.   
In some cases, we'd like to use it to show some extra visable information acoustic data transmisson( ADT ) system.   
> In theater, using mobile phones to listen to the audio of films and showing the hiding messages.   

For example, we can get decode package from the audio of a movie,the format perhaps is similar to XML file.   

    [name = April Story]     
    [company = Rockwell Eyes]   
    [language = jp]   
    [director = Shunji Iwai]   
    [/note]
    
    
    
##Major methods
* MCLT
	- Using Microsoft Audio Watermarking Tool to do MCLT( http://research.microsoft.com/en-us/downloads/885bb5c4-ae6d-418b-97f9-adc9da8d48bd/default.aspx )   
	- Another matlab version
* Echo hiding
* SS
* LSB
	- The demo used this method.
	
    
    
##LSB(Least Significant Bit)
* Reference key file
* Processing procedure includes **encode** and **decode**
* Perfect audio quality


##WAV file format
In this blog, it's absolutely clear for understanding.
http://blog.csdn.net/bluesoal/article/details/932395

And most thanks to Corinna John and his open source in codeproject.(not only his article but also his algorithm codes)
http://www.codeproject.com/Articles/6960/Steganography-VIII-Hiding-Data-in-Wave-Audio-Files?fid=43032&df=90&mpp=25&noise=3&prof=False&sort=Position&view=Normal&spc=Relaxed&fr=1#_articleTop

##How to use it
Because this demo is debugging on local and I'm so lazy that uses awful file path to test, so in this version maybe you'd like to change it.
But the noise attack and MCLT method reminds unavailable at present.