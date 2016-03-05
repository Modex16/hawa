import stations,scraper
import time

#continously update stations after 1200 seconds
while True:
	stations.updateRows()
	time.sleep(1200)
