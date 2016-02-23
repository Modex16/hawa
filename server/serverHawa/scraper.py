#Scraping script to scrape the data from the website of cpcb

import requests
from bs4 import BeautifulSoup
import unicodedata


#returns dictionary with pollutants levels as values and pollutant name as the key
def scraper_val(station_link='http://www.cpcb.gov.in/cpcbpa/Report.aspx?StationName=Anand+Vihar&FullStationName=Anand+Vihar&State=delhi'):
    t = requests.get(station_link)
    print t
    soupt = BeautifulSoup(t.content)
    if t.status_code!=200:
        return {}
    main_content = soupt.find(id = "ctl00_MainContent_test")
    i = 0
    keys = []
    #print main_content
    while main_content.find(id="ctl00_MainContent_dataRow"+str(i)+"dataColumn0")!=None:
        key = (main_content.find(id="ctl00_MainContent_dataRow"+str(i)+"dataColumn0")).text
        key = unicodedata.normalize('NFKD',key).encode('ascii','ignore')
        key = key.replace('g/m3','')
        keys.append(key)
        i+=1
    i = 0
    vals = []
    while main_content.find(id="ctl00_MainContent_dataRow"+str(i)+"dataColumn1")!=None:
        val = (main_content.find(id="ctl00_MainContent_dataRow"+str(i)+"dataColumn1")).text
        val = unicodedata.normalize('NFKD',val).encode('ascii','ignore')
	if val.find('NA')>0 or val[0:val.find('(')]=='NA':
		val = '-1'
		vals.append(float(val))
	else:
        	vals.append(float(val[0:val.find('(')]))
        i+=1
    y = {}
    main_dict = {'pm10':'PM10','sulfur dioxide':'SO2','nitrogen dioxide':'NO2','ozone':'O3','pm2.5':'PM25','carbon monoxidem':'CO2','ammonia':'asd'}
    for i in range(len(keys)):
        y[main_dict[keys[i]]] = vals[i]
    print y
    return y

#if __name__ == '__main__':
#    scraper_val('http://www.cpcb.gov.in/cpcbpa/Report.aspx?StationName=Punjabi%20Bagh&FullStationName=Punjabi%20Bagh&State=delhi')
