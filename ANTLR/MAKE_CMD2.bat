
rem without Xconversiontimeout it will only compile 10% of the times. 30000 probably means 30 sec.
set CLASSPATH=%CLASSPATH%;C:\Program Files (x86)\ANTLR\antlr-3.1.3.jar
java org.antlr.Tool -Xconversiontimeout 30000 -traceParser Cmd2.g >> ljadsf
pause

