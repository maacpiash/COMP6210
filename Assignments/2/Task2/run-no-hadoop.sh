#!/usr/bin/sh
./task2_mapper.py | sort -k1,1 | ./task2_reducer.py > output.txt
