#!/usr/bin/env python
import sys
from pymongo import MongoClient

client = MongoClient("mongodb+srv://maac:1234@cluster0-dtz35.mongodb.net/test?retryWrites=true&w=majority")
db = client.assingment2
db_tweets = list(db.tweets.find())

tweets = []
for tweet in db_tweets:
    try:
        if tweet['gnip']['profileLocations'][0]['displayName'] == 'Australia':
            print('Australia\t1')
    except: # No location data available, so, continue with the loop
        continue
