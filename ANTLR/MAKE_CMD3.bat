
rem without Xconversiontimeout it will only compile 10% of the times. 120000 probably means 120 sec.
rem Takes about 1 minute 15 s, june 2019. The 1.5 GB setting makes compilation possible, and does not cost extra time.
set CLASSPATH=%CLASSPATH%;C:\Thomas\Software\ANTLR\antlr-3.1.3.jar
"c:\Thomas\Software\Java\jre6\bin\java.exe" -Xmx500m org.antlr.Tool -Xconversiontimeout 120000 -traceParser Cmd3.g >> ljadsf
pause

