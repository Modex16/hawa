import dbAccess as mydb
from geopy.distance import vincenty
import scraper

#initialize a station with default/average parameter values
def putStation(name,url,lat,lon,no2=58.34,so2=10.65,co=0.98,o3=32.66,pm25=140.85,pm10=230.45):
	sql = "INSERT INTO STATIONS(NAME,URL,LATITUDE,LONGITUDE,NO2,SO2,CO,O3,PM25,PM10) \
	VALUES ('%s', '%s', '%f', '%f', '%f', '%f', '%f', '%f', '%f', '%f' ) \
	ON DUPLICATE KEY \
	UPDATE NAME = VALUES(NAME)\
		,  URL  = VALUES(URL)\
		,  LATITUDE  = VALUES(LATITUDE)\
		,  LONGITUDE = VALUES(LONGITUDE)\
		,  NO2  = VALUES(NO2)\
		,  SO2  = VALUES(SO2)\
		,  CO   = VALUES(CO)\
		,  O3   = VALUES(O3)\
		,  PM25 = VALUES(PM25)\
		,  PM10 = VALUES(PM10)" % \
	(name,url,lat,lon,no2,so2,co,o3,pm25,pm10)
	try:
		mydb.cursor.execute(sql)
		mydb.db.commit()
		print "success"
	except:
		print "failed"
		mydb.db.rollback()

#get total vulnerbality of a collection of points
def getVulnerability(Points):
	#sql query for rides in the origin and destination bounding boes
	sql = "SELECT * FROM STATIONS"
	mydb.cursor.execute(sql)
	results = mydb.cursor.fetchall()
	vulnerability=0
	for point in Points:
		myloc=(point[0],point[1])
		for stat in results:
			stationloc=(stat[2],stat[3])
			distance=vincenty(myloc, stationloc).meters
			if distance==0:
				continue
			no2 =stat[4]
			so2 =stat[5]
			co  =stat[6]
			o3  =stat[7]
			pm25=stat[8]
			pm10=stat[9]
			vulnerability += (float(no2+so2+co+o3+pm25+pm10))/distance
	return vulnerability/len(Points)

#update pollution parameters in db after data gathering	
def updateRows():
	sql = "SELECT * FROM STATIONS"
	mydb.cursor.execute(sql)
	results = mydb.cursor.fetchall()
	for station in results:
		url = station[1]
		parameters = scraper.scraper_val(url)
		try:
			if no2>0:
				no2 =parameters["NO2"]
			else:
				no2 = 58.34
		except:
			no2 = 58.34
		try:
			if so2>0:
				so2 =parameters["SO2"]
			else:
				so2 = 10.65
		except:
			so2 = 10.65
		try:
			if co>0:
				co  =parameters["CO"]
			else:
				co = 0.98
		except:
			co  = 0.98
		try:
			if o3>0:
				o3 =paramters["O3"]
			else:
				o3 = 32.66
		except:
			o3  = 32.66
		try:
			if pm25>0:
				pm25 =parameters["PM25"]
			else:
				pm25 = 60
		except:
			pm25 = 60
		try:
			if pm10 > 0:
				pm10 =parameters["PM10"]
			else:
				pm10 = 230.45
		except:
			pm10 = 230.45
		print no2,so2,co,o3,pm25,url
		sql  ="UPDATE STATIONS SET NO2='%f',SO2='%f',CO='%f',O3='%f',PM25='%f',PM10='%f' WHERE URL='%s' " % (no2,so2,co,o3,pm25,pm10,url)
		try:
			mydb.cursor.execute(sql)
			mydb.db.commit()
			print "success"
		except:
			print "failed"
			mydb.db.rollback()
