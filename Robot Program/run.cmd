: # This will run on both Windows batch and Linux bash
: # Start the robot's program over ssh by running this file
:<<"::CMDLITERAL"
@ECHO OFF
GOTO :CMDSCRIPT
::CMDLITERAL

# Linux Bash here
echo "Here we're running ${SHELL}"
exit $?

# Windows Batch here
:CMDSCRIPT
ECHO Here we're running %COMSPEC%