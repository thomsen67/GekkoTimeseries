
rem without Xconversiontimeout it will only compile 10% of the times. 60000 probably means 60 sec.
set CLASSPATH=%CLASSPATH%;C:\Program Files (x86)\ANTLR\antlr-3.1.3.jar
"c:\Program Files (x86)\Java\jre6\bin\java.exe" org.antlr.Tool -Xconversiontimeout 60000 -traceParser Cmd3.g >> ljadsf

pause

