#!/usr/bin/sh

# first, get all the tweets from the database, curate them, and put them into a text file
python ./task1_curation.py > input.txt

# then, put the contents of the file into stdin, and execute mapper and reducer by piping
cat ./input.txt | ./task1_mapper.py | sort -k1,1 | ./task1_reducer.py > output.txt
