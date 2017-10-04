: # This will run on both Windows batch and Linux bash
: # Start the robot's program over ssh by running this file
:<<"::CMDLITERAL"
@ECHO OFF
GOTO :CMDSCRIPT
::CMDLITERAL

# Linux Bash here
echo "Start the robot program with this line"
exit $?

# Windows Batch here
:CMDSCRIPT
ECHO Here we're running %COMSPEC%