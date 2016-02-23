#Control creation of database tables and handles the db cursor for the app

import MySQLdb
import warnings

warnings.filterwarnings("ignore")

USER = #specify the user
PASS = #specify the password

db = MySQLdb.connect("localhost",USER,PASS,"hawa")

cursor = db.cursor()

StationCreator="""CREATE TABLE IF NOT EXISTS STATIONS(
	NAME CHAR(30) NOT NULL,
	URL CHAR(200) NOT NULL UNIQUE,
	LATITUDE DECIMAL(9,6),
	LONGITUDE DECIMAL(9,6),
	NO2 DECIMAL(9,2),
	SO2 DECIMAL(9,2),
	CO DECIMAL(9,2),
	O3 DECIMAL(9,2),
	PM25 DECIMAL(9,2),
	PM10 DECIMAL(9,2))"""

try:
	cursor.execute(StationCreator)
except:
	pass

print "logged in"

def terminate():
	global db
	db.close()
