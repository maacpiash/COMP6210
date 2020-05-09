#!/usr/bin/env python

from operator import itemgetter
import sys

current_word = None
prev_index = None
current_freq = 0
word = None
N = 0
df = {}
l1 = []


for line in sys.stdin:
    line = line.strip()
    l1.append(line)
    index, wordfreq = line.split('\t', 1)
    word, freq = wordfreq.split(' ', 1)
    freq = int(freq)
    if prev_index == index:
        N = N + freq
    else:
       if prev_index != None:
            df[prev_index]=N
       N = 0
       prev_index = index
df[prev_index] = N

for h in l1:
    index, wordfreq = h.split('\t', 1)
    word, freq = wordfreq.split(' ', 1) 
    for k in df:
        if index == k:
           wf=word+' '+index
           nN=freq+' '+str(df[k])
           print(f'{word} {index} {freq} {df[k]}')
