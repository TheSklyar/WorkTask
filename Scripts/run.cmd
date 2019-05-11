@ECHO OFF
set server=localhost
set database=TestDB
call patch_server.cmd %server% %database%> %pathnum%.server.log