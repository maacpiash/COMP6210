import nltk
from pymongo import MongoClient
from string import punctuation
from nltk.tokenize import word_tokenize
from nltk.stem.wordnet import WordNetLemmatizer
from nltk.corpus import stopwords
import re

# nltk.download('punkt')
# nltk.download('wordnet')
# nltk.download ('stopwords')

client = MongoClient("mongodb+srv://maac:1234@cluster0-dtz35.mongodb.net/test?retryWrites=true&w=majority")
db = client.assingment2
db_tweets = list(db.tweets.find())

# step 1: convert whole text to lowercase
tweets = []
for tweet in db_tweets:
  tweets.append(tweet['body'].lower())

tokenized_tweets = []

for tweet in tweets:
    # step 2: tokenize all the words
    tokens = word_tokenize(tweet)

    # step 3: remove all numeric tokens
    re_num = re.compile(r'[0-9]')
    tokens = [i for i in tokens if not re_num.match(i)]
    
    # step 4: remove all punctuation
    re_pun = re.compile('[%s]' % re.escape(punctuation))
    tokens = [i for i in tokens if not re_pun.match(i)]

    tokenized_tweets.append(tokens)

# step 5: lemmatize all tokens
wnl = WordNetLemmatizer()
lemmatized_tweets = []

for tweet in tokenized_tweets:
    lemma = [wnl.lemmatize(token) for token in tweet]
    lemmatized_tweets.append(lemma)

# step 6: remove all stopwords
stop_words = set(stopwords.words("english"))
filtered_tweets = []

for tweet in lemmatized_tweets:
    filtered = [token for token in tweet if token not in stop_words]
    filtered_tweets.append(filtered)

for tweet in filtered_tweets:
    for word in tweet:
        print(word, end = '\t')
    print()
