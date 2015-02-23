:run.bat
@echo off
cd ./bin/debug
@echo Demonstrating Project #2
CompositeTextAnalyzer.exe *.txt .cs .c .cpp  ./TestFiles /A "/Tthread demo" "/Tjaffa demo" "/Mtag1,tag2,tag3" /R /N2
CompositeTextAnalyzer.exe *.txt .cs ../../TestFiles /O /TThread /Tdaffa /Mfilename /R /N3
CompositeTextAnalyzer.exe *.txt .cs .cpp ../../TestFiles /Mdependencies /Mkeywords /R /N4
CompositeTextAnalyzer.exe *.txt ../../TestFiles "/Tthread demo" /Mname /R /N5
CompositeTextAnalyzer.exe *.txt ../../TestFiles "/Tthread demo" /Mname /R /N6
cd ../../../CompositeTextAnalyzer